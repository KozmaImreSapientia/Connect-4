using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connect_4
{
    public class Program
    {
        static void Main(string[] args)
        {
            BoardTest();
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

        /*private static int AlphaBeta(State state, int alpha, int beta, int d)
        {
            if(IsTerminal(state) || d == 0)
            {
                return HeuristicEval(state);
            }

            if ()
            {

            }
        }

        private static int HeuristicEval(State state)
        {
            return 0;
        }

        private static bool IsTerminal(State state)
        {
            return false;
        }*/
    }
}
