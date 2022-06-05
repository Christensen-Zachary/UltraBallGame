using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectUI : MonoBehaviour
{
    [field: SerializeField]
    public GameObject LevelSelectMenu { get; set; }

    private void Awake()
    {
        int setNumber = ES3.Load(BGStrings.ES_LEVELSETNUMBER, 0);

        LevelSelectMenu.GetComponent<CreateLevelsButtons>().CreateButtons(setNumber * 50 + 1, (setNumber + 1) * 50);
        LevelSelectMenu.transform.SetParent(LevelSelectMenu.transform);
    }

    public void OpenMainMenu()
    {
        MainMenuUI.LoadMainMenu();
    }

    public static void LoadLevelSelect()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("LevelSelect");
    }

}
