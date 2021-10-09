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
            var size = Prompt.Input<int>("Board Size");

            var columns = new int[size][];
            var rows = new int[size][];

            for (var i = 0; i < size; i++)
            {
                columns[i] = Prompt.Input<string>($"Enter Column {i + 1} (space delim)").Split().Select(s => int.Parse(s.Trim())).ToArray();
            }

            for (var i = 0; i < size; i++)
            {
                rows[i] = Prompt.Input<string>($"Enter Row {i + 1} (space delim)").Split().Select(s => int.Parse(s.Trim())).ToArray();
            }

            var board = SolveBoard(rows, columns);

            PrintBoard(board);
        }

        public static bool[,] SolveBoard(int[][] columns, int[][] rows)
        {
            // while not all parts solved
            //   get all options for column\row
            //     try options on board
            //       for each possible option (based on current state) set any squares always or not set
            bool?[,] board = new bool?[columns.Length, rows.Length];

            while (board.Cast<bool?>().Any(i => i == null))
            {
                for (var x = 0; x < columns.Length; x++)
                {
                    var boardCol = GetBoardColumn(x, board);
                    var options = GetOptions(columns[x], board.GetLength(1))
                        .Where(o => IsValidOption(boardCol, o))
                        .ToArray();

                    for (int i = 0; i < boardCol.Length; i++)
                    {
                        if (boardCol[i] == null)
                        {
                            if (options.All(o => o[i] == true))
                            {
                                boardCol[i] = true;
                            }
                            else if (options.All(o => o[i] == false))
                            {
                                boardCol[i] = false;
                            }
                        }
                    }

                    SetBoardColumn(x, board, boardCol);
                }

                for (var y = 0; y < rows.Length; y++)
                {
                    var boardRow = GetBoardRow(y, board);
                    var options = GetOptions(rows[y], board.GetLength(0))
                        .Where(o => IsValidOption(boardRow, o))
                        .ToArray();

                    for (int i = 0; i < boardRow.Length; i++)
                    {
                        if (boardRow[i] == null)
                        {
                            if (options.All(o => o[i] == true))
                            {
                                boardRow[i] = true;
                            }
                            else if (options.All(o => o[i] == false))
                            {
                                boardRow[i] = false;
                            }
                        }
                    }

                    SetBoardRow(y, board, boardRow);
                }

                Console.Write(".");
            }

            var newBoard = new bool[columns.Length, rows.Length];
            for (int x = 0; x < board.GetLength(0); x++)
            {
                for (int y = 0; y < board.GetLength(1); y++)
                {
                    newBoard[x, y] = board[x, y].Value;
                }
            }

            Console.WriteLine();
            return newBoard;
        }

        public static bool IsValidOption(bool?[] board, bool[] option)
        {
            if (board.Length != option.Length)
                throw new InvalidOperationException();

            for (int i = 0; i < board.Length; i++)
            {
                if (board[i] != null && board[i].Value != option[i])
                {
                    return false;
                }
            }

            return true;
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

        private static void SetBoardColumn(int columnId, bool?[,] board, bool?[] column)
        {
            if (board.GetLength(1) != column.Length)
                throw new InvalidOperationException();

            for (int y = 0; y < board.GetLength(1); y++)
            {
                board[columnId, y] = column[y];
            }
        }

        private static bool?[] GetBoardRow(int rowId, bool?[,] board)
        {
            var result = new bool?[board.GetLength(0)];
            for (int x = 0; x < board.GetLength(0); x++)
            {
                result[x] = board[x, rowId];
            }

            return result;
        }

        private static void SetBoardRow(int rowId, bool?[,] board, bool?[] row)
        {
            if (board.GetLength(0) != row.Length)
                throw new InvalidOperationException();

            for (int x = 0; x < board.GetLength(0); x++)
            {
                board[x, rowId] = row[x];
            }
        }

        private static void PrintBoard(bool[,] board)
        {
            var noFill = "░░";
            var fill = "██";

            for (int x = 0; x < board.GetLength(0); x++)
            {
                for (int y = 0; y < board.GetLength(1); y++)
                {
                    Console.Write(board[x,y] ? fill : noFill);
                }

                Console.WriteLine();
            }
        }
    }
}
