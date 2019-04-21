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
        //private static State state = new State();
        //private static List<State> closedList = new List<State>();

        static void Main(string[] args)
        {
            GetCommandLineArguments(args);
            //BoardTest();
            //BoardTest2();
            //GameTest();

            State state = new State();
            

            /*state.AddToBoard(0, State.FIELD.MAX);
            state.AddToBoard(1, State.FIELD.MIN);
            state.AddToBoard(0, State.FIELD.MAX);
            state.AddToBoard(2, State.FIELD.MIN);
            state.AddToBoard(0, State.FIELD.MAX);
            PrintStateWithHeader(state);*/


            //maxDepth = 3;
            StartTwoPlayerGame(state);
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
                            if (maxDepth < 1)
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
            state2.AddToBoard(6, State.FIELD.MAX);
            state2.PrintBoard();
            state.PrintBoard();
        }

        private static void GameTest()
        {
            State.FIELD[,] board = new State.FIELD[State.HEIGHT, State.WIDTH];
            State s = new State(board);

            StartTwoPlayerGame(s);//ref s);

            Console.ReadLine();
        }

        private static int max(int alpha, int childValue)
        {
            if (alpha > childValue)
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
            var minTurn = 0;
            // TODO if max turn 
            //if (player ==1)
            //{
            //    minTurn = -1;
            //    RoundA(ref state);

            //}
            ////TODO if min turn
            //else
            //{
            //    minTurn = 1;
            //    RoundB(ref state);

            //}
            // ide meg jon  hogy leenorizze ha 4,3 ,2 re a maxnak es utana a min-nek utana meg return maxresult-minresult, persze itt vannaka sulyzok is hozza adva
            // computer turn
            var max4 = checkForWin(state, 1, 4) * 1000;
            if (max4 > 0)
            {
                return 1000000;
            }
            var max3 = checkForWin(state, 1, 3) * 100;
            var max2 = checkForWin(state, 1, 2) * 5;
            // player turn
            var min4 = checkForWin(state, -1, 4) * 1000;
            if (min4 > 0)
            {
                return -1000000;
            }
            var min3 = checkForWin(state, -1, 3) * 100;
            var min2 = checkForWin(state, -1, 2) * 5;


            return (max4 + max3 + max2) - (min4 + min3 + min2);
        }

        private static int checkForWin(State state, int currentTurn, int positionsOnPlace)
        {
            var count = 0;
            // height
            for (int i = 5; i >= 0; i--)
            {
                // width
                for (int j = 0; j < 7; j++)
                {
                    if (currentTurn == -1)
                    {
                        if (state.Board[i, j].Equals(State.FIELD.MIN))
                        {
                            count += verticalPositions(i, j, state, positionsOnPlace);
                            count += horizontalPositions(i, j, state, positionsOnPlace);
                            count += diagonalPositions(i, j, state, positionsOnPlace);
                            count += diagonalPositions2(i, j, state, positionsOnPlace);
                        }
                    }
                    else if (currentTurn == 1)
                    {
                        if (state.Board[i, j].Equals(State.FIELD.MAX))
                        {
                            count += verticalPositions(i, j, state, positionsOnPlace);
                            count += horizontalPositions(i, j, state, positionsOnPlace);
                            count += diagonalPositions(i, j, state, positionsOnPlace);
                            count += diagonalPositions2(i, j, state, positionsOnPlace);
                        }
                    }// maxon a lepes

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
            // up 
            for (int i = (row - positionsOnPlace) >= 0 ? (row - positionsOnPlace) : 0; i < row; i++)
            {
                try
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
                }
                catch (Exception e) { }
                // increment when row is incremented
                j += 1;

            }

            if (consecutiveCount >= positionsOnPlace)
            {
                totalCount += 1;
            }
            //! check for diagonals with negative slope
            consecutiveCount = 0;
            j = col;
            for (int i = row + 1; i < row + positionsOnPlace && i < 6; i++)
            {
                try
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
                }
                catch (Exception e) { }
                // increment when row is incremented
                j += 1;

            }
            if (consecutiveCount >= positionsOnPlace)
            {
                totalCount += 1;
            }
            return totalCount;
        }
        private static int diagonalPositions2(int row, int col, State state, int positionsOnPlace)
        {
            var consecutiveCount = 0;
            var totalCount = 0;
            //! check for diagonals ith positive slope
            var j = col;
            // up 
            for (int i = (row - positionsOnPlace) < 0 ? 0 : (row - positionsOnPlace); i < row; i++)
            {
                try
                {
                    if (j == 0) break;

                    else if (state.Board[i, j] == state.Board[row, col])
                    {
                        consecutiveCount += 1;
                    }
                    else
                    {
                        break;
                    }
                }
                catch (Exception e) { }
                // increment when row is incremented
                j -= 1;

            }

            if (consecutiveCount >= positionsOnPlace)
            {
                totalCount += 1;
            }
            //! check for diagonals with negative slope
            consecutiveCount = 0;
            j = col;
            for (int i = row + 1; i < row + positionsOnPlace && i < 6; i++)
            {
                try
                {
                    if (j == 0) break;

                    else if (state.Board[i, j] == state.Board[row, col])
                    {
                        consecutiveCount += 1;
                    }
                    else
                    {
                        break;
                    }
                }
                catch (Exception e) { }
                // increment when row is incremented
                j -= 1;

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
            for (int i = col; i < col + positionsOnPlace && i < 7; i++)
            {
                try
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
                catch (Exception e) { }
            }


            for (int i = col - 1; i < col - positionsOnPlace; i--)// or col-1 -> col-position->i--
            {
                try
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
                catch (Exception e) { }
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
            var consecutiveCount = 0;
            // checks for the bottom
            for (int i = row; i < row + positionsOnPlace && i < 6; i++)
            {
                try
                {
                    if (state.Board[i, col] == state.Board[row, col])
                    {
                        consecutiveCount += 1;
                    }
                    else
                    {
                        break;
                    }
                }
                catch (Exception e) { break; }
            }
            // checks for the top
            for (int i = row - 1; i < row - positionsOnPlace; i--)
            {
                try
                {
                    if (state.Board[i, col] == state.Board[row, col])
                    {
                        consecutiveCount += 1;
                    }
                    else
                    {
                        break;
                    }
                }
                catch (Exception) { }
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
            if (!brk)
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
                            //Console.Write("[" + x + "," + y + "]"+ tokencount + " ");
                            incTokenCount(ref tokencount, state.Board[x, y]);
                            ++x;
                            ++y;
                            isUsed = false;
                            if (Math.Abs(tokencount) >= 4)
                            {
                                brk = true;
                                break;
                            }
                        }
                        if (brk) { break; }
                        if (!isUsed)
                        {
                            isUsed = true;
                        }
                    }
                    //Console.WriteLine();
                    if (brk) { break; }
                }
            }
            if (!brk)
            {
                // upper part    \'
                bool isUsed = false;
                for (int k = 0; k < state.Board.GetLength(1); ++k)
                {
                    int x = 0;
                    for (int j = k + 1; j < state.Board.GetLength(1); ++j)
                    {
                        tokencount = 0;
                        int y = j;
                        while (x < state.Board.GetLength(0) - k && y < state.Board.GetLength(1))
                        {
                            //Console.Write("[" + x + "," + y + "] ");
                            incTokenCount(ref tokencount, state.Board[x, y]);
                            ++x;
                            ++y;
                            isUsed = false;
                            if (Math.Abs(tokencount) >= 4)
                            {
                                brk = true;
                                break;
                            }
                        }
                        if (!isUsed)
                        {
                            isUsed = true;
                        }
                        if (brk) { break; }
                    }
                    //Console.WriteLine();
                    if (brk) { break; }
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
                            //Console.Write("[" + x + "," + y + "] ");
                            incTokenCount(ref tokencount, state.Board[x, y]);
                            --x;
                            ++y;
                            isUsed = false;
                            if (Math.Abs(tokencount) >= 4)
                            {
                                brk = true;
                                break;
                            }
                        }
                        if (!isUsed)
                        {
                            isUsed = true;
                        }
                        if (brk) { break; }
                    }
                    //Console.WriteLine();
                    if (brk) { break; }
                }
            }
            if (!brk)
            {
                // lower part    /.
                bool isUsed = false;
                for (int k = 1; k < state.Board.GetLength(1); ++k)
                {
                    int x = state.Board.GetLength(0) - 1;
                    for (int j = k; j < state.Board.GetLength(1); ++j)
                    {
                        tokencount = 0;
                        int y = j;
                        while (x >= k - 1 && y < state.Board.GetLength(1))
                        {
                            //Console.Write("[" + x + "," + y + "] ");
                            --x;
                            ++y;
                            isUsed = false;
                            if (Math.Abs(tokencount) >= 4)
                            {
                                brk = true;
                                break;
                            }
                        }
                        if (!isUsed)
                        {
                            isUsed = true;
                        }
                        if (brk) { break; }
                    }
                    //Console.WriteLine();
                    if (brk) { break; }
                }
            }

            // result:
            if (Math.Abs(tokencount) >= 4)
            {
                if (tokencount > 0)
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
            for (int i = 0; i < State.WIDTH; i++)
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
                    child.AlphaBetaValue = alpha;
                    return alpha;
                }
            }

            state.AlphaBetaValue = alpha;
            return alpha;
        }

        private static int MyMax(List<State> children)
        {
            int max = 0;
            foreach (var item in children)
            {
                if (item.AlphaBetaValue > max)
                {
                    max = item.AlphaBetaValue;
                }
            }
            return max;
        }

        private static int AlphaBetaTimeAndDepth(State state, List<State> closedList, int alpha, int beta, int depth, int player, DateTime endTime)
        {
            if (IsTerminal(state) || depth == 0 || DateTime.Now >= endTime)
            {
                return player * HeuristicEval(state);
            }

            CreateChildrenList(state, closedList, player);

            foreach (State child in state.Children)
            {
                closedList.Add(child);
                alpha = max(alpha, (-1) * AlphaBeta(child, closedList, (-1) * beta, (-1) * alpha, depth - 1, -player));

                if (alpha >= beta)
                {
                    state.AlphaBetaValue = alpha;
                    return alpha;
                }
            }
            state.AlphaBetaValue = alpha;
            return alpha;
        }

        private static int AlphaBetaWithTime(State state, List<State> closedList, int alpha, int beta, int player)
        {
            DateTime now = DateTime.Now;
            DateTime endTime = now.AddSeconds(maxTime);
            int bestScore = Int32.MinValue;
            int i = 1;

            while (DateTime.Now < endTime)
            {
                int tempScore = AlphaBetaTimeAndDepth(state, closedList, Int32.MinValue, Int32.MaxValue, i, player, endTime);

                if (tempScore > bestScore)
                {
                    bestScore = tempScore;
                }

                i++;
            }

            state.AlphaBetaValue = bestScore;
            return bestScore;
        }

        /// <summary>
        /// Starts the game with user interactions.
        /// </summary>
        /// <param name="state">Game state</param>
        private static void StartTwoPlayerGame(State state)
        {//ref State state) {

            bool play = true;
            int roundNumber = 1;
            State.FIELD winner = State.FIELD.EMPTY;

            //State state = new State();
            List<State> closedList = new List<State>();
            int depth = 3;
            //int player = 1;
            state.AddToBoard(3, State.FIELD.MAX);
            while (play)
            {
                //Console.Clear();


                PrintStateWithHeader(state);

                if (roundNumber % 2 != 0)
                {
                    Console.Write("> Player (O) (B-MIN): ");
                    RoundB(ref state);
                }
                else
                {
                    Console.Write("> PC (X) (A-MAX)...\n");
                    RoundA(ref state, closedList, depth);
                }

                if (IsTerminalWithWinner(state, out winner))
                {
                    //Console.Clear();
                    PrintStateWithHeader(state);
                    Console.WriteLine("Finished!");
                    Console.WriteLine("   Winner: " + (winner == State.FIELD.MAX ? "Player A  = X (MAX)" : "Player B  = O (MIN)"));
                    Console.ReadLine();
                    play = false;
                }

                ++roundNumber;
            }

        }

        /// <summary>
        /// A's round.
        /// </summary>
        /// <param name="state">Game state</param>
        private static void RoundA(ref State state, List<State> closedList, int depth)
        {
            if (state.Children.Count > 0)
            {
                state.Children.Clear();
            }
            int score = AlphaBeta(state, closedList, Int32.MinValue, Int32.MaxValue, depth, player);

            bool moved = false;
            foreach (State child in state.Children)
            {
                if (Math.Abs(child.AlphaBetaValue) == Math.Abs(score) || MyMax(child.Children) == Math.Abs(score))
                {
                    moved = true;
                    state = child;
                    //PrintStateWithHeader(state);
                    break;
                }
            }
            if (!moved)
            {
                Random rand = new Random();
                int pos = rand.Next(0, 7);
                while (!state.CanAddToBoard(pos)){
                    Console.WriteLine(pos);
                    pos = rand.Next(0, 7);
                }
                state.AddToBoard(pos, State.FIELD.MAX);
            }

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

            while (!validInput)
            {
                Console.Write("\n   Select column (" + 1 + "-" + State.WIDTH + "):");
                ConsoleKeyInfo _key = Console.ReadKey();

                Console.WriteLine();
                try
                {
                    colNum = Int32.Parse(" " + (char)_key.Key);

                    if (colNum >= 1 && colNum <= State.WIDTH)
                    {
                        if (state.CanAddToBoard(colNum - 1))
                        {
                            validInput = true;
                        }
                        else
                        {
                            Console.WriteLine(" --> The " + colNum + " column is full. Try different one.");
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

        /// <summary>
        /// Testing another heuristic
        /// </summary>
        /// <param name="state">Game state</param>
        /// <returns></returns>
        private static int TestHeuristic(State state)
        {
            int defense = 0;    // danger factor
            int offense = 0;    // goodness factor

            // Danger Factor:
            defense += AtLeastNTokenInARow(state, 2, State.FIELD.MIN, 5, 3);    // 2= 30% of width(7)
            //defence +=

            // Goodness Factor:
            offense += AtLeastNTokenInARow(state, 2, State.FIELD.MAX, 5, 2);    // 2= 30% of width(7)
            //offense +=

            Console.WriteLine("Defense: " + defense);
            Console.WriteLine("Offense: " + offense);
            Console.WriteLine("Return:" + ((offense > defense) ? (offense - defense) : (defense - offense)));
            Console.WriteLine();

            return (offense > defense) ? (offense - defense) : (-(defense - offense));
        }

        // Ha egy sorban tobb mint 30% a van az ellenfelnek az mar veszelyt jelent
        private static int AtLeastNTokenInARow(State state, int n, State.FIELD field, int score_points, int danger_miltiplier)
        {
            int score = 0;
            int tokenPcs = 0;
            bool isUsed;

            // Horizontal
            for (int i = 0; i < state.Board.GetLength(0); ++i)
            {
                tokenPcs = 0;

                for (int j = 0; j < state.Board.GetLength(1); ++j)
                {
                    //if( i==5 && j == 3)
                    //{
                    //    Console.WriteLine("*");
                    //}
                    if (state.Board[i, j] == field)
                    {
                        // danger factor is bigger if 3 tokens are down from 4
                        score += ScoreCalc(tokenPcs, n, score_points, danger_miltiplier);
                        tokenPcs++;
                    }
                    else
                    {
                        tokenPcs = 0;
                    }
                }
            }

            // Vertical
            for (int j = 0; j < state.Board.GetLength(1); ++j)
            {
                tokenPcs = 0;

                for (int i = 0; i < state.Board.GetLength(0); ++i)
                {
                    if (state.Board[i, j] == field)
                    {
                        // danger factor is bigger if 3 tokens are down from 4
                        score += ScoreCalc(tokenPcs, n, score_points, danger_miltiplier);
                        tokenPcs++;
                    }
                    else
                    {
                        tokenPcs = 0;
                    }
                }
            }
            // Main Diagonal

            // lower part    .\
            isUsed = false;
            for (int i = 0; i < state.Board.GetLength(0); ++i)
            {
                int x = i;
                for (int j = 0; j < state.Board.GetLength(1); ++j)
                {
                    tokenPcs = 0;
                    int y = j;
                    while (x < state.Board.GetLength(0) && y < state.Board.GetLength(1))
                    {
                        //Console.Write("[" + x + "," + y + "]"+ tokencount + " ");
                        if (state.Board[x, y] == field)
                        {
                            // danger factor is bigger if 3 tokens are down from 4
                            score += ScoreCalc(tokenPcs, n, score_points, danger_miltiplier);
                            tokenPcs++;
                        }
                        else
                        {
                            tokenPcs = 0;
                        }
                        isUsed = false;
                        ++x;
                        ++y;
                    }
                    if (!isUsed)
                    {
                        isUsed = true;
                    }
                }
                //Console.WriteLine();
            }

            // upper part    \'
            isUsed = false;
            for (int k = 0; k < state.Board.GetLength(1); ++k)
            {
                int x = 0;
                for (int j = k + 1; j < state.Board.GetLength(1); ++j)
                {
                    tokenPcs = 0;
                    int y = j;
                    while (x < state.Board.GetLength(0) - k && y < state.Board.GetLength(1))
                    {
                        //Console.Write("[" + x + "," + y + "] ");
                        if (state.Board[x, y] == field)
                        {
                            // danger factor is bigger if 3 tokens are down from 4
                            score += ScoreCalc(tokenPcs, n, score_points, danger_miltiplier);
                            tokenPcs++;
                        }
                        else
                        {
                            tokenPcs = 0;
                        }
                        isUsed = false;
                        ++x;
                        ++y;
                    }
                    if (!isUsed)
                    {
                        isUsed = true;
                    }
                }
                //Console.WriteLine();
            }

            // Secondary Diagonal

            // upper part    '/
            isUsed = false;
            for (int i = 0; i < state.Board.GetLength(0); ++i)
            {
                int x = i;
                for (int j = 0; j < state.Board.GetLength(1); ++j)
                {
                    tokenPcs = 0;
                    int y = j;
                    while (x >= 0 && y < state.Board.GetLength(1))
                    {
                        //Console.Write("[" + x + "," + y + "] ");
                        if (state.Board[x, y] == field)
                        {
                            // danger factor is bigger if 3 tokens are down from 4
                            score += ScoreCalc(tokenPcs, n, score_points, danger_miltiplier);
                            tokenPcs++;
                        }
                        else
                        {
                            tokenPcs = 0;
                        }
                        isUsed = false;
                        --x;
                        ++y;
                    }
                    if (!isUsed)
                    {
                        isUsed = true;
                    }
                }
                //Console.WriteLine();
            }

            // lower part    /.
            isUsed = false;
            for (int k = 1; k < state.Board.GetLength(1); ++k)
            {
                int x = state.Board.GetLength(0) - 1;
                for (int j = k; j < state.Board.GetLength(1); ++j)
                {
                    tokenPcs = 0;
                    int y = j;
                    while (x >= k - 1 && y < state.Board.GetLength(1))
                    {
                        //Console.Write("[" + x + "," + y + "] ");
                        if (state.Board[x, y] == field)
                        {
                            // danger factor is bigger if 3 tokens are down from 4
                            score += ScoreCalc(tokenPcs, n, score_points, danger_miltiplier);
                            tokenPcs++;
                        }
                        else
                        {
                            tokenPcs = 0;
                        }
                        isUsed = false;
                        --x;
                        ++y;
                    }
                    if (!isUsed)
                    {
                        isUsed = true;
                    }
                }
                //Console.WriteLine();
            }


            return score;
        }

        /// <summary>
        /// Calculates additional score value for TestHeuristic()
        /// </summary>
        /// <param name="tokenPcs">FIELD's token number</param>
        /// <param name="n">token number</param>
        /// <param name="score_points">additional score when found at least "n" tokens after each other</param>
        /// <param name="danger_miltiplier">additional score if tokenPcs > 3</param>
        /// <returns></returns>
        private static int ScoreCalc(int tokenPcs, int n, int score_points, int danger_miltiplier)
        {
            int score = 0;
            // danger factor is bigger if 3 tokens are down from 4
            if (tokenPcs >= n)
            {
                if (tokenPcs >= 3)
                {
                    score += score_points * danger_miltiplier;
                }
                else
                {
                    score += score_points;
                }
            }
            else
            {
                // small score if needs later
            }
            return score;
        }


    }
}
