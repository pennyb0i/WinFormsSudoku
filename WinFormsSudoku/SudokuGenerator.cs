using System;

namespace WinFormsSudoku
{
    // Generator based on: https://www.geeksforgeeks.org/program-sudoku-generator/?ref=oin_asr7

    public class SudokuGenerator
    {
        private readonly int[,] _mat;
        private const int N = 9; // number of columns/rows.
        private readonly int _srn; // square root of N
        private const int K = 50; // No. Of missing digits

        // Constructor
        public SudokuGenerator()
        {
            // Compute square root of N
            var SRNd = Math.Sqrt(N);
            _srn = (int)SRNd;

            _mat = new int[N, N];
        }

        // Sudoku Generator
        public int[,] FillValues()
        {
            // Fill the diagonal of SRN x SRN matrices
            FillDiagonal();

            // Fill remaining blocks
            FillRemaining(0, _srn);

            // Remove Randomly K digits to make game
            removeKDigits();
            return _mat;
        }

        // Fill the diagonal SRN number of SRN x SRN matrices
        private void FillDiagonal()
        {
            for (var i = 0; i < N; i = i + _srn)

                // for diagonal box, start coordinates->i==j
                FillBox(i, i);
        }

        // Returns false if given 3 x 3 block contains num.
        private bool UnUsedInBox(int rowStart, int colStart, int num)
        {
            for (var i = 0; i < _srn; i++)
            for (var j = 0; j < _srn; j++)
                if (_mat[rowStart + i, colStart + j] == num)
                    return false;

            return true;
        }

        // Fill a 3 x 3 matrix.
        private void FillBox(int row, int col)
        {
            int num;
            for (var i = 0; i < _srn; i++)
            for (var j = 0; j < _srn; j++)
            {
                do
                {
                    num = RandomGenerator(N);
                } while (!UnUsedInBox(row, col, num));

                _mat[row + i, col + j] = num;
            }
        }

        // Random generator
        private int RandomGenerator(int num)
        {
            var rand = new Random();
            return (int)Math.Floor(rand.NextDouble() * num + 1);
        }

        // Check if safe to put in cell
        private bool CheckIfSafe(int i, int j, int num)
        {
            return UnUsedInRow(i, num) &&
                   UnUsedInCol(j, num) &&
                   UnUsedInBox(i - i % _srn, j - j % _srn, num);
        }

        // check in the row for existence
        private bool UnUsedInRow(int i, int num)
        {
            for (var j = 0; j < N; j++)
                if (_mat[i, j] == num)
                    return false;
            return true;
        }

        // check in the row for existence
        private bool UnUsedInCol(int j, int num)
        {
            for (var i = 0; i < N; i++)
                if (_mat[i, j] == num)
                    return false;
            return true;
        }

        // A recursive function to fill remaining 
        // matrix
        private bool FillRemaining(int i, int j)
        {
            //  System.out.println(i+" "+j);
            if (j >= N && i < N - 1)
            {
                i = i + 1;
                j = 0;
            }

            if (i >= N && j >= N)
                return true;

            if (i < _srn)
            {
                if (j < _srn)
                    j = _srn;
            }
            else if (i < N - _srn)
            {
                if (j == i / _srn * _srn)
                    j = j + _srn;
            }
            else
            {
                if (j == N - _srn)
                {
                    i = i + 1;
                    j = 0;
                    if (i >= N)
                        return true;
                }
            }

            for (var num = 1; num <= N; num++)
                if (CheckIfSafe(i, j, num))
                {
                    _mat[i, j] = num;
                    if (FillRemaining(i, j + 1))
                        return true;

                    _mat[i, j] = 0;
                }

            return false;
        }

        // Remove the K no. of digits to
        // complete game
        private void removeKDigits()
        {
            var count = K;
            while (count != 0)
            {
                var cellId = RandomGenerator(N * N) - 1;

                // System.out.println(cellId);
                // extract coordinates i  and j
                var i = cellId / N;
                var j = cellId % N;

                // System.out.println(i+" "+j);
                if (_mat[i, j] != 0)
                {
                    count--;
                    _mat[i, j] = 0;
                }
            }
        }
    }
}