# Tic-Tac-Toe WinForms (C#)

A simple Tic-Tac-Toe desktop game built with **C#** and **Windows Forms**.

## Requirements

- Windows
- Visual Studio 2022 (or newer) with .NET desktop workload

## Open and Run

1. Open `TicTacToeWinForms.sln` in Visual Studio.
2. Make sure `TicTacToeWinForms` is the startup project.
3. Press **F5** (or click **Start**) to run.

## Features

- 3x3 board
- Two game modes:
  - Player vs Player
  - Player vs Computer
- Mode selection at the start of each new game
- In Player vs Computer mode:
  - Human is **X** and moves first
  - Computer is **O**
  - Computer automatically plays after the human move
  - Computer logic:
    - Win in one move when possible
    - Block the player's win in one move
    - Otherwise prefer center, then corners, then any free cell
- Current turn display
- Win and draw detection
- Occupied cells cannot be clicked
- No extra moves allowed after game over
- **New Game** button to restart
