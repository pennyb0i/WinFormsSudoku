using System.Linq;

namespace WinFormsSudoku
{
    public static class SudokuSolver
    {
        /// <summary>
        /// Recursive function to solve sudoku by using backtracking <para/>
        /// The puzzle is solved by repeatedly attempting to assign numbers into the grid, and backtracking if an incorrect position is found
        /// </summary>
        /// <param name="board">The sudoku playing board</param>
        /// <param name="row">The row to fill in, 0 by default</param>
        /// <param name="column">The column to fill in, 0 by default</param>
        /// <returns><see cref="bool"/> indicating whether the puzzle was successfully solved or not</returns>
        public static bool Solve(int[,] board, int row = 0, int column = 0)
        {
            while (true)
            {
                // If we reach the end of the puzzle without encountering any issues, the puzzle is solved
                if (row == board.GetLength(0) - 1 && column == board.GetLength(1)) return true;

                // Move to the next column is the current row is filled in completely
                if (column == board.GetLength(1))
                {
                    row++;
                    column = 0;
                }

                // If the current square is already occupied, move to the next column
                if (board[row, column] != 0)
                {
                    column += 1;
                    continue;
                }

                // Assign a number (1-9)
                for (var i = 1; i < 10; i++)
                {
                    // Check if it's valid to place the number in the given row and column
                    if (IsNumberValid(board, row, column, i))
                    {
                        // Assign the number with the assumption is correct
                        board[row, column] = i;

                        // Assign the next number
                        // If the puzzle is completely filled in with no errors, return true
                        if (Solve(board, row, column + 1)) return true;
                    }

                    // Assignment was incorrect, retry with a different number;
                    board[row, column] = 0;
                }

                return false;
            }
        }

        /// <summary>
        /// Validate the playing board to ensure that there are no duplicates in any row,column or matrix <para/>
        /// This is to ensure that invalid grids are refused before even trying to solve them
        /// </summary>
        /// <param name="board">The playing board</param>
        /// <returns><see cref="bool"/> indicating if the grid is playable or not</returns>
        public static bool IsBoardValid(int[,] board)
        {
            for (var row = 0; row < board.GetLength(0); row++)
            {
                for (var col = 0; col < board.GetLength(1); col++)
                {
                    var value = board[row, col];
                    if (value != 0)
                    {
                        // Temporarily clear the slot to avoid false positives
                        board[row, col] = 0;
                        if (!IsNumberValid(board, row, col, value))
                            return false;
                        board[row, col] = value;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Verify if the specified value fits in the specific grid column and row, following uniqueness rules of Sudoku<para/>
        /// Checks the row, column and box
        /// </summary>
        /// <param name="board">The sudoku playing board</param>
        /// <param name="row">The row to check</param>
        /// <param name="column">The column to check</param>
        /// <param name="value">The value to verify</param>
        /// <returns><see cref="bool"/> indicating whether the value can br assigned or not</returns>
        private static bool IsNumberValid(int[,] board, int row, int column, int value)
        {
            //Validate row using hashSet
            var rowValues = Enumerable
                .Range(0, board.GetLength(0))
                .Select(x => board[x, column])
                .ToHashSet();
            if (!rowValues.Add(value))
            {
                return false;
            }
        
            //Validate column using hashSet
            var columnValues = Enumerable
                .Range(0, board.GetLength(1))
                .Select(x => board[row, x])
                .ToHashSet();
            if (!columnValues.Add(value))
            {
                return false;
            }
        
            //Validate box (3*3 matrix)
            var boxRow = row - row % 3;
            var boxCol = column - column % 3;
            for (var i = 0; i < 3; i++)
            for (var j = 0; j < 3; j++)
                if (board[i + boxRow, j + boxCol] == value)
                    return false;
            return true;
        }
    }
}