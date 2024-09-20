using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Chess
{
    public class Board
    {
        private Piece[,] board = new Piece[8, 8];

        public Piece[,] GameBoard 
        {
            get
            {
                return board;
            }
            set
            {
                board = value;
            }
        }

        public Board() 
        {
            board[0, 0] = new Rook(aColour: 0, true, 0, 0);
            board[0, 1] = new Knight(aColour: 0, true, 1, 0);
            board[0, 2] = new Bishop(aColour: 0, true, 2, 0);
            board[0, 3] = new Queen(aColour: 0, true, 3, 0);
            board[0, 4] = new King(aColour: 0, true, 4, 0);
            board[0, 5] = new Bishop(aColour: 0, true, 5, 0);
            board[0, 6] = new Knight(aColour: 0, true, 6, 0);
            board[0, 7] = new Rook(aColour: 0, true, 7, 0);

            for (int i = 0; i < 8; i++) 
            {
                board[1, i] = new Pawn(aColour: 0, true, i, 1);
            }

            for (int i = 2; i < 6; i++) 
            { 
                for (int j = 0; j < 8; j++) 
                {
                    board[i, j] = new Piece(aColour: -1, false, j, i);
                }
            }

            for (int i = 0; i < 8; i++) 
            {
                board[6, i] = new Pawn(aColour: 1, true, i, 6);
            }

            board[7, 0] = new Rook(aColour: 1, true, 0, 7);
            board[7, 1] = new Knight(aColour: 1, true, 1, 7);
            board[7, 2] = new Bishop(aColour: 1, true, 2, 7);
            board[7, 3] = new Queen(aColour: 1, true, 3, 7);
            board[7, 4] = new King(aColour: 1, true, 4, 7);
            board[7, 5] = new Bishop(aColour: 1, true, 5, 7);
            board[7, 6] = new Knight(aColour: 1, true, 6, 7);
            board[7, 7] = new Rook(aColour: 1, true, 7, 7);
        }

        public Piece GetPiece(int x, int y)
        {
            var piece = board[y, x];

            return piece;
        }

        public void SetPieceImage(int x, int y, PictureBox img)
        {
            board[y, x].PieceDisplay = img;
        }

        public void StringBoardOutput()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Console.Write(board[i, j].ToString() + "|");
                }
                Console.WriteLine();
            }
        }
    }
}
