using System.Linq;
using UnityEngine;

public class SudokuBoard : MonoBehaviour
{
    [SerializeField] private Cell sudokuCell;
    private Sudoku _sudokuAlgo;

    /// <summary>
    /// Initializes Sudoku-Board cells and sets start values.
    /// </summary>
    public void InitializeBoard(Sudoku sudoku)
    {
        _sudokuAlgo = GameManager.Instance.SudokuAlgo;

        for (int x = 0; x < 9; x++)
        {
            for (int y = 0; y < 9; y++)
            {
                var cell = Instantiate(sudokuCell, transform);
                var pos = new Vector2Int(x, y);
                cell.InitializeCell(sudoku.GetValue(pos), pos);
            }
        }
    }

    public void SetValue(Cell cell, int value)
    {
        var validNumbers = _sudokuAlgo.GetValidValuesForPosition(cell.Position);
        if (validNumbers.Contains(value))
        {
            _sudokuAlgo.SetValue(cell.Position, value);
            cell.UpdateText(value);
            CheckWinningCondition();
        }
    }

    private void CheckWinningCondition()
    {
        //Checks first if all values are filled before checking if they are valid.
        if (_sudokuAlgo.AllValues.All(x => x != 0))
        {
            if (_sudokuAlgo.IsComplete())
                GameManager.Instance.GameOver();
        }
    }

    public void EraseCell(Cell selectedCell)
    {
        _sudokuAlgo.SetValue(selectedCell.Position, 0);
        selectedCell.Erase();
    }
}