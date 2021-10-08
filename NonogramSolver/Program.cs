using System;
using System.Collections.Generic;
using System.Linq;
using Sharprompt;

namespace NonogramSolver
{
    public class Program
    {
        static void Main(string[] args)
        {
            var width = Prompt.Input<int>("Board Width");
            var height = Prompt.Input<int>("Board Height");

            var columns = new int[width][];
            var rows = new int[height][];

            for (var i = 0; i < width; i++)
            {
                columns[i] = Prompt.Input<string>($"Enter Column {i + 1} (space delim)").Split().Select(s => int.Parse(s.Trim())).ToArray();
            }

            for (var i = 0; i < height; i++)
            {
                rows[i] = Prompt.Input<string>($"Enter Row {i + 1} (space delim)").Split().Select(s => int.Parse(s.Trim())).ToArray();
            }

            var board = new bool?[width, height];
            SolveBoard(board, columns, rows);

            PrintBoard(board);
        }

        private static void SolveBoard(bool?[,] board, int[][] columns, int[][] rows)
        {
            // while not all parts solved
            //   get all options for column\row
            //     try options on board
            //       for each possible option (based on current state) set any squares always or not set

            for (var x = 0; x < columns.Length; x++)
            {
                var boardCol = GetBoardColumn(x, board);
                var options = GetOptions(columns[x], board.GetLength(1));
            }

        }

        public static IEnumerable<bool[]> GetOptions(int[] segment, int length)
        {
            // spacing before each segment
            var spacing = segment.Select(i => 1).ToArray();
            spacing[0] = 0;

            var segmentSum = segment.Sum();

            // while sum of segments & spacing < length && spacing[1..n] != 1
            do
            {
                // create layout
                // increment
                // if (incrementing spacing[0] && sum >= length) then done...

                var layout = new bool[length];
                var x = 0;
                for (int i = 0; i < segment.Length; i++)
                {
                    // add 1 to spacing for [1..n]
                    for (int j = 0; j < spacing[i]; j++)
                    {
                        layout[x++] = false;
                    }

                    for (int j = 0; j < segment[i]; j++)
                    {
                        layout[x++] = true;
                    }
                }

                yield return layout;
            } while (Increment(spacing, length - segmentSum + 1));
        }

        public static bool Increment(int[] spacing, int maxSum)
        {
            for (int i = spacing.Length - 1; i >= 0; i--)
            {
                spacing[i]++;
                if (spacing.Sum() < maxSum)
                {
                    return true;
                }

                spacing[i] = 1;
            }

            return false;
        }

        private static bool?[] GetBoardColumn(int columnId, bool?[,] board)
        {
            var result = new bool?[board.GetLength(1)];
            for (int y = 0; y < board.GetLength(1); y++)
            {
                result[y] = board[columnId, y];
            }

            return result;
        }

        private static void PrintBoard(bool?[,] board)
        {
            var noFill = "░░";
            var fill = "██";

            for (int x = 0; x < board.GetLength(0); x++)
            {
                for (int y = 0; y < board.GetLength(1); y++)
                {
                    Console.Write(board[x, y] == null ? "  " : board[x,y].Value ? fill : noFill);
                }

                Console.WriteLine();
            }
        }
    }
}
