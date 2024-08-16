using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    public class Queen : Piece
    {
        public Queen(int aColour, bool aExists) : base(aColour, aExists)
        {

        }

        public override String ToString()
        {
            return "Q ";
        }
    }
}
