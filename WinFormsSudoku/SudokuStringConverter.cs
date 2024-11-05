using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace WinFormsSudoku
{
    public static class SudokuStringConverter
    {
        /// <summary>
        /// Creates an ASCII representation of a sudoku grid, along with a bitmask for locked digits
        /// </summary>
        /// <param name="grid">The full sudoku grid as a 2D array of numbers</param>
        /// <param name="lockedGrid">The same grid, containing only the locked (readonly) digits</param>
        /// <returns>The sudoku grid written in ASCII characters</returns>
        public static string CreateGrid(int[,] grid, int[,] lockedGrid)
        {
            var bitString = "";
            for (var i = 0; i < 9; i++)
            {
                for (var j = 0; j < 9; j++)
                {
                    bitString += lockedGrid[i,j] == 0 ? "0" : "1";
                }
            }
            var gridString = "┏━┯━┯━┳━┯━┯━┳━┯━┯━┓";
            // Rows
            for (var i = 0; i < 9; i++)
            {
                gridString += "\n┃";
                // Columns
                for (var j = 0; j < 9; j++)
                {
                    gridString += $"{grid[i, j].ToString()}";
                    if ((j + 1) % 3 == 0 && j != 0)
                    {
                        gridString += "┃";
                    }
                    else
                    {
                        gridString += "│";
                    }
                }
                if(i == 2 || i == 5) 
                    gridString += "\n┣━┿━┿━╋━┿━┿━╋━┿━┿━┫";
            }
            gridString += "\n┗━┷━┷━┻━┷━┷━┻━┷━┷━┛";
            gridString = gridString.Replace('0', ' ');
            return bitString + Environment.NewLine + gridString;
        }

        /// <summary>
        /// Parses an ASCII representation of a sudoku grid into a 2D array of int
        /// </summary>
        /// <param name="grid">The sudoku grid written in ASCII characters</param>
        /// <param name="lockedGrid">The same grid, containing only the locked (readonly) digits</param>
        /// <returns>The sudoku grid as a 2D array of numbers</returns>
        /// <exception cref="ArgumentException">Thrown if the grid has an invalid amount of digits</exception>
        public static int[,] ParseGrid(string grid, out int[,] lockedGrid)
        {
            // Split the string into lines
            var lines = grid.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
            var bitMask = lines[0].ToCharArray().Select(x => x == '1').ToArray();
            // Trim every line in the string to clean it up
            for (var i = 1; i < lines.Length; i++)
            {
                lines[i] = lines[i].Trim();
            }
            grid = string.Join(Environment.NewLine, lines.Skip(1));
            
            // Remove all characters that aren't numbers
            const string numbersPattern = "[^0-9]";
            var result = grid.Replace(' ','0');
            result = Regex.Replace(result, numbersPattern, "");

            var gridChars = result.ToCharArray();
            if (gridChars.Length != 81)
            {
                throw new ArgumentException($"Invalid grid! Correct grid length is 81! Current grid length {gridChars.Length}");
            }

            var numericGrid = new int[9, 9];
            var numericGridLocked = new int[9, 9];
            
            for (var i = 0; i < 81; i++)
            {
                numericGrid[i / 9, i % 9] = int.Parse(gridChars[i].ToString());
                if (bitMask[i])
                {
                    numericGridLocked[i / 9, i % 9] = int.Parse(gridChars[i].ToString());
                }
            }
            
            lockedGrid = numericGridLocked;
            return numericGrid;
        }
    }
}