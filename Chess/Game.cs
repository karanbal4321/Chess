using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;

namespace Chess
{
    public class Game
    {
        private Board gameState;
        private bool whiteTurn = true;

        public bool WhiteTurn
        {
            get
            {
                return whiteTurn;
            }
        }

        public Board GameState
        {
            get
            {
                return gameState;
            }
        }

        public Game() 
        {
            gameState = new Board();
        }

        public bool validTurnForPiece(Piece piece)
        {
            if (piece.Colour == 0 && whiteTurn)
            {
                return false;
            }

            if (piece.Colour == 1 && !whiteTurn)
            {
                return false;
            }

            return true;
        }
        
        public void FindEntireBoardMoves()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    var piece = gameState.GameBoard[i, j]; 

                    if (piece.Exists)
                    {
                        piece.PossibleMoves = FindPieceMove(piece);
                    }
                }
            }
        }

        public int FindCheckResponseMoves(int colour)
        {
            int oppositeColour = -1;
            int winner = -1;
            bool winnerExists = true;

            if (colour == 0)
            {
                oppositeColour = 1;
            }
            else if (colour == 1)
            {
                oppositeColour = 0;
            }

            var newPieceMoves = new Dictionary<Piece, List<(int, int)>>();

            // For every piece on the check side, try the available move for the piece and check whether it is still an 
            // available move. Keep track of new available moves for piece and update it.

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    var piece = gameState.GameBoard[i, j];

                    Console.WriteLine("Considering piece " + piece.GetType());

                    if (piece.Colour == colour)
                    {
                        var availableMoves = piece.PossibleMoves;
                        var updatedAvailableMoves = new List<(int, int)>();

                        foreach ((int y, int x) move in availableMoves)
                        {
                            Console.WriteLine("Moving piece at (" + i.ToString() + ", " + j.ToString() + ") to (" + move.y.ToString() + ", " + move.x.ToString() + ")");
                            // Move piece and check whether it protects the king
                            gameState.GameBoard[i, j] = new Piece(-1, false, j, i);
                            piece.X = move.x;
                            piece.Y = move.y;

                            var pieceTaken = gameState.GameBoard[move.y, move.x];
                            gameState.GameBoard[move.y, move.x] = piece;

                            gameState.StringBoardOutput();

                            FindEntireBoardMoves();

                            var newMoveInCheck = isInCheck(oppositeColour);

                            Console.WriteLine("Is still in check?");
                            Console.WriteLine(newMoveInCheck);

                            if (!newMoveInCheck)
                            {
                                updatedAvailableMoves.Add((move.y, move.x));
                            }

                            newPieceMoves[piece] = updatedAvailableMoves;

                            // Move piece back to prepare for next move
                            piece.X = j;
                            piece.Y = i;
                            gameState.GameBoard[i, j] = piece;

                            if (pieceTaken.Exists)
                            {
                                gameState.GameBoard[move.y, move.x] = pieceTaken;
                            }
                            else
                            {
                                gameState.GameBoard[move.y, move.x] = new Piece(-1, false, move.x, move.y);
                            }

                            FindEntireBoardMoves();
                        }

                        if (updatedAvailableMoves.Count > 0)
                        {
                            winnerExists = false;
                        }
                    }
                }
            }

            Console.WriteLine("Is Board The Same?");
            gameState.StringBoardOutput();

            for (int x = 0; x < newPieceMoves.Count; x++)
            {
                var piece = newPieceMoves.Keys.ElementAt(x);
                var possibleMoves = newPieceMoves[piece];

                piece.PossibleMoves = possibleMoves;
            }

            if (winnerExists)
            {
                winner = oppositeColour;
            }

            return winner;
        }

        public void PrintBoard(Piece[,] board)
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

        public List<(int, int)> FindPieceMove(Piece piece)
        {
            var coordinates = new List<(int, int)>();

            if (piece is Pawn)
            {
                if (piece.Colour == 0)
                {
                    coordinates = getCoordinatesForBlackPawn(piece);
                }

                if (piece.Colour == 1)
                {
                    coordinates = getCoordinatesForWhitePawn(piece);
                }
            }
            else if (piece is Rook)
            {
                coordinates = getCoordinatesForRook(piece);
            }
            else if (piece is Queen)
            {
                coordinates = getCoordinatesForQueen(piece);
            }
            else if (piece is Bishop)
            {
                coordinates = getCoordinatesForBishop(piece);
            }
            else if (piece is King)
            {
                coordinates = getCoordinatesForKing(piece);
            }
            else if (piece is Knight)
            {
                coordinates = getCoordinatesForKnight(piece);
            }

            return coordinates;
        }
        
        public List<(int, int)> getCoordinatesForBlackPawn(Piece piece)
        {
            var coordinates = new List<(int, int)>();

            // forward move
            bool canMoveForward = true;

            if (piece.Y == 7)
            {
                canMoveForward = false;
            }
            else
            {
                var pieceBelow = gameState.GameBoard[piece.Y + 1, piece.X];

                if (pieceBelow.Exists)
                {
                    canMoveForward = false;
                }
                else
                {
                    coordinates.Add((piece.Y + 1, piece.X));
                }
            }

            // special spawn case
            if (piece.Y == 1)
            {
                var twoPieceBelow = gameState.GameBoard[piece.Y + 2, piece.X];

                if (twoPieceBelow.Exists)
                {
                    canMoveForward = false;
                }

                if (canMoveForward)
                {
                    coordinates.Add((piece.Y + 2, piece.X));
                }
            }

            if (piece.Y < 7)
            {
                // take piece diagonal
                if (piece.X > 0)
                {
                    var pieceDownLeft = gameState.GameBoard[piece.Y + 1, piece.X - 1];

                    if (pieceDownLeft.Exists && pieceDownLeft.Colour == 1)
                    {
                        coordinates.Add((piece.Y + 1, piece.X - 1));
                    }
                }

                if (piece.X < 7)
                {
                    var pieceDownRight = gameState.GameBoard[piece.Y + 1, piece.X + 1];

                    if (pieceDownRight.Exists && pieceDownRight.Colour == 1)
                    {
                        coordinates.Add((piece.Y + 1, piece.X + 1));
                    }
                }
            }

            return coordinates;
        }

        public List<(int, int)> getCoordinatesForWhitePawn(Piece piece)
        {
            // debug
            Console.WriteLine("Internal Game State");
            gameState.StringBoardOutput();

            var coordinates = new List<(int, int)>();

            // forward move
            bool canMoveForward = true;

            if (piece.Y == 0)
            {
                canMoveForward = false;
            }
            else
            {
                var pieceAbove = gameState.GameBoard[piece.Y - 1, piece.X];

                if (pieceAbove.Exists)
                {
                    canMoveForward = false;
                }
                else
                {
                    coordinates.Add((piece.Y - 1, piece.X));
                }
            }

            // special spawn case
            if (piece.Y == 6)
            {
                var twoPieceAbove = gameState.GameBoard[piece.Y - 2, piece.X];

                if (twoPieceAbove.Exists)
                {
                    canMoveForward = false;
                }

                if (canMoveForward)
                {
                    coordinates.Add((piece.Y - 2, piece.X));
                }
            }

            if (piece.Y > 0)
            {
                // take piece diagonal
                if (piece.X > 0)
                {
                    var pieceUpLeft = gameState.GameBoard[piece.Y - 1, piece.X - 1];

                    if (pieceUpLeft.Exists && pieceUpLeft.Colour == 0)
                    {
                        coordinates.Add((piece.Y - 1, piece.X - 1));
                    }
                }

                if (piece.X < 7)
                {
                    var pieceUpRight = gameState.GameBoard[piece.Y - 1, piece.X + 1];

                    if (pieceUpRight.Exists && pieceUpRight.Colour == 0)
                    {
                        coordinates.Add((piece.Y - 1, piece.X + 1));
                    }
                }
            }
            
            return coordinates;
        }

        public List<(int, int)> getCoordinatesForQueen(Piece piece)
        {
            var rookCoords = getCoordinatesForRook(piece);

            var bishopCoords = getCoordinatesForBishop(piece);

            List<(int, int)> combinedCoords = new List<(int, int)>();

            foreach (var rookC in rookCoords)
            {
                combinedCoords.Add(rookC);
            }

            foreach (var bishopC in bishopCoords)
            {
                combinedCoords.Add(bishopC); 
            }

            return combinedCoords;
        }

        public List<(int, int)> getCoordinatesForRook(Piece piece)
        {
            List<(int, int)> availableCoordinates = new List<(int, int)> ();
            int oppositeColour = piece.Colour == 0 ? 1 : 0;

            for (int j = piece.X + 1; j < 8; j++) 
            {
                var currentPiece = gameState.GameBoard[piece.Y, j];

                if (!currentPiece.Exists) 
                {
                    availableCoordinates.Add((piece.Y, j));
                }
                else
                {
                    if (currentPiece.Colour == oppositeColour)
                    {
                        availableCoordinates.Add((piece.Y, j));
                    }

                    break;
                }
            }

            for (int j = piece.X - 1; j >= 0; j--)
            {
                var currentPiece = gameState.GameBoard[piece.Y, j];

                if (!currentPiece.Exists)
                {
                    availableCoordinates.Add((piece.Y, j));
                }
                else
                {
                    if (currentPiece.Colour == oppositeColour)
                    {
                        availableCoordinates.Add((piece.Y, j));
                    }

                    break;
                }
            }

            for (int i = piece.Y - 1; i >= 0; i--)
            {
                var currentPiece = gameState.GameBoard[i, piece.X];

                if (!currentPiece.Exists)
                {
                    availableCoordinates.Add((i, piece.X));
                }
                else
                {
                    if (currentPiece.Colour == oppositeColour)
                    {
                        availableCoordinates.Add((i, piece.X));
                    }

                    break;
                }

            }

            for (int i = piece.Y + 1; i < 8; i++)
            {
                var currentPiece = gameState.GameBoard[i, piece.X];

                if (!currentPiece.Exists)
                {
                    availableCoordinates.Add((i, piece.X));
                }
                else
                {
                    if (currentPiece.Colour == oppositeColour)
                    {
                        availableCoordinates.Add((i, piece.X));
                    }

                    break;
                }
            }

            return availableCoordinates;
        }

        public List<(int, int)> getCoordinatesForBishop(Piece piece)
        {
            List<(int, int)> availableCoords = new List<(int, int)>();
            int oppositeColour = piece.Colour == 1 ? 0 : 1;

            // Up right
            int j = piece.X + 1;
            for (int i = piece.Y - 1; i >= 0; i--)
            {
                if (j > 7) break;

                var currentPiece = gameState.GameBoard[i, j];

                if (!currentPiece.Exists || currentPiece.Colour == oppositeColour)
                {
                    availableCoords.Add((i, j));

                    if (currentPiece.Colour == oppositeColour) break;
                }
                else
                {
                    break;
                }
                j++;
            }

            // Up left
            j = piece.X - 1;
            for (int i = piece.Y - 1; i >= 0; i--)
            {
                if (j < 0) break;

                var currentPiece = gameState.GameBoard[i, j];

                if (!currentPiece.Exists || currentPiece.Colour == oppositeColour)
                {
                    availableCoords.Add((i, j));

                    if (currentPiece.Colour == oppositeColour) break;
                }
                else
                {
                    break;
                }

                j--;
            }

            // Down right
            int curr_i = piece.Y + 1;

            for (int curr_x = piece.X + 1; curr_x < 8; curr_x++)
            {
                if (curr_i > 7) break;

                var currentPiece = gameState.GameBoard[curr_i, curr_x];

                if (!currentPiece.Exists || currentPiece.Colour == oppositeColour)
                {
                    availableCoords.Add((curr_i, curr_x));

                    if (currentPiece.Colour == oppositeColour) break;
                }
                else
                {
                    break;
                }

                curr_i++;
            }

            // Down left
            curr_i = piece.Y + 1;

            for (int curr_x = piece.X - 1; curr_x >= 0; curr_x--)
            {
                if (curr_i > 7) break;

                var currentPiece = gameState.GameBoard[curr_i, curr_x];

                if (!currentPiece.Exists || currentPiece.Colour == oppositeColour)
                {
                    availableCoords.Add((curr_i, curr_x));

                    if (currentPiece.Colour == oppositeColour) break;
                }
                else
                {
                    break;
                }

                curr_i++;
            }

            return availableCoords;
        }

        public List<(int, int)> getCoordinatesForKing(Piece piece)
        {
            var availableCoords = new List<(int, int)>();
            var possibleCoords = new List<(int, int)>();
            int oppositeColour = piece.Colour == 1 ? 0 : 1;

            possibleCoords.Add((piece.Y - 1, piece.X - 1));
            possibleCoords.Add((piece.Y - 1, piece.X));
            possibleCoords.Add((piece.Y - 1, piece.X + 1));

            possibleCoords.Add((piece.Y, piece.X - 1));
            possibleCoords.Add((piece.Y, piece.X));
            possibleCoords.Add((piece.Y, piece.X + 1));

            possibleCoords.Add((piece.Y + 1, piece.X - 1));
            possibleCoords.Add((piece.Y + 1, piece.X));
            possibleCoords.Add((piece.Y + 1, piece.X + 1));

            foreach (var coord in possibleCoords)
            {
                int y = coord.Item1;
                int x = coord.Item2;

                if (y >= 0 && y < 8 && x >= 0 && x < 8)
                {
                    var currentPiece = gameState.GameBoard[y, x];

                    if (!currentPiece.Exists)
                    {
                        availableCoords.Add((y, x));
                    }
                    else if (currentPiece.Colour == oppositeColour)
                    {
                        availableCoords.Add((y, x));
                    }
                }
            }

            return availableCoords;
        }

        public List<(int, int)> getCoordinatesForKnight(Piece piece)
        {
            var availableCoordinates = new List<(int, int)>();
            var possibleCoordinates = new List<(int, int)>();

            possibleCoordinates.Add((piece.Y - 1, piece.X - 2));
            possibleCoordinates.Add((piece.Y - 1, piece.X + 2));
            possibleCoordinates.Add((piece.Y + 1, piece.X - 2));
            possibleCoordinates.Add((piece.Y + 1, piece.X + 2));
            possibleCoordinates.Add((piece.Y - 2, piece.X - 1));
            possibleCoordinates.Add((piece.Y - 2, piece.X + 1));
            possibleCoordinates.Add((piece.Y + 2, piece.X - 1));
            possibleCoordinates.Add((piece.Y + 2, piece.X + 1));

            int oppositeColour = piece.Colour == 1 ? 0 : 1;

            foreach ((int y, int x) in possibleCoordinates)
            {
                if (x >= 0 && x <= 7 && y >= 0 && y <= 7)
                {
                    var currentPiece = gameState.GameBoard[y, x];

                    if (!currentPiece.Exists)
                    {
                        availableCoordinates.Add((y, x));
                    }
                    else if (currentPiece.Colour == oppositeColour)
                    {
                        availableCoordinates.Add((y, x));
                    }
                }
            }

            return availableCoordinates;
        }
        
        public int movePiece(Piece clickedPiece, Piece capturedPiece)
        {
            gameState.GameBoard[clickedPiece.Y, clickedPiece.X] = new Piece(aColour: -1, false, clickedPiece.X, clickedPiece.Y);

            clickedPiece.X = capturedPiece.X;
            clickedPiece.Y = capturedPiece.Y;

            if (capturedPiece.Y == 0 && clickedPiece is Pawn)
            {
                var whiteQueenAsPawn = new Queen(aColour: 1, true, capturedPiece.X, capturedPiece.Y);
                gameState.GameBoard[clickedPiece.Y, clickedPiece.X] = whiteQueenAsPawn;
                whiteQueenAsPawn.PossibleMoves = FindPieceMove(whiteQueenAsPawn);
            }
            else if (capturedPiece.Y == 7 && clickedPiece is Pawn)
            {
                var blackQueenAsPawn = new Queen(aColour: 0, true, capturedPiece.X, capturedPiece.Y);
                gameState.GameBoard[clickedPiece.Y, clickedPiece.X] = blackQueenAsPawn;
                blackQueenAsPawn.PossibleMoves = FindPieceMove(blackQueenAsPawn);
            }
            else
            {
                gameState.GameBoard[clickedPiece.Y, clickedPiece.X] = clickedPiece;
            }

            FindEntireBoardMoves();

            int responseColour = -1;

            if (clickedPiece.Colour == 1)
            {
                responseColour = 0;
            }
            else if (clickedPiece.Colour == 0)
            {
                responseColour = 1;
            }

            var winner = FindCheckResponseMoves(responseColour);

            whiteTurn = !whiteTurn;

            return winner;
        }

        public bool isInCheck(int colour)
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    var piece = gameState.GameBoard[i, j];

                    if (piece.Colour == colour)
                    {
                        var possibleMoves = piece.PossibleMoves;

                        foreach ((int y, int x) move in possibleMoves)
                        {
                            var possibleCapture = gameState.GameBoard[move.y, move.x];

                            if (possibleCapture is King)
                            {
                                return true;
                            }
                        }
                    }
                }
            }

            return false;
        }
    }
}
