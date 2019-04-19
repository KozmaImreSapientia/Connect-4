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
            BoardTest();

        }

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
                        }
                        break;
                    case "-t":
                        i++;
                        if (i < args.Length)
                        {
                            usingTime = true;
                            maxTime = Convert.ToInt32(args[i]);
                        }
                        break;
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

        private static bool IsTerminal(State state)
        {
            return false;
        }

        private static void CreateChildrenList(State state, List<State> clodesList, int color)
        {
            for(int i=0; i < State.WIDTH; i++)
            {
                State helper = new State(state.Board);

                if (state.CanAddToBoard(i))
                {
                    if (color == 1)
                    {
                        state.AddToBoard(i, State.FIELD.MAX);
                    }
                    else
                    {
                        state.AddToBoard(i, State.FIELD.MIN);
                    }
                }

                //if helper wasn't visited
                if (!clodesList.Contains(helper))
                {
                    state.Children.Add(helper);
                }
            }

        }

        private static int AlphaBeta(State state, List<State> closedList, int alpha, int beta, int depth, int color)
        {
            if (IsTerminal(state) || depth == 0)
            {
                return color * HeuristicEval(state);
            }

            CreateChildrenList(state, closedList, color);

            foreach (State child in state.Children)
            {
                alpha = max(alpha, (-1) * AlphaBeta(state, closedList, alpha, beta, depth - 1, -color));
                
                //TODO: testing: before or after the recursion
                closedList.Add(child);

                if (alpha >= beta)
                {
                    return alpha;
                }
            }
            return alpha;
        }
    }
}
