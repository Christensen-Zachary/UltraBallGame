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


    public void PlayGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void OpenLevelSetsMenu()
    {
        LevelSetsUI.LoadLevelSets();
    }

    
    public static void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

}
