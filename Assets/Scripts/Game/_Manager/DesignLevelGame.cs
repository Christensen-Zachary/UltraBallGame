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

    [field: SerializeField]
    public GameObject SelectedCursorPrefab { get; set; } // reference set in editor in prefab
    private Transform _selectedCursor;


    private BrickType _brickType = BrickType.Square;
    private List<DesignerBrick> _bricks = new List<DesignerBrick>();
    private DesignerBrick _selected;
    private Brick _currentBrickInfo = new Brick(BrickType.Square, 0, 1);
    private int _startRow = 1;
    
    private Camera _mainCamera;

    private Grid _grid;
    private FacBrick _facBrick;

    private void Awake() 
    {
        ResourceLocator.AddResource("DesignLevelGame", this);

        _grid = ResourceLocator.GetResource<Grid>("Grid");
        _facBrick = ResourceLocator.GetResource<FacBrick>("FacBrick");

        _startRow = (int)(_grid.NumberOfDivisions * Background.BACKGROUND_RATIO / 2f);
        _currentBrickInfo.Row = _startRow;

        _mainCamera = Camera.main;
    }

    private void CreateBrick()
    {
        GameObject obj = _facBrick.Create(GetCurrentBrickInfo(), typeof(Advanceable));
        DesignerBrick designerBrick = obj.AddComponent<DesignerBrick>();
        GetCurrentBrickInfo().CopySelfInto(designerBrick.Brick);

        _bricks.Add(SetSelected(designerBrick));
    }

    private DesignerBrick SetSelected(DesignerBrick designerBrick)
    {
        _selected = designerBrick;
        _selectedCursor.transform.SetParent(designerBrick.transform);
        _selectedCursor.transform.localPosition = Vector3.zero;
        _selectedCursor.transform.localScale = Vector3.one;
        return _selected;
    }

    private void MoveSelected(int col, int row)
    {
        _selected.Brick.Col += col;
        _selected.Brick.Row += row;

        _selected.transform.localPosition += new Vector3(col * _grid.UnitScale, row * _grid.UnitScale, 0);
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
        if (Input.GetKey(KeyCode.RightAlt)) moveAmount = 3;
        if (Input.GetKeyDown(KeyCode.UpArrow)) MoveSelected(0, moveAmount);
        if (Input.GetKeyDown(KeyCode.DownArrow)) MoveSelected(0, -moveAmount);
        if (Input.GetKeyDown(KeyCode.LeftArrow)) MoveSelected(-moveAmount, 0);
        if (Input.GetKeyDown(KeyCode.RightArrow)) MoveSelected(moveAmount, 0);

        if (Input.GetMouseButton(0))
        {
            Vector3 mousePosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
            mousePosition = new Vector3(mousePosition.x, mousePosition.y, 0);

            _bricks.ForEach(x => { if (x.SpriteRenderer.bounds.Contains(mousePosition)) SetSelected(x); });
        }

    }

    // multiple brick edit
    public void MovingPlayer()
    {

    }

    public void SetupLevel()
    {
        _selectedCursor = Instantiate(SelectedCursorPrefab).transform;
        
        GameState.State = GState.WaitingForPlayerInput;
    }
}