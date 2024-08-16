using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Chess
{
    public class Piece
    {
        private int colour; // black = 0, white = 1, none = -1
        private bool exists; 

        public int Colour
        {
            get
            {
                return colour;
            }
        }

        public bool Exists
        {
            get { return exists; }
            set { exists = value; }
        }

        public Piece(int aColour, bool aExists) 
        {
            colour = aColour;
            exists = aExists;
        }

        public virtual String ToString()
        {
            return "N ";
        }
    }
}
