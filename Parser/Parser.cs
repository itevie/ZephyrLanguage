using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;
using ZephyrNew.Lexer;
using ZephyrNew.Lexer.Syntax;
using ZephyrNew.Parser.AST;
using ZephyrNew.Runtime;

namespace ZephyrNew.Parser
{
    internal class Parser
    {
        private List<Token> tokens = new List<Token>();

        // ----- Basic parsing details -----
        private static List<string> ValidMultiplicativeOperators = new List<string>()
        {
            Operators.ArithmeticOperators["Multiply"].Symbol,
            Operators.ArithmeticOperators["Divide"].Symbol,
            Operators.ArithmeticOperators["Percentage"].Symbol,
            Operators.ArithmeticOperators["ReversePercentage"].Symbol,
            Operators.ArithmeticOperators["Power"].Symbol,
            Operators.ArithmeticOperators["Modulo"].Symbol,
            Operators.ArithmeticOperators["Exponent"].Symbol,
        };

        private static List<string> ValidAdditiveOperators = new List<string>()
        {
            Operators.ArithmeticOperators["Plus"].Symbol,
            Operators.ArithmeticOperators["Minus"].Symbol,
        };

        // ----- Basic syntax helper functions -----
        private Token At()
        {
            return tokens[0];
        }

        private Token Eat()
        {
            Token token = At();
            tokens.RemoveAt(0);
            return token;
        }

        private Token Expect(TokenType expectedType, string? customMessage = null)
        {
            if (At().Type != expectedType)
                throw new ParserException(customMessage ?? $"Expected token type {expectedType} but got {At().Type}", At().Location);
            return Eat();
        }

        private Token ExpectExact(TokenType expectedType, string specificType, string? customMessage = null)
        {
            if (At().Type != expectedType || At().Value != specificType)
                throw new ParserException(customMessage ?? $"Expected ({specificType}) but got {At().Type}", At().Location);
            return Eat();
        }

        private void NeedsSemiColon(Expression expression)
        {
            if (expression.NeedsSemicolon)
            {
                if (At().Type != TokenType.Semicolon)
                {
                    throw new ParserException($"Semicolon expected after {expression.Kind}", expression.Location);
                }
            }
        }

        public AST.Program ProduceAST(string sourceCode, string fileName)
        {
            // Lex the sourcecode
            tokens = Lexer.Lexer.Tokenize(sourceCode, fileName);

            AST.Program program = new AST.Program(tokens[0].Location);

            // Parse until EOF is reached
            while (At().Type != TokenType.EOF)
            {
                // Remove random semicolons
                if (At().Type == TokenType.Semicolon)
                {
                    Eat();
                    continue;
                }

                // Get expression
                Expression expression = ParseStatement();

                NeedsSemiColon(expression);
                program.Body.Add(expression);
            }

            return program;
        }
       
        public Location ExpandLocation(Location? a, Location? b)
        {
            if (a != null && b == null) return a;
            if (a == null || b == null) return Location.UnknownLocation;

            if (a.Line != b.Line)
            {
                a.TillEnd = true;
                return a;
            }

            a.TokenEnd = b.TokenEnd;

            return a;
        }

        public bool Lookahead(TokenType forWhat, List<TokenType> allowed)
        {
            int index = 0;

            while (index != tokens.Count - 1)
            {
                if (tokens[index].Type == forWhat)
                    return true;
                else if (!allowed.Contains(tokens[index].Type))
                    return false;
                index++;
            }

            return false;
        }

        // ----- Helpers -----
        private TypeNameCombo ParseName(bool allowVoid = false)
        {
            AST.Type type = ParseType(allowVoid);
            Token name = Expect(TokenType.Identifier, $"Expected an identifier");

            return new TypeNameCombo(type, new Identifier(name.Location, name.Value));
        }

        private AST.Type ParseType(bool allowVoid = false)
        {
            Location typeLocation;
            bool isStruct = false;
            Expression? structName = null;
            Runtime.Values.ValueType type;

            if (At().Type == TokenType.Struct)
            {
                typeLocation = Eat().Location;
                type = Runtime.Values.ValueType.Object;
                isStruct = true;
                structName = ParseMemberExpression();
            } else
            {
                Token tok = Expect(TokenType.Type);
                type = (Runtime.Values.ValueType)int.Parse(tok.Value);
                typeLocation = tok.Location;
            }

            //List<AST.Type>? generics = ParseGenericsList();

            // Check if it is nullable
            bool isNullable = At().Type == TokenType.QuestionMark;
            if (isNullable) Eat();

            bool isArray = false;
            int arrayDepth = 0;

            // Check if it is an array type
            while (At().Type == TokenType.OpenSquare)
            {
                Eat();
                Expect(TokenType.CloseSquare);
                isArray = true;
                arrayDepth++;
            }

            if (allowVoid == false && type == Runtime.Values.ValueType.Void)
                throw new RuntimeException($"Cannot use void here", typeLocation);

            return new AST.Type(type, isNullable, typeLocation)
            {
                IsArray = isArray,
                ArrayDepth = arrayDepth,
                IsStruct = isStruct,
                StructIdentifier = structName,
                GenericsList = null,
            };
        }

        private Expression ParseBlock()
        {
            if (At().Type != TokenType.OpenBrace)
            {
                // Parse single expr
                BlockStatement stmt = new BlockStatement(At().Location, new List<Expression>() { ParseStatement() });
                NeedsSemiColon(stmt.Body[0]);
                Eat();
                return stmt;
            }

            Token start = Expect(TokenType.OpenBrace, "Expected opening of body");

            List<Expression> body = new List<Expression>();

            while (At().Type != TokenType.CloseBrace && At().Type != TokenType.EOF)
            {
                // Remove random semicolons
                if (At().Type == TokenType.Semicolon)
                {
                    Eat();
                    continue;
                }

                Expression value = ParseStatement();
                body.Add(value);

                NeedsSemiColon(value);
            }

            Expect(TokenType.CloseBrace, "Expected closing of body");

            return new BlockStatement(start.Location, body);
        }

        private Expression ExpectValueStatement()
        {
            return At().Type switch
            {
                TokenType.Var => ParseVariableDeclaration(),
                TokenType.Function => ParseFunctionDeclaration(),
                TokenType.Struct => ParseStructDeclaration(),
                _ => throw new ParserException($"Expected a value-type statement here (var, function, etc.)", At().Location)
            };
        }

        private Identifier CreateIdentifier(Token token)
        {
            return new Identifier(token.Location, token.Value);
        }

        private List<FunctionParameter> ParseParameterList()
        {
            Expect(TokenType.OpenParenthesis, "Expected opening of function argument list");

            List<FunctionParameter> parameters = new List<FunctionParameter>();

            if (At().Type == TokenType.Params)
            {
                Token paramToken = Eat();
                AST.Type type = ParseType();

                // Expect atleast 1 array depth
                if (type.ArrayDepth < 1)
                    throw new ParserException($"The params parameter type must have an array depth of at least one, try {type.TypeName}[]", type.Location);

                Identifier name = CreateIdentifier(Expect(TokenType.Identifier));
                Expect(TokenType.CloseParenthesis);
                parameters.Add(new FunctionParameter(paramToken.Location, name, type)
                {
                    IsParams = true,
                });
                return parameters;
            }

            while (At().Type != TokenType.CloseParenthesis)
            {
                AST.Type type = ParseType();
                Token name = Expect(TokenType.Identifier, $"Expected parameter name");

                parameters.Add(new FunctionParameter(name.Location, CreateIdentifier(name), type));

                if (At().Type != TokenType.Comma)
                    break;
                else Eat();
            }

            Expect(TokenType.CloseParenthesis, "Expected closing of function argument list");

            return parameters;
        }

        private List<Expression> ParseArgumentList()
        {
            Expect(TokenType.OpenParenthesis);

            List<Expression> parameters = new List<Expression>();

            while (At().Type != TokenType.CloseParenthesis)
            {
                parameters.Add(ParseExpression());

                if (At().Type != TokenType.Comma) break;
                else Eat();
            }

            Expect(TokenType.CloseParenthesis);

            return parameters;
        }

        private List<AST.Type>? ParseGenericsList()
        {
            // Check if it is a <
            if (At().Type == TokenType.ComparisonOperator && At().Value == Lexer.Syntax.Operators.ComparisonOperators["LessThan"].Symbol)
            {
                Token startToken = Eat();
                List<AST.Type> generics = new List<AST.Type>();

                while (At().Type != TokenType.ComparisonOperator && At().Value != Lexer.Syntax.Operators.ComparisonOperators["GreaterThan"].Symbol && At().Type != TokenType.EOF)
                {
                    AST.Type type = ParseType();
                    generics.Add(type);

                    if (At().Type != TokenType.Comma) break;
                    Eat();
                }

                ExpectExact(TokenType.ComparisonOperator, Operators.ComparisonOperators["GreaterThan"].Symbol, "Expected closing of generic list");

                return generics;
            }

            return null;
        }

        private Expression ParseIndexer()
        {
            Expect(TokenType.OpenSquare);
            Expression expr = ParseExpression();
            Expect(TokenType.CloseSquare);
            return expr;
        }

        // ----- Statements -----
        private Expression ParseStatement()
        {
            return At().Type switch
            {
                TokenType.If => ParseIfStatement(),
                TokenType.Loop => ParseLoopStatement(),
                TokenType.Echo => ParseEchoStatement(),
                TokenType.Break => new BreakStatement(Eat().Location),
                TokenType.Continue => new ContinueStatement(Eat().Location),
                TokenType.Return => new ReturnStatement(Eat().Location, ParseExpression()),
                TokenType.Modifier => ParseModifierStatement(),
                TokenType.Var => ParseVariableDeclaration(),
                TokenType.Function => ParseFunctionDeclaration(),
                TokenType.Import => ParseImportStatement(),
                TokenType.Export => ParseExportStatement(),
                TokenType.From => ParseImportStatement(),
                TokenType.Struct => ParseStructDeclaration(),
                TokenType.For => ParseForStatement(),
                TokenType.Try => ParseTryStatement(),
                TokenType.Decorator => ParseDecorator(),
                _ => ParseExpression()
            };
        }

        private Expression ParseDecorator()
        {
            Token decoratorToken = Eat();

            // Get the name of the function to call
            Expression name = ParseCallExpression();

            // Expect an identifier
            if (name.Kind != Kind.Identifier)
            {
                throw new ParserException($"Expected identifier as decorator name", name.Location);
            }

            // Get the body
            Expression body = ParseStatement();

            // Expect function
            if (body.Kind != Kind.FunctionDeclaration && body.Kind != Kind.DecoratorApplierStatement && body.Kind != Kind.ApplyModifierStatement)
            {
                throw new ParserException($"Expected a function as the decorator body", body.Location);
            }

            // Done
            return new DecoratorApplierStatement(decoratorToken.Location, name, body);
        }

        private Expression ParseTryStatement()
        {
            Token token = Eat();

            // Parse body
            Expression body = ParseBlock();

            // Check if it contains a catch
            Expression? catchBody = null;
            Identifier? catchIdent = null;

            if (At().Type == TokenType.Catch)
            {
                Eat();

                // Check for identifier to be declared
                if (At().Type == TokenType.Identifier)
                    catchIdent = CreateIdentifier(Eat());

                catchBody = ParseBlock();
            }

            return new TryStatement(token.Location, body, catchBody, catchIdent);
        }

        private Expression ParseForStatement()
        {
            Token forToken = Eat();

            // Check if it is in a for in loop
            if (Lookahead(TokenType.In, new List<TokenType> { TokenType.Type, TokenType.Identifier, TokenType.Comma }))
            {
                // It is a for in loop
                TypeNameCombo combo = ParseName();
                TypeNameCombo? index = null;

                // Check if there is a index
                if (At().Type == TokenType.Comma)
                {
                    Eat();
                    index = ParseName();
                }

                Token inToken = Expect(TokenType.In);
                Expression inWhat = ParseExpression();
                Expression body = ParseBlock();

                ForEachStatement stmt = new ForEachStatement(forToken.Location, combo, inWhat, body)
                {
                    DeclarationIndex = index,
                };
                return stmt;
            }

            Expression declaration = At().Type == TokenType.Var ? ParseVariableDeclaration() : ParseExpression();
            Expect(TokenType.Semicolon);
            Expression test = ParseExpression();
            Expect(TokenType.Semicolon);
            Expression increment = ParseExpression();
            Expression b = ParseBlock();

            return new ForStatement(forToken.Location, declaration, test, increment, b);
        }

        private Expression ParseModifierStatement()
        {
            Token token = At();
            Modifier modifier = (Modifier)int.Parse(Eat().Value);
            Expression value = At().Type == TokenType.Modifier
                ? ParseModifierStatement()
                : ExpectValueStatement();
            NeedsSemiColon(value);

            return new ApplyModifierStatement(token.Location, value, modifier);
        }

        private Expression ParseEchoStatement()
        {
            Token echoToken = Eat();

            return new EchoStatement(echoToken.Location, ParseExpression());
        }

        private Expression ParseIfStatement()
        {
            Token ifToken = Eat();

            // Dissallow open brace
            if (At().Type == TokenType.OpenBrace)
            {
                throw new ParserException($"Expected if test here", At().Location);
            }

            // Get the test
            Expression test = ParseExpression();

            // Parse the body
            Expression successBody = ParseBlock();

            Expression? alternate = null;

            //Check if there is an alternative
            if (At().Type == TokenType.Else)
            {
                Eat();

                // Check if the next is else if
                if (At().Type == TokenType.If)
                {
                    alternate = ParseIfStatement();
                }

                // Check for body
                else
                {
                    alternate = ParseBlock();
                }
            }

            // Done
            return new IfStatement(ifToken.Location, test, successBody)
            {
                Alternate = alternate
            };
        }

        private Expression ParseLoopStatement()
        {
            Token loopToken = Expect(TokenType.Loop);
            Expression body = ParseBlock();

            return new LoopStatement(loopToken.Location, body);
        }

        private Expression ParseVariableDeclaration()
        {
            // Get [var type ident]
            Token varToken = Eat();
            TypeNameCombo combo = ParseName();

            // Expect =
            Token assignmentOperator = ExpectExact(
                TokenType.AssignmentOperator,
                Operators.GetOperatorByName("NormalAssignment").Symbol
            );

            // Get value
            Expression value = ParseExpression();

            return new VariableDeclaration(varToken.Location, combo.Identifier, combo.Type, value);
        }

        private Expression ParseFunctionDeclaration()
        {
            // Get [func type ident]
            Token functionToken = Eat();
            TypeNameCombo combo = ParseName(true);
            List<FunctionParameter> parameters = ParseParameterList();
            Expression body = ParseBlock();

            return new FunctionDeclaration(functionToken.Location, combo.Identifier, combo.Type, parameters, body);
        }

        private Expression ParseStructDeclaration()
        {
            Token structToken = Eat();
            Token structName = Expect(TokenType.Identifier);

            Expect(TokenType.OpenBrace, $"Expected opening of struct body");

            Dictionary<Identifier, AST.Type> fields = new Dictionary<Identifier, AST.Type>();

            while (At().Type != TokenType.CloseBrace && At().Type != TokenType.EOF)
            {
                TypeNameCombo combo = ParseName();
                fields.Add(combo.Identifier, combo.Type);
                Expect(TokenType.Semicolon);
            }

            Expect(TokenType.CloseBrace);

            return new StructDeclaration(structToken.Location, CreateIdentifier(structName), fields);
        }

        private Expression ParseImportStatement()
        {
            // import "xxx" as x;
            if (At().Type == TokenType.Import)
            {
                Token importToken = Eat();
                Token stringToken = Expect(TokenType.String, $"Expected string containing filename to import");
                Token atToken = Expect(TokenType.As);
                Token identifier = Expect(TokenType.Identifier, $"Expected identifier to import as");

                return new ImportStatement(importToken.Location, new StringLiteral(stringToken.Location, stringToken.Value), CreateIdentifier(identifier));
            }

            // from "xxx" import x, x, x
            else if (At().Type == TokenType.From)
            {
                Token fromToken = Eat();
                Token stringToken = Expect(TokenType.String, $"Expected string containing filename to import");
                Token importToken = Expect(TokenType.Import);
                List<Identifier> list = new List<Identifier>();

                while (At().Type != TokenType.Semicolon && At().Type != TokenType.EOF)
                {
                    list.Add(CreateIdentifier(Expect(TokenType.Identifier)));
                    if (At().Type != TokenType.Comma) break;
                    Eat();
                }

                return new ImportStatement(fromToken.Location, new StringLiteral(stringToken.Location, stringToken.Value), list);
            }

            throw new ParserException($"", Location.UnknownLocation);
        }

        private Expression ParseExportStatement()
        {
            Token exportToken = Eat();
            Expression after = ParseStatement();

            if ((new[] { Kind.ImportStatement, Kind.Identifier, Kind.VariableDeclaration, Kind.FunctionDeclaration, Kind.ObjectLiteral, Kind.ApplyModifierStatement }).Contains(after.Kind) == false)
            {
                throw new RuntimeException($"{after.Kind} cannot be used with export", exportToken.Location);
            }

            NeedsSemiColon(after);

            return new ExportStatement(exportToken.Location, after);
        }

        // ----- Expressions -----
        private Expression ParseExpression()
        {
            return ParseAssignmentExpression();
        }

        private Expression ParseAssignmentExpression()
        {
            Expression left = ParseTernaryExpression();

            if (At().Type == TokenType.AssignmentOperator)
            {
                Token operatorToken = Eat();
                Expression value = ParseAssignmentExpression();

                AssignmentExpression expr = new AssignmentExpression(operatorToken.Location, left, operatorToken.Value, value);
                expr.FullLocation = ExpandLocation(left.Location, value.Location);
                return expr;
            }

            return left;
        }

        private Expression ParseTernaryExpression()
        {
            Expression left = ParseLogicalExpression();

            // Check for ?
            if (At().Type == TokenType.QuestionMark)
            {
                Token startToken = Eat();

                Expression success = ParseTernaryExpression();

                Expect(TokenType.Colon, $"Expected alternate for ternary");

                Expression alternate = ParseTernaryExpression();

                return new TernaryExpression(startToken.Location, success, left, alternate);
            }

            return left;
        }

        private Expression ParseLogicalExpression()
        {
            Expression left = ParseComparisonExpression();

            if (At().Type == TokenType.LogicalOperator)
            {
                Token operatorToken = Eat();
                Expression right = ParseLogicalExpression();

                return new LogicalExpression(operatorToken.Location, left, operatorToken.Value, right);
            }

            return left;
        }

        private Expression ParseComparisonExpression()
        {
            Expression left = ParseCastExpression();

            if (At().Type == TokenType.ComparisonOperator)
            {
                Token operatorToken = Eat();
                Expression right = ParseComparisonExpression();

                return new ComparisonExpression(operatorToken.Location, left, operatorToken.Value, right);
            }

            return left;
        }

        private Expression ParseCastExpression()
        {
            Expression left = ParsePipeExpression();

            while (At().Type == TokenType.CastOperator)
            {
                Token castOperator = Eat();

                AST.Type type = ParseType();

                left = new CastExpression(castOperator.Location, left, type);
            }
            
            return left;
        }

        private Expression ParsePipeExpression()
        {
            Expression left = ParseInExpression();

            while (At().Type == TokenType.Pipe)
            {
                Token pipeOperator = Eat();
                
                // Check if it is a . for quick member expression
                if (At().Type == TokenType.Dot)
                {
                    Expression expr = ParseMemberExpression(left);
                    left = expr;
                } else
                {
                    Expression expr = ParseCallExpression();

                    if (expr.Kind != Kind.CallExpression && expr.Kind != Kind.MemberExpression && expr.Kind != Kind.Identifier)
                        throw new ParserException($"Expected call, member or identifier expression", expr.Location);
                    left = new PipeExpression(pipeOperator.Location, left, expr);
                }
            }

            return left;
        }

        private Expression ParseInExpression()
        {
            Expression left = ParseRangeExpression();

            if (At().Type == TokenType.In)
            {
                Token inToken = Eat();

                Expression right = ParseRangeExpression();

                return new InExpression(inToken.Location, left, right);
            }

            return left;
        }

        private Expression ParseRangeExpression()
        {
            Expression left = ParseAdditiveExpression();

            // Check if it is a range
            if (At().Type == TokenType.Range || At().Type == TokenType.RangeUninclusive)
            {
                Token rangeOperator = Eat();

                Expression right = ParseAdditiveExpression();
                Expression? step = null;

                if (At().Type == TokenType.Step)
                {
                    Eat();
                    step = ParseAdditiveExpression();
                }

                return new RangeExpression(rangeOperator.Location, left, right)
                {
                    Step = step,
                    Uninclusive = rangeOperator.Type == TokenType.RangeUninclusive
                };
            }

            return left;
        }

        private Expression ParseAdditiveExpression()
        {

            Expression left = ParseMultiplicativeExpression();

            while (ValidAdditiveOperators.Contains(At().Value))
            {
                Token operatorToken = Eat();
                Expression right = ParseMultiplicativeExpression();

                // Check if both are literal
                if (left.Kind == Kind.NumericLiteral && right.Kind == Kind.NumericLiteral)
                {
                    double l = ((NumericLiteral)left).Value;
                    double r = ((NumericLiteral)right).Value;

                    if (operatorToken.Value == Operators.ArithmeticOperators["Plus"].Symbol)
                        left = new NumericLiteral(ExpandLocation(left.Location, right.Location), l + r);
                    else if (operatorToken.Value == Operators.ArithmeticOperators["Minus"].Symbol)
                        left = new NumericLiteral(ExpandLocation(left.Location, right.Location), l - r);
                    else
                        left = new BinaryExpression(operatorToken.Location, left, right, operatorToken.Value);
                }

                // Update left
                else left = new BinaryExpression(operatorToken.Location, left, right, operatorToken.Value);
            }

            return left;
        }

        private Expression ParseMultiplicativeExpression()
        {
            Expression left = ParseUnaryExpression();

            while (ValidMultiplicativeOperators.Contains(At().Value))
            {
                Token operatorToken = Eat();
                Expression right = ParseUnaryExpression();

                // Updat left
                left = new BinaryExpression(operatorToken.Location, left, right, operatorToken.Value);
            }

            return left;
        }

        private Expression ParseUnaryExpression()
        {
            TokenType[] unaryOperatorList = new TokenType[] { TokenType.UnaryOperator, TokenType.BinaryOperator };

            // Check if current token is unary
            if (unaryOperatorList.Contains(At().Type))
            {
                // Check if it is a binary and if it is valid unary binary
                if (At().Type == TokenType.BinaryOperator)
                {
                    // Check if this binary op is allowed to be unary
                    if (new[] { Operators.ArithmeticOperators["Minus"].Symbol, Operators.ArithmeticOperators["Plus"].Symbol }.Contains(At().Value) == false)
                    {
                        throw new LexerException($"Cannot use this binary operator as a unary operator", At().Location);
                    }
                }

                Token unaryOperator = Eat();

                // Get the right hand side value
                Expression right = unaryOperatorList.Contains(At().Type)
                    ? ParseUnaryExpression()
                    : ParseAwaitExpression();

                return new UnaryExpression(unaryOperator.Location, right, unaryOperator.Value);
            }

            Expression left = ParseAwaitExpression();

            // Check if there is a unary on the right
            if (At().Type == TokenType.UnaryOperator)
            {
                Token oper = Eat();
                return new UnaryRightExpression(oper.Location, left, oper.Value);
            }

            return left;
        }

        private Expression ParseAwaitExpression()
        {
            if (At().Type == TokenType.Await)
            {
                Token awaitToken = Eat();
                Expression right = ParseCallExpression();

                return new AwaitExpression(awaitToken.Location, right)
                {
                    FullLocation = ExpandLocation(awaitToken.Location, right.Location)
                };
            }

            return ParseCallExpression();
        }

        private Expression ParseCallExpression(Expression? setLeft = null)
        {
            Expression left = setLeft ?? ParseMemberExpression();

            if (At().Type == TokenType.OpenParenthesis)
            {
                List<Expression> parameters = ParseArgumentList();

                CallExpression expr = new CallExpression(left.Location, left, parameters);

                return At().Type == TokenType.Dot || At().Type == TokenType.OpenSquare ? ParseMemberExpression(expr) : ParseCallExpression(expr);
            }

            return left;
        }

        private Expression ParseMemberExpression(Expression? setLeft = null)
        {
            Expression left = setLeft ?? ParseLambdaExpression();

            while (At().Type == TokenType.Dot || At().Type == TokenType.OpenSquare)
            {
                Expression right;
                bool isComputed = false;
                if (At().Type == TokenType.Dot)
                {
                    Eat();
                    right = ParseIdentifier(true);
                } else if (At().Type == TokenType.OpenSquare)
                {
                    right = ParseIndexer();
                    isComputed = true;
                } else
                {
                    throw new ParserException($"Expected identifier or indexer", At().Location);
                }

                MemberExpression temp = new MemberExpression(ExpandLocation(left.Location, right.Location), left, right);
                temp.IsComputed = isComputed;
                left = temp;
            }   

            return ParseCallExpression(left);
        }

        private Expression ParseLambdaExpression()
        {
            // Check for => _
            if (At().Type == TokenType.Lambda)
            {
                Token lambdaToken = Eat();
                Expression body;

                if (At().Type == TokenType.OpenBrace)
                    body = ParseBlock();
                else body = new BlockStatement(
                    At().Location,
                    new List<Expression>() {
                    new ReturnStatement(At().Location, ParseExpression()),
                    }
                );

                return new LambdaExpression(lambdaToken.Location, body, new List<Identifier>());
            }

            Expression left = ParseSpreadOperator();

            // Check for lambda
            if (At().Type == TokenType.Lambda)
            {
                Token lambdaToken = Eat();
                Expression body;

                if (At().Type == TokenType.OpenBrace)
                    body = ParseBlock();
                else body = new BlockStatement(
                    At().Location,
                    new List<Expression>() {
                    new ReturnStatement(At().Location, ParseExpression()),
                    }
                );

                // Check if x => _
                if (left.Kind == Kind.Identifier)
                {
                    return new LambdaExpression(lambdaToken.Location, body, new List<Identifier>()
                    {
                        (Identifier)left,
                    });
                } 
                
                // Check if x, x, x => _
                else if (left.Kind == Kind.LambdaArgumentList)
                {
                    return new LambdaExpression(lambdaToken.Location, body, ((LambdaArgumentList)left).Arguments);
                }

                else
                {
                    throw new ParserException($"Cannot use {left.Kind} as lambda arguments", left.Location);
                }
            }

            if (left.Kind == Kind.LambdaArgumentList)
                throw new ParserException($"Unexpected lambda argument list", left.Location);

            return left;
        }

        private Expression ParseSpreadOperator()
        {
            if (At().Type == TokenType.Spread)
            {
                Token spreadToken = Eat();

                Expression right = ParsePrimaryExpression();

                return new SpreadExpression(spreadToken.Location, right);
            }

            return ParsePrimaryExpression(); 
        }

        // ----- Literals -----
        private Expression ParsePrimaryExpression()
        {
            return At().Type switch
            {
                TokenType.Number => ParseNumericLiteral(),
                TokenType.Identifier => ParseIdentifier(),
                TokenType.OpenPipe => ParseLambdaArgumentList(),
                TokenType.String => ParseStringLiteral(),
                TokenType.OpenSquare => ParseArrayLiteral(),
                TokenType.OpenParenthesis => ParseParenthesisedExpression(),
                TokenType.OpenBrace => ParseObjectLiteral(),
                TokenType.Function => ParseFunctionExpression(),
                _ => throw new ParserException($"Unexpected token: {At().Type}", At().Location)
            };
        }

        // ----- Literal parsing functions -----
        private Expression ParseParenthesisedExpression()
        {
            Expect(TokenType.OpenParenthesis);

            Expression expression = ParseExpression();

            Expect(TokenType.CloseParenthesis);

            return expression;
        }

        private Expression ParseObjectLiteral()
        {
            Token startingToken = Expect(TokenType.OpenBrace);

            Dictionary<Identifier, Expression> keyValues = new Dictionary<Identifier, Expression>();
            List<string> identifiersUsed = new List<string>();

            while (At().Type != TokenType.CloseBrace)
            {
                // Get the name
                Token ident;

                if (At().Type == TokenType.Identifier || At().Type == TokenType.String)
                    ident = Eat();
                else
                {
                    throw new ParserException($"Expected identifier", At().Location);
                }

                if (identifiersUsed.Contains(ident.Value))
                {
                    throw new ParserException($"The identifier {ident.Value} has already been defined in this object", ident.Location);
                }

                identifiersUsed.Add(ident.Value);

                // Check whether it is implied
                if (At().Type != TokenType.Colon)
                {
                    keyValues.Add(CreateIdentifier(ident), CreateIdentifier(ident));
                } else if (At().Type == TokenType.Colon)
                {
                    Eat();
                    Expression expression = ParseExpression();
                    keyValues.Add(CreateIdentifier(ident), expression);
                }

                // Check for comma or end of object
                if (At().Type != TokenType.CloseBrace && At().Type != TokenType.Comma)
                {
                    Expect(TokenType.Comma, "Expected comma here");
                }

                if (At().Type == TokenType.Comma)
                {
                    Eat();
                    continue;
                }
                else break;
            }

            Expect(TokenType.CloseBrace);

            return new ObjectLiteral(startingToken.Location, keyValues);
        }

        private Expression ParseArrayLiteral()
        {
            Token token = Expect(TokenType.OpenSquare);

            List<Expression> items = new List<Expression>();

            while (At().Type != TokenType.CloseSquare)
            {
                items.Add(ParseExpression());

                if (At().Type != TokenType.Comma) break;
                Eat();
            }

            Expect(TokenType.CloseSquare, "Expected closing of array literal");

            return new ArrayLiteral(token.Location, items);
        }

        private Expression ParseLambdaArgumentList()
        {
            Token pipeToken = Eat();

            List<Identifier> parameters = new List<Identifier>();

            while (At().Type != TokenType.OpenPipe && At().Type != TokenType.EOF)
            {
                parameters.Add(CreateIdentifier(Expect(TokenType.Identifier)));

                if (At().Type != TokenType.Comma) break;
                if (At().Type == TokenType.Comma)
                {
                    Eat();
                }
            }

            Expect(TokenType.OpenPipe);

            return new LambdaArgumentList(pipeToken.Location, parameters);
        }

        private Expression ParseIdentifier(bool allowSpecialNames = false)
        {
            Identifier identifier;

            if (!allowSpecialNames || At().Type == TokenType.Identifier)
                identifier = CreateIdentifier(Eat());
            else
            {
                if (At().Type != TokenType.Identifier && At().Type != TokenType.Type)
                    throw new ParserException($"Expected name here", At().Location);
                Token a = Eat();
                identifier = new Identifier(a.Location, a.StringValue);
            }

            return identifier;
        }

        private Expression ParseNumericLiteral()
        {
            Token numberToken = Eat();
            return new NumericLiteral(numberToken.Location, double.Parse(numberToken.Value))
            {
                IsReal = numberToken.Value.Contains(".")
            };
        }

        private Expression ParseStringLiteral()
        {
            Token stringToken = Eat();

            return new StringLiteral(stringToken.Location, stringToken.Value);
        }

        private Expression ParseFunctionExpression()
        {
            Token functionToken = Eat();
            TypeNameCombo combo = ParseName(true);
            List<FunctionParameter> parameters = ParseParameterList();
            Expression body = ParseBlock();

            return new FunctionExpression(functionToken.Location, combo.Identifier, combo.Type, parameters, body);
        }
    }
}
