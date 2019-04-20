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

        static void Main(string[] args)
        {
            GetCommandLineArguments(args);
            //BoardTest();
            BoardTest2();
            /*State state = new State();
            List<State> closedList = new List<State>();
            int maxDepth = 3;
            int player = 1;

            AlphaBeta(state, closedList, Int32.MinValue, Int32.MaxValue, maxDepth, player);
            PrintStateWithHeader(state);*/
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

        private static int HeuristicEval(State state)
        {
            return 0;
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

        private static bool IsTerminal(State state)
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
            if ( ! brk )
            {
                for (int j = 0; j < state.Board.GetLength(1); ++j)
                {
                    tokencount = 0;

                    for (int i = 0; i < state.Board.GetLength(0); ++i)
                    {
                        incTokenCount( ref tokencount, state.Board[i, j] );

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
            if ( ! brk )
            {
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
                            Console.WriteLine("[" + x + "," + y + "]");
                            --x;
                            ++y;
                            isUsed = false;
                        }
                        if (!isUsed)
                        {
                           
                            isUsed = true;
                        }
                    }
                    Console.WriteLine();
                }
            }

            /*if ( ! brk )
            {
                // upper part
                for (int i=0; i< state.Board.GetLength(0); ++i)
                {
                    tokencount = 0;

                    for (int k=0; k<= i; ++k)
                    {
                        if (k == 0)
                        {
                            Console.WriteLine("[" + i + "," + 0 + "]");
                            incTokenCount(ref tokencount, state.Board[i, 0]);
                        }
                        else
                        {
                            Console.WriteLine("[" + (i - k) + "," + k + "]");
                            incTokenCount(ref tokencount, state.Board[i - k, k]);
                        }                     

                        if (Math.Abs(tokencount) >= 4)
                        {
                            brk = true;
                            break;
                        }
                    }
                    Console.WriteLine();
                }

                // lower part
                int n = state.Board.GetLength(0);
                int m = state.Board.GetLength(1);
                for (int i = n-5; i < state.Board.GetLength(0); ++i)
                {
                    tokencount = 0;

                    for (int k = 0; k <= i; ++k)
                    {
                        if (k == 0)
                        {
                            Console.WriteLine("[" + i + "," + 0 + "]");
                            incTokenCount(ref tokencount, state.Board[i, 0]);
                        }
                        else
                        {
                            Console.WriteLine("[" + (i - k) + "," + k + "]");
                            incTokenCount(ref tokencount, state.Board[i - k, k]);
                        }

                        if (Math.Abs(tokencount) >= 4)
                        {
                            brk = true;
                            break;
                        }
                    }
                    Console.WriteLine();
                }
                
            }

            // secondary diagonal:
            if ( ! brk )
            {

            }*/

            // result:
            if (Math.Abs(tokencount) >= 4)
            {
                // return tokencount > 0 ? 1 : 0;
                return true;
            }
            else
            {
                //return -1;
                return false;
            }
        }

        /*from a given position creating a list ofevery possible steps in the next round.*/
        private static void CreateChildrenList(State state, List<State> clodesList, int player)
        {
            for(int i=0; i < State.WIDTH; i++)
            {
                State helper = new State(state.Board);

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
         AlphaBeta(state, closedList, -inf, inf, maxdepth, player)
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
                alpha = max(alpha, (-1) * AlphaBeta(state, closedList, alpha, beta, depth - 1, -player));
                
                //TODO: testing: before or after the recursion
                closedList.Add(child);

                if (alpha >= beta)
                {
                    return alpha;
                }
            }
            return alpha;
        }

        private static void StartTwoPlayerGame(ref State state) {

            bool play = true;
            int roundNumber = 0;

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

                if( IsTerminal( state ))
                {
                    Console.Clear();
                    PrintStateWithHeader(state);
                    Console.WriteLine("Finished!");
                    play = false;
                }

                ++roundNumber;
            }

        } 

        private static void RoundA(ref State state)
        {
            int colNum = GetUserInput(ref state);
            state.AddToBoard(colNum, State.FIELD.MAX);
        }

        private static void RoundB(ref State state)
        {
            int colNum = GetUserInput(ref state);
            state.AddToBoard(colNum, State.FIELD.MIN);
        }

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

        private static void PrintStateWithHeader(State state)
        {
            for (int i = 1; i <= State.WIDTH; ++i)
            {
                Console.Write(i + " ");
            }
            Console.WriteLine();

            state.PrintBoard();
        }

    }
}
