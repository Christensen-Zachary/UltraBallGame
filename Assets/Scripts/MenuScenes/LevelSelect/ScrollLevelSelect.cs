using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Linq;

public class ScrollLevelSelect : MonoBehaviour
{
    [field: SerializeField]
    public GameObject LevelButtonPrefab { get; set; } // reference set in editor 
    [field: SerializeField]
    public GameObject BackgroundPrefab { get; set; } // reference set in editor 
    [field: SerializeField]
    public Image HeaderFade { get; set; } // reference set in editor 
    [field: SerializeField]
    public Image Header { get; set; } // reference set in editor 
    [field: SerializeField]
    public Image HeaderEdge { get; set; } // reference set in editor 
    [field: SerializeField]
    public Image FooterFade { get; set; } // reference set in editor 
    [field: SerializeField]
    public Image Footer { get; set; } // reference set in editor 
    [field: SerializeField]
    public Image FooterEdge { get; set; } // reference set in editor 

    [field: SerializeField]
    public GameObject ButtonParent { get; set; } // reference set in editor 

    [field: SerializeField]
    public float HeightPadding { get; set; } = 0.1f;
    [field: SerializeField]
    public float HeightWidthRatio { get; set; } = 0.2f;
    [field: SerializeField]
    public float ButtonPadding { get; set; } = 0.8f;
    
    private GameObject background;
    float height = 0f;
    float width = 0f;
    float unitScale = 0f;
    Vector2 origin = Vector2.zero;
    int columnCount = 5;

    bool startedScroll = false;
    Vector2 scrollStartPosition = Vector2.zero;
    Vector2 buttonParentStartPosition = Vector2.zero;
    float minDistanceToStartScroll = 0.5f;
    bool doSelectOnInputUp = true;
    Camera mainCamera;
    int startRow = 0;
    int endRow = 0;
    int latestLevelUnlocked = 1;
    int highestLevel = 500;
    int highestVisibleRow = 6;
    int minimumRow = -50;
    float rollDuration = 1f;
    float rollDecay = 0.99f;
    float rollTimer = 0;
    float[] lastMovements = new float[10];
    int lastMovementIndex = 0;
    float lastMovement = 0;
    

    private List<List<LevelButton>> levelButtons = new List<List<LevelButton>>();
    LevelButton firstLevelButton, lastLevelButton;

    private void Awake() 
    {
        latestLevelUnlocked = 1;// ES3.Load<int>(BGStrings.ES_LEVELNUM, 1);
        
        mainCamera = Camera.main;

        (height, width) = BGUtils.GetScreenSize();

        // always make grid width a ratio to the screen height. Should fit on iphone and everything, then fit to height
        // put image over top of screen that is same color as background that fades out. Buttons will go underneath but appear to fade out
        background = Instantiate(BackgroundPrefab);
        background.name = "Background";
        background.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.25f);
        background.transform.position = Vector3.zero;
        height = height * (1 - HeightPadding * 2);
        width = width * (1 - HeightPadding);//height * HeightWidthRatio;
        background.transform.localScale = new Vector3(width, height, 0);

        unitScale = width /columnCount;
        // bottom left corners
        origin = new Vector2(-background.GetComponent<SpriteRenderer>().bounds.extents.x, -background.GetComponent<SpriteRenderer>().bounds.extents.y) + new Vector2(0.5f, 0.5f) * unitScale;
        // five buttons per column
        // create 100 rows
        int levelNum = latestLevelUnlocked - (latestLevelUnlocked % 5) + 1;
        if (latestLevelUnlocked > 10) // if on greater levels than 10 create two lag rows, levelNum is start of lag row
        {
            startRow = -2;
            levelNum = latestLevelUnlocked - (latestLevelUnlocked % 10) - 9;
        }
        else if (latestLevelUnlocked > 5) // if on levels 5 - 10 only create one lag row, levelNum then must be 1
        {
            startRow = -1;
            levelNum = 1;
        }

        for (int row = startRow; row < 250; row++)
        {
            levelButtons.Add(new List<LevelButton>());
            for (int col = 0; col < columnCount; col++)
            {
                CreateLevelButton(levelNum, row, col);
                levelNum++;
            }
            endRow = row;
        }

        FooterFade.color = DivideColor(ThemeData.ThemeColors[ThemeItem.SuperBackground], 2);
        HeaderFade.color = DivideColor(ThemeData.ThemeColors[ThemeItem.SuperBackground], 2);//ThemeData.ThemeColors[ThemeItem.SuperBackground];
        FooterEdge.color = ThemeData.ThemeColors[ThemeItem.GameboardBorder];
        HeaderEdge.color = ThemeData.ThemeColors[ThemeItem.GameboardBorder];

        if (PlayerPrefs.GetInt(ToggleHDR.HDR_ENABLED_KEY, 1) == 1)
        {
            HeaderEdge.material.SetFloat("_Brightness", ThemeData.ThemeBorderBrightness / 12f);
            FooterEdge.material.SetFloat("_Brightness", ThemeData.ThemeBorderBrightness / 12f);
        }
        else
        {
            HeaderEdge.material.SetFloat("_Brightness", 3);
            FooterEdge.material.SetFloat("_Brightness", 3);
        }

        Header.color = ThemeData.ThemeColors[ThemeItem.SuperBackground];
        Footer.color = ThemeData.ThemeColors[ThemeItem.SuperBackground];
    }

    private void CreateLevelButton(int levelNum, int row, int col)
    {
        LevelButton levelButton = Instantiate(LevelButtonPrefab).GetComponent<LevelButton>();
        
        levelButtons[GetRowIndex(row)].Add(levelButton);
        InstantiateLevelButton(levelButton, row, col, levelNum);
    }

    private int GetRowIndex(int row)
    {
        return row - startRow;
    }

    private void InstantiateLevelButton(LevelButton levelButton, int row, int col, int levelNum)
    {
        //print($"Instantiating button for level {levelNum}");
        if (levelButton.levelNumber == 1) // was already first level and is being instantiated to something else
        {
            firstLevelButton = null;
        }
        else if (levelButton.levelNumber == highestLevel) // was already last level and is being instatiated to something else
        {
            lastLevelButton = null;
        }

        levelButton.levelNumber = levelNum;
        levelButton.row = row;
        levelButton.col = col;

        if (levelNum == 1)
        {
            firstLevelButton = levelButton;
        }
        else if (levelNum == highestLevel)
        {
            lastLevelButton = levelButton;
        }

        Transform button = levelButton.transform;
        button.name = $"LevelButton {Guid.NewGuid()}";
        button.SetParent(ButtonParent.transform);
        button.localScale = (1 - ButtonPadding) * unitScale * Vector3.one;
        button.localPosition = GetPosition(levelButton.col, levelButton.row);
        if (true)//(levelNum <= latestLevelUnlocked)
        {
            button.GetComponentInChildren<TextMeshPro>().text = levelButton.levelNumber.ToString();
            button.transform.GetChild(1).gameObject.SetActive(false);
        }
        else
        {
            button.GetComponentInChildren<TextMeshPro>().text = "";
            button.transform.GetChild(1).gameObject.SetActive(true);
            button.transform.GetChild(1).GetComponent<SpriteRenderer>().color = ThemeData.ThemeColors[ThemeItem.MaxDamage];
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (firstLevelButton == null) print($"FirstLevelButton is null");
            else print($"FirstLevelButton is not null");

            if (lastLevelButton == null) print($"LastLevelButton is null");
            else print($"LastLevelButton is not null");
        }

        if (Input.GetMouseButtonDown(0))
        {
            scrollStartPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            buttonParentStartPosition = ButtonParent.transform.position;
            startedScroll = true;
            doSelectOnInputUp = true;
            rollTimer = 0;

            for (int i = 0; i < lastMovements.Length; i++)
            {
                lastMovements[i] = 0;
            }
        }

        if (startedScroll)
        {
            if (Vector2.Distance(scrollStartPosition, mainCamera.ScreenToWorldPoint(Input.mousePosition)) > minDistanceToStartScroll)
            {
                doSelectOnInputUp = false;
                scrollStartPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
                buttonParentStartPosition = ButtonParent.transform.position;
            }

            if (!doSelectOnInputUp) // then scroll
            {
                // if level 1 is in the bottom row, do not allow to scroll up ButtonParent.y past 0
                // if highest level is in the highest visible row, do not allow to scroll down ButtonParent.y past it's position that many rows down
                MoveButtonParent(new Vector3(0, buttonParentStartPosition.y - scrollStartPosition.y + mainCamera.ScreenToWorldPoint(Input.mousePosition).y, 0));
            }
            

            if (Input.GetMouseButtonUp(0))
            {
                if (doSelectOnInputUp) // try to select button
                {

                }
                lastMovement = GetAverage(lastMovements);
                startedScroll = false;
            }
        }
        else if (rollTimer < rollDuration)
        {
            print($"Moving player residual amount");
            rollTimer += Time.deltaTime;
            MoveButtonParent(new Vector3(0, ButtonParent.transform.position.y + lastMovement, 0));
            lastMovement *= rollDecay;
        }
    }

    private void MoveButtonParent(Vector2 newPosition)
    {
        bool doMove = true;
        bool movingUp = newPosition.y - ButtonParent.transform.position.y > 0;
        bool movingDown = newPosition.y - ButtonParent.transform.position.y < 0;

        if (firstLevelButton != null && movingUp)
        {
            print($"Checking to stop first level");
            if (firstLevelButton.transform.position.y > GetPosition(0, 0).y) // if 1st level is in the bottom row and aligned with column
            {
                doMove = false;
            }
        }
        if (lastLevelButton != null && movingDown)
        {
            print($"Checking to stop last level");
            if (lastLevelButton.transform.position.y < GetPosition(0, highestVisibleRow).y)
            {
                doMove = false;
            }
        }

        if (doMove)
        {
            if (lastMovementIndex >= lastMovements.Length) lastMovementIndex = 0;
            lastMovements[lastMovementIndex++] = newPosition.y - ButtonParent.transform.position.y;
            ButtonParent.transform.position = newPosition;

            float moveAmount = Mathf.Abs(ButtonParent.transform.position.y - buttonParentStartPosition.y);
            while (moveAmount > unitScale) // moved a row
            {
                if (firstLevelButton != null && firstLevelButton.transform.position.y > GetPosition(0, minimumRow).y) break; // don't rotate while 1st level exists
                moveAmount -= unitScale;
                // rotate buttons
                if (ButtonParent.transform.position.y - buttonParentStartPosition.y > 0) // above start position, rotate a top row to the bottom
                {
                    int lowestLevelDisplayed = levelButtons[0].Min(x => x.levelNumber);
                    if (lowestLevelDisplayed == 1) break; // if level one is displayed, never put buttons underneath
                    //print($"Lowest Level Displayed {lowestLevelDisplayed}");
                    List<LevelButton> buttons = levelButtons[levelButtons.Count - 1];
                    levelButtons.RemoveAt(levelButtons.Count - 1);
                    levelButtons.Insert(0, buttons);
                    for (int i = 0; i < buttons.Count; i++)
                    {
                        InstantiateLevelButton(buttons[i], startRow - 1, i, lowestLevelDisplayed - 4 + i);
                    }
                    startRow--;
                    endRow--;
                }
                else // below start position, rotate a bottom row to the top
                {
                    int highestLevelDisplayed = levelButtons[levelButtons.Count - 1].Max(x => x.levelNumber);
                    //print($"Highest Level Displayed {highestLevelDisplayed}");
                    List<LevelButton> buttons = levelButtons[0];
                    levelButtons.RemoveAt(0);
                    levelButtons.Add(buttons);
                    for (int i = 0; i < buttons.Count; i++)
                    {
                        InstantiateLevelButton(buttons[i], endRow + 1, i, highestLevelDisplayed + 1 + i);
                    }
                    endRow++;
                    startRow++;
                }

                buttonParentStartPosition = ButtonParent.transform.position;
                scrollStartPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
                scrollStartPosition = new Vector3(scrollStartPosition.x, scrollStartPosition.y - minDistanceToStartScroll, 0);

            }
        }
    }

    private float GetAverage(float[] arr)
    {
        float sum = 0;
        for (int i = 0; i < arr.Length; i++)
        {
            sum += arr[i];
        }
        return sum / arr.Length;
    }

    private Vector2 GetPosition(float col, float row)
    {
        return origin + new Vector2(col, row) * new Vector2(unitScale, unitScale);
    }

    private Color DivideColor(Color color, float divisor)
    {
        return new Color(color.r / divisor, color.g / divisor, color.b / divisor, color.a);
    }

}
