using TMPro;
using UnityEngine;

public class GameOverWindow : MonoBehaviour
{
    [SerializeField] private TMP_Text timeText;

    private void Start()
    {
        gameObject.SetActive(false);
    }

    public void Show()
    {
        gameObject.SetActive(true);
        float timeInSeconds = Time.timeSinceLevelLoad;
        int minutes = Mathf.FloorToInt(timeInSeconds / 60);
        int seconds = Mathf.FloorToInt(timeInSeconds % 60);
        timeText.text = $"Time:\n{minutes:00}:{seconds:00}";
    }
}