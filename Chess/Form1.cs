using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Timers;

namespace Chess
{
    public partial class Form1 : Form
    {
        private Game currentGame;

        private TableLayoutPanel[,] displayBoard = new TableLayoutPanel[8, 8];

        private PictureBox[] blackCaptures;
        private Piece[] blackCapturePieces;
        private int currentBlackCaptureIndex;

        private PictureBox[] whiteCaptures;
        private Piece[] whiteCapturePieces;
        private int currentWhiteCaptureIndex;

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

        private System.Windows.Forms.Timer whitePieceTimer;
        private static int whitePieceMinutes = 5;
        private static int whitePieceSeconds = 0;

        private System.Windows.Forms.Timer blackPieceTimer;
        private static int blackPieceMinutes = 5;
        private static int blackPieceSeconds = 0;

        private (int, int) clickedPiecePosition;
        private List<(int, int)> availableCoordinates = new List<(int, int)>();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            currentGame = new Game();

            SetupTimers();
            SetupCaptureProperties();
            CreatePieces();
            CreateBoard();

            currentGame.FindEntireBoardMoves();
        }

        private void SetupTimers()
        {
            BlackPieceMin.Text = GetMinuteAsString(whitePieceMinutes);
            BlackPieceSec.Text = GetSecondAsString(whitePieceSeconds);

            whitePieceTimer = new System.Windows.Forms.Timer();
            whitePieceTimer.Tick += WhiteTimerUpdateText;
            whitePieceTimer.Interval = 1000;
            whitePieceTimer.Enabled = true;

            WhitePieceMin.Text = GetMinuteAsString(blackPieceMinutes);
            WhitePieceSec.Text = GetSecondAsString(blackPieceSeconds);

            blackPieceTimer = new System.Windows.Forms.Timer();
            blackPieceTimer.Interval = 1000;
            blackPieceTimer.Enabled = true;
        }

        private void WhiteTimerUpdateText(object source, EventArgs e)
        {
            DetermineNextTime(1);

            WhitePieceMin.Text = GetMinuteAsString(whitePieceMinutes);
            WhitePieceSec.Text = GetSecondAsString(whitePieceSeconds);

            if (whitePieceMinutes == 0 && whitePieceSeconds == 0)
            {
                whitePieceTimer.Tick -= WhiteTimerUpdateText;

                DisplayWinScreen(0);
            }
        }

        private void BlackTimerUpdateText(object source, EventArgs e)
        {
            DetermineNextTime(0);

            BlackPieceMin.Text = GetMinuteAsString(blackPieceMinutes);
            BlackPieceSec.Text = GetSecondAsString(blackPieceSeconds);

            if (blackPieceMinutes == 0 && blackPieceSeconds == 0)
            {
                blackPieceTimer.Tick -= BlackTimerUpdateText;

                DisplayWinScreen(1);
            }
        }

        private string GetMinuteAsString(int minutes)
        {
            string sMinutes = minutes.ToString();

            if (sMinutes.Length == 1)
            {
                sMinutes = '0' + sMinutes;
            }

            return sMinutes;
        }

        private string GetSecondAsString(int seconds)
        {
            string sSeconds = seconds.ToString();

            if (sSeconds.Length == 1)
            {
                sSeconds = '0' + sSeconds;
            }

            return sSeconds;
        }

        private static void DetermineNextTime(int colour)
        {
            if (colour == 1)
            {
                if (whitePieceSeconds == 0)
                {
                    whitePieceMinutes -= 1;
                    whitePieceSeconds = 59;
                }
                else
                {
                    whitePieceSeconds -= 1;
                }
            }
            else if (colour == 0)
            {
                if (blackPieceSeconds == 0)
                {
                    blackPieceMinutes -= 1;
                    blackPieceSeconds = 59;
                }
                else
                {
                    blackPieceSeconds -= 1;
                }
            }
        }

        private void SetupCaptureProperties()
        {
            blackCaptures = new PictureBox[]{blackCaptureBox1, blackCaptureBox2, blackCaptureBox3, blackCaptureBox4,
                                             blackCaptureBox5, blackCaptureBox6, blackCaptureBox7, blackCaptureBox8,
                                             blackCaptureBox9, blackCaptureBox10, blackCaptureBox11, blackCaptureBox12,
                                             blackCaptureBox13, blackCaptureBox14, blackCaptureBox15, blackCaptureBox16};

            blackCapturePieces = new Piece[16];

            currentBlackCaptureIndex = -1;

            whiteCaptures = new PictureBox[]{whiteCaptureBox1, whiteCaptureBox2, whiteCaptureBox3, whiteCaptureBox4,
                                             whiteCaptureBox5, whiteCaptureBox6, whiteCaptureBox7, whiteCaptureBox8,
                                             whiteCaptureBox9, whiteCaptureBox10, whiteCaptureBox11, whiteCaptureBox12,
                                             whiteCaptureBox13, whiteCaptureBox14, whiteCaptureBox15, whiteCaptureBox16};

            whiteCapturePieces = new Piece[16];

            currentWhiteCaptureIndex = -1;
        }

        private void CreateBoard()
        {
            int xCoord = 0;
            int yCoord = 50;
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
            currentGame.GameState.SetPieceImage(0, 0, blackRook1);

            blackRook1.Click += PieceClick;

            blackRook2 = new PictureBox();
            blackRook2.Size = new Size(85, 73);
            blackRook2.SizeMode = PictureBoxSizeMode.Zoom;
            blackRook2.Image = Image.FromFile("Resources/rook-b.png");
            currentGame.GameState.SetPieceImage(7, 0, blackRook2);

            blackRook2.Click += PieceClick;

            blackKnight1 = new PictureBox();
            blackKnight1.Size = new Size(85, 73);
            blackKnight1.SizeMode = PictureBoxSizeMode.Zoom;
            blackKnight1.Image = Image.FromFile("Resources/knight-b.png");
            currentGame.GameState.SetPieceImage(1, 0, blackKnight1);

            blackKnight1.Click += PieceClick;

            blackKnight2 = new PictureBox();
            blackKnight2.Size = new Size(85, 73);
            blackKnight2.SizeMode = PictureBoxSizeMode.Zoom;
            blackKnight2.Image = Image.FromFile("Resources/knight-b.png");
            currentGame.GameState.SetPieceImage(6, 0, blackKnight2);

            blackKnight2.Click += PieceClick;

            blackBishop1 = new PictureBox();
            blackBishop1.Size = new Size(85, 73);
            blackBishop1.SizeMode = PictureBoxSizeMode.Zoom;
            blackBishop1.Image = Image.FromFile("Resources/bishop-b.png");
            currentGame.GameState.SetPieceImage(2, 0, blackBishop1);

            blackBishop1.Click += PieceClick;

            blackBishop2 = new PictureBox();
            blackBishop2.Size = new Size(85, 73);
            blackBishop2.SizeMode = PictureBoxSizeMode.Zoom;
            blackBishop2.Image = Image.FromFile("Resources/bishop-b.png");
            currentGame.GameState.SetPieceImage(5, 0, blackBishop2);

            blackBishop2.Click += PieceClick;

            blackKing = new PictureBox();
            blackKing.Size = new Size(85, 73);
            blackKing.SizeMode = PictureBoxSizeMode.Zoom;
            blackKing.Image = Image.FromFile("Resources/king-b.png");
            currentGame.GameState.SetPieceImage(4, 0, blackKing);

            blackKing.Click += PieceClick;

            blackQueen = new PictureBox();
            blackQueen.Size = new Size(85, 73);
            blackQueen.SizeMode = PictureBoxSizeMode.Zoom;
            blackQueen.Image = Image.FromFile("Resources/queen-b.png");
            currentGame.GameState.SetPieceImage(3, 0, blackQueen);

            blackQueen.Click += PieceClick;

            blackPawn = new PictureBox();
            blackPawn.Size = new Size(85, 73);
            blackPawn.SizeMode = PictureBoxSizeMode.Zoom;
            blackPawn.Image = Image.FromFile("Resources/pawn-b.png");
            currentGame.GameState.SetPieceImage(0, 1, blackPawn);

            blackPawn.Click += PieceClick;

            blackPawn2 = new PictureBox();
            blackPawn2.Size = new Size(85, 73);
            blackPawn2.SizeMode = PictureBoxSizeMode.Zoom;
            blackPawn2.Image = Image.FromFile("Resources/pawn-b.png");
            currentGame.GameState.SetPieceImage(1, 1, blackPawn2);

            blackPawn2.Click += PieceClick;

            blackPawn3 = new PictureBox();
            blackPawn3.Size = new Size(85, 73);
            blackPawn3.SizeMode = PictureBoxSizeMode.Zoom;
            blackPawn3.Image = Image.FromFile("Resources/pawn-b.png");
            currentGame.GameState.SetPieceImage(2, 1, blackPawn3);

            blackPawn3.Click += PieceClick;

            blackPawn4 = new PictureBox();
            blackPawn4.Size = new Size(85, 73);
            blackPawn4.SizeMode = PictureBoxSizeMode.Zoom;
            blackPawn4.Image = Image.FromFile("Resources/pawn-b.png");
            currentGame.GameState.SetPieceImage(3, 1, blackPawn4);

            blackPawn4.Click += PieceClick;

            blackPawn5 = new PictureBox();
            blackPawn5.Size = new Size(85, 73);
            blackPawn5.SizeMode = PictureBoxSizeMode.Zoom;
            blackPawn5.Image = Image.FromFile("Resources/pawn-b.png");
            currentGame.GameState.SetPieceImage(4, 1, blackPawn5);

            blackPawn5.Click += PieceClick;

            blackPawn6 = new PictureBox();
            blackPawn6.Size = new Size(85, 73);
            blackPawn6.SizeMode = PictureBoxSizeMode.Zoom;
            blackPawn6.Image = Image.FromFile("Resources/pawn-b.png");
            currentGame.GameState.SetPieceImage(5, 1, blackPawn6);

            blackPawn6.Click += PieceClick;

            blackPawn7 = new PictureBox();
            blackPawn7.Size = new Size(85, 73);
            blackPawn7.SizeMode = PictureBoxSizeMode.Zoom;
            blackPawn7.Image = Image.FromFile("Resources/pawn-b.png");
            currentGame.GameState.SetPieceImage(6, 1, blackPawn7);

            blackPawn7.Click += PieceClick;

            blackPawn8 = new PictureBox();
            blackPawn8.Size = new Size(85, 73);
            blackPawn8.SizeMode = PictureBoxSizeMode.Zoom;
            blackPawn8.Image = Image.FromFile("Resources/pawn-b.png");
            currentGame.GameState.SetPieceImage(7, 1, blackPawn8);

            blackPawn8.Click += PieceClick;

            whitePawn = new PictureBox();
            whitePawn.Size = new Size(85, 73);
            whitePawn.SizeMode = PictureBoxSizeMode.Zoom;
            whitePawn.Image = Image.FromFile("Resources/pawn-w.png");
            currentGame.GameState.SetPieceImage(0, 6, whitePawn);

            whitePawn.Click += PieceClick;

            whitePawn2 = new PictureBox();
            whitePawn2.Size = new Size(85, 73);
            whitePawn2.SizeMode = PictureBoxSizeMode.Zoom;
            whitePawn2.Image = Image.FromFile("Resources/pawn-w.png");
            currentGame.GameState.SetPieceImage(1, 6, whitePawn2);

            whitePawn2.Click += PieceClick;

            whitePawn3 = new PictureBox();
            whitePawn3.Size = new Size(85, 73);
            whitePawn3.SizeMode = PictureBoxSizeMode.Zoom;
            whitePawn3.Image = Image.FromFile("Resources/pawn-w.png");
            currentGame.GameState.SetPieceImage(2, 6, whitePawn3);

            whitePawn3.Click += PieceClick;

            whitePawn4 = new PictureBox();
            whitePawn4.Size = new Size(85, 73);
            whitePawn4.SizeMode = PictureBoxSizeMode.Zoom;
            whitePawn4.Image = Image.FromFile("Resources/pawn-w.png");
            currentGame.GameState.SetPieceImage(3, 6, whitePawn4);

            whitePawn4.Click += PieceClick;

            whitePawn5 = new PictureBox();
            whitePawn5.Size = new Size(85, 73);
            whitePawn5.SizeMode = PictureBoxSizeMode.Zoom;
            whitePawn5.Image = Image.FromFile("Resources/pawn-w.png");
            currentGame.GameState.SetPieceImage(4, 6, whitePawn5);

            whitePawn5.Click += PieceClick;

            whitePawn6 = new PictureBox();
            whitePawn6.Size = new Size(85, 73);
            whitePawn6.SizeMode = PictureBoxSizeMode.Zoom;
            whitePawn6.Image = Image.FromFile("Resources/pawn-w.png");
            currentGame.GameState.SetPieceImage(5, 6, whitePawn6);

            whitePawn6.Click += PieceClick;

            whitePawn7 = new PictureBox();
            whitePawn7.Size = new Size(85, 73);
            whitePawn7.SizeMode = PictureBoxSizeMode.Zoom;
            whitePawn7.Image = Image.FromFile("Resources/pawn-w.png");
            currentGame.GameState.SetPieceImage(6, 6, whitePawn7);

            whitePawn7.Click += PieceClick;

            whitePawn8 = new PictureBox();
            whitePawn8.Size = new Size(85, 73);
            whitePawn8.SizeMode = PictureBoxSizeMode.Zoom;
            whitePawn8.Image = Image.FromFile("Resources/pawn-w.png");
            currentGame.GameState.SetPieceImage(7, 6, whitePawn8);

            whitePawn8.Click += PieceClick;

            whiteRook = new PictureBox();
            whiteRook.Size = new Size(85, 73);
            whiteRook.SizeMode = PictureBoxSizeMode.Zoom;
            whiteRook.Image = Image.FromFile("Resources/rook-w.png");
            currentGame.GameState.SetPieceImage(0, 7, whiteRook);

            whiteRook.Click += PieceClick;

            whiteRook2 = new PictureBox();
            whiteRook2.Size = new Size(85, 73);
            whiteRook2.SizeMode = PictureBoxSizeMode.Zoom;
            whiteRook2.Image = Image.FromFile("Resources/rook-w.png");
            currentGame.GameState.SetPieceImage(7, 7, whiteRook2);

            whiteRook2.Click += PieceClick;

            whiteKnight = new PictureBox();
            whiteKnight.Size = new Size(85, 73);
            whiteKnight.SizeMode = PictureBoxSizeMode.Zoom;
            whiteKnight.Image = Image.FromFile("Resources/knight-w.png");
            currentGame.GameState.SetPieceImage(1, 7, whiteKnight);

            whiteKnight.Click += PieceClick;

            whiteKnight2 = new PictureBox();
            whiteKnight2.Size = new Size(85, 73);
            whiteKnight2.SizeMode = PictureBoxSizeMode.Zoom;
            whiteKnight2.Image = Image.FromFile("Resources/knight-w.png");
            currentGame.GameState.SetPieceImage(6, 7, whiteKnight2);

            whiteKnight2.Click += PieceClick;

            whiteBishop = new PictureBox();
            whiteBishop.Size = new Size(85, 73);
            whiteBishop.SizeMode = PictureBoxSizeMode.Zoom;
            whiteBishop.Image = Image.FromFile("Resources/bishop-w.png");
            currentGame.GameState.SetPieceImage(2, 7, whiteBishop);

            whiteBishop.Click += PieceClick;

            whiteBishop2 = new PictureBox();
            whiteBishop2.Size = new Size(85, 73);
            whiteBishop2.SizeMode = PictureBoxSizeMode.Zoom;
            whiteBishop2.Image = Image.FromFile("Resources/bishop-w.png");
            currentGame.GameState.SetPieceImage(5, 7, whiteBishop2);

            whiteBishop2.Click += PieceClick;

            whiteKing = new PictureBox();
            whiteKing.Size = new Size(85, 73);
            whiteKing.SizeMode = PictureBoxSizeMode.Zoom;
            whiteKing.Image = Image.FromFile("Resources/king-w.png");
            currentGame.GameState.SetPieceImage(4, 7, whiteKing);

            whiteKing.Click += PieceClick;

            whiteQueen = new PictureBox();
            whiteQueen.Size = new Size(85, 73);
            whiteQueen.SizeMode = PictureBoxSizeMode.Zoom;
            whiteQueen.Image = Image.FromFile("Resources/queen-w.png");
            currentGame.GameState.SetPieceImage(3, 7, whiteQueen);

            whiteQueen.Click += PieceClick;
        }

        private void PieceClick(object sender, EventArgs e)
        {
            var pictureBox = (PictureBox)sender;
            (int y, int x) cellCoordinate = getCoordinateFromCellTag(pictureBox.Parent.Tag.ToString());
            var piece = currentGame.GameState.GetPiece(cellCoordinate.x, cellCoordinate.y);
            bool validTurn = currentGame.validTurnForPiece(piece);
            
            if (validTurn)
            {
                clickedPiecePosition = cellCoordinate;

                RecolourBoard();

                findPossibleMoves(piece);
            }
            else if (availableCoordinates != null && availableCoordinates.Contains(cellCoordinate)) 
            {
                var clickedPiece = currentGame.GameState.GetPiece(clickedPiecePosition.Item2, clickedPiecePosition.Item1);

                var winner = currentGame.movePiece(clickedPiece, piece);
                
                ResumeStopTimers();

                availableCoordinates.Clear();

                PictureBox oldPiece = (PictureBox)displayBoard[clickedPiecePosition.Item1, clickedPiecePosition.Item2].Controls[0];
                displayBoard[clickedPiecePosition.Item1, clickedPiecePosition.Item2].Controls.Clear();

                SetCapturePiece(clickedPiece, piece);

                displayBoard[cellCoordinate.Item1, cellCoordinate.Item2].Controls.Clear();

                if (cellCoordinate.Item1 == 0 && clickedPiece is Pawn)
                {
                    var whiteQ = new PictureBox();
                    whiteQ.Size = new Size(85, 73);
                    whiteQ.SizeMode = PictureBoxSizeMode.Zoom;
                    whiteQ.Image = Image.FromFile("Resources/queen-w.png");

                    whiteQ.Click += PieceClick;

                    currentGame.GameState.SetPieceImage(cellCoordinate.Item2, cellCoordinate.Item1, whiteQ);
                    displayBoard[cellCoordinate.Item1, cellCoordinate.Item2].Controls.Add(whiteQ);
                }
                else if (cellCoordinate.Item1 == 7 && clickedPiece is Pawn)
                {
                    var blackQ = new PictureBox();
                    blackQ.Size = new Size(85, 73);
                    blackQ.SizeMode = PictureBoxSizeMode.Zoom;
                    blackQ.Image = Image.FromFile("Resources/queen-b.png");

                    blackQ.Click += PieceClick;

                    currentGame.GameState.SetPieceImage(cellCoordinate.Item2, cellCoordinate.Item1, blackQ);
                    displayBoard[cellCoordinate.Item1, cellCoordinate.Item2].Controls.Add(blackQ);
                }
                else
                {
                    displayBoard[cellCoordinate.Item1, cellCoordinate.Item2].Controls.Add(oldPiece);
                }

                RecolourBoard();

                if (winner != -1)
                {
                    DisplayWinScreen(winner);
                }
            }
        }

        private void CellClick(object sender, EventArgs e)
        {
            var tablePanel = (TableLayoutPanel)sender;
            (int y, int x) cellCoordinate = getCoordinateFromCellTag(tablePanel.Tag.ToString());
            var piece = currentGame.GameState.GetPiece(cellCoordinate.x, cellCoordinate.y);

            if (availableCoordinates != null && availableCoordinates.Contains(cellCoordinate))
            {
                var clickedPiece = currentGame.GameState.GetPiece(clickedPiecePosition.Item2, clickedPiecePosition.Item1);

                var winner = currentGame.movePiece(clickedPiece, piece);

                ResumeStopTimers();

                availableCoordinates.Clear();

                PictureBox oldPiece = (PictureBox)displayBoard[clickedPiecePosition.Item1, clickedPiecePosition.Item2].Controls[0];
                displayBoard[clickedPiecePosition.Item1, clickedPiecePosition.Item2].Controls.Clear();

                if (displayBoard[cellCoordinate.Item1, cellCoordinate.Item2].Controls.Count > 0)
                {
                    SetCapturePiece(clickedPiece, piece);
                }

                displayBoard[cellCoordinate.Item1, cellCoordinate.Item2].Controls.Clear();

                if (cellCoordinate.Item1 == 0 && clickedPiece is Pawn)
                {
                    var whiteQ = new PictureBox();
                    whiteQ.Size = new Size(85, 73);
                    whiteQ.SizeMode = PictureBoxSizeMode.Zoom;
                    whiteQ.Image = Image.FromFile("Resources/queen-w.png");

                    whiteQ.Click += PieceClick;

                    currentGame.GameState.SetPieceImage(cellCoordinate.Item2, cellCoordinate.Item1, whiteQ);
                    displayBoard[cellCoordinate.Item1, cellCoordinate.Item2].Controls.Add(whiteQ);
                }
                else if (cellCoordinate.Item1 == 7 && clickedPiece is Pawn)
                {
                    var blackQ = new PictureBox();
                    blackQ.Size = new Size(85, 73);
                    blackQ.SizeMode = PictureBoxSizeMode.Zoom;
                    blackQ.Image = Image.FromFile("Resources/queen-b.png");

                    blackQ.Click += PieceClick;

                    currentGame.GameState.SetPieceImage(cellCoordinate.Item2, cellCoordinate.Item1, blackQ);
                    displayBoard[cellCoordinate.Item1, cellCoordinate.Item2].Controls.Add(blackQ);
                }
                else
                {
                    displayBoard[cellCoordinate.Item1, cellCoordinate.Item2].Controls.Add(oldPiece);
                }

                RecolourBoard();

                if (winner != -1)
                {
                    DisplayWinScreen(winner);
                }
            }
        }

        private void ResumeStopTimers()
        {
            var whiteTurn = currentGame.WhiteTurn;

            if (whiteTurn)
            {
                blackPieceTimer.Tick -= BlackTimerUpdateText;
                whitePieceTimer.Tick -= WhiteTimerUpdateText;
                whitePieceTimer.Tick += WhiteTimerUpdateText;
            }
            else
            {
                whitePieceTimer.Tick -= WhiteTimerUpdateText;
                blackPieceTimer.Tick -= BlackTimerUpdateText;
                blackPieceTimer.Tick += BlackTimerUpdateText;
            }
        }

        private void SetCapturePiece(Piece capturingPiece, Piece capturedPiece)
        {
            if (capturingPiece.Colour == 0)
            {
                insertNewPieceIntoCaptures(ref currentBlackCaptureIndex, blackCaptures, blackCapturePieces, capturedPiece);
            }
            else
            {
                insertNewPieceIntoCaptures(ref currentWhiteCaptureIndex, whiteCaptures, whiteCapturePieces, capturedPiece);
            }
        }

        private void insertNewPieceIntoCaptures(ref int currentCaptureIndex, PictureBox[] captures, Piece[] capturesPieces, Piece capturedPiece)
        {
            int foundIndex = -1;

            for (int i = currentCaptureIndex; i >= 0; i--)
            {
                var currentPiece = capturesPieces[i];

                if (currentPiece != null && currentPiece.GetType() == capturedPiece.GetType())
                {
                    foundIndex = i;
                    break;
                }
            }

            currentCaptureIndex += 1;

            if (foundIndex == -1)
            {
                captures[currentCaptureIndex].Image = capturedPiece.PieceDisplay.Image;
                capturesPieces[currentCaptureIndex] = capturedPiece;
            }
            else
            {
                for (int i = currentCaptureIndex - 1; i >= foundIndex + 1; i--)
                {
                    captures[i + 1].Image = captures[i].Image;
                    capturesPieces[currentCaptureIndex + 1] = capturesPieces[currentCaptureIndex];
                    Thread.Sleep(100);
                }

                captures[foundIndex + 1].Image = capturedPiece.PieceDisplay.Image;
                capturesPieces[foundIndex + 1] = capturedPiece;
            }
        }

        private (int, int) getCoordinateFromCellTag(string coordinate)
        {
            int xCoordinate = coordinate[0] - '0';
            int yCoordinate = coordinate[2] - '0';

            return (xCoordinate, yCoordinate);
        }

        private void findPossibleMoves(Piece piece)
        {
            availableCoordinates = piece.PossibleMoves;

            foreach ((int y, int x) coord in availableCoordinates)
            {
                this.displayBoard[coord.y, coord.x].BackColor = Color.LightGreen;
            }
        }

        private void DisplayWinScreen(int winner)
        {
            Console.WriteLine("Winner Found!!");
            Console.WriteLine(winner);
            whitePieceTimer.Tick -= WhiteTimerUpdateText;
            blackPieceTimer.Tick -= BlackTimerUpdateText;

            string colourWon = "";

            if (winner == 1)
            {
                colourWon = "White";
            }
            else if (winner == 0)
            {
                colourWon = "Black";
            }
            else
            {
                colourWon = "None";
            }

            GameResultForm gameForm = new GameResultForm(this, colourWon);

            gameForm.ShowDialog();
        }

        public void RestartGame()
        {
            whitePieceMinutes = 5;
            whitePieceSeconds = 0;

            blackPieceMinutes = 5;
            blackPieceSeconds = 0;

            List<Control> controlsToRemove = new List<Control>();

            foreach (Control control in Controls)
            {
                if (control is TableLayoutPanel)
                {
                    controlsToRemove.Add(control);
                }
            }

            foreach (Control control in controlsToRemove)
            {
                Controls.Remove(control);
            }

            blackCaptureBox1.Image = null;
            blackCaptureBox2.Image = null;
            blackCaptureBox3.Image = null;
            blackCaptureBox4.Image = null;
            blackCaptureBox5.Image = null;
            blackCaptureBox6.Image = null;
            blackCaptureBox7.Image = null;
            blackCaptureBox8.Image = null;
            blackCaptureBox9.Image = null;
            blackCaptureBox10.Image = null;
            blackCaptureBox11.Image = null;
            blackCaptureBox12.Image = null;
            blackCaptureBox13.Image = null;
            blackCaptureBox14.Image = null;
            blackCaptureBox15.Image = null;
            blackCaptureBox16.Image = null;

            whiteCaptureBox1.Image = null;
            whiteCaptureBox2.Image = null;
            whiteCaptureBox3.Image = null;
            whiteCaptureBox4.Image = null;
            whiteCaptureBox5.Image = null;
            whiteCaptureBox6.Image = null;
            whiteCaptureBox7.Image = null;
            whiteCaptureBox8.Image = null;
            whiteCaptureBox9.Image = null;
            whiteCaptureBox10.Image = null;
            whiteCaptureBox11.Image = null;
            whiteCaptureBox12.Image = null;
            whiteCaptureBox13.Image = null;
            whiteCaptureBox14.Image = null;
            whiteCaptureBox15.Image = null;
            whiteCaptureBox16.Image = null;

            currentGame = new Game();

            SetupTimers();
            SetupCaptureProperties();
            CreatePieces();
            CreateBoard();

            currentGame.FindEntireBoardMoves();
        }

        public void ExitGame()
        {
            this.Close();
        }
    }
}
