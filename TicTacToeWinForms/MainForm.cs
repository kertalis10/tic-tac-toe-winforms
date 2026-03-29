using System.Drawing;
using System.Windows.Forms;

namespace TicTacToeWinForms;

public class MainForm : Form
{
    private readonly Button[,] _boardButtons = new Button[3, 3];
    private readonly Label _turnLabel;
    private readonly Button _newGameButton;

    private char _currentPlayer = 'X';
    private int _movesPlayed;
    private bool _gameOver;

    public MainForm()
    {
        Text = "Tic-Tac-Toe";
        StartPosition = FormStartPosition.CenterScreen;
        ClientSize = new Size(320, 420);
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;

        _turnLabel = new Label
        {
            AutoSize = false,
            TextAlign = ContentAlignment.MiddleCenter,
            Font = new Font(FontFamily.GenericSansSerif, 11, FontStyle.Bold),
            Dock = DockStyle.Top,
            Height = 50,
            Text = "Current turn: X"
        };

        _newGameButton = new Button
        {
            Text = "New Game",
            Dock = DockStyle.Bottom,
            Height = 45
        };
        _newGameButton.Click += (_, _) => StartNewGame();

        var boardPanel = new TableLayoutPanel
        {
            RowCount = 3,
            ColumnCount = 3,
            Dock = DockStyle.Fill,
            Padding = new Padding(10)
        };

        for (int i = 0; i < 3; i++)
        {
            boardPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 33.33f));
            boardPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33f));
        }

        for (int row = 0; row < 3; row++)
        {
            for (int col = 0; col < 3; col++)
            {
                var button = new Button
                {
                    Dock = DockStyle.Fill,
                    Font = new Font(FontFamily.GenericSansSerif, 24, FontStyle.Bold),
                    Tag = (row, col)
                };
                button.Click += BoardButton_Click;
                _boardButtons[row, col] = button;
                boardPanel.Controls.Add(button, col, row);
            }
        }

        Controls.Add(boardPanel);
        Controls.Add(_turnLabel);
        Controls.Add(_newGameButton);

        StartNewGame();
    }

    private void BoardButton_Click(object? sender, EventArgs e)
    {
        if (_gameOver || sender is not Button clickedButton)
        {
            return;
        }

        if (!string.IsNullOrEmpty(clickedButton.Text))
        {
            return;
        }

        clickedButton.Text = _currentPlayer.ToString();
        _movesPlayed++;

        if (HasPlayerWon(_currentPlayer))
        {
            _gameOver = true;
            MessageBox.Show($"Player {_currentPlayer} wins!", "Game Over", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        if (_movesPlayed == 9)
        {
            _gameOver = true;
            MessageBox.Show("It's a draw!", "Game Over", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        _currentPlayer = _currentPlayer == 'X' ? 'O' : 'X';
        _turnLabel.Text = $"Current turn: {_currentPlayer}";
    }

    private bool HasPlayerWon(char player)
    {
        string mark = player.ToString();

        for (int i = 0; i < 3; i++)
        {
            if (_boardButtons[i, 0].Text == mark && _boardButtons[i, 1].Text == mark && _boardButtons[i, 2].Text == mark)
            {
                return true;
            }

            if (_boardButtons[0, i].Text == mark && _boardButtons[1, i].Text == mark && _boardButtons[2, i].Text == mark)
            {
                return true;
            }
        }

        if (_boardButtons[0, 0].Text == mark && _boardButtons[1, 1].Text == mark && _boardButtons[2, 2].Text == mark)
        {
            return true;
        }

        if (_boardButtons[0, 2].Text == mark && _boardButtons[1, 1].Text == mark && _boardButtons[2, 0].Text == mark)
        {
            return true;
        }

        return false;
    }

    private void StartNewGame()
    {
        _currentPlayer = 'X';
        _movesPlayed = 0;
        _gameOver = false;
        _turnLabel.Text = "Current turn: X";

        for (int row = 0; row < 3; row++)
        {
            for (int col = 0; col < 3; col++)
            {
                _boardButtons[row, col].Text = string.Empty;
            }
        }
    }
}
