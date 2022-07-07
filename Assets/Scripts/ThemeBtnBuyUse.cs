using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ThemeBtnBuyUse : MonoBehaviour
{
    public bool _unlocked = false;
    public TextMeshProUGUI _text;

    public ThemeType _themeType = ThemeType.Default;

    private void Awake()
    {
        if (_unlocked)
        {
            _text.text = "Use";
        }
        else
        {
            _text.text = "Buy\n$4.99";
        }
    }



    public void BuyUse()
    {
        if (_unlocked)
        {
            ES3.Save(BGStrings.ES_THEMETYPE, _themeType);
            ES3.Save(BGStrings.ES_THEMETYPEB4PREVIEW, _themeType);

            SceneManager.LoadScene("ThemeSelect");
        }
        else
        {

        }
    }
}