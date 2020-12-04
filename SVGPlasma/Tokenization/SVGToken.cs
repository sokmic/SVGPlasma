using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SVGPlasma
{
    public enum TokenType
    {        
        Command,
        Number,     
        Error,
        EOP
    }

    class SVGToken : IDisposable
    {
        public string value { get; set; }
        public TokenType tokType { get; set; }

        public SVGToken(string val, TokenType tok)
        {
            value = val;
            tokType = tok;
        }

        public void Dispose()
        {
        }
    }
}
