using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScrollLevelSelectUI : MonoBehaviour
{


    public Animator animator; // reference set in editor

    public void OpenMainMenu()
    {
        StartCoroutine(OpenMainMenuCoroutine());
    }

    private IEnumerator OpenMainMenuCoroutine()
    {
        animator.SetTrigger("Close");

        yield return new WaitForSeconds(MainMenuUI.SCENE_TRANSITION_WAIT_TIME);

        MainMenuUI.LoadMainMenu();
    }


    public void OpenLevel(int levelNumber)
    {
        StartCoroutine(OpenLevelRoutine(levelNumber));
    }

    private IEnumerator OpenLevelRoutine(int levelNumber)
    {
        animator.SetTrigger("Close");

        yield return new WaitForSeconds(MainMenuUI.SCENE_TRANSITION_WAIT_TIME);

        LoadGameLevel(levelNumber);
    }

    public static void LoadScrollLevelSelect()
    {
        SceneManager.LoadScene("ScrollLevelSelect");
    }

    public static void LoadGameLevel(int levelNumber)
    {
        ES3.Save(BGStrings.ES_LEVELNUM, levelNumber);
        UnityEngine.SceneManagement.SceneManager.LoadScene("Game");
    }
}
