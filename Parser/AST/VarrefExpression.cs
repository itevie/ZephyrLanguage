﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zephyr.Parser.AST
{
    internal class VarrefExpression : Expression
    {
        public Identifier Identifier = new Identifier();

        public VarrefExpression()
        {
            Kind = Kind.Varref;
        }
    }
}