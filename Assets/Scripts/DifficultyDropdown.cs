using TMPro;
using UnityEngine;

public class DifficultyDropdown : MonoBehaviour
{
    [SerializeField] Setting settings;
    [SerializeField] TMP_Dropdown dropdown;

    private void Start()
    {
        dropdown.onValueChanged.AddListener(HandeDropDownData);
        dropdown.SetValueWithoutNotify((int)settings.difficulty);
    }

    private void HandeDropDownData(int val)
    {
        GameManager.Instance.SetDifficulty((SudokuDifficulty)val);
    }
}