using System.Xml.Serialization;
using ZephyrNew.Lexer.Syntax;

namespace ZephyrNew.Lexer
{
    internal class Lexer
    {
        public static Dictionary<string, string> AllSources = new Dictionary<string, string>();

        private static bool IsSkippable(char what)
        {
            return what == ' ' || what == '\t' || what == '\r';
        }

        private static bool LookAhead(string source, string what)
        {
            return source.StartsWith(what);
        }

        private static bool LookAhead(string source, string what, bool ignoreCase)
        {
            return source.StartsWith(what, StringComparison.OrdinalIgnoreCase);
        }

        public static List<Token> Tokenize(string sourceCode, string fileName)
        {
            Debug.Log($"Lexing {fileName}", LogType.Lexer);

            string id = Guid.NewGuid().ToString();
            AllSources[id] = sourceCode;

            List<Token> tokens = new List<Token>();
            char[] source = sourceCode.ToCharArray();

            // Current location
            int lineIndex = 0;
            int currentLine = 0;

            // Loop through all the characters of source
            while (source.Length != 0)
            {
                string tokenValue = "";
                TokenType? tokenType = null;
                string? stringValue = null;

                // Setup the current location
                Location location = new Location()
                {
                    TokenStart = lineIndex,
                    Line = currentLine,
                    FileName = fileName,
                    Source = id,
                };

                // Removes and returns the first character of the source
                string removeFirst()
                {
                    string value = source[0].ToString();
                    source = source.Skip(1).ToArray();
                    lineIndex++;
                    return value;
                }

                void removeMultiple(int amount)
                {
                    for (int i = 0; i != amount; i++)
                        removeFirst();
                }

                void setToken(string val, TokenType type, string? strValue = null)
                {
                    tokenValue = val;
                    tokenType = type;
                    stringValue = strValue;
                }

                // ----- Basic Character -----
                if (source.Length >= 2 && source[0] == '/' && source[1] == '/')
                {
                    // Repeat until new line
                    while (source.Length > 0 && source[0] != '\n')
                    {
                        removeFirst();
                    }

                    continue;
                }

                else if (source[0] == ';')
                    setToken(removeFirst(), TokenType.Semicolon);

                else if (IsSkippable(source[0]))
                {
                    removeFirst();
                    continue;
                }

                else if (source[0] == '\n')
                {
                    removeFirst();
                    currentLine++;
                    lineIndex = 0;
                    continue;
                }

                // ----- Literals -----

                // String literal
                else if (
                    // Check for prefixes
                    LookAhead(string.Join("", source), Prefixes.VerbatimString.Symbol + "\"", true) ||
                    source[0] == '"'
                    )
                {
                    Prefix? prefix = null;

                    // Check for verbatim
                    if (string.Join("", source).StartsWith(Prefixes.VerbatimString.Symbol, StringComparison.OrdinalIgnoreCase))
                    {
                        prefix = Prefixes.VerbatimString;

                        // Remove from source
                        for (int i = 0; i != Prefixes.VerbatimString.Symbol.Length; i++)
                            removeFirst();
                    }

                    // By now it should be "
                    if (source[0] != '"')
                    {
                        throw new LexerException($"Expected \"", location);
                    }

                    removeFirst();

                    string value = "";
                    bool wasClosed = false;
                    bool wasEscaped = false;

                    while (source.Length > 0)
                    {
                        if (wasEscaped)
                        {
                            value += removeFirst();
                            wasEscaped = false;
                            continue;
                        }

                        // Check if escaped
                        if (source[0] == '\\' && prefix != Prefixes.VerbatimString)
                        {
                            wasEscaped = true;
                            value += removeFirst();
                            continue;
                        }

                        if (source[0] == '"')
                        {
                            wasClosed = true;
                            removeFirst();
                            break;
                        }

                        if (source[0] == '\n' || source[0] == '\r')
                            if (prefix != Prefixes.VerbatimString)
                                throw new LexerException($"Cannot use new line characters in a string", location);

                        value += removeFirst();
                    }

                    if (!wasClosed)
                        throw new LexerException($"String was not closed", location);

                    // Check if should unescape
                    try
                    {
                        if (prefix != Prefixes.VerbatimString)
                            value = System.Text.RegularExpressions.Regex.Unescape(value);
                    } catch (Exception e)
                    {
                        throw new LexerException($"Failed to parse string \"{value}\": {e.Message}", location);
                    }

                    setToken(value, TokenType.String);
                }

                // Number literal (0-9)
                else if (Char.IsDigit(source[0]))
                {
                    // Stores the current number
                    string number = "";

                    // Stores if the decimal point has already been used
                    bool decimalUsed = false;

                    // Stores if the character after the decimal is a number
                    bool numberUsedAfterFloat = false;

                    while (source.Length > 0 && (Char.IsDigit(source[0]) || source[0] == '.'))
                    {
                        // If the current character is a . and a decimal is already used then that's bad
                        if (source[0] == '.' && decimalUsed == true)
                            break;

                        // Check if the current character is a decimal point
                        if (source[0] == '.')
                        {
                            // Check if it is not a hanging decimal point
                            if (source.Length != 1 && Char.IsDigit(source[1]))
                                decimalUsed = true;
                            // Illegal as something is expected after the deimal
                            else break;
                        }

                        if (decimalUsed && source[0] != '.')
                            numberUsedAfterFloat = true;

                        // Add to final number
                        number += removeFirst();
                    }

                    // Validate number
                    if (decimalUsed && !numberUsedAfterFloat)
                        throw new LexerException($"Expected a digit after decimal point", location);

                    setToken(number, TokenType.Number);
                }

                // Check for identifier
                else if (LookAhead(string.Join("", source), Prefixes.VerbatimIdentifier.Symbol) || Char.IsLetter(source[0]) || source[0] == '_')
                {
                    bool isVerbatim = false;
                    if (LookAhead(string.Join("", source), Prefixes.VerbatimIdentifier.Symbol))
                    {
                        isVerbatim = true;
                        removeMultiple(Prefixes.VerbatimIdentifier.Symbol.Length);

                        if (!(Char.IsLetter(source[0]) || source[0] == '_'))
                        {
                            throw new LexerException($"Expected identifier after verbatim", location);
                        }
                    }

                    string identifier = "";

                    // Keep reading until there is no more
                    do
                    {
                        identifier += removeFirst();
                    } while (source.Length > 0 && (Char.IsLetter(source[0]) || Char.IsDigit(source[0]) || source[0] == '_'));

                    if (isVerbatim)
                    {
                        setToken(identifier, TokenType.Identifier);
                    }

                    // Check if the identifier is an operator keyword
                    else if (Operators.OperatorsContainingLetters.Any(x => x.Value.Symbol == identifier))
                    {
                        KeyValuePair<string, Operator> oper = Operators.OperatorsContainingLetters.First(x => x.Value.Symbol == identifier);
                        setToken(identifier, oper.Value.TokenType);
                    }

                    // Check if it is a keyword
                    else if (Keywords.KeywordList.ContainsKey(identifier))
                    {
                        setToken(identifier, Keywords.KeywordList.GetValueOrDefault(identifier));
                    }

                    // Check if it is a type
                    else if (Keywords.Types.ContainsKey(identifier))
                    {
                        setToken(((int)Keywords.Types.GetValueOrDefault(identifier)).ToString(), TokenType.Type, identifier);
                    }

                    // Check if it is a modifier
                    else if (Keywords.Modifiers.ContainsKey(identifier))
                    {
                        setToken(((int)Keywords.Modifiers.GetValueOrDefault(identifier)).ToString(), TokenType.Modifier, identifier);
                    }

                    // Generic identifier
                    else
                    {
                        setToken(identifier, TokenType.Identifier);
                    }
                }

                else
                {
                    // Check if it an operator
                    Operator? operatorToken = null;
                    string joinedSource = string.Join("", source);

                    foreach (KeyValuePair<string, Operator> oper in Operators.AllOperators)
                    {
                        if (LookAhead(joinedSource, oper.Value.Symbol))
                        {
                            source = source.Skip(oper.Value.Symbol.Length).ToArray();
                            lineIndex += oper.Value.Symbol.Length;
                            operatorToken = oper.Value;
                            break;
                        }
                    }

                    // Check if an operator was found
                    if (operatorToken != null)
                    {
                        setToken(operatorToken.Symbol, operatorToken.TokenType);
                    }

                    // Unrecognised character
                    else
                    {
                        location.TokenEnd = lineIndex;
                        throw new LexerException($"Unrecognised character found in source: {source[0]}", location);
                    }
                }


                if (tokenType == null)
                    throw new LexerException($"Token type was null for unknown reason ({tokenValue})", location);

                // Finalise token
                location.TokenEnd = lineIndex;
                tokens.Add(new Token(tokenValue, (TokenType)tokenType, location, stringValue));
            }

            // Add EOF
            tokens.Add(new Token("", TokenType.EOF, new()
            {
                TokenStart = sourceCode.Length,
                TokenEnd = sourceCode.Length,
                FileName = fileName,
                Line = currentLine,
                Source = id,
            }));

            // Finish
            return tokens;
        }
    }
}
