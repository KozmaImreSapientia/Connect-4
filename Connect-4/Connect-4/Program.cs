using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connect_4
{
    public class Program
    {
        private static bool usingDepth = false;
        private static int maxDepth = -1;

        private static bool usingTime = false;
        private static int maxTime = -1; //seconds
        private static int player = 1;

        static void Main(string[] args)
        {
            //GetCommandLineArguments(args);
            //BoardTest();
            //BoardTest2();
            //GameTest();
            State state = new State();
            List<State> closedList = new List<State>();
            int maxDepth = 3;
           

            AlphaBeta(state, closedList, Int32.MinValue, Int32.MaxValue, maxDepth, player);
        }

        /// <summary>
        /// Sets the control defining fields according to the given command line arguments, or to default test arguments
        /// </summary>
        /// <param name="args"> The command line arguments </param>
        private static void GetCommandLineArguments(string[] args)
        {
            for (int i = 0; i < args.Length; ++i)
            {
                switch (args[i])
                {
                    case "-d":
                        i++;
                        if (i < args.Length)
                        {
                            usingDepth = true;
                            maxDepth = Convert.ToInt32(args[i]);
                            if(maxDepth < 1)
                            {
                                usingDepth = false;
                                maxDepth = 0;
                            }
                        }
                        break;
                    case "-t":
                        i++;
                        if (i < args.Length)
                        {
                            usingTime = true;
                            maxTime = Convert.ToInt32(args[i]);
                            if (maxTime < 1)
                            {
                                usingTime = false;
                                maxTime = 0;
                            }
                        }
                        break;
                    default:
                        break;
                }
            }

            //default args for testing
            //usingDepth = true;
            //maxDepth = 10;

            //usingTime = true;
            //maxTime = 5;

        }

        private static void BoardTest()
        {
            State state = new State();

            for (int i = 0; i < 20; i++)
            {
                while (!state.CanAddToBoard(i % 7))
                {
                    i++;
                }
                state.AddToBoard(i % 7, State.FIELD.MAX);
                ++i;
                while (!state.CanAddToBoard(i % 7))
                {
                    i++;
                }
                state.AddToBoard(i % 7, State.FIELD.MIN);
            }

            state.PrintBoard();
        }


        

        private static void BoardTest2()
        {
            State state = new State();

            for (int i = 0; i < 20; i++)
            {
                if (state.CanAddToBoard(2))
                {
                    state.AddToBoard(2, State.FIELD.MAX);
                }
                state.AddToBoard(0, State.FIELD.MIN);
                state.PrintBoard();
            }

            state.PrintBoard();

            State state2 = new State(State.CopyBoard(state.Board));
            state2.AddToBoard(6,State.FIELD.MAX);
            state2.PrintBoard();
            state.PrintBoard();
        }

        private static void GameTest()
        {
            State.FIELD[,] board = new State.FIELD[State.HEIGHT, State.WIDTH];
            State s = new State(board);

            StartTwoPlayerGame(ref s);

            Console.ReadLine();
        }

        private static int max(int alpha, int childValue)
        {
            if(alpha > childValue)
            {
                return alpha;
            }
            else
            {
                return childValue;
            }
        }
        // get the current board
        private static int HeuristicEval(State state)
        {
            var minTurn=0;
            // TODO if max turn 
            if (player ==1)
            {
                 minTurn = -1;
                RoundA(ref state);
                
            }
            //TODO if min turn
            else
            {
                minTurn = 1;
                RoundB(ref state);

            }
            // ide meg jon  hogy leenorizze ha 4,3 ,2 re a maxnak es utana a min-nek utana meg return maxresult-minresult, persze itt vannaka sulyzok is hozza adva
            // computer turn
            var max4 = checkForWin(state,player,4);
            if(max4 > 0)
            {
                return 1000000;
            }
            var max3 = checkForWin(state, player, 3);
            var max2 = checkForWin(state, player, 2);
            // player turn
            var min4 = checkForWin(state, player, 4);
            if (min4 > 0)
            {
                return -1000000;
            }
            var min3 = checkForWin(state, player, 3);
            var min2 = checkForWin(state, player, 2);


            return (max4*100 + max3*10 + max2 *5 )-(min4 * 100 + min3 * 10 + min2 * 5);
        }

        private static int checkForWin(State state, int currentTurn, int positionsOnPlace)
        {
            var count = 0;
            // height
            for(int i=0;i< 6; i++)
            {
                // width
                for(int j = 0; j < 7; j++)
                {
                    if (state.Board[i,j].Equals( currentTurn))
                    {
                        count += verticalPositions(i, j, state, positionsOnPlace);
                        count += horizontalPositions(i, j, state, positionsOnPlace);
                        count += diagonalPositions(i, j, state, positionsOnPlace);
                    }
                }

            }
            return count;
        }

        private static int diagonalPositions(int row, int col, State state, int positionsOnPlace)
        {
            var consecutiveCount = 0;
            var totalCount = 0;
            //! check for diagonals ith positive slope
            var j = col;
            for (int i = 0; i < row; i++)
            {
                if (j > 6) break;

                else if (state.Board[i, j] == state.Board[row, col])
                {
                    consecutiveCount += 1;
                }
                else
                {
                    break;
                }
                // increment when row is incremented
                j += 1;

            }

            if (consecutiveCount >= positionsOnPlace)
            {
                totalCount+= 1;
            }
            //! check for diagonals with negative slope
            consecutiveCount = 0;
            j = col;
            for (int i = row; i < 0; i--)
            {
                if (j > 6) break;

                else if (state.Board[i, j] == state.Board[row, col])
                {
                    consecutiveCount += 1;
                }
                else
                {
                    break;
                }
                // increment when row is incremented
                j += 1;

            }
            if (consecutiveCount >= positionsOnPlace)
            {
                totalCount += 1;
            }
            return totalCount;
        }

        private static int horizontalPositions(int row, int col, State state, int positionsOnPlace)
        {
            var consecutiveCount = 0;
            for (int i = 0; i < col; i++)
            {
                if (state.Board[row, i] == state.Board[row, col])
                {
                    consecutiveCount += 1;
                }
                else
                {
                    break;
                }

            }

            if (consecutiveCount >= positionsOnPlace)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        private static int verticalPositions(int row, int col, State state, int positionsOnPlace)
        {
           var  consecutiveCount = 0;
            for(int i = 0; i < row; i++)
            {
                if (state.Board[i,col] == state.Board[row,col])
                {
                    consecutiveCount += 1;
                }
                else
                {
                    break;
                }

            }

            if(consecutiveCount>= positionsOnPlace)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        private static void incTokenCount(ref int tokencount, State.FIELD field)
        {
            if (field == State.FIELD.EMPTY)
            {
                tokencount = 0;
            }
            else
            {
                if (field == State.FIELD.MIN)
                {
                    if (tokencount < 0)
                    {
                        tokencount--;
                    }
                    else
                    {
                        tokencount = -1;
                    }
                }
                else
                {
                    if (tokencount > 0)
                    {
                        tokencount++;
                    }
                    else
                    {
                        tokencount = 1;
                    }
                }
            }
        }

        /// <summary>
        /// Decides is the game finished or not.
        /// </summary>
        /// <param name="state">Game state</param>
        /// <returns></returns>
        private static bool IsTerminal(State state)
        {
            State.FIELD f;
            return IsTerminalWithWinner(state, out f);
        }

        /// <summary>
        /// Decides is the game finished or not giving the winner.
        /// If the game is not finished then winner == State.FEILD.EMPTY
        /// </summary>
        /// <param name="state"></param>
        /// <param name="winner"></param>
        /// <returns></returns>
        private static bool IsTerminalWithWinner(State state, out State.FIELD winner)
        {
            int tokencount = 0;
            // - tokencount = enemy's token count
            // + tokencount = our token count

            bool brk = false; // break indicator, true when return value is decided

            // horizontal:
            for (int i = 0; i < state.Board.GetLength(0); ++i)
            {
                tokencount = 0;

                for (int j = 0; j < state.Board.GetLength(1); ++j)
                {
                    incTokenCount(ref tokencount, state.Board[i, j]);

                    if (Math.Abs(tokencount) >= 4)
                    {
                        brk = true;
                        break;
                    }
                }

                if (brk) { break; }
            }

            // verical:
            if (!brk)
            {
                for (int j = 0; j < state.Board.GetLength(1); ++j)
                {
                    tokencount = 0;

                    for (int i = 0; i < state.Board.GetLength(0); ++i)
                    {
                        incTokenCount(ref tokencount, state.Board[i, j]);

                        if (Math.Abs(tokencount) >= 4)
                        {
                            brk = true;
                            break;
                        }
                    }

                    if (brk) { break; }
                }
            }

            // main diagonal:
            if (!brk )
            {
                // lower part    .\
                bool isUsed = false;
                for (int i = 0; i < state.Board.GetLength(0); ++i)
                {
                    int x = i;
                    for (int j = 0; j < state.Board.GetLength(1); ++j)
                    {
                        tokencount = 0;
                        int y = j;
                        while (x < state.Board.GetLength(0) && y < state.Board.GetLength(1))
                        {
                            //Console.Write("[" + x + "," + y + "] ");
                            ++x;
                            ++y;
                            isUsed = false;
                        }
                        if (!isUsed)
                        {
                            isUsed = true;
                        }
                    }
                    //Console.WriteLine();
                }
                // lower part    \'
                Console.WriteLine("*");
                isUsed = false;
                for (int k = 0; k < state.Board.GetLength(1); ++k)
                {
                    int x = 0;
                    for (int j = k+1; j < state.Board.GetLength(1); ++j)
                    {
                        tokencount = 0;
                        int y = j;
                        while (x < state.Board.GetLength(0)-k && y < state.Board.GetLength(1))
                        {
                            //Console.Write("[" + x + "," + y + "] ");
                            ++x;
                            ++y;
                            isUsed = false;
                        }
                        if (!isUsed)
                        {
                            isUsed = true;
                        }
                    }
                    //Console.WriteLine();
                }
            }

            // secondary diagonal
            if (!brk)
            {
                // upper part    '/
                bool isUsed = false;
                for (int i = 0; i < state.Board.GetLength(0); ++i)
                {
                    int x = i;
                    for (int j = 0; j < state.Board.GetLength(1); ++j)
                    {
                        tokencount = 0;
                        int y = j;
                        while (x >= 0 && y < state.Board.GetLength(1))
                        {
                            //Console.WriteLine("[" + x + "," + y + "]");
                            --x;
                            ++y;
                            isUsed = false;
                        }
                        if (!isUsed)
                        {
                            isUsed = true;
                        }
                    }
                    //Console.WriteLine();
                }
                // lower part    /.
                Console.WriteLine("*");
                isUsed = false;
                for (int k = 1; k < state.Board.GetLength(1); ++k)
                {
                    int x = state.Board.GetLength(0) - 1;
                    for (int j = k; j < state.Board.GetLength(1); ++j)
                    {
                        tokencount = 0;
                        int y = j;
                        while (x >= k-1 && y < state.Board.GetLength(1))
                        {      
                            //Console.Write("[" + x + "," + y + "] ");
                            --x;
                            ++y;
                            isUsed = false;
                        }
                        if (!isUsed)
                        {
                            isUsed = true;
                        }
                    }
                    //Console.WriteLine();
                }
            }

            // result:
            if (Math.Abs(tokencount) >= 4)
            {
                if(tokencount > 0)
                {
                    winner = State.FIELD.MAX;
                }
                else
                {
                    winner = State.FIELD.MIN;
                }
                return true;
            }
            else
            {
                winner = State.FIELD.EMPTY;
                return false;
            }
        }

        /*from a given position creating a list ofevery possible steps in the next round.*/
        private static void CreateChildrenList(State state, List<State> clodesList, int player)
        {
            for(int i=0; i < State.WIDTH; i++)
            {
                State helper = new State(State.CopyBoard(state.Board));

                if (helper.CanAddToBoard(i))
                {
                    if (player == 1)
                    {
                        helper.AddToBoard(i, State.FIELD.MAX);
                    }
                    else
                    {
                        helper.AddToBoard(i, State.FIELD.MIN);
                    }
                }

                //if helper wasn't visited
                if (!clodesList.Contains(helper))
                {
                    state.Children.Add(helper);
                }

                //PrintStateWithHeader(helper);
            }

        }

        /*The apha - beta search to a specific depth.
         Player mens MIN (-1) or MAX (1) state.

         Initial calling: 
         AlphaBeta(state, closedList, Int32.MinValue, Int32.MaxValue, maxdepth, player)
         */
        private static int AlphaBeta(State state, List<State> closedList, int alpha, int beta, int depth, int player)
        {
            if (IsTerminal(state) || depth == 0)
            {
                return player * HeuristicEval(state);
            }

            CreateChildrenList(state, closedList, player);

            foreach (State child in state.Children)
            {
                closedList.Add(child);
                alpha = max(alpha, (-1) * AlphaBeta(child, closedList, (-1) * beta, (-1) * alpha, depth - 1, -player));
                //closedList.Add(child);

                if (alpha >= beta)
                {
                    return alpha;
                }
            }
            return alpha;
        }

        /// <summary>
        /// Starts the game with user interactions.
        /// </summary>
        /// <param name="state">Game state</param>
        private static void StartTwoPlayerGame(ref State state) {

            bool play = true;
            int roundNumber = 0;
            State.FIELD winner = State.FIELD.EMPTY;

            while (play)
            {
                Console.Clear();


                PrintStateWithHeader(state);

                if ( roundNumber % 2 == 0)
                {
                    Console.Write("> Player 1 (X) (A-MAX): ");
                    RoundA(ref state);
                }
                else
                {
                    Console.Write("> Player 2 (O) (B-MIN): ");
                    RoundB(ref state);
                    
                }
                
                if( IsTerminalWithWinner( state, out winner) )
                {
                    Console.Clear();
                    PrintStateWithHeader(state);
                    Console.WriteLine("Finished!");
                    Console.WriteLine("   Winner: " + (winner==State.FIELD.MAX?"MAX":"MIN") );
                    play = false;
                }

                ++roundNumber;
            }

        }

        /// <summary>
        /// A's round.
        /// </summary>
        /// <param name="state">Game state</param>
        private static void RoundA(ref State state)
        {
            int colNum = GetUserInput(ref state);
            state.AddToBoard(colNum, State.FIELD.MAX);
        }

        /// <summary>
        /// B's round.
        /// </summary>
        /// <param name="state">Game state</param>
        private static void RoundB(ref State state)
        {
            int colNum = GetUserInput(ref state);
            state.AddToBoard(colNum, State.FIELD.MIN);
        }

        /// <summary>
        /// Reads the button pressed by the user, ONLY 1 press is allowed.
        /// </summary>
        /// <param name="state">Game state</param>
        /// <returns></returns>
        private static int GetUserInput(ref State state)
        {
            bool validInput = false;
            int colNum = -1;

            while ( !validInput)
            {
                Console.Write("\n   Select column ("+ 1 +"-"+ State.WIDTH +"):");
                ConsoleKeyInfo _key = Console.ReadKey();

                try {
                    colNum = Int32.Parse(  " " + (char)_key.Key );

                    if (colNum >=1 && colNum <= State.WIDTH)
                    {
                        if( state.CanAddToBoard(colNum-1))
                        {
                            validInput = true;
                        }
                        else
                        {
                            Console.WriteLine(" --> The " + colNum +" column is full. Try different one.");
                        }                        
                    }
                    else
                    {
                        Console.WriteLine(" --> Invalid Input! Use number!");
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine(" --> Invalid Input! Use number!");
                }
            }
            return colNum - 1;
        }

        /// <summary>
        /// Prints the board with header with the column numbers.
        /// </summary>
        /// <param name="state">Game state</param>
        private static void PrintStateWithHeader(State state)
        {
            for (int i = 1; i <= State.WIDTH; ++i)
            {
                Console.Write(i + "  ");
            }
            Console.WriteLine();

            state.PrintBoard();
        }

    }
}
