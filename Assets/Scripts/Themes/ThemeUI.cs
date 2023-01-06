using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ThemeUI : MonoBehaviour
{
    public Animator animator;

    public void OpenThemePreview(int themeTypeInt)
    {
        StartCoroutine(OpenThemePreviewRoutine(themeTypeInt));
    }

    public IEnumerator OpenThemePreviewRoutine(int themeTypeInt)
    {
        animator.SetTrigger("Close");

        yield return new WaitForSeconds(MainMenuUI.SCENE_TRANSITION_WAIT_TIME);

        ThemeType themeType = (ThemeType)themeTypeInt;
        ThemeType currentTheme = ES3.Load<ThemeType>(BGStrings.ES_THEMETYPE, ThemeType.Default);
        ES3.Save(BGStrings.ES_THEMETYPEB4PREVIEW, currentTheme);

        ES3.Save(BGStrings.ES_THEMETYPE, themeType);

        SceneManager.LoadScene("ThemePreview");
    }

    public void OpenMainMenu()
    {
        StartCoroutine(OpenMainMenuRoutine());
    }

    public IEnumerator OpenMainMenuRoutine()
    {
        animator.SetTrigger("Close");

        yield return new WaitForSeconds(MainMenuUI.SCENE_TRANSITION_WAIT_TIME);

        SceneManager.LoadScene("MainMenu");
    }

}
