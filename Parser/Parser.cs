using Spectre.Console.Rendering;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using Zephyr.Lexer;
using Zephyr.Lexer.Syntax;
using Zephyr.Parser.AST;

namespace Zephyr.Parser
{
    internal class Parser
    {
        private List<Token> tokens = new List<Token>();

        private bool NotEOF()
        {
            return tokens[0].TokenType != TokenType.EOF;
        }

        private Token At()
        {
            return tokens[0];
        }

        private Token Eat()
        {
            Token previous = At();
            tokens.RemoveAt(0);
            return previous;
        }

        private Token Expect(TokenType type, string error = "", string specific = "")
        {
            Token previous = Eat();

            // Check if correct type
            if (previous == null || previous.TokenType != type)
            {
                throw new ParserException(new()
                {
                    Token = previous,
                    Error = $"Unexpected {previous?.TokenType}, " + (error != "" ? error : $" expected {type}")
                });
            }

            return previous;
        }

        private bool NeedSemiColon(Expression value)
        {
            if (value.Kind == Kind.FunctionDeclaration || value.Kind == Kind.ObjectLiteral || 
                value.Kind == Kind.IfStatement || value.Kind == Kind.WhileStatement ||
                value.Kind == Kind.ForEachStatement || value.Kind == Kind.TryStatement)
                return false;
            return true;
        }

        public AST.Program ProduceAST(string sourceCode, string fileName)
        {
            tokens = Lexer.Lexer.Tokenize(sourceCode, fileName);

            AST.Program program = new AST.Program();

            // Parse until end of file
            while (NotEOF())
            {
                // Remove random semi colons
                if (At().TokenType == TokenType.Semicolon)
                {
                    Eat();
                    continue;
                }

                // Get value
                Expression value = ParseControlFlowStatement();

                // Check if require semicolon
                if (NeedSemiColon(value))
                    Expect(TokenType.Semicolon, $"Expected semi colon after statement");

                program.Body.Add(value);
            }

            return program;
        }

        private Expression ParseControlFlowStatement()
        {
            switch (At().TokenType)
            {
                case TokenType.Let:
                    return ParseVariableDeclaration();
                case TokenType.Const:
                    return ParseVariableDeclaration();
                case TokenType.If:
                    return ParseIfStatement();
                case TokenType.While:
                    return ParseWhileStatement();
                case TokenType.For:
                    return ParseForStatement();
                case TokenType.Return:
                    return ParseReturnStatement();
                case TokenType.Try:
                    return ParseTryStatement();
                case TokenType.Import:
                    return ParseImportStatement();
                case TokenType.Export:
                    return ParseExportStatement();
                default:
                    return ParseStatement();
            }
        }

        private Expression ParseStatement()
        {
            switch (At().TokenType)
            {
                case TokenType.Function:
                    return ParseFunctionDeclaration();
                default:
                    return ParseExpression();
            }
        }

        /*
         * STATEMENTS - CONTROL FLOW
         */
        private Expression ParseImportStatement()
        {
            Token importToken = Eat();

            // Expect string literal
            Expression identifier = ParsePrimaryExpression();

            if (identifier.Kind != Kind.StringLiteral)
            {
                throw new ParserException(new()
                {
                    Location = identifier.Location,
                    Error = "Expected string here"
                });
            }

            // Check for as
            Expression? importAs = null;

            if (At().TokenType == TokenType.As)
            {
                Eat();
                importAs = ParsePrimaryExpression();

                // Check type
                if (importAs.Kind != Kind.Identifier)
                {
                    throw new ParserException(new()
                    {
                        Location = importAs.Location,
                        Error = "Expected identifier here"
                    });
                }
            }

            return new ImportStatement()
            {
                Location = importToken.Location,
                ToImport = identifier,
                ImportAs = importAs,
            };
        }

        private Expression ParseExportStatement()
        {
            Token exportToken = Eat();

            Expression toExport = ParseStatement();

            Expect(TokenType.As, "Expected as keyword");

            Expression exportAs = ParsePrimaryExpression();

            if (exportAs.Kind != Kind.Identifier)
            {
                throw new ParserException(new()
                {
                    Location = exportAs.Location,
                    Error = "Expected identifier here"
                });
            }

            return new ExportStatement()
            {
                ToExport = toExport,
                ExportAs = exportAs,
                Location = exportToken.Location,
            };
        }

        private Expression ParseVariableDeclaration()
        {
            Token variableDeclarationToken = Eat();

            // Check for modifiers
            List<Runtime.Values.Modifier> modifiers = new();

            while (At().TokenType == TokenType.Modifier)
            {
                Token modifierToken = Eat();
                Runtime.Values.Modifier mod = (Runtime.Values.Modifier)int.Parse(modifierToken.Value);

                // Check already contains
                if (modifiers.Contains(mod))
                {
                    throw new ParserException(new()
                    {
                        Token = modifierToken,
                        Error = $"The declaration already has this modifier"
                    });
                }

                modifiers.Add(mod);
            }

            TypeIdentifierCombo identifierCombo = ParseName();

            // Generate location
            Location declarationLocation = variableDeclarationToken.Location;
            if (identifierCombo.Identifier.Location != null)
                declarationLocation.TokenEnd = identifierCombo.Identifier.Location.TokenEnd;

            bool isConstant = variableDeclarationToken.TokenType == TokenType.Const;

            // Check if this is only a declaration without assignment
            if (At().TokenType == TokenType.Semicolon || At().TokenType == TokenType.EOF)
            {
                // Check if trying to create const without valie
                if (isConstant)
                {
                    throw new ParserException(new()
                    {
                        Token = variableDeclarationToken,
                        Error = "Must assign a value to a const variable at declaration"
                    });
                }

                // Create
                return new VariableDeclaration()
                {
                    IsConstant = isConstant,
                    Identifier = identifierCombo.Identifier,
                    Type = identifierCombo.Type,
                    IsTypeNullable = identifierCombo.isNullable,
                    Location = declarationLocation,
                    Modifiers = modifiers,
                };
            }

            Expect(TokenType.AssignmentOperator, "Expected equals operator", "=");

            return new VariableDeclaration()
            {
                Value = ParseExpression(),
                IsConstant = isConstant,
                Identifier = identifierCombo.Identifier,
                Type = identifierCombo.Type,
                IsTypeNullable = identifierCombo.isNullable,
                Location = declarationLocation,
                Modifiers = modifiers,
            };
        }

        private Expression ParseIfStatement()
        {
            Eat();

            // Dissalow open-brace
            if (At().TokenType == TokenType.OpenBrace)
            {
                throw new ParserException(new()
                {
                    Token = At(),
                    Error = "Unexpected open brace, expected if statement test"
                });
            }

            // Get the test
            Expression test = ParseAssignmentExpression();

            // Parse the body
            Expression successBody = ParseBlock();

            // Check if there is an alternate
            Expression? alternate = null;

            if (At().TokenType == TokenType.Else)
            {
                Eat();

                // Check what is next
                if (At().TokenType == TokenType.If)
                {
                    alternate = ParseIfStatement();
                }
                else if (At().TokenType == TokenType.OpenBrace)
                {
                    alternate = ParseBlock();
                }
                else throw new ParserException(new()
                {
                    Token = At(),
                    Error = $"Cannot use token of type {At().TokenType} with an else statement"
                });
            }

            return new IfStatement()
            {
                Success = successBody,
                Alternate = alternate,
                Test = test,
            };
        }

        private Expression ParseReturnStatement()
        {
            Token returnToken = Eat();
            Expression? value = At().TokenType != TokenType.Semicolon ? ParseExpression() : null;

            return new ReturnStatement()
            {
                Location = returnToken.Location,
                Value = value,
            };
        }

        private Expression ParseWhileStatement()
        {
            Token whileToken = Eat();

            // Dissalow open-brace
            if (At().TokenType == TokenType.OpenBrace)
            {
                throw new ParserException(new()
                {
                    Token = At(),
                    Error = "Unexpected open brace, expected while test"
                });
            }

            // Get the test
            Expression test = ParseAssignmentExpression();
            Expression body = ParseBlock();

            return new WhileStatement()
            {
                Test = test,
                Body = body,
                Location = whileToken.Location
            };
        }

        private Expression ParseForStatement()
        {
            Token forToken = Eat();

            // Get variable to create
            Expression identifier = ParsePrimaryExpression();

            if (identifier.Kind != Kind.Identifier)
            {
                throw new ParserException(new()
                {
                    Location = identifier.Location,
                    Error = "Expected identifier"
                });
            }

            Expect(TokenType.ForEachIn, $"Expected {Operators.SingleOperators["ForEachIn"].Symbol} operator for for-each loop");

            // Value to enumerate
            Expression valueToEnumerate = ParseAdditiveExpression();

            // Expect body
            Expression body = ParseBlock();

            // Create
            return new ForEachStatement()
            {
                Identifier = identifier,
                ValueToEnumerate = valueToEnumerate,
                Location = forToken.Location,
                Body = body,
            };
        }

        public Expression ParseTryStatement()
        {
            Token tryToken = Eat();

            Expression body = new Expression();

            // Get the body
            if (At().TokenType == TokenType.OpenBrace)
            {
                body = ParseBlock();
            }
            else
            {
                body = ParseExpression();
                Expect(TokenType.Semicolon, "Expected semi colon");
            }

            Expression? catchBody = null;
            Expression? createident = null;

            // Check for catch
            if (At().TokenType == TokenType.Catch)
            {
                Token catchToken = Eat();

                // Only allow if body is block
                if (body.Kind != Kind.BlockStatement)
                {
                    throw new ParserException(new()
                    {
                        Location = catchToken.Location,
                        Error = $"Can only use try-catch when body is a block statement, got {body.Kind} as body"
                    });
                }

                // Check if there is ident
                if (At().TokenType == TokenType.Identifier)
                {
                    createident = ParsePrimaryExpression();
                }

                catchBody = ParseBlock();
            }

            return new TryStatement()
            {
                Body = body,
                CatchBody = catchBody,
                Location = tryToken.Location,
                IdentifierToCreate = createident,
            };
        }

        /*
         * STATEMENTS - NOT CONTROL FLOW
         */
        private Expression ParseFunctionDeclaration()
        {
            Token functionKeywordToken = Eat();

            Expression? functionNameToken = null;

            if (At().TokenType == TokenType.Identifier)
            {
                functionNameToken = ParsePrimaryExpression();
            }

            List<Expression> arguments = ParseArguments();
            List<Expression> parameters = new List<Expression>();

            foreach (Expression arg in arguments)
            {
                // Check type
                if (arg.Kind != Kind.Identifier)
                    throw new ParserException(new()
                    {
                        Location = arg.Location,
                        Error = $"All function paremeters must be an identifier"
                    });
                parameters.Add(arg);
            }

            Expression body = ParseBlock();

            return new FunctionDeclaration()
            {
                Name = functionNameToken,
                Parameters = parameters,
                Body = body,
                Location = functionKeywordToken.Location
            };
        }

        /*
         * HELPERS
         */
        private Expression ParseBlock()
        {
            Expect(TokenType.OpenBrace, "Expected opening of body");
            List<Expression> body = new List<Expression>();

            while (At().TokenType != TokenType.CloseBrace && At().TokenType != TokenType.EOF)
            {
                Expression value = ParseControlFlowStatement();
                body.Add(value);

                if (NeedSemiColon(value))
                    Expect(TokenType.Semicolon, "Expected semi colon");
            }

            Expect(TokenType.CloseBrace, "Expected closing of body");

            return new BlockStatement()
            {
                Body = body,
            };
        }

        private TypeIdentifierCombo ParseName()
        {
            Runtime.Values.ValueType type = Runtime.Values.ValueType.Any;
            bool isNullable = false;
            Token identifier;

            // Check for type
            if (At().TokenType == TokenType.Type)
            {
                string t = Eat().Value;
                type = (Runtime.Values.ValueType)int.Parse(t);

                // Check if nullable
                if (At().TokenType == TokenType.QuestionMark)
                {
                    Eat();
                    isNullable = true;
                }
            }

            // Expect identifier now
            identifier = Expect(TokenType.Identifier, "Expected ifentifier");

            Identifier ident = new Identifier()
            {
                Location = identifier.Location,
                Symbol = identifier.Value,
            };

            return new TypeIdentifierCombo(ident, type, isNullable);
        }

        private Location? ExpandLocation(Location? a, Location? b)
        {
            if (a != null && b == null) return a;
            if (a == null || b == null) return null;

            if (a.Line != b.Line)
            {
                a.TillEnd = true;
                return a;
            }

            a.TokenEnd = b.TokenEnd;

            return a;
        }

        /*
         * EXPRESSIONS
         */
        private AST.Expression ParseExpression()
        {
            return ParseAssignmentExpression();
        }

        private AST.Expression ParseAssignmentExpression()
        {
            AST.Expression left = ParserTernaryExpression();

            // Check if next is assignment operator
            if (At().TokenType == TokenType.AssignmentOperator)
            {
                Token operatorToken = Eat();
                string op = operatorToken.Value;

                Expression val = ParseAssignmentExpression();

                return new AssignmentExpression()
                {
                    Assignee = left,
                    Value = val,
                    Type = op,
                    Location = operatorToken.Location
                };
            }

            return left;
        }

        private Expression ParserTernaryExpression()
        {
            Expression left = ParseLogicalExpression();

            if (At().TokenType == TokenType.QuestionMark)
            {
                Eat();
                Expression success = ParseLogicalExpression();

                Expect(TokenType.Colon, "Expected : for ternary expression");

                Expression alternate = ParseLogicalExpression();

                return new TernaryExpression()
                {
                    Test = left,
                    Success = success,
                    Alternate = alternate,
                    Location = ExpandLocation(left.Location, alternate.Location)
                };
            }

            return left;
        }

        private AST.Expression ParseLogicalExpression()
        {
            AST.Expression left = ParseComparisonExpression();

            // Check if next is logical
            if (At().TokenType == TokenType.LogicalOperator)
            {
                string op = Eat().Value;
                AST.Expression right = ParseLogicalExpression();

                return new LogicalExpression()
                {
                    Left = left,
                    Right = right,
                    Operator = op
                };
            }

            // It is not
            return left;
        }

        private AST.Expression ParseComparisonExpression()
        {
            AST.Expression left = ParseAdditiveExpression();

            // Check if next is comparison
            if (At().TokenType == TokenType.ComparisonOperator)
            {
                Token operatorToken = Eat();
                string op = operatorToken.Value;

                AST.Expression right = At().TokenType == TokenType.ComparisonOperator
                    ? ParseComparisonExpression()
                    : ParseAdditiveExpression();

                return new ComparisonExpression()
                {
                    Left = left,
                    Right = right,
                    Operator = op,
                    Location = operatorToken.Location
                };
            }

            // It is not a comparison expression
            return left;
        }

        private AST.Expression ParseAdditiveExpression()
        {
            AST.Expression left = ParseMultiplicativeExpression();

            while (
                At().Value == Operators.ArithmeticOperators["Plus"].Symbol ||
                At().Value == Operators.ArithmeticOperators["Subtract"].Symbol
            )
            {
                Token operatorToken = Eat();
                string op = operatorToken.Value;
                AST.Expression right = ParseMultiplicativeExpression();

                left = new BinaryExpression()
                {
                    Right = right,
                    Left = left,
                    Operator = op,
                    Location = operatorToken.Location,
                };
            }

            return left;
        }

        private AST.Expression ParseMultiplicativeExpression()
        {
            AST.Expression left = ParseCastExpression();

            while (
                At().Value == Operators.ArithmeticOperators["Multiply"].Symbol ||
                At().Value == Operators.ArithmeticOperators["Divide"].Symbol ||
                At().Value == Operators.ArithmeticOperators["Modulus"].Symbol ||
                At().Value == Operators.ArithmeticOperators["Power"].Symbol
            )
            {
                Token operatorToken = Eat();
                string op = operatorToken.Value;
                AST.Expression right = ParseCastExpression();

                left = new BinaryExpression()
                {
                    Right = right,
                    Left = left,
                    Operator = op,
                    Location = operatorToken.Location,
                };
            }

            return left;
        }

        private Expression ParseCastExpression()
        {
            Expression left = ParseRangeExpression();

            if (At().TokenType == TokenType.Cast)
            {
                Token castToken = Eat();

                Token right = Expect(TokenType.Type, "Expected type");

                return new CastExpression()
                {
                    Type = (Runtime.Values.ValueType)int.Parse(right.Value),
                    Left = left,
                    Location = castToken.Location
                };
            }

            return left;
        }

        private Expression ParseRangeExpression()
        {
            Expression left = ParseUnaryExpression();

            if (At().TokenType == TokenType.DoubleDot || At().TokenType == TokenType.DoubleDotUninclusive)
            {
                bool uninclusive = At().TokenType == TokenType.DoubleDotUninclusive;
                Eat();

                Expression right = new Expression();
                Expression? middle = null;

                Expression temp = ParseUnaryExpression();

                // Check whether or not it has a step
                if (At().TokenType == TokenType.DoubleDot || At().TokenType == TokenType.DoubleDotUninclusive)
                {
                    uninclusive = At().TokenType == TokenType.DoubleDotUninclusive;
                    Eat();

                    middle = temp;
                    right = ParseUnaryExpression();
                }
                else
                {
                    right = temp;
                }

                return new RangeExpression()
                {
                    Start = left,
                    Step = middle,
                    End = right,
                    Location = ExpandLocation(left.Location, right.Location),
                    Uninclusive = uninclusive,
                };
            }

            return left;
        }

        private AST.Expression ParseUnaryExpression()
        {
            // Check if current is unary
            if (At().TokenType == TokenType.UnaryOperator)
            {
                Token operatorToken = Eat();

                // Get the righ hand side
                AST.Expression right = At().TokenType == TokenType.UnaryOperator
                    ? ParseUnaryExpression()
                    : ParseCallMemberExpression();

                // Done
                return new UnaryExpression()
                {
                    Operator = operatorToken.Value,
                    Right = right,
                    Location = operatorToken.Location,
                };
            }

            // Get the value
            Expression value = ParseCallMemberExpression();

            // Check if the next value is a unary
            if (At().TokenType == TokenType.UnaryOperator)
            {
                Token operatorToken = Eat();
                string op = operatorToken.Value;

                // Check if a unary is allowed after
                if (op != Operators.UnaryOperators["Increment"].Symbol &&
                    op != Operators.UnaryOperators["Decrement"].Symbol)
                {
                    throw new ParserException(new()
                    {
                        Token = operatorToken,
                        Error = $"Operator {op} cannot be used as a postfix unary operator"
                    });
                }

                // It is a right handed uanry
                return new UnaryRightExpression()
                {
                    Left = value,
                    Operator = op,
                    Location = operatorToken.Location,
                };
            }

            // It is not a unary
            return value;
        }

        /*private Expression ParseIndexerExpression()
        {
            Expression left = ParseCallMemberExpression();

            if (At().TokenType == TokenType.OpenSquare)
            {
                Eat();
                Expression indexer = ParseExpression();
                Expect(TokenType.CloseSquare, "Expected closing of indexer");

                return new IndexerExpression()
                {
                    Left = left,
                    Indexer = indexer
                };
            }

            return left;
        }*/

        private AST.Expression ParseCallMemberExpression()
        {
            AST.Expression member = ParseMemberExpression();

            if (At().TokenType == TokenType.OpenParan)
            {
                return ParseCallExpression(member);
            }

            return member;
        }

        private AST.Expression ParseMemberExpression()
        {
            AST.Expression obj = ParsePrimaryExpression();

            Location? fullLocation = obj.Location;

            while (At().TokenType == TokenType.Dot || At().TokenType == TokenType.OpenSquare)
            {
                Token op = Eat();
                AST.Expression property;
                bool computed = false;
                fullLocation = ExpandLocation(fullLocation, op.Location);

                // Non computed values (dot.dot.dot)
                if (op.TokenType == TokenType.Dot)
                {
                    computed = false;
                    property = ParsePrimaryExpression();

                    if (property.Kind != Kind.Identifier)
                        throw new ParserException(new()
                        {
                            Location = property.Location,
                            Error = $"Expected identifier, but got {property.Kind}"
                        });
                    fullLocation = ExpandLocation(fullLocation, property.Location);
                }

                // Computer values object["value"]
                else
                {
                    computed = true;
                    property = ParseExpression();
                    Token endSquare = Expect(TokenType.CloseSquare, "Expected closing square");
                    fullLocation = ExpandLocation(fullLocation, endSquare.Location);
                }

                obj = new MemberExpression()
                {
                    Object = obj,
                    IsComputed = computed,
                    Property = property,
                    Location = obj.Location,
                    FullExpressionLocation = fullLocation
                };
            }

            return obj;
        }

        private AST.Expression ParseCallExpression(AST.Expression caller)
        {
            AST.CallExpression callExpression = new CallExpression()
            {
                Caller = caller,
                Arguments = ParseArguments(),
                FullExpressionLocation = caller.Location,
            };

            if (At().TokenType == TokenType.OpenParan)
            {
                CallExpression temp = (CallExpression)ParseCallExpression(callExpression);
                callExpression.FullExpressionLocation = ExpandLocation(caller?.FullExpressionLocation, temp?.Location);
                callExpression = temp;
            }

            return callExpression;
        }

        private List<AST.Expression> ParseArguments()
        {
            Expect(TokenType.OpenParan, $"Expected open parenthesis");

            List<AST.Expression> args = At().TokenType == TokenType.CloseParan
                ? new()
                : ParseArgumentsList();

            Expect(TokenType.CloseParan, $"Expected close parenthesis");

            return args;
        }

        private List<AST.Expression> ParseArgumentsList()
        {
            List<AST.Expression> args = new List<AST.Expression>()
            {
                ParseExpression()
            };

            while (At().TokenType == TokenType.Comma)
            {
                Eat();
                args.Add(ParseExpression());
            }

            return args;
        }

        private AST.Expression ParseObjectLiteral()
        {
            Token openingbrace = Expect(TokenType.OpenBrace, "Expected open brace for object");

            List<Property> properties = new();

            while (this.NotEOF() && At().TokenType != TokenType.CloseBrace)
            {
                Token key = Expect(TokenType.Identifier, "Expected key name");

                // Check for shorthand { key, }
                if (At().TokenType == TokenType.Comma)
                {
                    Eat();
                    properties.Add(new Property()
                    {
                        Key = key.Value,
                        IsAlone = true
                    });
                    continue;
                }

                // Shorthand key { key }
                else if (At().TokenType == TokenType.CloseBrace)
                {
                    properties.Add(new Property()
                    {
                        Key = key.Value,
                        IsAlone = true,
                    });
                    continue;
                }

                // Expect colon
                Expect(TokenType.Colon, "Missing colon following identifier");

                Expression value = ParseExpression();

                properties.Add(new Property()
                {
                    Key = key.Value,
                    Value = value,
                    Location = key.Location
                });

                if (At().TokenType != TokenType.CloseBrace)
                {
                    Expect(TokenType.Comma, "Expected comma or close brace following key-value pair");
                }
            }

            Expect(TokenType.CloseBrace, "Expected close brace");

            return new ObjectLiteral()
            {
                Properties = properties,
                Location = openingbrace.Location
            };
        }

        private AST.Expression ParsePrimaryExpression()
        {
            TokenType token = At().TokenType;

            switch (token)
            {
                case TokenType.Identifier:
                    Token identToken = Eat();
                    return new Identifier()
                    {
                        Symbol = identToken.Value,
                        Location = identToken.Location
                    };
                case TokenType.Number:
                    Token numberToken = Eat();
                    return new AST.NumericLiteral()
                    {
                        Value = float.Parse(numberToken.Value),
                        Location = numberToken.Location,
                        IsFloat = numberToken.Value.Contains(".")
                    };
                case TokenType.BinaryOperator:
                    if (At().Value == Operators.ArithmeticOperators["Subtract"].Symbol)
                    {
                        Eat();
                        Token numTok = Expect(TokenType.Number, "Expected number");

                        return new AST.NumericLiteral()
                        {
                            Value = -float.Parse(numTok.Value),
                            Location = numTok.Location,
                            IsFloat = numTok.Value.Contains(".")
                        };
                    }

                    throw new ParserException(new ZephyrExceptionOptions()
                    {
                        Token = At(),
                        Error = $"Unexpected token: {At().TokenType}",
                    });
                case TokenType.String:
                    Token stringToken = Eat();
                    return new StringLiteral()
                    {
                        Value = stringToken.Value,
                        Location = stringToken.Location,
                    };

                case TokenType.Function:
                    return ParseFunctionDeclaration();
                case TokenType.OpenSquare:
                    Token openSquareToken = Eat();
                    List<Expression> arr = new List<Expression>();

                    while (At().TokenType != TokenType.CloseSquare && At().TokenType != TokenType.EOF)
                    {
                        arr.Add(ParseStatement());

                        if (At().TokenType != TokenType.Comma) break;
                        else Eat();
                    }

                    Token endSquare = Expect(TokenType.CloseSquare, "Expected closing of array");
                    openSquareToken.Location = ExpandLocation(openSquareToken.Location, endSquare.Location);
                    return new ArrayLiteral()
                    {
                        Items = arr,
                        Location = openSquareToken.Location
                    };
                case TokenType.OpenBrace:
                    return ParseObjectLiteral();
                   
                case TokenType.OpenParan:
                    Eat();

                    Expression statement = (Expression)ParseStatement();
                    Expect(TokenType.CloseParan, "Expected close paren");

                    return statement;
                case TokenType.Semicolon:
                    throw new ParserException(new()
                    {
                        Token = At(),
                        Error = "Unexpected semicolon"
                    });
                default:
                    throw new ParserException(new ZephyrExceptionOptions()
                    {
                        Token = At(),
                        Error = $"Unexpected token: {At().TokenType}",
                    });
            }
        }
    }
}
