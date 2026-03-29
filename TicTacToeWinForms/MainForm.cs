using System.Drawing;
using System.Windows.Forms;

namespace TicTacToeWinForms;

public class MainForm : Form
{
    private enum GameMode
    {
        PlayerVsPlayer,
        PlayerVsComputer
    }

    private readonly Button[,] _boardButtons = new Button[3, 3];
    private readonly Label _turnLabel;
    private readonly Button _newGameButton;

    private GameMode _gameMode = GameMode.PlayerVsPlayer;
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

        // In Player vs Computer mode, the human always plays as X.
        if (_gameMode == GameMode.PlayerVsComputer && _currentPlayer != 'X')
        {
            return;
        }

        MakeMove(clickedButton, _currentPlayer);

        if (TryFinishGame())
        {
            return;
        }

        _currentPlayer = GetOtherPlayer(_currentPlayer);
        _turnLabel.Text = $"Current turn: {_currentPlayer}";

        if (_gameMode == GameMode.PlayerVsComputer)
        {
            MakeComputerMove();
        }
    }

    private void MakeComputerMove()
    {
        if (_gameOver || _currentPlayer != 'O')
        {
            return;
        }

        Button? computerButton = FindWinningMove('O')
            ?? FindWinningMove('X')
            ?? GetCenterMove()
            ?? GetCornerMove()
            ?? GetFirstFreeMove();

        if (computerButton is null)
        {
            return;
        }

        MakeMove(computerButton, 'O');

        if (TryFinishGame())
        {
            return;
        }

        _currentPlayer = 'X';
        _turnLabel.Text = "Current turn: X";
    }

    private void MakeMove(Button button, char player)
    {
        button.Text = player.ToString();
        _movesPlayed++;
    }

    private bool TryFinishGame()
    {
        if (HasPlayerWon(_currentPlayer))
        {
            _gameOver = true;
            _turnLabel.Text = "Game over";
            MessageBox.Show($"Player {_currentPlayer} wins!", "Game Over", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return true;
        }

        if (_movesPlayed == 9)
        {
            _gameOver = true;
            _turnLabel.Text = "Game over";
            MessageBox.Show("It's a draw!", "Game Over", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return true;
        }

        return false;
    }

    private static char GetOtherPlayer(char player)
    {
        return player == 'X' ? 'O' : 'X';
    }

    private Button? FindWinningMove(char player)
    {
        for (int row = 0; row < 3; row++)
        {
            for (int col = 0; col < 3; col++)
            {
                if (!string.IsNullOrEmpty(_boardButtons[row, col].Text))
                {
                    continue;
                }

                _boardButtons[row, col].Text = player.ToString();
                bool isWinningMove = HasPlayerWon(player);
                _boardButtons[row, col].Text = string.Empty;

                if (isWinningMove)
                {
                    return _boardButtons[row, col];
                }
            }
        }

        return null;
    }

    private Button? GetCenterMove()
    {
        return string.IsNullOrEmpty(_boardButtons[1, 1].Text) ? _boardButtons[1, 1] : null;
    }

    private Button? GetCornerMove()
    {
        (int row, int col)[] corners =
        {
            (0, 0),
            (0, 2),
            (2, 0),
            (2, 2)
        };

        foreach (var (row, col) in corners)
        {
            if (string.IsNullOrEmpty(_boardButtons[row, col].Text))
            {
                return _boardButtons[row, col];
            }
        }

        return null;
    }

    private Button? GetFirstFreeMove()
    {
        for (int row = 0; row < 3; row++)
        {
            for (int col = 0; col < 3; col++)
            {
                if (string.IsNullOrEmpty(_boardButtons[row, col].Text))
                {
                    return _boardButtons[row, col];
                }
            }
        }

        return null;
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
        SelectGameMode();

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

    private void SelectGameMode()
    {
        var modeChoice = MessageBox.Show(
            "Choose game mode:\nYes = Player vs Computer\nNo = Player vs Player",
            "New Game",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question);

        _gameMode = modeChoice == DialogResult.Yes
            ? GameMode.PlayerVsComputer
            : GameMode.PlayerVsPlayer;
    }
}
