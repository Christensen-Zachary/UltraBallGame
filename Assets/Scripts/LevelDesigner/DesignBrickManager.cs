using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesignBrickManager : MonoBehaviour
{
    [field: SerializeField]
    public ResourceLocator ResourceLocator { get; set; }

    public List<DesignerBrick> Bricks { get; } = new List<DesignerBrick>();
    public List<DesignerBrick> Selected { get; } = new List<DesignerBrick>();


    [field: SerializeField]
    public GameObject SelectedCursorPrefab { get; set; } // reference set in editor in prefab
    private Transform _selectedCursor;


    private BrickType _brickType = BrickType.Square;
    private Brick _currentBrickInfo = new Brick(BrickType.Square, 0, 1);
    private int _startRow = 1;


    private Camera _mainCamera;

    private Grid _grid;
    private FacBrick _facBrick;

    private void Awake()
    {

        _grid = ResourceLocator.GetResource<Grid>("Grid");
        _facBrick = ResourceLocator.GetResource<FacBrick>("FacBrick");
        
        ResourceLocator.AddResource("DesignBrickManager", this);

        _startRow = (int)(_grid.NumberOfDivisions * Background.BACKGROUND_RATIO / 2f);
        _currentBrickInfo.Row = _startRow;

        _mainCamera = Camera.main;
    }

    public void SetupLevel()
    {
        _selectedCursor = Instantiate(SelectedCursorPrefab).transform;

    }



    private Brick GetCurrentBrickInfo()
    {
        _currentBrickInfo.BrickType = _brickType;
        return _currentBrickInfo;
    }

    public void CreateBrick()
    {
        GameObject obj = _facBrick.Create(GetCurrentBrickInfo(), typeof(Advanceable));
        DesignerBrick designerBrick = obj.AddComponent<DesignerBrick>();
        GetCurrentBrickInfo().CopySelfInto(designerBrick.Brick);

        Bricks.Add(SetSingleSelected(designerBrick));
    }

    public void MoveSelected(int col, int row)
    {
        Selected.ForEach(x => x.Brick.Col += col);
        Selected.ForEach(x => x.Brick.Row += row);

        Selected.ForEach(x => x.transform.localPosition += new Vector3(col * _grid.UnitScale, row * _grid.UnitScale, 0));
    }

    private DesignerBrick SetSingleSelected(DesignerBrick designerBrick)
    {
        Selected.Clear();
        Selected.Add(designerBrick);
        _selectedCursor.transform.SetParent(designerBrick.transform);
        _selectedCursor.transform.localPosition = Vector3.zero;
        _selectedCursor.transform.localScale = Vector3.one;
        return Selected[0];
    }

    public void TryClickSelectSingle()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
            mousePosition = new Vector3(mousePosition.x, mousePosition.y, 0);

            Bricks.ForEach(x => { if (x.SpriteRenderer.bounds.Contains(mousePosition)) SetSingleSelected(x); });
        }
    }

    public void TryMoveBricks()
    {
        int moveAmount = 1;
        if (Input.GetKey(KeyCode.RightAlt)) moveAmount = 3;
        if (Input.GetKeyDown(KeyCode.UpArrow)) MoveSelected(0, moveAmount);
        if (Input.GetKeyDown(KeyCode.DownArrow)) MoveSelected(0, -moveAmount);
        if (Input.GetKeyDown(KeyCode.LeftArrow)) MoveSelected(-moveAmount, 0);
        if (Input.GetKeyDown(KeyCode.RightArrow)) MoveSelected(moveAmount, 0);
    }


}
