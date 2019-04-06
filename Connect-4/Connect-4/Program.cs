using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connect_4
{
    class Program
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
    }
}
