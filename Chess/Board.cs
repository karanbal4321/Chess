using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            board[0, 0] = new Rook(aColour: 0, true);
            board[0, 1] = new Knight(aColour: 0, true);
            board[0, 2] = new Bishop(aColour: 0, true);
            board[0, 3] = new Queen(aColour: 0, true);
            board[0, 4] = new King(aColour: 0, true);
            board[0, 5] = new Bishop(aColour: 0, true);
            board[0, 6] = new Knight(aColour: 0, true);
            board[0, 7] = new Rook(aColour: 0, true);

            for (int i = 0; i < 8; i++) 
            {
                board[1, i] = new Pawn(aColour: 0, true);
            }

            for (int i = 2; i < 6; i++) 
            { 
                for (int j = 0; j < 8; j++) 
                {
                    board[i, j] = new Piece(aColour: -1, false);
                }
            }

            for (int i = 0; i < 8; i++) 
            {
                board[6, i] = new Pawn(aColour: 1, true);
            }

            board[7, 0] = new Rook(aColour: 1, true);
            board[7, 1] = new Knight(aColour: 1, true);
            board[7, 2] = new Bishop(aColour: 1, true);
            board[7, 3] = new Queen(aColour: 1, true);
            board[7, 4] = new King(aColour: 1, true);
            board[7, 5] = new Bishop(aColour: 1, true);
            board[7, 6] = new Knight(aColour: 1, true);
            board[7, 7] = new Rook(aColour: 1, true);
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
