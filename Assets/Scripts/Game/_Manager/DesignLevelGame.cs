using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DesignLevelGame : MonoBehaviour, IWaitingForPlayerInput, ISetupLevel, IMovingPlayer
{
    [field: SerializeField]
    public ResourceLocator ResourceLocator { get; set; }
    [field: SerializeField]
    private GameState GameState { get; set; } // reference set in editor


    private BrickType _brickType = BrickType.Square;
    private List<(GameObject obj, Brick brick)> _bricks = new List<(GameObject obj, Brick brick)>();
    private (GameObject obj, Brick brick) _selected;
    private Brick _currentBrickInfo = new Brick(BrickType.Square, 0, 1);
    private int _startRow = 1;
    

    private Grid _grid;
    private FacBrick _facBrick;

    private void Awake() 
    {
        ResourceLocator.AddResource("DesignLevelGame", this);

        _grid = ResourceLocator.GetResource<Grid>("Grid");
        _facBrick = ResourceLocator.GetResource<FacBrick>("FacBrick");

        _startRow = (int)(_grid.NumberOfDivisions * 3f / 4f);
        _currentBrickInfo.Row = _startRow;
    }

    private void CreateBrick()
    {
        _bricks.Add(SetSelected(_facBrick.Create(GetCurrentBrickInfo(), typeof(Advanceable)), CreateNewBrickInfo()));
    }

    private (GameObject obj, Brick brick) SetSelected(GameObject obj, Brick brick)
    {
        _selected = (obj, brick);
        return _selected;
    }

    private void MoveSelected(int col, int row)
    {
        _selected.brick.Col += col;
        _selected.brick.Row += row;

        _selected.obj.transform.localPosition += new Vector3(col * _grid.UnitScale, row * _grid.UnitScale, 0);
    }

    private Brick GetCurrentBrickInfo()
    {
        _currentBrickInfo.BrickType = _brickType;
        return _currentBrickInfo;
    }

    private Brick CreateNewBrickInfo()
    {
        return new Brick(_brickType, 0, _startRow);
    }

    // single brick editing
    public void WaitingForPlayerInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            CreateBrick();
        

        int moveAmount = 1;
        if (Input.GetKey(KeyCode.RightAlt))
        {
            moveAmount = 3;
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            MoveSelected(0, moveAmount);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            MoveSelected(0, -moveAmount);
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            MoveSelected(-moveAmount, 0);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            MoveSelected(moveAmount, 0);
        }


    }

    // multiple brick edit
    public void MovingPlayer()
    {

    }

    public void SetupLevel()
    {

        GameState.State = GState.WaitingForPlayerInput;
    }
}
