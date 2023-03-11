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

    public static void LoadScrollLevelSelect()
    {
        SceneManager.LoadScene("ScrollLevelSelect");
    }
}
