using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [field: SerializeField]
    public ScrollRect ScrollRect { get; set; }

    [field: SerializeField]
    public GameObject MainPanel { get; set; }
    [field: SerializeField]
    public GameObject LevelSetsMenu { get; set; }
    [field: SerializeField]
    public GameObject LevelSetMenuPrefab { get; set; }
    [field: SerializeField]
    public List<GameObject> LevelSetMenus { get; set; }
    [field: SerializeField]
    public GameObject LevelSetsButtonPrefab { get; set; }
    [field: SerializeField]
    public GameObject ExitLevelMenus { get; set; }

    public Animator animator;

    private void Awake()
    {
        ThemeType b4Theme = ES3.Load(BGStrings.ES_THEMETYPEB4PREVIEW, ThemeType.Default);
        ThemeType currentTheme = ES3.Load(BGStrings.ES_THEMETYPE, ThemeType.Default);

        if (currentTheme != b4Theme)
        {
            ES3.Save(BGStrings.ES_THEMETYPE, b4Theme);
        }
    }

    public void PlayGame()
    {
        StartCoroutine(PlayGameCoroutine());
    }

    private IEnumerator PlayGameCoroutine()
    {
        animator.SetTrigger("Close");

        yield return new WaitForSeconds(MainMenuUI.SCENE_TRANSITION_WAIT_TIME);

        SceneManager.LoadScene("Game");
    }

    public void OpenThemeSelect()
    {
        StartCoroutine(OpenThemeSelectRoutine());
    }

    public IEnumerator OpenThemeSelectRoutine()
    {
        animator.SetTrigger("Close");

        yield return new WaitForSeconds(MainMenuUI.SCENE_TRANSITION_WAIT_TIME);

        SceneManager.LoadScene("ThemeSelect");
    }

    public void OpenLevelSetsMenu()
    {
        StartCoroutine(OpenLevelSetsCoroutine());
        
    }

    private IEnumerator OpenLevelSetsCoroutine()
    {
        animator.SetTrigger("Close");

        yield return new WaitForSeconds(MainMenuUI.SCENE_TRANSITION_WAIT_TIME);

        ScrollLevelSelectUI.LoadScrollLevelSelect();
    }
    
    public static void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public static readonly float SCENE_TRANSITION_WAIT_TIME = 0.5F;

}
