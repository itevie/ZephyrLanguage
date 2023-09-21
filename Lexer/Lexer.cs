﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Zephyr.Lexer.Syntax;

namespace Zephyr.Lexer
{
    internal static class Lexer
    {
        private static Dictionary<string, Operator> _operators = Operators.AllOperators;
        private static Dictionary<string, TokenType> _keywords = Keywords.TheKeywords;

        private static Dictionary<string, string> _escapeCharacters = new()
        {
            { "n", "\n" }
        };

        private static bool LookAhead(string src, string what)
        {
            return src.StartsWith(what);
        }

        private static bool IsSkippable(char src)
        {
            return src == ' ' || src == '\t' || src == '\r';
        }

        /// <summary>
        /// Converts any given source code into a token list
        /// </summary>
        /// <param name="sourceCode">The source code to convert</param>
        /// <param name="fileName">The file-name the source code belongs to. Only used for locational reasons.</param>
        /// <returns>The tokenized output</returns>
        /// <exception cref="Exception"></exception>
        /// <exception cref="LexerException"></exception>
        public static List<Token> Tokenize(string sourceCode, string fileName = "")
        {
            // Start stopwatch for monitoring how long the lexing took
            Stopwatch stopwatch = new();
            stopwatch.Start();

            List<Token> tokens = new();
            char[] src = sourceCode.ToCharArray();

            int lineIdx = 0;
            int currentLine = 0;

            // Loop through all characters of source
            while (src.Length > 0)
            {
                string tokenValue = "";
                TokenType? tokenType = null;

                // Construct base location for others to add onto
                Location location = new()
                {
                    TokenStart = lineIdx,
                    Source = sourceCode,
                    Line = currentLine,
                    FileName = fileName
                };

                // For removing the first character in the source, returns the removed character
                string removeFirst()
                {
                    string value = src[0].ToString();
                    src = src.Skip(1).ToArray();
                    lineIdx++;
                    return value;
                }

                // Sets the token instead of having to set it manually
                void setToken(string val, TokenType tt)
                {
                    tokenValue = val;
                    tokenType = tt;
                }

                // Comments
                if (src.Length >= 3 &&  src[0] == '/' && src[1] == '/')
                {
                    // Repeat until new line
                    while (src.Length > 0 && src[0] != '\n')
                    {
                        removeFirst();
                    }

                    continue;
                }

                // Parenthesis
                else if (src[0] == '(')
                {
                    setToken(removeFirst(), TokenType.OpenParan);
                }
                else if (src[0] == ')')
                {
                    setToken(removeFirst(), TokenType.CloseParan);
                }

                // Braces
                else if (src[0] == '{')
                {
                    setToken(removeFirst(), TokenType.OpenBrace);
                }
                else if (src[0] == '}')
                {
                    setToken(removeFirst(), TokenType.CloseBrace);
                }

                // Square
                else if (src[0] == '[')
                {
                    setToken(removeFirst(), TokenType.OpenSquare);
                }
                else if (src[0] == ']')
                {
                    setToken(removeFirst(), TokenType.CloseSquare);
                }

                // Other important syntax
                else if (src[0] == ';')
                {
                    setToken(removeFirst(), TokenType.Semicolon);
                }
                else if (src[0] == ',')
                {
                    setToken(removeFirst(), TokenType.Comma);
                }
                else if (src[0] == '"')
                {
                    removeFirst();
                    string value = "";
                    bool isEscape = false;
                    bool wasClosed = false;

                    while (src.Length > 0)
                    {
                        // Check for illegal characters
                        if (src[0] == '\r' || src[0] == '\n')
                            throw new Exception($"Unexpected new line in string literal");

                        // Check for end quote
                        if (src[0] == '"' && isEscape == false)
                        {
                            removeFirst();
                            wasClosed = true;
                            break;
                        }

                        // Chcek for escape
                        if (src[0] == '\\' && isEscape == false)
                        {
                            removeFirst();
                            isEscape = true;
                            continue;
                        }

                        if (isEscape == true && _escapeCharacters.ContainsKey(src[0].ToString()))
                        {
                            value += _escapeCharacters[src[0].ToString()];
                            removeFirst();
                            isEscape = false;
                            continue;
                        }

                        isEscape = false;
                        value += removeFirst();
                    }

                    if (wasClosed == false)
                    {
                        throw new LexerException_new()
                        {
                            Location = location,
                            Error = $"Unterminated string"
                        };
                    }

                    setToken(value, TokenType.String);
                }

                // Literals, operators
                else
                {
                    // Check if it is an operator
                    Operator? operatorToken = null;
                    foreach (var op in _operators) {
                        if (LookAhead(string.Join("", src), op.Value.Symbol))
                        {
                            // The token was found
                            src = src.Skip(op.Value.Symbol.Length).ToArray();
                            lineIdx += op.Value.Symbol.Length;
                            operatorToken = op.Value;
                            break;
                        }
                    }

                    // Check if it was an operator
                    if (operatorToken != null)
                    {
                        setToken(operatorToken.Symbol, operatorToken.TokenType);
                    }

                    // Check for number token
                    else if (Char.IsDigit(src[0]))
                    {
                        string number = "";
                        bool floatUsed = false;
                        bool isNumberAfterFloat = false;

                        while (src.Length > 0 && (Char.IsDigit(src[0]) || src[0] == '.'))
                        {
                            if (src[0] == '.' && floatUsed == true)
                                break;

                            if (src[0] == '.')
                                floatUsed = true;

                            if (floatUsed && src[0] != '.')
                                isNumberAfterFloat = true;

                            number += removeFirst();
                        }

                        if (floatUsed == true && isNumberAfterFloat == false)
                        {
                            throw new LexerException_new()
                            {
                                Location = location,
                                Error = $"Invalid decimal"
                            };
                        }

                        setToken(number, TokenType.Number);
                    }

                    // Check or identifier
                    else if (Char.IsLetter(src[0]) || src[0] == '_' || src[0] == Operators.SpecialIdentifierPrefix)
                    {
                        string identifier = "";

                        // Check if it is special ident.
                        if (src[0] == Operators.SpecialIdentifierPrefix)
                        {
                            identifier += removeFirst();
                        }

                        while (src.Length > 0 && (Char.IsLetter(src[0]) || src[0] == '_' || Char.IsNumber(src[0])))
                        {
                            identifier += removeFirst();
                        }

                        // Check for operator keywords
                        if (Operators.LetterContainingOperators.Any(x => x.Value.Symbol == identifier))
                        {
                            KeyValuePair<string, Operator> op = Operators.LetterContainingOperators.First(x => x.Value.Symbol == identifier);
                            setToken(identifier, op.Value.TokenType);
                        }

                        // Check if it is a keyword
                        else if (_keywords.ContainsKey(identifier))
                        {
                            setToken(identifier, _keywords.GetValueOrDefault(identifier));
                        }

                        // Check if it was a type
                        else if (Keywords.Types.ContainsKey(identifier))
                        {
                            setToken(((int)Keywords.Types.GetValueOrDefault(identifier)).ToString(), TokenType.Type);
                        }

                        // Check modifier
                        else if (Keywords.Modifiers.ContainsKey(identifier))
                        {
                            setToken(((int)Keywords.Modifiers.GetValueOrDefault(identifier)).ToString(), TokenType.Modifier);
                        }

                        else
                        {
                            setToken(identifier, TokenType.Identifier);
                        }
                    }

                    // Check if skippable
                    else if (IsSkippable(src[0]))
                    {
                        removeFirst();
                        continue;
                    }

                    else if (src[0] == '\n')
                    {
                        currentLine++;
                        lineIdx = -1;
                        removeFirst();
                        continue;
                    }

                    // Unrecognised
                    else
                    {
                        throw new LexerException_new()
                        {
                            Location = location,
                            Error = $"Unrecognised character found in source: {src[0]}"
                        };
                    }
                }

                if (tokenType == null)
                    throw new LexerException(new());

                // Update location
                location.TokenEnd = lineIdx;
                tokens.Add(new Token(tokenValue, (TokenType)tokenType, location));
            }

            // Add EOF
            tokens.Add(new Token("", TokenType.EOF, new Location()
            {
                Source = sourceCode,
                TokenStart = sourceCode.Length,
                TokenEnd = sourceCode.Length,
                Line = currentLine,
                FileName = fileName
            }));

            stopwatch.Stop();
            Debug.Log($"Lexer took {stopwatch.ElapsedMilliseconds}ms");

            return tokens;
        }
    }
}
