using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private Setting settings;
    [SerializeField] private GameOverWindow gameOverWindow;
    [SerializeField] private SudokuBoard board;

    public Sudoku SudokuAlgo { get; private set; }

    private Dictionary<SudokuDifficulty, int> RevealedCellsDictionary { get; } = new Dictionary<SudokuDifficulty, int>()
    {
        { SudokuDifficulty.Easy, 36 },
        { SudokuDifficulty.Medium, 32 },
        { SudokuDifficulty.Hard, 25 },
        { SudokuDifficulty.Expert, 23 }
    };

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);

        StartGame();
    }

    private void StartGame()
    {
        int revealedCells = RevealedCellsDictionary[settings.difficulty];
        SudokuAlgo = Sudoku.Generate(revealedCells);
        board.InitializeBoard(SudokuAlgo);
    }

    public void SetDifficulty(SudokuDifficulty newDifficulty)
    {
        settings.difficulty = newDifficulty;
        ReloadCurrentScene();
    }

    public void GameOver()
    {
        gameOverWindow.Show();
    }

    public void ReloadCurrentScene() => SceneManager.LoadScene("Sudoku");
}

[Serializable]
public enum SudokuDifficulty
{
    Easy,
    Medium,
    Hard,
    Expert
}