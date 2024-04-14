using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Cell : MonoBehaviour
{
    #region Serialized Fields

    [SerializeField] TMP_Text tmpText;
    [SerializeField] Image image;
    [SerializeField] Color highlightedColor;

    #endregion

    #region Properties

    public Vector2Int Position { get; private set; }
    public bool Modifiable { get; private set; }
    private Color DefaultColor => Color.white;

    #endregion


    /// <summary>
    /// Cell constructor. Separates starting cell from modifiable ones. 
    /// </summary>
    public void InitializeCell(int value, Vector2Int pos)
    {
        if (value != 0)
        {
            UpdateText(value);
        }
        else
        {
            tmpText.color = Color.blue;
            Modifiable = true;
        }

        Position = pos;
        gameObject.name = $"{pos.x} : {pos.y}";
    }

    public void UpdateText(int value)
    {
        tmpText.text = value.ToString();
    }

    public void Select()
    {
        image.color = highlightedColor;
    }

    public void ResetColor() => image.color = DefaultColor;

    public void Erase() => tmpText.text = "";
}