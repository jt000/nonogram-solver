using System.Linq;
using Xunit;

namespace NonogramSolver
{
    public class NonogramSolverTests
    {
        public class Increment
        {
            [Fact]
            public void NonogramSolver_Increment_Simple()
            {
                var spacing = new[] {0};
                var maxSum = 2;

                var result = Program.Increment(spacing, maxSum);
                Assert.True(result);
                Assert.Equal(new[] {1}, spacing);
            }

            [Fact]
            public void NonogramSolver_Increment_SimpleExceedsSum()
            {
                var spacing = new[] {1};
                var maxSum = 2;

                var result = Program.Increment(spacing, maxSum);
                Assert.False(result);
            }

            [Fact]
            public void NonogramSolver_Increment_TwoDigit()
            {
                var spacing = new[] {0, 1};
                var maxSum = 3;

                var result = Program.Increment(spacing, maxSum);
                Assert.True(result);
                Assert.Equal(new[] {0, 2}, spacing);
            }

            [Fact]
            public void NonogramSolver_Increment_TwoDigitCarryover()
            {
                var spacing = new[] {0, 2};
                var maxSum = 3;

                var result = Program.Increment(spacing, maxSum);
                Assert.True(result);
                Assert.Equal(new[] {1, 1}, spacing);
            }

            [Fact]
            public void NonogramSolver_Increment_TwoDigitCarryoverExceeds()
            {
                var spacing = new[] {1, 1};
                var maxSum = 2;

                var result = Program.Increment(spacing, maxSum);
                Assert.False(result);
            }

            [Fact]
            public void NonogramSolver_Increment_SanityCheckCountToSumOf5()
            {
                var spacing = new[] {0, 1, 1};
                var expectedResults = new[]
                {
                    new[] {0, 1, 2},
                    new[] {0, 1, 3},
                    new[] {0, 2, 1},
                    new[] {0, 2, 2},
                    new[] {0, 3, 1},
                    new[] {1, 1, 1},
                    new[] {1, 1, 2},
                    new[] {1, 2, 1},
                    new[] {2, 1, 1},
                    new[] {0, 0, 0}
                };
                var maxSum = 5;

                for (var i = 0; i < expectedResults.Length; i++)
                {
                    var result = Program.Increment(spacing, maxSum);
                    if (i == expectedResults.Length - 1)
                    {
                        Assert.False(result);
                    }
                    else
                    {
                        Assert.True(result);
                        Assert.Equal(expectedResults[i], spacing);
                    }
                }
            }
        }

        public class GetOptions
        {
            [Fact]
            public void NonogramSolver_GetOptions_OneValue()
            {
                var results = Program.GetOptions(new[] {1}, 1).ToArray();

                Assert.Single(results);
                Assert.Equal(new[] {true}, results[0]);
            }

            [Fact]
            public void NonogramSolver_GetOptions_TwoOptionsOneValue()
            {
                var results = Program.GetOptions(new[] {1}, 2).ToArray();

                Assert.Equal(2, results.Length);
                Assert.Equal(new[] {true, false}, results[0]);
                Assert.Equal(new[] {false, true}, results[1]);
            }

            [Fact]
            public void NonogramSolver_GetOptions_TwoValues()
            {
                var results = Program.GetOptions(new[] {1, 3}, 5).ToArray();

                Assert.Equal(1, results.Length);
                Assert.Equal(new[] {true, false, true, true, true}, results[0]);
            }

            [Fact]
            public void NonogramSolver_GetOptions_TwoValuesThreeOptions()
            {
                var results = Program.GetOptions(new[] {1, 2}, 5).ToArray();

                Assert.Equal(3, results.Length);
                Assert.Equal(new[] {true, false, true, true, false}, results[0]);
                Assert.Equal(new[] {true, false, false, true, true}, results[1]);
                Assert.Equal(new[] {false, true, false, true, true}, results[2]);
            }
        }

        public class IsValidOption
        {
            [Fact]
            public void BoardIsNull()
            {
                var board = new bool?[] {null};
                var option = new bool[] {true};

                var result = Program.IsValidOption(board, option);
                Assert.True(result);
            }

            [Fact]
            public void BoardIsSame()
            {
                var board = new bool?[] { true };
                var option = new bool[] { true };

                var result = Program.IsValidOption(board, option);
                Assert.True(result);
            }

            [Fact]
            public void BoardIsDifferent()
            {
                var board = new bool?[] { true };
                var option = new bool[] { false };

                var result = Program.IsValidOption(board, option);
                Assert.False(result);
            }

            [Fact]
            public void SanityCheck()
            {
                var board = new bool?[] { true, false, null, null };
                var option = new bool[] { true, false, true, false };

                var result = Program.IsValidOption(board, option);
                Assert.True(result);
            }
        }

        public class SolveBoard
        {
            [Fact]
            public void SingleEntryBoard()
            {
                int[][] columns = {
                    new [] { 1 }
                };

                int[][] rows = {
                    new [] { 1 }
                };

                var board = Program.SolveBoard(columns, rows);

                var expected = new bool[,]
                {
                    { true }
                };

                Assert.Equal(expected, board);
            }

            [Fact]
            public void DblBoard()
            {
                int[][] columns = {
                    new [] { 1 },
                    new [] { 0 },
                };

                int[][] rows = {
                    new [] { 1 },
                    new [] { 0 }
                };

                var board = Program.SolveBoard(columns, rows);

                var expected = new bool[,]
                {
                    { true, false },
                    { false, false },
                };

                Assert.Equal(expected, board);
            }

            [Fact]
            public void TwoSegmentsBoard()
            {
                int[][] columns = {
                    new [] { 1, 1 },
                    new [] { 1 },
                    new [] { 1, 1 }
                };

                int[][] rows = {
                    new [] { 1, 1 },
                    new [] { 1 },
                    new [] { 1, 1 }
                };

                var board = Program.SolveBoard(columns, rows);

                var expected = new bool[,]
                {
                    { true, false, true },
                    { false, true, false },
                    { true, false, true },
                };

                Assert.Equal(expected, board);
            }

            [Fact]
            public void SanityCheck5x5()
            {
                int[][] columns = {
                    new [] { 2, 1 },
                    new [] { 5 },
                    new [] { 5 },
                    new [] { 5 },
                    new [] { 2 }
                };

                int[][] rows = {
                    new [] { 3 },
                    new [] { 4 },
                    new [] { 4 },
                    new [] { 4 },
                    new [] { 5 },
                };

                var board = Program.SolveBoard(columns, rows);

                var expected = new bool[,]
                {
                    { false, true, true, false, true },
                    { true, true, true, true, true },
                    { true, true, true, true, true },
                    { true, true, true, true, true },
                    { false, false, false, true, true },
                };

                Assert.Equal(expected, board);
            }
        }
    }
}