using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SudokuButtons : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private SudokuBoard sudokuBoard;
    [SerializeField] private Button numpadPrefab;
    [SerializeField] private TMP_Text eraseButtonTMP;
    [SerializeField] private float textOpacity = 0.25f;

    private readonly TMP_Text[] _tmpArr = new TMP_Text[9];

    private void Start()
    {
        InitializeNumpad();
    }

    private void InitializeNumpad()
    {
        for (int i = 0; i < 9; i++)
        {
            var button = Instantiate(numpadPrefab, transform);
            var text = button.GetComponentInChildren<TMP_Text>();
            button.onClick.AddListener(() => NumberPressed(text));
            text.text = (i + 1).ToString();
            _tmpArr[i] = text;
        }
    }

    /// <summary>
    /// Fades out non-valid numbers on numpad
    /// </summary>
    public void SetNumpadOpacity(Cell selectedCell)
    {
        var validValues = GameManager.Instance.SudokuAlgo.GetValidValuesForPosition(selectedCell.Position);
        for (int i = 0; i < _tmpArr.Length; i++)
        {
            if (selectedCell.Modifiable && validValues.Contains(i + 1))
            {
                _tmpArr[i].color = Color.black;
                continue;
            }

            _tmpArr[i].color = ChangeAlpha(_tmpArr[i].color, textOpacity);
        }
    }

    /// <summary>
    /// Fades out erase button.
    /// </summary>
    public void SetEraseButtonOpacity(Cell selectedCell)
    {
        eraseButtonTMP.color = selectedCell.Modifiable ? Color.black : ChangeAlpha(eraseButtonTMP.color, textOpacity);
    }

    public void NumberPressed(TMP_Text text)
    {
        var selectedCell = playerController.GetSelectedCell();
        if (selectedCell != null && int.TryParse(text.text, out int value))
        {
            sudokuBoard.SetValue(selectedCell, value);
        }
    }

    public void Erase()
    {
        var selectedCell = playerController.GetSelectedCell();
        if (selectedCell != null)
        {
            sudokuBoard.EraseCell(selectedCell);
            SetNumpadOpacity(selectedCell);
            SetEraseButtonOpacity(selectedCell);
        }
    }

    private Color ChangeAlpha(Color current, float newAlpha)
    {
        var color = current;
        color.a = newAlpha;
        current = color;
        return current;
    }

    public void NewGame()
    {
        GameManager.Instance.ReloadCurrentScene();
    }
}