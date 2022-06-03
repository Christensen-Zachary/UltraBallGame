
using TMPro;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    //[field: SerializeField]
    private TextMeshProUGUI TextMesh { get; set; }


    private void Awake()
    {
        TextMesh = GetComponent<TextMeshProUGUI>();
    }

    public void SetNumber(int number)
    {
        TextMesh.text = number.ToString() + "%";
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

