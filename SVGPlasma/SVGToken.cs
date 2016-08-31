using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SVGPlasma
{
    public enum TokenType
    {
        Whitespace,
        Command,
        Number,
        Comma,        
        Error,
        EOP
    }

    class SVGToken
    {
        public string value { get; set; }
        public TokenType tokType { get; set; }

        public SVGToken(string val, TokenType tok)
        {
            value = val;
            tokType = tok;
        }

    }
}
