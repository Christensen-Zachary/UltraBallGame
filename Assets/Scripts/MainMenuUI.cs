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


    private void Awake()
    {
        for (int i = 0; i < 6; i++)
        {
            // create button to access menu of levels
            GameObject obj = Instantiate(LevelSetsButtonPrefab);
            obj.SetActive(transform);
            obj.GetComponent<BtnLoadLevel>().SetText((i * 50 + 1).ToString() + " - " + ((i + 1) * 50).ToString());
            obj.transform.SetParent(LevelSetsMenu.transform);

            Action<int> setListener = (capturedi) => { obj.GetComponent<Button>().onClick.AddListener(() => OpenLevelSets(capturedi)); };
            setListener(i);

            // create buttons for menu of levels
            obj = Instantiate(LevelSetMenuPrefab);

            LevelSetMenus.Add(obj);
            obj.GetComponent<CreateLevelsButtons>().CreateButtons(i * 50 + 1, (i + 1) * 50);
            obj.transform.SetParent(transform);
        }
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void LoadLevel(int levelNumber)
    {
        ES3.Save(BGStrings.ES_LEVELNUM, levelNumber);
        PlayGame();
    }

    public void OpenLevelSetsMenu()
    {
        ExitLevelMenus.SetActive(true);
        LevelSetsMenu.SetActive(true);
        MainPanel.SetActive(false);
    }

    public void CloseLevelSetsMenu()
    {
        LevelSetsMenu.SetActive(false);
        LevelSetMenus.ForEach(x => x.SetActive(false));
        ExitLevelMenus.SetActive(false);

        MainPanel.SetActive(true);
    }

    public void OpenLevelSets(int setNumber)
    {
        if (LevelSetMenus.Count > setNumber && setNumber >= 0)
        {
            LevelSetMenus[setNumber].SetActive(true);
            LevelSetsMenu.SetActive(false);
        }
    }
}
