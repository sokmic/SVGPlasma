using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SVGPlasma
{
    class Coordinate : IEquatable<Coordinate>
    {
        public Coordinate(decimal x, decimal y)
        {
            X = x;
            Y = y;
        }
            
        public decimal X { get; set; }
        public decimal Y { get; set; }

        public bool Equals(Coordinate other)
        {
            return X == other.X && Y == other.Y;
        }

        public override bool Equals(object obj)
        {
            return Equals((Coordinate)obj);
        }

        public override int GetHashCode()
        {
            return Tuple.Create(X, Y).GetHashCode();
        }

        public static Coordinate operator +(Coordinate a, Coordinate b)
        {
            return new Coordinate(a.X + b.X, a.Y + b.Y);
        }        
        public static bool operator ==(Coordinate a, Coordinate b)
        {
            return a.Equals(b);
        }
        public static bool operator !=(Coordinate a, Coordinate b)
        {
            return a.X != b.X || a.Y != b.Y;
        }
    }
}
