using UnityEngine;

[CreateAssetMenu(fileName = "new Setting", menuName = "Settings")]
public class Setting : ScriptableObject
{
    public SudokuDifficulty difficulty = SudokuDifficulty.Easy;
}