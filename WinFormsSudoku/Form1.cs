using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace WinFormsSudoku
{
    public partial class Form1 : Form
    {
        // Helper class used for generating sudoku grids
        private readonly SudokuGenerator _generator = new SudokuGenerator();
        // The hidden sudoku grid, used for all solving logic
        private int[,] _numberGrid = new int[9, 9];
        // Locked version of the hidden grid, used for saving the puzzle
        private int[,] _lockedNumberGrid = new int[9, 9];
        // Edited version of the hidden grid, based on the current state of the textBoxes. Used for saving the puzzle
        private int[,] _progressNumberGrid = new int[9, 9];
        // The visible sudoku grid, comprised of TextBoxes
        private readonly TextBox[,] _textBoxGrid = new TextBox[9, 9];
        private bool _gameInProgress;

        #region Initialization
        
        public Form1()
        {
            InitializeComponent();
        }
        
        private void Form1_Load(object sender, EventArgs e)
        {
            CreateSudokuGrid();
        }

        /// <summary>
        /// Initializes and populates the Sudoku grid with TextBox controls.
        /// Configures the size and properties of each TextBox to fit into a 9x9 grid.
        /// Adds the TextBox controls to the GroupBox and sets up event handling for input validation and updates.
        /// </summary>
        private void CreateSudokuGrid()
        {
            // Ensure that sudokuGroupBox is already added in the designer
            const int gridSize = 9; // 9x9 grid
            const int cellSize = 40; // Size of each cell

            // Set the size of the GroupBox to fit the grid
            gBoxSudoku.Size = new Size(gridSize * cellSize, gridSize * cellSize);

            for (var row = 0; row < gridSize; row++)
            for (var col = 0; col < gridSize; col++)
            {
                // Create a new TextBox control
                var textBox = new TextBox
                {
                    Name = $"{row}_{col}",
                    Width = cellSize,
                    Height = cellSize,
                    Multiline = true,
                    MaxLength = 1, // Allows only one digit
                    TextAlign = HorizontalAlignment.Center,
                    Font = new Font("Arial", 20, FontStyle.Bold),
                    Location = new Point(col * cellSize, row * cellSize)
                };

                // Add margins to 3*3 grids
                if (row % 3 == 0 && row != 0)
                    textBox.Top += 5;
                if (col % 3 == 0 && col != 0)
                    textBox.Left += 5;
                
                textBox.KeyPress += TextBox_KeyPress;

                var updateRow = row;
                var updateCol = col;
                textBox.TextChanged += (sender, e) =>
                {
                    UpdateTextBox(sender, updateRow, updateCol, textBox);
                };

                // Add the TextBox to the GroupBox
                gBoxSudoku.Controls.Add(textBox);
                _textBoxGrid[row, col] = textBox;
            }
        }

        #endregion

        #region Buttons

        /// <summary>
        /// Handles the click event for the "Generate/Clear" button.
        /// Clears the Sudoku grid and resets the button texts if a game is in progress.
        /// Otherwise, generates a new Sudoku puzzle and populates the grid with the generated numbers.
        /// </summary>
        /// <param name="sender">The source of the event, typically the button that was clicked.</param>
        /// <param name="e">The event data.</param>
        private void btnGenerateClear_Click(object sender, EventArgs e)
        {
            ClearGrid();
            if (_gameInProgress)
            {
                btnGenerateClear.Text = "Generate puzzle";
                btnLoadSave.Text = "Load puzzle";
                btnStartSolve.Text = "Start game";
                _gameInProgress = false;
            }
            else
            {
                _numberGrid = _generator.FillValues();
                AssignNumbersToGrid(_numberGrid);
            }
        }

        /// <summary>
        /// Handles the Load/Save button click event.
        /// If a game is in progress, saves the game state to a file.
        /// If no game is in progress, attempts to load a game state from a file
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An EventArgs that contains the event data.</param>
        private void btnLoadSave_Click(object sender, EventArgs e)
        {
            if (_gameInProgress)
            {
                SaveGameToFile();
            }
            else
            {
                LoadGameFromFile();
            }
        }

        /// <summary>
        /// Handles the click event for the "Start Solve" button. If a game is in progress, it updates the UI with the solved Sudoku grid,
        /// disables the button to prevent further clicks, and resets the color for non-readonly text boxes. If no game is in progress, it starts a new game.
        /// </summary>
        /// <param name="sender">The source of the click event.</param>
        /// <param name="e">The event data associated with the click event.</param>
        private void btnStartSolve_Click(object sender, EventArgs e)
        {
            if (_gameInProgress)
            {
                lblStatus.Text = "This is how it's done!";
                AssignNumbersToGrid(_numberGrid);
                btnStartSolve.Enabled = false;
                // Reset color for unlocked boxes
                foreach (var textBox in _textBoxGrid)
                {
                    if (!textBox.ReadOnly)
                        textBox.ResetBackColor();
                }
            }
            else
            {
                StartGame();
            }
        }
        
        #endregion

        #region TextBox

        /// <summary>
        /// Handles the KeyPress event for the TextBox controls within the Sudoku grid.
        /// Restricts input to only digits and control characters, disallowing '0'.
        /// Updates the status label to clear any previous messages upon key press.
        /// </summary>
        /// <param name="sender">The source of the event, typically a TextBox.</param>
        /// <param name="e">A KeyPressEventArgs that contains the event data.</param>
        private void TextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            lblStatus.Text = "";
            // Allow only digits and control keys (e.g., backspace)
            if (!char.IsDigit(e.KeyChar) || e.KeyChar == '0')
            {
                if (!char.IsControl(e.KeyChar))
                {
                    e.Handled = true;
                }
            }
        }
        
        // Kind of an event handler, but separate because extra parameters 
        private void UpdateTextBox(object sender, int updateRow, int updateCol, TextBox textBox)
        {
            if (!(sender is TextBox box)) 
                return;
            if (_gameInProgress)
            {
                if (box.Text.Length != 0)
                {
                    _progressNumberGrid[updateRow, updateCol] = int.Parse(box.Text);
                    if (_numberGrid[updateRow, updateCol] != int.Parse(box.Text))
                        textBox.BackColor = Color.IndianRed;
                }
                else
                {
                    _progressNumberGrid[updateRow, updateCol] = 0;
                    textBox.ResetBackColor();
                }
            }
            else
            {
                if (box.Text.Length != 0)
                    _numberGrid[updateRow, updateCol] = int.Parse(box.Text);
                else
                    _numberGrid[updateRow, updateCol] = 0;
            }
        }

        #endregion

        #region Utility methods

        /// <summary>
        /// Assigns numbers from a 2D array to the corresponding TextBox controls in the Sudoku grid.
        /// Updates the Text property of each TextBox to reflect the value in the grid.
        /// Optionally locks non-zero values in place, making them read-only and changing their background color.
        /// </summary>
        /// <param name="numGrid">The 2D array of integers representing the Sudoku grid values.</param>
        /// <param name="lockDigits">A boolean flag indicating whether to lock non-zero digits as read-only.</param>
        private void AssignNumbersToGrid(int[,] numGrid, bool lockDigits = false)
        {
            var rows = numGrid.GetLength(0);
            var cols = numGrid.GetLength(1);

            for (var row = 0; row < rows; row++)
            for (var col = 0; col < cols; col++)
                // Ensure that the corresponding TextBox exists
                if (_textBoxGrid[row, col] != null)
                {
                    // Set the Text property with the integer value, converting to string
                    _textBoxGrid[row, col].Text = numGrid[row, col] == 0 ? "" : numGrid[row, col].ToString();
                    if (lockDigits && numGrid[row, col] != 0)
                    {
                        _textBoxGrid[row, col].ReadOnly = true;
                        _textBoxGrid[row, col].BackColor = Color.LightGray;
                    }
                }
        }

        /// <summary>
        /// Clears the Sudoku grid by resetting all TextBox controls and the internal number grid.
        /// Unlocks the TextBox controls, clears their values, and resets their background colors.
        /// Enables the start/solve button and resets the status label.
        /// </summary>
        private void ClearGrid()
        {
            Array.Clear(_numberGrid, 0, _numberGrid.Length);
            Array.Clear(_progressNumberGrid, 0, _progressNumberGrid.Length);
            Array.Clear(_lockedNumberGrid, 0, _lockedNumberGrid.Length);
            foreach (var textBox in _textBoxGrid)
            {
                textBox.ReadOnly = false;
                textBox.Clear();
                textBox.ResetBackColor();
                lblStatus.Text = "";
            }
            btnStartSolve.Enabled = true;
        }

        /// <summary>
        /// Starts a new Sudoku game.
        /// Loads initial numbers into the grid, locks them in place, and validates the puzzle's solvability.
        /// Updates the game status and enables interaction based on the puzzle's solvability.
        /// </summary>
        private void StartGame()
        {
            // Load and lock the numbers to grid
            AssignNumbersToGrid(_numberGrid, true);
            _lockedNumberGrid = _numberGrid.Clone() as int[,];
            _progressNumberGrid = _numberGrid.Clone() as int[,];
            // Validate and pre-solve the grid for error correction when playing
            var isSolved = SudokuSolver.IsBoardValid(_numberGrid);
            isSolved = isSolved && SudokuSolver.Solve(_numberGrid);
            // If the game is solvable, start playing
            if (isSolved)
            {
                lblStatus.ResetForeColor();
                lblStatus.Text = "Have fun!";
                btnGenerateClear.Text = "Clear board";
                btnLoadSave.Text = "Save puzzle";
                btnStartSolve.Text = "Solve";
                _gameInProgress = true;
            }
            // If the game is unsolvable, unlock the grid and allow the player to make corrections
            else
            {
                lblStatus.ForeColor = Color.Red;
                lblStatus.Text = "Unsolvable puzzle!";
                foreach (var textBox in _textBoxGrid)
                {
                    textBox.ReadOnly = false;
                    textBox.ResetBackColor();
                }
            }
        }

        /// <summary>
        /// Saves the current state of the Sudoku game to a file.
        /// Prompts the user with a Save File Dialog to select the destination.
        /// The file is saved with a timestamp in its name and contains the current Sudoku grid.
        /// Shows a success or error message based on the result of the operation.
        /// </summary>
        private void SaveGameToFile()
        {
            using (var saveFileDialog = new SaveFileDialog())
            {
                var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                
                saveFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
                saveFileDialog.Title = "Save the puzzle";
                saveFileDialog.FileName = $"SudokuGrid_{timestamp}.txt";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    var filePath = saveFileDialog.FileName;
                    try
                    {
                        File.WriteAllText(filePath, SudokuStringConverter.CreateGrid(_progressNumberGrid, _lockedNumberGrid));
                        MessageBox.Show("File saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        /// <summary>
        /// Loads a Sudoku game from a user-selected file and parses the contents.
        /// Displays an error message if the file cannot be read or parsed.
        /// The loaded game content is converted into a 9x9 integer grid for processing by the application.
        /// </summary>
        private void LoadGameFromFile()
        {
            using (var openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
                openFileDialog.Title = "Load a puzzle";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    var filePath = openFileDialog.FileName;
                    try
                    {
                        // Load the locked grid, start the game and then add the rest of the digits filled in before saving
                        var content = File.ReadAllText(filePath);
                        var progressGrid = SudokuStringConverter.ParseGrid(content, out var lockedGrid);
                        _numberGrid = lockedGrid.Clone() as int[,];
                        StartGame();
                        _progressNumberGrid = progressGrid.Clone() as int[,];
                        AssignNumbersToGrid(_progressNumberGrid);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        #endregion
    }
}