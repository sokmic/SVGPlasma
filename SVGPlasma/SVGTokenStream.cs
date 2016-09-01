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
        private SVGToken tokPeek = null;

        public SVGTokenStream(System.IO.Stream s)
        {
            charstream = s;
            sr = new System.IO.StreamReader(charstream);
        }

        public SVGToken peek()
        {
            if (tokPeek == null)
                tokPeek = getToken();
            return tokPeek;
        }

        public SVGToken getToken()
        {
            if (tokPeek != null)
            {
                SVGToken t = tokPeek;
                tokPeek = null;
                return t;
            }
            if (sr.EndOfStream)
            {
                return new SVGToken("", TokenType.EOP);
            }
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
                    return fsmNumber(new string(c, 1));
                case '.':
                    return fsmFrac(new string(c, 1));
                default:
                    return new SVGToken(new string(c, 1), TokenType.Error);
            }                                  
        }        
        
        private SVGToken fsmNumber(string s)
        {
            if (sr.EndOfStream)
            {
                if (s == "+" || s == "-" || s == ".")
                    return new SVGToken(s, TokenType.Error);
                else
                    return new SVGToken(s, TokenType.Number);
            }
            char c;
            switch((char)sr.Peek())
            {
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
                    c = (char)sr.Read();
                    return fsmNumber(s + c);
                case '.':
                    if (s == ".")
                        return new SVGToken(s, TokenType.Error);
                    c = (char)sr.Read();
                    return fsmFrac(s + c);
                case 'E':
                case 'e':
                    if (s == "+" || s == "-" || s == ".")
                        return new SVGToken(s, TokenType.Error);
                    c = (char)sr.Read();
                    return fsmFloatExp(s + c);
                default:
                    if (s == "+" || s == "-" || s == ".")
                        return new SVGToken(s, TokenType.Error);
                    return new SVGToken(s, TokenType.Number);
            }                            
        }

        private SVGToken fsmFrac(string s)
        {
            if (sr.EndOfStream)
            {
                if (s == "+." || s == "-.")
                {
                    // no good
                    //put back the "." and error the sign
                    sr.BaseStream.Seek(-1, System.IO.SeekOrigin.Current);
                    return new SVGToken(s.Substring(0, 1), TokenType.Error);
                }
                else if (s == ".")
                {
                    return new SVGToken(s, TokenType.Error);
                }
                return new SVGToken(s, TokenType.Number);          
            }
            //consumed a "."
            char c;
            switch ((char)sr.Peek())
            {
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
                    c = (char)sr.Read();
                    return fsmFrac(s + c);
                case 'E':
                case 'e':
                    if (s == ".")
                    {
                        // no good                                                
                        return new SVGToken(s, TokenType.Error);
                    }
                    if (s == "+." || s == "-.")
                    {
                        // no good
                        //put back the "." and error the sign
                        sr.BaseStream.Seek(-1, System.IO.SeekOrigin.Current);
                        return new SVGToken(s.Substring(0,1), TokenType.Error);
                    }
                    c = (char)sr.Read();
                    return fsmFloatExp(s + c);
                default:
                    if (s == "+." || s == "-.")
                    {
                        // no good
                        //put back the "." and error the sign
                        sr.BaseStream.Seek(-1, System.IO.SeekOrigin.Current);
                        return new SVGToken(s.Substring(0, 1), TokenType.Error);
                    }
                    else if (s == ".")
                    {
                        return new SVGToken(s, TokenType.Error);
                    }
                    return new SVGToken(s, TokenType.Number);            
            }
        }
        private SVGToken fsmFloatExp(string s)
        {
            if (sr.EndOfStream)
            {
                //was not a float
                //put back the E
                sr.BaseStream.Seek(-1, System.IO.SeekOrigin.Current);
                return new SVGToken(s.Substring(0, s.Length - 1), TokenType.Number);
            }
            char c;
            switch ((char)sr.Peek())
            {
                case '+':
                case '-':
                    c = (char)sr.Read();
                    return fsmFloatExpDigitsReq(s + c);
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
                    c = (char)sr.Read();
                    return fsmFloatExpDigits(s + c);
                default:
                    //was not a float
                    //put back the E
                    sr.BaseStream.Seek(-1, System.IO.SeekOrigin.Current);
                    return new SVGToken(s.Substring(0, s.Length - 1), TokenType.Number);
            }
        }    
        private SVGToken fsmFloatExpDigitsReq(string s)
        {
            if (sr.EndOfStream)
            {
                //was not a float
                //put back the E and the sign
                sr.BaseStream.Seek(-2, System.IO.SeekOrigin.Current);
                return new SVGToken(s.Substring(0, s.Length - 2), TokenType.Number);
            }
            char c;
            switch ((char)sr.Peek())
            {                
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
                    c = (char)sr.Read();
                    return fsmFloatExpDigits(s + c);
                default:
                    //was not a float
                    //put back the E and the sign
                    sr.BaseStream.Seek(-2, System.IO.SeekOrigin.Current);
                    return new SVGToken(s.Substring(0, s.Length - 2), TokenType.Number);
            }
        }
        private SVGToken fsmFloatExpDigits(string s)
        {
            if (sr.EndOfStream)
            {
                return new SVGToken(s, TokenType.Number);
            }
            char c;
            switch ((char)sr.Peek())
            {
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
                    c = (char)sr.Read();
                    return fsmFloatExpDigits(s + c);
                default:             
                    return new SVGToken(s, TokenType.Number);
            }
        }
    }
}
