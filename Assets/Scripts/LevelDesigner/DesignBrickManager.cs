using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DesignBrickManager : MonoBehaviour
{
    [field: SerializeField]
    public ResourceLocator ResourceLocator { get; set; }

    public List<DesignerBrick> Bricks { get; } = new List<DesignerBrick>();
    public List<DesignerBrick> Selected { get; } = new List<DesignerBrick>();
    

    private Brick _currentBrickInfo = new Brick(BrickType.Square, 0, 1);
    private int _startRow = 1;


    private Camera _mainCamera;

    private Grid _grid;
    private FacBrick _facBrick;
    private SelectedCursorManager _selectedCursorManager;

    private void Awake()
    {

        _grid = ResourceLocator.GetResource<Grid>("Grid");
        _facBrick = ResourceLocator.GetResource<FacBrick>("FacBrick");
        _selectedCursorManager = ResourceLocator.GetResource<SelectedCursorManager>("SelectedCursorManager");
        
        ResourceLocator.AddResource("DesignBrickManager", this);

        _startRow = (int)(_grid.NumberOfDivisions * Background.BACKGROUND_RATIO / 2f);
        _currentBrickInfo.Row = _startRow;

        _mainCamera = Camera.main;
    }


    private Brick GetCurrentBrickInfo()
    {
        return _currentBrickInfo;
    }

    public void CreateBrickAndSingleSelect()
    {
        GameObject obj = _facBrick.Create(GetCurrentBrickInfo(), typeof(Advanceable));
        DesignerBrick designerBrick = obj.AddComponent<DesignerBrick>();
        GetCurrentBrickInfo().CopySelfInto(designerBrick.Brick);

        Bricks.Add(SetSingleSelected(designerBrick));
    }

    public void CreateBrickAndSelect()
    {
        GameObject obj = _facBrick.Create(GetCurrentBrickInfo(), typeof(Advanceable));
        DesignerBrick designerBrick = obj.AddComponent<DesignerBrick>();
        GetCurrentBrickInfo().CopySelfInto(designerBrick.Brick);

        Bricks.Add(designerBrick);
        SelectBrick(designerBrick);
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
        _selectedCursorManager.ReturnSelectedCursors();
        SelectBrick(designerBrick);
        return Selected[0];
    }

    private void SelectRemoveBrick(DesignerBrick designerBrick)
    {
        if (Selected.Contains(designerBrick)) 
        {
            Selected.Remove(designerBrick);
            SelectedCursor cursor = designerBrick.GetComponentInChildren<SelectedCursor>();
            if (cursor != null)
            {
                cursor.ReturnSelectedCursor();
            }
            return;
        }

        SelectBrick(designerBrick);
    }

    private void SelectBrick(DesignerBrick designerBrick)
    {
        if (Selected.Contains(designerBrick)) return;

        Selected.Add(designerBrick);
        SelectedCursor selectedCursor = _selectedCursorManager.GetSelectedCursor();
        selectedCursor.transform.SetParent(designerBrick.transform);
        selectedCursor.transform.localPosition = Vector3.zero;
        selectedCursor.transform.localScale = Vector3.one;
    }

    public void SelectAllBricks()
    {
        Bricks.ForEach(x => SelectBrick(x));
    }

    public void InvertBrickSelection()
    {
        Bricks.ForEach(x => SelectRemoveBrick(x));
    }

    public void TryClickAddBrick()
    {
        if (!Input.GetMouseButtonDown(0)) return;
    
        Vector3 mousePosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mousePosition = new Vector3(mousePosition.x, mousePosition.y, 0);
        Bricks.ForEach(x => { if (x.SpriteRenderer.bounds.Contains(mousePosition)) SelectRemoveBrick(x); });
    }

    public void TryClickSelectSingle()
    {
        if (!Input.GetMouseButtonDown(0)) return;
        
        Vector3 mousePosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mousePosition = new Vector3(mousePosition.x, mousePosition.y, 0);
        Bricks.ForEach(x => { if (x.SpriteRenderer.bounds.Contains(mousePosition)) SetSingleSelected(x); });
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

    public void SetSelectedSingleBrick()
    {
        if (Selected.Count == 0) return;

        SetSingleSelected(Selected.Last());
    }

    public void TryUpdateBrickOptions()
    {
        BrickType newBrickType = BrickType.Square;
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            newBrickType = BrickType.Square;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            newBrickType = BrickType.Triangle0;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            newBrickType = BrickType.Triangle90;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            newBrickType = BrickType.Triangle180;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            newBrickType = BrickType.Triangle270;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            newBrickType = BrickType.EvilBrick;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            newBrickType = BrickType.FirePowerup;
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            newBrickType = BrickType.InvincibleSquare;
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            newBrickType = BrickType.InvincibleTriangle0;
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            newBrickType = BrickType.InvincibleTriangle90;
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            newBrickType = BrickType.InvincibleTriangle180;
        }
        else if (Input.GetKeyDown(KeyCode.T))
        {
            newBrickType = BrickType.InvincibleTriangle270;
        }
        else
        {
            return;
        }

        List<DesignerBrick> oldBricks = new List<DesignerBrick>(); // copy previous bricks to be removed/destroyed later
        Selected.ForEach(x => oldBricks.Add(x));
        Selected.Clear();
        _selectedCursorManager.ReturnSelectedCursors();
        oldBricks.ForEach(x => 
        { 
            x.Brick.BrickType = newBrickType; 
            x.Brick.CopySelfInto(GetCurrentBrickInfo()); 
            CreateBrickAndSelect(); 
        }); // to create brick of choice, copy values to current brick info, then create brick
        oldBricks.ForEach(x => 
        {
            Bricks.Remove(x);
            Destroy(x.gameObject); 
        }); // remove then destroy each of the old bricks

        // reset row and column
        GetCurrentBrickInfo().Row = _startRow;
        GetCurrentBrickInfo().Col = 0;
    }

}
