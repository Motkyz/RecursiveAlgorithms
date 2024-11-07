using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecursiveAlgorithms.Utils
{
    public static class HanoiLogic
    {
        /// <summary>
        /// Specifies a peg in Tower of Hanoi.
        /// </summary>
        public enum Peg { P1, P2, P3 }

        public static async Task SolveHanoi(Peg src, Peg aux, Peg dst, int height, List<Move> moves, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (height < 1)
            {
                throw new ArgumentOutOfRangeException("invalid height");
            }

            if (height == 1)
            {
                moves.Add(new Move(src, dst));
            }
            else
            {
                await SolveHanoi(src, dst, aux, height - 1, moves, cancellationToken);
                moves.Add(new Move(src, dst));
                await SolveHanoi(aux, src, dst, height - 1, moves, cancellationToken);
            }
        }
    }
}
