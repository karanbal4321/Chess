using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
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

        public Game() 
        {
            gameState = new Board();
        }

        public bool validTurnForPiece((int y, int x) piecePosition)
        {
            var piece = gameState.GameBoard[piecePosition.y, piecePosition.x];

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

        public List<(int, int)> getPossibleMovesForPiece((int y, int x) piecePosition) 
        {
            var piece = gameState.GameBoard[piecePosition.y, piecePosition.x];
            
            List<(int, int)> coordinates = new List<(int, int)>();

            if (piece is Pawn) 
            {
                if (piece.Colour == 0)
                {
                    coordinates = getCoordinatesForBlackPawn(piecePosition);
                }

                if (piece.Colour == 1)
                {
                    coordinates = getCoordinatesForWhitePawn(piecePosition);
                }
            }
            else if (piece is Rook)
            {
                coordinates = getCoordinatesForRook(piece, piecePosition);
            }
            else if (piece is Queen)
            {
                coordinates = getCoordinatesForQueen(piece, piecePosition);
            }
            else if (piece is Bishop)
            {
                coordinates = getCoordinatesForBishop(piece, piecePosition);
            }
            else if (piece is King)
            {
                coordinates = getCoordinatesForKing(piece, piecePosition);
            }
            else if (piece is Knight)
            {
                coordinates = getCoordinatesForKnight(piece, piecePosition);
            }

            return coordinates;
        }

        public List<(int, int)> getCoordinatesForBlackPawn((int y, int x) piecePosition)
        {
            var coordinates = new List<(int, int)>();

            // forward move
            bool canMoveForward = true;

            if (piecePosition.y == 7)
            {
                canMoveForward = false;
            }
            else
            {
                var pieceBelow = gameState.GameBoard[piecePosition.y + 1, piecePosition.x];

                if (pieceBelow.Exists)
                {
                    canMoveForward = false;
                }
                else
                {
                    coordinates.Add((piecePosition.y + 1, piecePosition.x));
                }
            }

            // special spawn case
            if (piecePosition.y == 1)
            {
                var twoPieceBelow = gameState.GameBoard[piecePosition.y + 2, piecePosition.x];

                if (twoPieceBelow.Exists)
                {
                    canMoveForward = false;
                }

                if (canMoveForward)
                {
                    coordinates.Add((piecePosition.y + 2, piecePosition.x));
                }
            }

            // take piece diagonal
            if (piecePosition.x > 0)
            {
                var pieceDownLeft = gameState.GameBoard[piecePosition.y + 1, piecePosition.x - 1];

                if (pieceDownLeft.Exists && pieceDownLeft.Colour == 1)
                {
                    coordinates.Add((piecePosition.y + 1, piecePosition.x - 1));
                }
            }

            if (piecePosition.x < 7)
            {
                var pieceDownRight = gameState.GameBoard[piecePosition.y + 1, piecePosition.x + 1];

                if (pieceDownRight.Exists && pieceDownRight.Colour == 1)
                {
                    coordinates.Add((piecePosition.y + 1, piecePosition.x + 1));
                }
            }

            return coordinates;
        }

        public List<(int, int)> getCoordinatesForWhitePawn((int y, int x) piecePosition)
        {
            var coordinates = new List<(int, int)>();

            // forward move
            bool canMoveForward = true;

            if (piecePosition.y == 0)
            {
                canMoveForward = false;
            }
            else
            {
                var pieceAbove = gameState.GameBoard[piecePosition.y - 1, piecePosition.x];

                if (pieceAbove.Exists)
                {
                    canMoveForward = false;
                }
                else
                {
                    coordinates.Add((piecePosition.y - 1, piecePosition.x));
                }
            }

            // special spawn case
            if (piecePosition.y == 6)
            {
                var twoPieceAbove = gameState.GameBoard[piecePosition.y - 2, piecePosition.x];

                if (twoPieceAbove.Exists)
                {
                    canMoveForward = false;
                }

                if (canMoveForward)
                {
                    coordinates.Add((piecePosition.y - 2, piecePosition.x));
                }
            }

            // take piece diagonal
            if (piecePosition.x > 0)
            {
                var pieceUpLeft = gameState.GameBoard[piecePosition.y - 1, piecePosition.x - 1];

                if (pieceUpLeft.Exists && pieceUpLeft.Colour == 0)
                {
                    coordinates.Add((piecePosition.y - 1, piecePosition.x - 1));
                }
            }

            if (piecePosition.x < 7)
            {
                var pieceUpRight = gameState.GameBoard[piecePosition.y - 1, piecePosition.x + 1];

                if (pieceUpRight.Exists && pieceUpRight.Colour == 0)
                {
                    coordinates.Add((piecePosition.y - 1, piecePosition.x + 1));
                }
            }
            
            return coordinates;
        }

        public List<(int, int)> getCoordinatesForQueen(Piece piece, (int y, int x) piecePosition)
        {
            var rookCoords = getCoordinatesForRook(piece, piecePosition);

            var bishopCoords = getCoordinatesForBishop(piece, piecePosition);

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

        public List<(int, int)> getCoordinatesForRook(Piece piece, (int y, int x) piecePosition)
        {
            List<(int, int)> availableCoordinates = new List<(int, int)> ();
            int oppositeColour = piece.Colour == 0 ? 1 : 0;

            for (int j = piecePosition.x + 1; j < 8; j++) 
            {
                var currentPiece = gameState.GameBoard[piecePosition.y, j];

                if (!currentPiece.Exists) 
                {
                    availableCoordinates.Add((piecePosition.y, j));
                }
                else
                {
                    if (currentPiece.Colour == oppositeColour)
                    {
                        availableCoordinates.Add((piecePosition.y, j));
                    }

                    break;
                }
            }

            for (int j = piecePosition.x - 1; j >= 0; j--)
            {
                var currentPiece = gameState.GameBoard[piecePosition.y, j];

                if (!currentPiece.Exists)
                {
                    availableCoordinates.Add((piecePosition.y, j));
                }
                else
                {
                    if (currentPiece.Colour == oppositeColour)
                    {
                        availableCoordinates.Add((piecePosition.y, j));
                    }

                    break;
                }
            }

            for (int i = piecePosition.y - 1; i >= 0; i--)
            {
                var currentPiece = gameState.GameBoard[i, piecePosition.x];

                if (!currentPiece.Exists)
                {
                    availableCoordinates.Add((i, piecePosition.x));
                }
                else
                {
                    if (currentPiece.Colour == oppositeColour)
                    {
                        availableCoordinates.Add((i, piecePosition.x));
                    }

                    break;
                }

            }

            for (int i = piecePosition.y + 1; i < 8; i++)
            {
                var currentPiece = gameState.GameBoard[i, piecePosition.x];

                if (!currentPiece.Exists)
                {
                    availableCoordinates.Add((i, piecePosition.x));
                }
                else
                {
                    if (currentPiece.Colour == oppositeColour)
                    {
                        availableCoordinates.Add((i, piecePosition.x));
                    }

                    break;
                }
            }

            return availableCoordinates;
        }

        public List<(int, int)> getCoordinatesForBishop(Piece piece, (int y, int x) piecePosition)
        {
            List<(int, int)> availableCoords = new List<(int, int)>();

            int oppositeColour = piece.Colour == 1 ? 0 : 1;

            // Up right
            int j = piecePosition.x + 1;
            for (int i = piecePosition.y - 1; i >= 0; i--)
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
            j = piecePosition.x - 1;
            for (int i = piecePosition.y - 1; i >= 0; i--)
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
            int curr_i = piecePosition.y + 1;

            for (int curr_x = piecePosition.x + 1; curr_x < 8; curr_x++)
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
            curr_i = piecePosition.y + 1;

            for (int curr_x = piecePosition.x - 1; curr_x >= 0; curr_x--)
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

        public List<(int, int)> getCoordinatesForKing(Piece piece, (int y, int x) piecePosition)
        {
            var availableCoords = new List<(int, int)>();
            var possibleCoords = new List<(int, int)>();
            int oppositeColour = piece.Colour == 1 ? 0 : 1;

            possibleCoords.Add((piecePosition.y - 1, piecePosition.x - 1));
            possibleCoords.Add((piecePosition.y - 1, piecePosition.x));
            possibleCoords.Add((piecePosition.y - 1, piecePosition.x + 1));

            possibleCoords.Add((piecePosition.y, piecePosition.x - 1));
            possibleCoords.Add((piecePosition.y, piecePosition.x));
            possibleCoords.Add((piecePosition.y, piecePosition.x + 1));

            possibleCoords.Add((piecePosition.y + 1, piecePosition.x - 1));
            possibleCoords.Add((piecePosition.y + 1, piecePosition.x));
            possibleCoords.Add((piecePosition.y + 1, piecePosition.x + 1));

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

        public List<(int, int)> getCoordinatesForKnight(Piece piece, (int y, int x) piecePosition)
        {
            var availableCoordinates = new List<(int, int)>();
            var possibleCoordinates = new List<(int, int)>();

            possibleCoordinates.Add((piecePosition.y - 1, piecePosition.x - 2));
            possibleCoordinates.Add((piecePosition.y - 1, piecePosition.x + 2));
            possibleCoordinates.Add((piecePosition.y + 1, piecePosition.x - 2));
            possibleCoordinates.Add((piecePosition.y + 1, piecePosition.x + 2));
            possibleCoordinates.Add((piecePosition.y - 2, piecePosition.x - 1));
            possibleCoordinates.Add((piecePosition.y - 2, piecePosition.x + 1));
            possibleCoordinates.Add((piecePosition.y + 2, piecePosition.x - 1));
            possibleCoordinates.Add((piecePosition.y + 2, piecePosition.x + 1));

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

        public void movePiece((int y1, int x1) oldCoordinate, (int y2, int x2) newCoordinate)
        {
            var oldPiece = gameState.GameBoard[oldCoordinate.y1, oldCoordinate.x1];

            gameState.GameBoard[oldCoordinate.y1, oldCoordinate.x1] = new Piece(aColour: -1, false);

            gameState.GameBoard[newCoordinate.y2, newCoordinate.x2] = oldPiece;

            whiteTurn = !whiteTurn;

            gameState.StringBoardOutput();

            Console.WriteLine();
            Console.WriteLine();
        }
    }
}
