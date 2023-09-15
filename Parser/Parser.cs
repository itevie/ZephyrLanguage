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
    /// <summary>
    /// Parses given lexed tokens into an AST.
    /// </summary>
    internal class Parser
    {
        private List<Token> tokens = new();

        /// <summary>
        /// Checks if the current token is an End Of File token
        /// </summary>
        /// <returns>Whether or not the current token is an EOF</returns>
        private bool NotEOF()
        {
            return tokens[0].TokenType != TokenType.EOF;
        }

        /// <summary>
        /// Returns the current token
        /// </summary>
        /// <returns>The current token</returns>
        private Token At()
        {
            return tokens[0];
        }


        /// <summary>
        /// Consumes the current token, removes it and returns it
        /// </summary>
        /// <returns>Returns the current token</returns>
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

        /// <summary>
        /// Used to check whether or not the expression requires a semi colon after it
        /// </summary>
        /// <param name="value">The value to check</param>
        /// <returns>Yes or no</returns>
        private static bool NeedSemiColon(Expression value)
        {
            if (value.Kind == Kind.FunctionDeclaration || value.Kind == Kind.ObjectLiteral || 
                value.Kind == Kind.IfStatement || value.Kind == Kind.WhileStatement ||
                value.Kind == Kind.ForEachStatement || value.Kind == Kind.TryStatement)
                return false;
            return true;
        }


        /// <summary>
        /// The base function for pasing the source code
        /// </summary>
        /// <param name="sourceCode">The source code as a string</param>
        /// <param name="fileName">The file-name for locational reasons</param>
        /// <returns>The parsed AST tree</returns>
        public AST.Program ProduceAST(string sourceCode, string fileName)
        {
            tokens = Lexer.Lexer.Tokenize(sourceCode, fileName);

            AST.Program program = new();

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

        /// <summary>
        /// These are statements which MUST be on their own and cannot be used in any sort of way as a value
        /// </summary>
        /// <returns></returns>
        private Expression ParseControlFlowStatement()
        {
            return At().TokenType switch
            {
                TokenType.Let => ParseVariableDeclaration(),
                TokenType.Event => ParseEventStatement(),
                TokenType.Const => ParseVariableDeclaration(),
                TokenType.If => ParseIfStatement(),
                TokenType.While => ParseWhileStatement(),
                TokenType.For => ParseForStatement(),
                TokenType.Return => ParseReturnStatement(),
                TokenType.Try => ParseTryStatement(),
                TokenType.Struct => ParseStructStatement(),
                TokenType.Import => ParseImportStatement(),
                TokenType.Export => ParseExportStatement(),
                // Keywords with no extra info
                TokenType.Break => new BreakStatement()
                {
                    Location = Eat().Location
                },
                _ => ParseStatement(),
            };
        }

        /// <summary>
        /// These are statements, but CAN be used as a value (in PrimaryExpression)
        /// </summary>
        /// <returns></returns>
        private Expression ParseStatement()
        {
            return At().TokenType switch
            {
                TokenType.Function => ParseFunctionDeclaration(),
                _ => ParseExpression(),
            };
        }

        /*
         * STATEMENTS - CONTROL FLOW
         */
        private Expression ParseStructStatement()
        {
            Token structToken = Eat();

            // Identifier name for struct
            Token structName = Expect(TokenType.Identifier, "Expected struct name");

            Expect(TokenType.OpenBrace, "Expected opening of struct body");

            StructStatement structStatement = new()
            {
                Name = structName.Value,
                Location = structToken.Location
            };

            // Expect var dec. or func dec.
            while (At().TokenType != TokenType.CloseBrace)
            {
                Expression expr = ParseControlFlowStatement();
                Console.WriteLine(expr.Kind);

                // Check type
                if (expr.Kind != Kind.VariableDeclaration && expr.Kind != Kind.FunctionDeclaration)
                {
                    throw new ParserException(new()
                    {
                        Location = expr.Location,
                        Error = $"Expected variable declaration or function declaration"
                    });
                }

                Expect(TokenType.Semicolon, "Expected semi colon");

                structStatement.Properties.Add(expr);
            }

            Eat();

            return structStatement;
        }

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

        /// <summary>
        /// Syntax:
        ///     - export [value] as [identifier];
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ParserException"></exception>
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

        /// <summary>
        /// Syntax:
        ///     - event [type] [identifier];
        /// </summary>
        /// <returns></returns>
        private Expression ParseEventStatement()
        {
            Token eventToken = Eat();

            TypeIdentifierCombo combo = ParseName();

            return new EventDeclarationStatement()
            {
                Location = eventToken.Location,
                Identifier = combo.Identifier,
                Type = new TypeExpression()
                {
                    Type = combo.Type,
                    IsNullable = combo.isNullable
                }
            };
        }

        /// <summary>
        /// Syntax:
        ///     - [modifiers?] var|const [identifier];
        ///     - [modifiers?] var|const [identifier] = [value];
        ///     - [modifiers?] var|const [type]? [identifier];
        ///     - [modifiers?] var|const [type][?] [identifier] = [value];
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ParserException"></exception>
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

            Expression body = new();

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

            // Parse parameters
            Expect(TokenType.OpenParan);

            List<TypeIdentifierCombo> typeIdentifiers = new();

            while (At().TokenType != TokenType.CloseParan)
            {
                TypeIdentifierCombo combo = ParseName();
                typeIdentifiers.Add(combo);
            }

            Expect(TokenType.CloseParan);

            List<Expression> parameters = new();

            foreach (TypeIdentifierCombo paran in typeIdentifiers)
            {
                parameters.Add(new Identifier()
                {
                    Symbol = paran.Identifier.Symbol
                });
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
            List<Expression> body = new();

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

        /// <summary>
        /// Parses a type + name combo, e.g. int[]? a
        /// </summary>
        /// <returns></returns>
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

            Identifier ident = new()
            {
                Location = identifier.Location,
                Symbol = identifier.Value,
            };

            return new TypeIdentifierCombo(ident, type, isNullable);
        }

        /// <summary>
        /// Expands a location by taking the start of a and the end of b and combining into one
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        private static Location? ExpandLocation(Location? a, Location? b)
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
        private Expression ParseExpression()
        {
            return ParseAssignmentExpression();
        }

        private Expression ParseAssignmentExpression()
        {
            Expression left = ParserTernaryExpression();

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

        private Expression ParseLogicalExpression()
        {
            Expression left = ParseComparisonExpression();

            // Check if next is logical
            if (At().TokenType == TokenType.LogicalOperator)
            {
                string op = Eat().Value;
                Expression right = ParseLogicalExpression();

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

        private Expression ParseComparisonExpression()
        {
            Expression left = ParseAdditiveExpression();

            // Check if next is comparison
            if (At().TokenType == TokenType.ComparisonOperator)
            {
                Token operatorToken = Eat();
                string op = operatorToken.Value;

                Expression right = At().TokenType == TokenType.ComparisonOperator
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

        private Expression ParseAdditiveExpression()
        {
            Expression left = ParseMultiplicativeExpression();

            while (
                At().Value == Operators.ArithmeticOperators["Plus"].Symbol ||
                At().Value == Operators.ArithmeticOperators["Subtract"].Symbol
            )
            {
                Token operatorToken = Eat();
                string op = operatorToken.Value;
                Expression right = ParseMultiplicativeExpression();

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

        private Expression ParseMultiplicativeExpression()
        {
            Expression left = ParseCastExpression();

            while (
                At().Value == Operators.ArithmeticOperators["Multiply"].Symbol ||
                At().Value == Operators.ArithmeticOperators["Divide"].Symbol ||
                At().Value == Operators.ArithmeticOperators["Modulus"].Symbol ||
                At().Value == Operators.ArithmeticOperators["Power"].Symbol ||
                At().Value == Operators.BinaryOperators["Coalesence"].Symbol
            )
            {
                Token operatorToken = Eat();
                string op = operatorToken.Value;
                Expression right = ParseCastExpression();

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

                Expression right = new();
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

        private Expression ParseUnaryExpression()
        {
            // Check if current is unary
            if (At().TokenType == TokenType.UnaryOperator)
            {
                Token operatorToken = Eat();

                // Get the righ hand side
                Expression right = At().TokenType == TokenType.UnaryOperator
                    ? ParseUnaryExpression()
                    : ParseMemberExpression();

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

        private Expression ParseCallMemberExpression(bool noStart = false)
        {
            Expression member = ParseMemberExpression(noStart);

            if (At().TokenType == TokenType.OpenParan)
            {
                return ParseCallExpression(member);
            }

            return member;
        }

        private Expression ParseMemberExpression(bool skipObj = false)
        {
            Expression obj = skipObj == false ? ParsePrimaryExpression() : new();

            Location? fullLocation = obj.Location;

            while (At().TokenType == TokenType.Dot || At().TokenType == TokenType.OpenSquare)
            {
                Token op = Eat();
                Expression property;
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

        private Expression ParseCallExpression(Expression caller)
        {
            CallExpression callExpression = new()
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

        private List<Expression> ParseArguments()
        {
            Expect(TokenType.OpenParan, $"Expected open parenthesis");

            List<Expression> args = At().TokenType == TokenType.CloseParan
                ? new()
                : ParseArgumentsList();

            Expect(TokenType.CloseParan, $"Expected close parenthesis");

            return args;
        }

        private List<Expression> ParseArgumentsList()
        {
            List<Expression> args = new()
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

        private Expression ParseObjectLiteral()
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

        /// <summary>
        /// Used for parsing general values, like literals, functions etc.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ParserException"></exception>
        private Expression ParsePrimaryExpression()
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
                // Literals - any number
                case TokenType.Number:
                    Token numberToken = Eat();
                    return new NumericLiteral()
                    {
                        Value = double.Parse(numberToken.Value),
                        Location = numberToken.Location,
                        IsFloat = numberToken.Value.Contains('.')
                    };
                // Literals - any negative number
                case TokenType.BinaryOperator:
                    if (At().Value == Operators.ArithmeticOperators["Subtract"].Symbol)
                    {
                        Eat();
                        Token numTok = Expect(TokenType.Number, "Expected number");

                        return new NumericLiteral()
                        {
                            Value = -double.Parse(numTok.Value),
                            Location = numTok.Location,
                            IsFloat = numTok.Value.Contains('.')
                        };
                    }

                    throw new ParserException(new ZephyrExceptionOptions()
                    {
                        Token = At(),
                        Error = $"Unexpected token: {At().TokenType}",
                    });
                // Literals - string
                case TokenType.String:
                    Token stringToken = Eat();
                    return new StringLiteral()
                    {
                        Value = stringToken.Value,
                        Location = stringToken.Location,
                    };

                // Literals - functions
                case TokenType.Function:
                    return ParseFunctionDeclaration();
                // Literals - arrays
                case TokenType.OpenSquare:
                    Token openSquareToken = Eat();
                    List<Expression> arr = new();

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
                // Literals - objects
                case TokenType.OpenBrace:
                    return ParseObjectLiteral();
                // Parameters, 1 - ( 2 + 2 ) etc.
                case TokenType.OpenParan:
                    Eat();

                    Expression statement = (Expression)ParseStatement();
                    Expect(TokenType.CloseParan, "Expected close paren");

                    return statement;
                // Variable reference
                case TokenType.Varref:
                    Token varrefToken = Eat();

                    // Expect ident.
                    Token varrefIdentifier = Expect(TokenType.Identifier, "Expected variable name");

                    VarrefExpression varrefExpr = new()
                    {
                        Identifier = new Identifier()
                        {
                            Symbol = varrefIdentifier.Value,
                            Location = varrefIdentifier.Location,
                        },
                        Location = varrefToken.Location
                    };

                    return varrefExpr;
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
