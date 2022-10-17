
using TMPro;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    //[field: SerializeField]
    private TextMeshProUGUI TextMesh { get; set; }

    public Color MaxHealthColor { get; set;}
    public Color MinHealthColor { get; set; }

    private void Awake()
    {
        TextMesh = GetComponent<TextMeshProUGUI>();

        ThemeVisitor.Visit(this);
    }

    public void SetNumber(int number)
    {
        TextMesh.text = number.ToString() + "%";
        TextMesh.color = Color.Lerp(MinHealthColor, MaxHealthColor, (number < 0 ? 0 : (number > 100 ? 1 : number / 100f)));
    }

    public void Subtract(int number)
    {
        try
        {
            int current = System.Convert.ToInt32(TextMesh.text.Substring(0, TextMesh.text.Length - 1));
            SetNumber(current - number);
        }
        catch
        {

            return;
        }
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}

