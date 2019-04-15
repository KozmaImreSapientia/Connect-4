using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connect_4
{
    class State
    {
        public enum FIELD
        {
            EMPTY = 0,
            MIN = 1,
            MAX = 2
        }

        public static readonly int WIDTH = 7;
        public static readonly int HEIGHT = 6;

        private FIELD[,] board { get; set; }
        private List<State> children;

        public State()
        {
            this.Board = new FIELD[HEIGHT, WIDTH];
            InitializeBoard();
            this.Children = new List<State>();
        }

        public State(FIELD[,] board)
        {
            this.Board = new FIELD[HEIGHT, WIDTH];
            InitializeBoard();
            this.Children = new List<State>();
        }

        public List<State> Children
        {
            get
            {
                return this.children;
            }
            private set
            {
                this.children = value;
            }
        }

        public FIELD[,] Board
        {
            get { return this.board; }
            set { this.board = value; }
        }

        /*public void SetBoardValue(int row, int col, FIELD value)
        {
            Board[row, col] = value;
        }*/

        public bool CanAddToBoard(int col)
        {
            if (col < Board.GetLength(1))
            {
                for (int i = Board.GetLength(0)-1; i >= 0; --i)
                {
                    if (Board[i, col] == FIELD.EMPTY)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public void AddToBoard(int col, FIELD value)
        {
            Console.WriteLine(col);
            if (col < Board.GetLength(1))
            {
                for (int i = Board.GetLength(0)-1; i >= 0; --i)
                {
                    if (Board[i, col] == FIELD.EMPTY)
                    {
                        Board[i, col] = value;
                        return;
                    }
                }
            }
        }

        public void PrintBoard()
        {
            for(int i = 0; i < Board.GetLength(0); ++i)
            {
                for (int j = 0; j < Board.GetLength(1); ++j)
                {
                    switch (Board[i,j]) {
                        case FIELD.EMPTY:
                            Console.Write("_");
                            break;
                        case FIELD.MIN:
                            Console.Write("O");
                            break;
                        case FIELD.MAX:
                            Console.Write("X");
                            break;
                        default:
                            break;
                    }
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        private void InitializeBoard()
        {
            for (int i = 0; i < Board.GetLength(0); ++i)
            {
                for (int j = 0; j < Board.GetLength(1); j++)
                {
                    Board[i, j] = FIELD.EMPTY;
                }
            }
        }
    }
}
