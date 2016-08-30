using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SVGPlasma
{
    class SVGTokenStream
    {
        private System.IO.Stream charstream;
        private System.IO.StreamReader sr;
        public SVGTokenStream(System.IO.Stream s)
        {
            charstream = s;
            sr = new System.IO.StreamReader(charstream);
        }

        public SVGToken getToken()
        {
            char c = (char)sr.Read();
            switch (c)
            {
                case ' ':
                case '\t':
                case '\n':
                case '\v':
                case '\f':
                case '\r':
                    //whitespace
                    return new SVGToken(new string(c,1), TokenType.Whitespace);
                case 'M':
                case 'm':
                case 'Z':
                case 'z':
                case 'L':
                case 'l':
                case 'H':
                case 'h':
                case 'V':
                case 'v':
                case 'C':
                case 'c':
                case 'S':
                case 's':
                case 'Q':
                case 'q':
                case 'T':
                case 't':
                case 'A':
                case 'a':
                    return new SVGToken(new string(c, 1), TokenType.Command);
                case ',':
                    return new SVGToken(new string(c, 1), TokenType.Comma);
                case '+':
                case '-':
                case '0':
                case '1':
                case '2':
                case '3':
                case '4':
                case '5':
                case '6':
                case '7':
                case '8':
                case '9':
                    return ParseNumber(c);             
            }                                  
        }
        private SVGToken ParseNumber(char c)
        {


        }
    }
}
