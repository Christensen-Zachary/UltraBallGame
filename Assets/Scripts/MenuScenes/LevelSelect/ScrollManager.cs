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

    int levelNum = 1;
    int rowCount = 6 + 10; // 2 * abs(startRow) + numberOfCenterRows
    int startRow = -3;
    readonly int highestVisibleRow = 5;
    public bool allowButtonSelect = true;
    private float distanceToDisableSelect;
    private float moveSinceStart = 0;

    private Vector2 _lastMousePosition = Vector2.zero;
    private Vector2 _currentMousePosition = Vector2.zero;
    private Vector2 _deltaMousePosition = Vector2.zero;

    private float _buttonParentMoveAmount = 0; // relative to itself. Its starting position is 0
    private Vector2 bottomRowPosition;
    private Vector2 highestVisibleRowPosition;
    private float unitScale = 0;

    private void Start()
    {
        mainCamera = Camera.main;

        bottomRowPosition = ScrollLevelSelect.GetPosition(0, 0);
        highestVisibleRowPosition = ScrollLevelSelect.GetPosition(0, highestVisibleRow);
        unitScale = ScrollLevelSelect.unitScale;
        distanceToDisableSelect = unitScale / 4f;

        levelNum = ScrollLevelSelect.latestLevelUnlocked;
        if (levelNum % ScrollLevelSelect.columnCount == 0) levelNum = levelNum - ScrollLevelSelect.columnCount + 1;
        else levelNum = levelNum - levelNum % ScrollLevelSelect.columnCount + 1;
        levelNum += startRow * ScrollLevelSelect.columnCount; // startRow is negative

        CreateRows();
    }

    private void CreateRows()
    {
        for (int row = startRow; row < rowCount; row++)
        {
            for (int col = 0; col < ScrollLevelSelect.columnCount; col++)
            {
                FacLevelButton.CreateLevelButton(row, col, levelNum++);
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
                MoveButtonParent();
            }
        }

        if (Input.GetMouseButtonUp(0) && allowButtonSelect)
        {
            TryButtonSelect();
        }

        _lastMousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition); // must be at end of update
    }

    private void MoveButtonParent()
    {
        bool movingUp = _deltaMousePosition.y > 0;
        bool movingDown = _deltaMousePosition.y < 0;
        if (movingUp && FacLevelButton.FirstLevelButton != null)
        {
            if (FacLevelButton.FirstLevelButton.transform.position.y >= bottomRowPosition.y)
                return;
        }
        else if (movingDown && FacLevelButton.LastLevelButton != null)
        {
            if (FacLevelButton.LastLevelButton.transform.position.y <= highestVisibleRowPosition.y)
                return;
        }

        _buttonParentMoveAmount += _deltaMousePosition.y;
        if (_buttonParentMoveAmount >= unitScale && _buttonParentMoveAmount > 0) // moved up a row
        {
            RotateTopToBottom();
        }
        else if (_buttonParentMoveAmount <= -unitScale && _buttonParentMoveAmount < 0) // moved down a row
        {
            RotateBottomToTop();
        }

        ButtonParent.Translate(new Vector3(0, _deltaMousePosition.y, 0));
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
        for (int col = 0; col < ScrollLevelSelect.columnCount; col++)
        {
            FacLevelButton.CreateLevelButton(row, col, levelNum++);
        }
    }

    private void ReturnTopRow()
    {
        FacLevelButton.ReturnRow(FacLevelButton.TopRow);
    }

    private void AddBottomRow()
    {
        int levelNum = FacLevelButton.LowestLevelButton - ScrollLevelSelect.columnCount;

        int row = FacLevelButton.BottomRow - 1;
        for (int col = 0; col < ScrollLevelSelect.columnCount; col++)
        {
            FacLevelButton.CreateLevelButton(row, col, levelNum++);
        }
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
