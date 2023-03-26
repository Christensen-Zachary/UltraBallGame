using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class FacLevelButton : MonoBehaviour
{
    [field: SerializeField]
    private GameObject LevelButtonPrefab { get; set; } // reference set in editor

    [field: SerializeField]
    public GameObject ButtonParent { get; set; } // reference set in editor 

    public ScrollLevelSelect scrollLevelSelect; // reference set in editor

    [field: SerializeField]
    public float ButtonPadding { get; set; } = 0.8f;


    private List<LevelButton> LevelButtons { get; set; } = new List<LevelButton>();
    private List<LevelButton> LevelButtonPool { get; set; } = new List<LevelButton>();

    public int HighestLevelButton => LevelButtons.Max(x => x.levelNumber);
    public int LowestLevelButton => LevelButtons.Min(x => x.levelNumber);
    public int TopRow => LevelButtons.Max(x => x.row);
    public int BottomRow => LevelButtons.Min(x => x.row);

    public LevelButton FirstLevelButton;
    public LevelButton LastLevelButton;

    private int latestLevelUnlocked = 1;

    private void Awake()
    {
        latestLevelUnlocked = ES3.Load<int>(BGStrings.ES_LATEST_UNLOCKED_LEVELNUM, 1);
    }

    public void ReturnRow(int row)
    {
        List<LevelButton> temp = new List<LevelButton>();
        LevelButtons.ForEach(x => temp.Add(x));
        temp.ForEach(x =>
        {
            if (x.row == row) ReturnLevelButton(x);
        });
    }

    private void ReturnLevelButton(LevelButton levelButton)
    {
        LevelButtons.Remove(levelButton);
        LevelButtonPool.Add(levelButton);

        levelButton.Return();
    }

    public LevelButton CreateLevelButton(int row, int col, int levelNum, bool isForwards)
    {
        GameObject obj;
        if (LevelButtonPool.Count > 0)
        {
            obj = LevelButtonPool[0].gameObject;
            LevelButtonPool.RemoveAt(0);
        }
        else obj = Instantiate(LevelButtonPrefab);

        InstantiateLevelButton(obj.GetComponent<LevelButton>(), row, col, levelNum, isForwards);
        return obj.GetComponent<LevelButton>();
    }


    private void InstantiateLevelButton(LevelButton levelButton, int row, int col, int levelNum, bool isForwards)
    {
        if (levelButton.levelNumber == 1) FirstLevelButton = null;
        if (levelNum == 1) FirstLevelButton = levelButton;
        if (levelButton.levelNumber == MainMenuUI.LAST_LEVEL_NUMBER) LastLevelButton = null;
        if (levelNum == MainMenuUI.LAST_LEVEL_NUMBER) LastLevelButton = levelButton;

        levelButton.levelNumber = levelNum;
        levelButton.row = row;
        levelButton.col = col;

        Transform button = levelButton.transform;
        button.name = $"LevelButton {Guid.NewGuid()}";
        button.SetParent(ButtonParent.transform);
        button.localScale = (1 - ButtonPadding) * scrollLevelSelect.unitScale * Vector3.one;
        button.localPosition = scrollLevelSelect.GetPosition(levelButton.col, levelButton.row);

        if (levelNum <= 0) // hide button
        {
            button.GetComponentInChildren<TextMeshPro>().text = "";
            button.transform.GetChild(1).gameObject.SetActive(false);
            levelButton.HideArrows();
            button.transform.GetComponent<SpriteRenderer>().color = Color.clear;
        }
        else
        {
            if (levelNum <= latestLevelUnlocked || levelNum == 1) // show unlocked button, never lock first level
            {
                button.GetComponentInChildren<TextMeshPro>().text = levelButton.levelNumber.ToString();
                button.transform.GetChild(1).gameObject.SetActive(false);
                button.transform.GetComponent<SpriteRenderer>().color = Color.white;
            }
            else // show locked button
            {
                button.GetComponentInChildren<TextMeshPro>().text = "";
                button.transform.GetChild(1).gameObject.SetActive(true);
                button.transform.GetChild(1).GetComponent<SpriteRenderer>().color = ThemeData.ThemeColors[ThemeItem.MaxDamage];
                button.transform.GetComponent<SpriteRenderer>().color = Color.white;
            }


            if (levelNum % scrollLevelSelect.columnCount == 0) // is last button in row
                levelButton.SetBetweenButtonUp();
            else if (isForwards)
                levelButton.SetBetweenButtonRight();
            else
                levelButton.SetBetweenButtonLeft();
        }

        LevelButtons.Add(levelButton);
    }

}
