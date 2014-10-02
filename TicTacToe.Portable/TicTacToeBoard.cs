using System;

namespace TicTacToe.Portable
{
  public class TicTacToe
  {
    public static int WinCountX { get; private set; }
    public static int WinCountO { get; private set; }
    private static Player NewGameTurn = Player.X;
    public static readonly int BoardSize = 3;

    public Player PlayerTurn { get; private set; }
    public bool GameOver { get; private set; }
    private CellState[,] _gameBoard = new CellState[BoardSize, BoardSize];
    private int _turnsCompleted = 0;

    public TicTacToe()
    {
      GameOver = false;
      PlayerTurn = NewGameTurn;

      for (int i = 0; i < BoardSize; ++i) {
        for (int j = 0; j < BoardSize; ++j) {
          _gameBoard[i, j] = CellState.Default;
        }
      }
    }

    public CellState[,] NewMoveMade(int row, int column)
    {
      if (_gameBoard[row, column] != CellState.Default) {
        return _gameBoard;
      }

      CellState currentPlayerState = CellState.Default;
      if (PlayerTurn == Player.X) {
        currentPlayerState = CellState.NormalX;
      } else {
        currentPlayerState = CellState.NormalO;
      }

      _gameBoard[row, column] = currentPlayerState;

      CheckForWinner(currentPlayerState);
      TogglePlayer();
      NewGameTurn = PlayerTurn;
      return _gameBoard;
    }

    private void CheckRowsOrColumns(CellState searchState, bool rows)
    {
      for (int i = 0; i < BoardSize; ++i) {
        for (int j = 0; j < BoardSize; ++j) {
          // If we're checking rows, 'i' is the row and 'j' is the column.  If we're checking columns,
          // it's the other way round.  If any of the cells isn't the required searchState, it's not a win.
          if ((rows && _gameBoard[i, j] != searchState) || (!rows && _gameBoard[j, i] != searchState))
            return;
        }
        // Mark the row or column for the winning state
        CellState winnerState = SetGameWon();
        for (int j = 0; j < BoardSize; ++j) {
          if (rows)
            _gameBoard[i, j] = winnerState;
          else
            _gameBoard[j, i] = winnerState;
        }
      }
    }

    private void CheckDiagonals(CellState searchState)
    {
      // diagonal1 will represent [0,0] -> [n,n] and diagonal2 [0,n] -> [n,0]
      bool diagonal1wins = true;
      bool diagonal2wins = true;
      for (int i = 0; i < BoardSize; ++i) {
        if (_gameBoard[i, i] != searchState)
          diagonal1wins = false;
        if (_gameBoard[i, BoardSize - i - 1] != searchState)
          diagonal2wins = false;
        if (!diagonal1wins && !diagonal2wins)
          return;
      }

      // One or both of the diagonals wins, so mark the winning cells
      CellState winnerState = SetGameWon();
      for (int i = 0; i < BoardSize; ++i) {
        if (diagonal1wins)
          _gameBoard[i, i] = winnerState;
        if (diagonal2wins)
          _gameBoard[i, BoardSize - i - 1] = winnerState;
      }
    }

    private CellState SetGameWon()
    {
      if (PlayerTurn == Player.X) {
        if (!GameOver) {
          ++WinCountX;
          GameOver = true;
        }
        return CellState.WinX;
      }
      if (!GameOver) {
        ++WinCountO;
        GameOver = true;
      }
      return CellState.WinO;
    }

    private void CheckForWinner(CellState searchState)
    {
      CheckRowsOrColumns(searchState, true);
      CheckRowsOrColumns(searchState, false);
      CheckDiagonals(searchState);

      ++_turnsCompleted;
      if (_turnsCompleted == (BoardSize ^ 2))
        GameOver = true;
    }

    private void TogglePlayer() {
      if (PlayerTurn == Player.X) {
        PlayerTurn = Player.O;
        return;
      }

      PlayerTurn = Player.X;
    }
  }
}