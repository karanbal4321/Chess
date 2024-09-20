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
        private static int id = 0;
        private int colour; // black = 0, white = 1, none = -1
        private bool exists;
        private int x;
        private int y;
        private PictureBox pieceDisplay;
        private List<(int, int)> possibleMoves = new List<(int, int)>();

        public int X
        {
            get
            {
                return x;
            }
            set
            {
                x = value;
            }
        }

        public int Y
        {
            get
            {
                return y;
            }
            set
            {
                y = value;
            }
        }

        public int Colour
        {
            get
            {
                return colour;
            }
        }

        public int ID
        {
            get
            {
                return id;
            }
        }

        public PictureBox PieceDisplay
        {
            get
            {
                return pieceDisplay;
            }
            set
            {
                pieceDisplay = value;
            }
        }

        public bool Exists
        {
            get { return exists; }
            set { exists = value; }
        }

        public List<(int, int)> PossibleMoves
        {
            get { return possibleMoves; }

            set {  possibleMoves = value; }
        }

        public Piece(int aColour, bool aExists, int aX, int aY) 
        {
            colour = aColour;
            exists = aExists;
            x = aX;
            y = aY;

            if (exists)
            {
                id += 1;
            }
        }

        public virtual String ToString()
        {
            return "N ";
        }
    }
}
