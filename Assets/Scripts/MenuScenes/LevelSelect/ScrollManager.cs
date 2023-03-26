using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollManager : MonoBehaviour
{
    [field: SerializeField]
    public FacLevelButton FacLevelButton { get; set; } // reference set in editor
    [field: SerializeField]
    public ScrollLevelSelect ScrollLevelSelect { get; set; } // reference set in editor
    [field: SerializeField]
    public ScrollLevelSelectUI ScrollLevelSelectUI { get; set; } // reference set in editor
    [field: SerializeField]
    public Transform ButtonParent { get; set; } // reference set in editor


    // used to get mouse position
    private Camera mainCamera;

    int rowCount = 10;
    int startRow = -2;
    public bool allowButtonSelect = true;
    private float distanceToDisableSelect;
    private float moveSinceStart = 0;
    private float residualScrollAmount = 0;
    private readonly float maxResidualScrollAmount = 0.4f;
    private readonly float residualScrollDuration = 1.5f;
    private float residualScrollTimer = 0;
    private float[] movementsOverTime = new float[3];
    private int movementCounter = 0;

    private Vector2 _lastMousePosition = Vector2.zero;
    private Vector2 _currentMousePosition = Vector2.zero;
    private Vector2 _deltaMousePosition = Vector2.zero;

    private float _buttonParentMoveAmount = 0; // relative to itself. Its starting position is 0
    private Vector2 bottomRowPosition;
    private float unitScale = 0;



    private void Start()
    {
        mainCamera = Camera.main;

        bottomRowPosition = ScrollLevelSelect.GetPosition(0, 0);
        unitScale = ScrollLevelSelect.unitScale;
        distanceToDisableSelect = unitScale / 4f;

        int levelNum = ScrollLevelSelect.latestLevelUnlocked;
        // if latest level is at the end of the row, set starting level to create to that level minus the columns plus 1
        if (levelNum % ScrollLevelSelect.columnCount == 0) levelNum = levelNum - ScrollLevelSelect.columnCount + 1;
        // otherwise subtract the amount the level is above the lowest level in the row plus 1
        else levelNum = levelNum - levelNum % ScrollLevelSelect.columnCount + 1;
        // startRow is negative, so subtract that many rows from the level num
        levelNum += startRow * ScrollLevelSelect.columnCount; 



        CreateRows(levelNum);
    }

    private void CreateRows(int levelNum)
    {
        for (int row = startRow; row < rowCount; row++)
        {
            CreateRow(false, row, levelNum);
            levelNum += ScrollLevelSelect.columnCount;
        }
    }

    private void CreateRow(bool isForwards, int row, int levelNum)
    {
        if (!isForwards)
        {
            for (int col = 0; col < ScrollLevelSelect.columnCount; col++)
            {
                FacLevelButton.CreateLevelButton(row, col, levelNum++);
            }
        }
        else
        {
            int tempLevelNum = levelNum + ScrollLevelSelect.columnCount - 1; // set to last level in row
            for (int col = 0; col < ScrollLevelSelect.columnCount; col++)
            {
                FacLevelButton.CreateLevelButton(row, col, tempLevelNum--);
            }

            for (int col = 0; col < ScrollLevelSelect.columnCount; col++)
            {
                levelNum++;
            }
        }
    }

    private void Update()
    {
        _currentMousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition); // must be at start of update
        _deltaMousePosition = _currentMousePosition - _lastMousePosition; // must be at start of update

        if (Input.GetMouseButtonDown(0))
        {
            moveSinceStart = 0;
            residualScrollAmount = 0;
            residualScrollTimer = 0;
            movementCounter = 0;
            for (int i = 0; i < movementsOverTime.Length; i++)
            {
                movementsOverTime[i] = 0;
            }
            allowButtonSelect = true;
        }

        if (Input.GetMouseButton(0))
        {
            if (allowButtonSelect)
            {
                moveSinceStart += _deltaMousePosition.y;

                if (Mathf.Abs(moveSinceStart) >= distanceToDisableSelect)
                {
                    allowButtonSelect = false;
                }
            }
            else
            {
                MoveButtonParent(_deltaMousePosition.y);

                movementsOverTime[movementCounter] = _deltaMousePosition.y;
                if (++movementCounter >= movementsOverTime.Length) movementCounter = 0;

            }
        }
        else if (residualScrollTimer < residualScrollDuration)
        {
            residualScrollTimer += Time.deltaTime;

            MoveButtonParent(residualScrollAmount);
            residualScrollAmount *= 0.97f;
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (allowButtonSelect) TryButtonSelect();
            else
            {
                foreach (var movement in movementsOverTime)
                {
                    if (Mathf.Abs(movement) > Mathf.Abs(residualScrollAmount))
                        residualScrollAmount = movement;
                }

                if (Mathf.Abs(residualScrollAmount) > maxResidualScrollAmount)
                {
                    residualScrollAmount = Mathf.Sign(residualScrollAmount) * maxResidualScrollAmount;
                }
            }
        }

        _lastMousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition); // must be at end of update
    }

    private void MoveButtonParent(float amount)
    {
        bool movingUp = amount > 0;
        bool movingDown = amount < 0;
        if (movingUp && FacLevelButton.FirstLevelButton != null)
        {
            if (FacLevelButton.FirstLevelButton.transform.position.y >= bottomRowPosition.y)
                return;
        }
        else if (movingDown && FacLevelButton.LastLevelButton != null)
        {
            if (FacLevelButton.LastLevelButton.transform.position.y <= bottomRowPosition.y)
                return;
        }

        _buttonParentMoveAmount += amount;
        if (_buttonParentMoveAmount >= unitScale && _buttonParentMoveAmount > 0) // moved up a row
        {
            RotateTopToBottom();
        }
        else if (_buttonParentMoveAmount <= -unitScale && _buttonParentMoveAmount < 0) // moved down a row
        {
            RotateBottomToTop();
        }

        ButtonParent.Translate(new Vector3(0, amount, 0));
    }

    private void RotateBottomToTop()
    {
        _buttonParentMoveAmount += unitScale;
        ReturnBottomRow();
        AddTopRow();
    }

    private void RotateTopToBottom()
    {
        _buttonParentMoveAmount -= unitScale;
        ReturnTopRow();
        AddBottomRow();
    }

    private void AddTopRow()
    {
        int levelNum = FacLevelButton.HighestLevelButton + 1;

        int row = FacLevelButton.TopRow + 1;
        CreateRow(false, row, levelNum);
        //for (int col = 0; col < ScrollLevelSelect.columnCount; col++)
        //{
        //    FacLevelButton.CreateLevelButton(row, col, levelNum++);
        //}
    }

    private void ReturnTopRow()
    {
        FacLevelButton.ReturnRow(FacLevelButton.TopRow);
    }

    private void AddBottomRow()
    {
        int levelNum = FacLevelButton.LowestLevelButton - ScrollLevelSelect.columnCount;

        int row = FacLevelButton.BottomRow - 1;
        CreateRow(false, row, levelNum);
        //for (int col = 0; col < ScrollLevelSelect.columnCount; col++)
        //{
        //    FacLevelButton.CreateLevelButton(row, col, levelNum++);
        //}
    }

    private void ReturnBottomRow()
    {
        FacLevelButton.ReturnRow(FacLevelButton.BottomRow);
    }

    private void TryButtonSelect()
    {
        Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mousePosition = new Vector3(mousePosition.x, mousePosition.y, 0);
        if (ScrollLevelSelect.backgroundSpriteRenderer.bounds.Contains(mousePosition)) // then allow click
        {
            RaycastHit2D[] hits = Physics2D.RaycastAll(mousePosition, Vector2.zero, 0);
            foreach (var hit in hits)
            {
                if (hit.transform.TryGetComponent(out LevelButton levelButton))
                {
                    if (levelButton.levelNumber > ScrollLevelSelect.highestLevel) break; // do not load levels higher than highestLevel
                    if (levelButton.levelNumber > ScrollLevelSelect.latestLevelUnlocked) break; // do not load locked levels
                    if (levelButton.levelNumber <= 0) break; // do not load levels below 1

                    ScrollLevelSelectUI.OpenLevel(levelButton.levelNumber);
                    allowButtonSelect = false;
                    break;
                }
            }
        }
    }
}
