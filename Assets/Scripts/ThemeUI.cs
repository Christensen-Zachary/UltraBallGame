using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ThemeUI : MonoBehaviour
{
    public void OpenThemePreview(int themeTypeInt)
    {
        ThemeType themeType = (ThemeType)themeTypeInt;
        ThemeType currentTheme = ES3.Load<ThemeType>(BGStrings.ES_THEMETYPE, ThemeType.Default);
        ES3.Save(BGStrings.ES_THEMETYPEB4PREVIEW, currentTheme);

        ES3.Save(BGStrings.ES_THEMETYPE, themeType);

        SceneManager.LoadScene("ThemePreview");
    }

    public void OpenMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

}
