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
    public ScrollLevelSelectUI ScrollLevelSelectUI { get; set; } // reference set in editor
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

    public GameObject betweenButtonSprite; // reference set in editor
    
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
    int endRow = 250; // this is also how many rows above your current level will be created on start
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
    bool rowDirection = true;

    public SpriteRenderer backgroundSpriteRenderer;
    

    private List<List<LevelButton>> levelButtons = new List<List<LevelButton>>();
    LevelButton firstLevelButton, lastLevelButton;

    private bool levelButtonSelected = false; // choke point to end routine after clicking button

    private void Awake() 
    {
        latestLevelUnlocked = 7;// ES3.Load<int>(BGStrings.ES_LEVELNUM, 1);
        
        mainCamera = Camera.main;

        (height, width) = BGUtils.GetScreenSize();

        // always make grid width a ratio to the screen height. Should fit on iphone and everything, then fit to height
        // put image over top of screen that is same color as background that fades out. Buttons will go underneath but appear to fade out
        background = Instantiate(BackgroundPrefab);
        background.name = "Background";
        // background.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.25f);
        background.transform.position = Vector3.zero;
        height = height * (1 - HeightPadding * 2);
        width = width * (1 - HeightPadding);//height * HeightWidthRatio;
        background.transform.localScale = new Vector3(width, height, 0);
        backgroundSpriteRenderer = background.GetComponent<SpriteRenderer>();

        unitScale = width /columnCount;
        // bottom left corners
        origin = new Vector2(-background.GetComponent<SpriteRenderer>().bounds.extents.x, -background.GetComponent<SpriteRenderer>().bounds.extents.y) + new Vector2(0.5f, 0.5f) * unitScale;
        // five buttons per column
        // create 100 rows
        int levelNum = latestLevelUnlocked - (latestLevelUnlocked % 5) + 1;
        //if (latestLevelUnlocked > 10) // if on greater levels than 10 create two lag rows, levelNum is start of lag row
        //{
        //    levelNum = latestLevelUnlocked - (latestLevelUnlocked % 10) - 9;
        //}
        //else if (latestLevelUnlocked > 5) // if on levels 5 - 10 only create one lag row, levelNum then must be 1
        //{
        //    levelNum = 1;
        //}
        startRow = 0;
        if (latestLevelUnlocked >= 10)
        {
            for (int i = latestLevelUnlocked; i > 10; i -= 5)
            {
                startRow -= 1;
                levelNum = 1;
            }
        }
        else
        {
            for (int i = latestLevelUnlocked; i > 5; i -= 5)
            {
                startRow -= 1;
                levelNum = 1;
            }
        }


        endRow = startRow + highestLevel / columnCount + 1;
        for (int row = startRow; row <= endRow; row++)
        {
            CreateRow(levelNum, row, rowDirection);
            levelNum += 5;
            rowDirection = !rowDirection;
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

    private void CreateRow(int levelNumber, int row, bool doLeftToRight)
    {
        levelButtons.Add(new List<LevelButton>());

        int col = doLeftToRight ? 0 : columnCount - 1;
        while (true)
        {
            if (doLeftToRight && !(col < columnCount))
            {
                break;
            }
            else if (doLeftToRight)
            {
                GameObject betweenButtonSprite = Instantiate(this.betweenButtonSprite);
                betweenButtonSprite.transform.SetParent(ButtonParent.transform);
                betweenButtonSprite.transform.localScale = unitScale / 4f * Vector3.one;
                if (col + 1 == columnCount)
                {
                    betweenButtonSprite.transform.localPosition = GetPosition(col, row + 0.5f);
                }
                else
                {
                    betweenButtonSprite.transform.localPosition = GetPosition(col + 0.5f, row);
                    betweenButtonSprite.transform.Rotate(new Vector3(0, 0, -90));
                }
            }

            if (!doLeftToRight && !(col >= 0))
            {
                break;
            }
            else if (!doLeftToRight)
            {
                GameObject betweenButtonSprite = Instantiate(this.betweenButtonSprite);
                betweenButtonSprite.transform.SetParent(ButtonParent.transform);
                betweenButtonSprite.transform.localScale = unitScale / 4f * Vector3.one;
                if (col - 1 == -1)
                {
                    betweenButtonSprite.transform.localPosition = GetPosition(col, row + 0.5f);
                }
                else
                {
                    betweenButtonSprite.transform.localPosition = GetPosition(col - 0.5f, row);
                    betweenButtonSprite.transform.Rotate(new Vector3(0, 0, 90));
                }
            }

            CreateLevelButton(levelNumber, row, col);
            levelNumber++;

            
            if (doLeftToRight)
            {
                // create between button to the right except the last one do above
                
                col++;
            }
            else
            {
                // create between button to the left except the last one do above
                betweenButtonSprite.transform.localPosition = GetPosition(col - 0.5f, row);
                col--;
            }
        }
    }

    private void PlaceRow(List<LevelButton> levelButtons, int levelNumber, int row, bool doLeftToRight)
    {
        int col = doLeftToRight ? 0 : columnCount - 1;
        while (true)
        {
            if (doLeftToRight && !(col < columnCount))
                break;
            else if (!(col >= 0))
                break;

            levelButtons[col].transform.localPosition = GetPosition(col, row);
            print($"Col is {col} while placing #{levelNumber}");
            InstantiateLevelButton(levelButtons[col], levelNumber, row, col);
            levelNumber++;

            if (doLeftToRight)
            {
                col++;
            }
            else
            {
                col--;
            }
        }
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
        print($"Just placed button #{levelNum} in col {col}");
        if (levelNum <= latestLevelUnlocked)
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
        if (levelButtonSelected) return;

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
                    Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
                    mousePosition = new Vector3(mousePosition.x, mousePosition.y, 0);
                    if (backgroundSpriteRenderer.bounds.Contains(mousePosition)) // then allow click
                    {
                        RaycastHit2D[] hits = Physics2D.RaycastAll(mousePosition, Vector2.zero, 0);
                        foreach (var hit in hits)
                        {
                            if (hit.transform.TryGetComponent(out LevelButton levelButton))
                            {
                                if (levelButton.levelNumber > highestLevel) break; // do not load levels higher than highestLevel
                                if (levelButton.levelNumber > latestLevelUnlocked) break; // do not load locked levels

                                ScrollLevelSelectUI.OpenLevel(levelButton.levelNumber);
                                levelButtonSelected = true;
                                break;
                            }
                        }
                    }
                }
                lastMovement = GetAverage(lastMovements);
                startedScroll = false;
            }
        }
        else if (rollTimer < rollDuration)
        {
            //print($"Moving player residual amount");
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
            //print($"Checking to stop first level");
            if (firstLevelButton.transform.position.y > GetPosition(0, 0).y) // if 1st level is in the bottom row and aligned with column
            {
                doMove = false;
            }
        }
        if (lastLevelButton != null && movingDown)
        {
            //print($"Checking to stop last level");
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

            /***   DO NOT EVEN ROTATE BUTTONS. IT IS CONVOLUTED. JUST CREATE ALL BUTTONS AND SCROLL    ***/

        }
    }

    // only get average of values of the same sign, whichever sign there is the most of
    private float GetAverage(float[] arr)
    {
        int positiveCount = 0;
        int negativeCount = 0;
        for (int i = 0; i < arr.Length; i++)
        {
            if (arr[i] >= 0) positiveCount++;
            else if (arr[i] < 0) negativeCount++;
        }

        int numbersUsedCount = 0;
        float sum = 0;
        for (int i = 0; i < arr.Length; i++)
        {
            if (arr[i] == 0) continue;

            if (positiveCount >= negativeCount)
            {
                if (arr[i] > 0)
                {
                    sum += arr[i];
                    numbersUsedCount++; 
                }
            }
            else
            {
                if (arr[i] < 0)
                {
                    sum += arr[i];
                    numbersUsedCount++;
                }
            }
        }

        if (numbersUsedCount == 0) numbersUsedCount = 1; // prevent divide by zero
        return sum / numbersUsedCount;
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
