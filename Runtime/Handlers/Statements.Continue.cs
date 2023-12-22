﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZephyrNew.Parser.AST;
using ZephyrNew.Runtime.Exceptions;
using ZephyrNew.Runtime.Values;

namespace ZephyrNew.Runtime.Handlers
{
    internal partial class Statements
    {
        public static RuntimeValue EvaluateContinueStatement(ContinueStatement statement, Environment environment)
        {
            throw new ContinueException(statement.Location);
        }
    }
}
