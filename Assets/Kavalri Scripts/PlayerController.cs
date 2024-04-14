using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private SudokuButtons sudokuButtons;
    private Camera _cam;

    private Cell _selectedCell;

    public Cell GetSelectedCell()
    {
        if (_selectedCell && _selectedCell.Modifiable)
            return _selectedCell;

        return null;
    }

    private void Start()
    {
        _cam = GetComponent<Camera>();
    }

    public void OnLeftClick(InputAction.CallbackContext context)
    {
        if (context.started)
            return;

        SelectSudokuCell();
    }

    public void Quit(InputAction.CallbackContext context)
    {
        if (context.started)
            return;
        Application.Quit();
    }


    private void SelectSudokuCell()
    {
        Vector2 mousePos = _cam.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        RaycastHit2D hitInfo = Physics2D.Raycast(mousePos, Vector2.zero, Mathf.Infinity);
        if (hitInfo.collider)
        {
            if (_selectedCell)
                _selectedCell.ResetColor();

            _selectedCell = hitInfo.collider.GetComponent<Cell>();
            if (!_selectedCell)
                return;

            sudokuButtons.SetNumpadOpacity(_selectedCell);
            sudokuButtons.SetEraseButtonOpacity(_selectedCell);
            _selectedCell.Select();
        }
    }
}