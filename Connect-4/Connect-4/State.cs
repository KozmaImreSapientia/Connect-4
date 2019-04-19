﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connect_4
{
    class State
    {
        /// <summary>
        /// Enum containing the possible types of fields present on the board
        /// </summary>
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

        /// <summary>
        /// Creates a new State with an empty board and without children
        /// </summary>
        public State()
        {
            this.Board = new FIELD[HEIGHT, WIDTH];
            InitializeBoard();
            this.Children = new List<State>();
        }

        /// <summary>
        /// Creates a new State with a given board and without children
        /// </summary>
        /// <param name="board"> The Field[,] board that correspunds to the state</param>
        public State(FIELD[,] board)
        {
            this.Board = board;
            this.Children = new List<State>();
        }

        /// <summary>
        /// Property for getting the list of children belonging to a state
        /// </summary>
        public List<State> Children
        {
            get { return this.children; }
            private set { this.children = value; }
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

        /// <summary>
        /// Method to check if a disc can be added to a column of the board
        /// </summary>
        /// <param name="col"> The column in which we want to add </param>
        /// <returns> True if a disc can be added </returns>
        public bool CanAddToBoard(int col)
        {
            if (col < Board.GetLength(1))
            {
                for (int i = Board.GetLength(0) - 1; i >= 0; --i)
                {
                    if (Board[i, col] == FIELD.EMPTY)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Adds a disc to the specified column
        /// </summary>
        /// <param name="col"> The column where the disc will be added </param>
        /// <param name="value"> The disc (FIELD) type that will be added </param>
        public void AddToBoard(int col, FIELD value)
        {
            Console.WriteLine(col);
            if (col < Board.GetLength(1))
            {
                for (int i = Board.GetLength(0) - 1; i >= 0; --i)
                {
                    if (Board[i, col] == FIELD.EMPTY)
                    {
                        Board[i, col] = value;
                        return;
                    }
                }
            }
            else
            {
                throw new Exception("Column out of range");
            }
        }

        /// <summary>
        /// Prints the current board
        /// </summary>
        public void PrintBoard()
        {
            for (int i = 0; i < Board.GetLength(0); ++i)
            {
                for (int j = 0; j < Board.GetLength(1); ++j)
                {
                    switch (Board[i, j])
                    {
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
