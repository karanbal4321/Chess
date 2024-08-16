using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Chess
{
    public partial class Form1 : Form
    {
        private Game currentGame;

        private TableLayoutPanel[,] displayBoard = new TableLayoutPanel[8, 8];

        private PictureBox blackRook1;
        private PictureBox blackRook2;
        private PictureBox blackKnight1;
        private PictureBox blackKnight2;
        private PictureBox blackBishop1;
        private PictureBox blackBishop2;
        private PictureBox blackKing;
        private PictureBox blackQueen;
        private PictureBox blackPawn;
        private PictureBox blackPawn2;
        private PictureBox blackPawn3;
        private PictureBox blackPawn4;
        private PictureBox blackPawn5;
        private PictureBox blackPawn6;
        private PictureBox blackPawn7;
        private PictureBox blackPawn8;

        private PictureBox whiteRook;
        private PictureBox whiteRook2;
        private PictureBox whiteKnight;
        private PictureBox whiteKnight2;
        private PictureBox whiteBishop;
        private PictureBox whiteBishop2;
        private PictureBox whiteKing;
        private PictureBox whiteQueen;
        private PictureBox whitePawn;
        private PictureBox whitePawn2;
        private PictureBox whitePawn3;
        private PictureBox whitePawn4;
        private PictureBox whitePawn5;
        private PictureBox whitePawn6;
        private PictureBox whitePawn7;
        private PictureBox whitePawn8;

        private (int, int) clickedPiecePosition;
        private List<(int, int)> availableCoordinates; 


        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            currentGame = new Game();

            CreatePieces();
            CreateBoard();
        }


        private void CreateBoard()
        {
            int xCoord = 0;
            int yCoord = 0;
            PictureBox[] piecesOrder = {blackRook1, blackKnight1, blackBishop1, blackQueen, blackKing, blackBishop2, blackKnight2,
                                        blackRook2, blackPawn, blackPawn2, blackPawn3, blackPawn4, blackPawn5, blackPawn6, blackPawn7,
                                        blackPawn8, whitePawn, whitePawn2, whitePawn3, whitePawn4, whitePawn5, whitePawn6, whitePawn7,
                                        whitePawn8, whiteRook, whiteKnight, whiteBishop, whiteQueen, whiteKing, whiteBishop2,
                                        whiteKnight2, whiteRook2};
            int currentPiecePointer = 0;

            for (int i = 0; i < 8; i++)
            {
                xCoord = 0;
                for (int j = 0; j < 8; j++)
                {
                    displayBoard[i, j] = new TableLayoutPanel();
                    displayBoard[i, j].Size = new Size(95, 82);
                    displayBoard[i, j].Location = new Point(xCoord, yCoord);
                    displayBoard[i, j].RowCount = 1;
                    displayBoard[i, j].ColumnCount = 1;
                    displayBoard[i, j].Tag = i.ToString() + " " + j.ToString();
                    displayBoard[i, j].Margin = new Padding(3, 2, 3, 2);
                    displayBoard[i, j].CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;

                    
                    displayBoard[i, j].BackColor = ColourForCell((i, j));
                    displayBoard[i, j].Click += CellClick;


                    Controls.Add(displayBoard[i, j]);

                    if (i < 2 || i > 5)
                    {
                        displayBoard[i, j].Controls.Add(piecesOrder[currentPiecePointer]);
                    }

                    if (i < 2)
                    {
                        currentPiecePointer += 1;
                    }

                    if (i > 5)
                    {
                        currentPiecePointer += 1;
                    }

                    xCoord += 95;
                }

                yCoord += 82;
            }
        }

        public void RecolourBoard()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    displayBoard[i, j].BackColor = ColourForCell((i, j));
                }
            }
        }

        public Color ColourForCell((int y, int x) coordinate)
        {
            if (coordinate.y % 2 == 0)
            {
                if (coordinate.x % 2 == 0)
                {
                    return Color.NavajoWhite;
                }
                else
                {
                    return Color.Sienna;
                }
            }
            else
            {
                if (coordinate.x % 2 != 0)
                {
                    return Color.NavajoWhite;
                }
                else
                {
                    return Color.Sienna;
                }
            }
        }

        public void CreatePieces()
        {
            blackRook1 = new PictureBox();
            blackRook1.Size = new Size(85, 73);
            blackRook1.SizeMode = PictureBoxSizeMode.Zoom;
            blackRook1.Image = Image.FromFile("Resources/rook-b.png");

            blackRook1.Click += PieceClick;

            blackRook2 = new PictureBox();
            blackRook2.Size = new Size(85, 73);
            blackRook2.SizeMode = PictureBoxSizeMode.Zoom;
            blackRook2.Image = Image.FromFile("Resources/rook-b.png");

            blackRook2.Click += PieceClick;

            blackKnight1 = new PictureBox();
            blackKnight1.Size = new Size(85, 73);
            blackKnight1.SizeMode = PictureBoxSizeMode.Zoom;
            blackKnight1.Image = Image.FromFile("Resources/knight-b.png");

            blackKnight1.Click += PieceClick;

            blackKnight2 = new PictureBox();
            blackKnight2.Size = new Size(85, 73);
            blackKnight2.SizeMode = PictureBoxSizeMode.Zoom;
            blackKnight2.Image = Image.FromFile("Resources/knight-b.png");

            blackKnight2.Click += PieceClick;

            blackBishop1 = new PictureBox();
            blackBishop1.Size = new Size(85, 73);
            blackBishop1.SizeMode = PictureBoxSizeMode.Zoom;
            blackBishop1.Image = Image.FromFile("Resources/bishop-b.png");

            blackBishop1.Click += PieceClick;

            blackBishop2 = new PictureBox();
            blackBishop2.Size = new Size(85, 73);
            blackBishop2.SizeMode = PictureBoxSizeMode.Zoom;
            blackBishop2.Image = Image.FromFile("Resources/bishop-b.png");

            blackBishop2.Click += PieceClick;

            blackKing = new PictureBox();
            blackKing.Size = new Size(85, 73);
            blackKing.SizeMode = PictureBoxSizeMode.Zoom;
            blackKing.Image = Image.FromFile("Resources/king-b.png");

            blackKing.Click += PieceClick;

            blackQueen = new PictureBox();
            blackQueen.Size = new Size(85, 73);
            blackQueen.SizeMode = PictureBoxSizeMode.Zoom;
            blackQueen.Image = Image.FromFile("Resources/queen-b.png");

            blackQueen.Click += PieceClick;

            blackPawn = new PictureBox();
            blackPawn.Size = new Size(85, 73);
            blackPawn.SizeMode = PictureBoxSizeMode.Zoom;
            blackPawn.Image = Image.FromFile("Resources/pawn-b.png");

            blackPawn.Click += PieceClick;

            blackPawn2 = new PictureBox();
            blackPawn2.Size = new Size(85, 73);
            blackPawn2.SizeMode = PictureBoxSizeMode.Zoom;
            blackPawn2.Image = Image.FromFile("Resources/pawn-b.png");

            blackPawn2.Click += PieceClick;

            blackPawn3 = new PictureBox();
            blackPawn3.Size = new Size(85, 73);
            blackPawn3.SizeMode = PictureBoxSizeMode.Zoom;
            blackPawn3.Image = Image.FromFile("Resources/pawn-b.png");

            blackPawn3.Click += PieceClick;

            blackPawn4 = new PictureBox();
            blackPawn4.Size = new Size(85, 73);
            blackPawn4.SizeMode = PictureBoxSizeMode.Zoom;
            blackPawn4.Image = Image.FromFile("Resources/pawn-b.png");

            blackPawn4.Click += PieceClick;

            blackPawn5 = new PictureBox();
            blackPawn5.Size = new Size(85, 73);
            blackPawn5.SizeMode = PictureBoxSizeMode.Zoom;
            blackPawn5.Image = Image.FromFile("Resources/pawn-b.png");

            blackPawn5.Click += PieceClick;

            blackPawn6 = new PictureBox();
            blackPawn6.Size = new Size(85, 73);
            blackPawn6.SizeMode = PictureBoxSizeMode.Zoom;
            blackPawn6.Image = Image.FromFile("Resources/pawn-b.png");

            blackPawn6.Click += PieceClick;

            blackPawn7 = new PictureBox();
            blackPawn7.Size = new Size(85, 73);
            blackPawn7.SizeMode = PictureBoxSizeMode.Zoom;
            blackPawn7.Image = Image.FromFile("Resources/pawn-b.png");

            blackPawn7.Click += PieceClick;

            blackPawn8 = new PictureBox();
            blackPawn8.Size = new Size(85, 73);
            blackPawn8.SizeMode = PictureBoxSizeMode.Zoom;
            blackPawn8.Image = Image.FromFile("Resources/pawn-b.png");

            blackPawn8.Click += PieceClick;

            whiteRook = new PictureBox();
            whiteRook.Size = new Size(85, 73);
            whiteRook.SizeMode = PictureBoxSizeMode.Zoom;
            whiteRook.Image = Image.FromFile("Resources/rook-w.png");

            whiteRook.Click += PieceClick;

            whiteRook2 = new PictureBox();
            whiteRook2.Size = new Size(85, 73);
            whiteRook2.SizeMode = PictureBoxSizeMode.Zoom;
            whiteRook2.Image = Image.FromFile("Resources/rook-w.png");

            whiteRook2.Click += PieceClick;

            whiteKnight = new PictureBox();
            whiteKnight.Size = new Size(85, 73);
            whiteKnight.SizeMode = PictureBoxSizeMode.Zoom;
            whiteKnight.Image = Image.FromFile("Resources/knight-w.png");

            whiteKnight.Click += PieceClick;

            whiteKnight2 = new PictureBox();
            whiteKnight2.Size = new Size(85, 73);
            whiteKnight2.SizeMode = PictureBoxSizeMode.Zoom;
            whiteKnight2.Image = Image.FromFile("Resources/knight-w.png");

            whiteKnight2.Click += PieceClick;

            whiteBishop = new PictureBox();
            whiteBishop.Size = new Size(85, 73);
            whiteBishop.SizeMode = PictureBoxSizeMode.Zoom;
            whiteBishop.Image = Image.FromFile("Resources/bishop-w.png");

            whiteBishop.Click += PieceClick;

            whiteBishop2 = new PictureBox();
            whiteBishop2.Size = new Size(85, 73);
            whiteBishop2.SizeMode = PictureBoxSizeMode.Zoom;
            whiteBishop2.Image = Image.FromFile("Resources/bishop-w.png");

            whiteBishop2.Click += PieceClick;

            whiteKing = new PictureBox();
            whiteKing.Size = new Size(85, 73);
            whiteKing.SizeMode = PictureBoxSizeMode.Zoom;
            whiteKing.Image = Image.FromFile("Resources/king-w.png");

            whiteKing.Click += PieceClick;

            whiteQueen = new PictureBox();
            whiteQueen.Size = new Size(85, 73);
            whiteQueen.SizeMode = PictureBoxSizeMode.Zoom;
            whiteQueen.Image = Image.FromFile("Resources/queen-w.png");

            whiteQueen.Click += PieceClick;

            whitePawn = new PictureBox();
            whitePawn.Size = new Size(85, 73);
            whitePawn.SizeMode = PictureBoxSizeMode.Zoom;
            whitePawn.Image = Image.FromFile("Resources/pawn-w.png");

            whitePawn.Click += PieceClick;

            whitePawn2 = new PictureBox();
            whitePawn2.Size = new Size(85, 73);
            whitePawn2.SizeMode = PictureBoxSizeMode.Zoom;
            whitePawn2.Image = Image.FromFile("Resources/pawn-w.png");

            whitePawn2.Click += PieceClick;

            whitePawn3 = new PictureBox();
            whitePawn3.Size = new Size(85, 73);
            whitePawn3.SizeMode = PictureBoxSizeMode.Zoom;
            whitePawn3.Image = Image.FromFile("Resources/pawn-w.png");

            whitePawn3.Click += PieceClick;

            whitePawn4 = new PictureBox();
            whitePawn4.Size = new Size(85, 73);
            whitePawn4.SizeMode = PictureBoxSizeMode.Zoom;
            whitePawn4.Image = Image.FromFile("Resources/pawn-w.png");

            whitePawn4.Click += PieceClick;

            whitePawn5 = new PictureBox();
            whitePawn5.Size = new Size(85, 73);
            whitePawn5.SizeMode = PictureBoxSizeMode.Zoom;
            whitePawn5.Image = Image.FromFile("Resources/pawn-w.png");

            whitePawn5.Click += PieceClick;

            whitePawn6 = new PictureBox();
            whitePawn6.Size = new Size(85, 73);
            whitePawn6.SizeMode = PictureBoxSizeMode.Zoom;
            whitePawn6.Image = Image.FromFile("Resources/pawn-w.png");

            whitePawn6.Click += PieceClick;

            whitePawn7 = new PictureBox();
            whitePawn7.Size = new Size(85, 73);
            whitePawn7.SizeMode = PictureBoxSizeMode.Zoom;
            whitePawn7.Image = Image.FromFile("Resources/pawn-w.png");

            whitePawn7.Click += PieceClick;

            whitePawn8 = new PictureBox();
            whitePawn8.Size = new Size(85, 73);
            whitePawn8.SizeMode = PictureBoxSizeMode.Zoom;
            whitePawn8.Image = Image.FromFile("Resources/pawn-w.png");

            whitePawn8.Click += PieceClick;
        }

        private void PieceClick(object sender, EventArgs e)
        {
            var pictureBox = (PictureBox)sender;
            var cellCoordinate = getCoordinateFromCellTag(pictureBox.Parent.Tag.ToString());

            bool validTurn = currentGame.validTurnForPiece(cellCoordinate);

            if (validTurn)
            {
                clickedPiecePosition = cellCoordinate;

                RecolourBoard();

                findPossibleMoves(cellCoordinate);
            }
            else if (availableCoordinates != null && availableCoordinates.Contains(cellCoordinate)) 
            {
                currentGame.movePiece(clickedPiecePosition, cellCoordinate);

                availableCoordinates.Clear();

                PictureBox oldPiece = (PictureBox)displayBoard[clickedPiecePosition.Item1, clickedPiecePosition.Item2].Controls[0];
                displayBoard[clickedPiecePosition.Item1, clickedPiecePosition.Item2].Controls.Clear();
                displayBoard[cellCoordinate.Item1, cellCoordinate.Item2].Controls.Clear();
                displayBoard[cellCoordinate.Item1, cellCoordinate.Item2].Controls.Add(oldPiece);

                RecolourBoard();
            }
        }

        private void CellClick(object sender, EventArgs e)
        {
            var tablePanel = (TableLayoutPanel)sender;
            var cellCoordinate = getCoordinateFromCellTag(tablePanel.Tag.ToString());

            if (availableCoordinates != null && availableCoordinates.Contains(cellCoordinate))
            {
                currentGame.movePiece(clickedPiecePosition, cellCoordinate);

                availableCoordinates.Clear();

                PictureBox oldPiece = (PictureBox)displayBoard[clickedPiecePosition.Item1, clickedPiecePosition.Item2].Controls[0];
                displayBoard[clickedPiecePosition.Item1, clickedPiecePosition.Item2].Controls.Clear();
                displayBoard[cellCoordinate.Item1, cellCoordinate.Item2].Controls.Clear();
                displayBoard[cellCoordinate.Item1, cellCoordinate.Item2].Controls.Add(oldPiece);

                RecolourBoard();
            }
        }

        private (int, int) getCoordinateFromCellTag(string coordinate)
        {
            int xCoordinate = coordinate[0] - '0';
            int yCoordinate = coordinate[2] - '0';

            return (xCoordinate, yCoordinate);
        }

        private void findPossibleMoves((int, int) coordinate)
        {
            availableCoordinates = currentGame.getPossibleMovesForPiece(coordinate);

            foreach ((int y, int x) coord in availableCoordinates)
            {
                this.displayBoard[coord.y, coord.x].BackColor = Color.LightGreen;
            }
        }
    }
}
