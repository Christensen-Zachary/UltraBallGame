using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions.Must;

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
    private NumberInputService _numberInputService;
    private DesignerInputs _designerInputs = new DesignerInputs();

    private void Awake()
    {

        _grid = ResourceLocator.GetResource<Grid>("Grid");
        _facBrick = ResourceLocator.GetResource<FacBrick>("FacBrick");
        _selectedCursorManager = ResourceLocator.GetResource<SelectedCursorManager>("SelectedCursorManager");

        ResourceLocator.AddResource("DesignBrickManager", this);

        _numberInputService = gameObject.AddComponent<NumberInputService>();

        _startRow = (int)(_grid.NumberOfDivisions * Background.BACKGROUND_RATIO / 2f);
        _currentBrickInfo.Row = _startRow;

        _mainCamera = Camera.main;
    }


    public void Save()
    {
        print($"Saving level");
        LevelService.SaveLevel(new Level(0, _grid.NumberOfDivisions, Bricks.Select(x => x.Brick).ToList(), new List<Ball>(), 20, 20));
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

    private void CreateBrickAndSelect(Brick x, BrickType newBrickType)
    {
        x.BrickType = newBrickType;
        x.CopySelfInto(GetCurrentBrickInfo());
        CreateBrickAndSelect();
    }

    public void CloneSelected()
    {
        List<DesignerBrick> bricks = new List<DesignerBrick>(); // copy previous bricks to be cloned
        Selected.ForEach(x => bricks.Add(x));
        DeselectAll();
        bricks.ForEach(x => { CreateBrickAndSelect(x.Brick, x.Brick.BrickType); });
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
            DeselectBrick(designerBrick);
            return;
        }

        SelectBrick(designerBrick);
    }

    private void DeselectBrick(DesignerBrick designerBrick)
    {
        if (!Selected.Contains(designerBrick)) return;

        Selected.Remove(designerBrick);
        SelectedCursor cursor = designerBrick.GetComponentInChildren<SelectedCursor>();
        if (cursor != null)
        {
            cursor.ReturnSelectedCursor();
        }
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

    public void HoverSelect()
    {
        Vector3 mousePosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mousePosition = new Vector3(mousePosition.x, mousePosition.y, 0);
        Bricks.ForEach(x => { if (x.SpriteRenderer.bounds.Contains(mousePosition)) SelectBrick(x); });
    }

    public void HoverDeselect()
    {
        Vector3 mousePosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mousePosition = new Vector3(mousePosition.x, mousePosition.y, 0);
        Bricks.ForEach(x => { if (x.SpriteRenderer.bounds.Contains(mousePosition)) DeselectBrick(x); });
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
        if (_designerInputs.InputMoveRun()) moveAmount = 3;
        if (_designerInputs.InputMoveUp()) MoveSelected(0, moveAmount);
        if (_designerInputs.InputMoveDown()) MoveSelected(0, -moveAmount);
        if (_designerInputs.InputMoveLeft()) MoveSelected(-moveAmount, 0);
        if (_designerInputs.InputMoveRight()) MoveSelected(moveAmount, 0);
    }

    public void TrySetHealth()
    {
        _numberInputService.AcceptInput = true;

        if (_designerInputs.InputEndSetHealth()) // only resets number
        {
            print($"Resetting health number");
            _numberInputService.ResetNumber();
        }
        
        if (_designerInputs.InputStartSetHealth()) // only sets health to current number
        {
            print($"Trying to set health to {_numberInputService.GetNumber()}");
            if (_numberInputService.GetNumber() < 0) return;

            Selected.ForEach(x =>
            {
                x.Brick.Health = _numberInputService.GetNumber();
                Damageable damageable = x.gameObject.GetComponentInChildren<Damageable>();
                if (damageable != null)
                {
                    damageable.Health = _numberInputService.GetNumber();
                }
            });
        }
        
    }

    public void ResetHealthInput()
    {
        _numberInputService.ResetNumber();
    }

    public void SetSelectedSingleBrick()
    {
        if (Selected.Count == 0) return;

        SetSingleSelected(Selected.Last());
    }

    public void TryDeleteBricks()
    {
        if (!_designerInputs.InputDeletedSelectedBrick()) return;

        _selectedCursorManager.ReturnSelectedCursors();
        Selected.ForEach(x => { Bricks.Remove(x); Destroy(x.gameObject); });
        Selected.Clear();
    }

    public void TryUpdateBrickOptions()
    {
        if (_designerInputs.InputGetBrickType() < 0) return;
        
        List<DesignerBrick> oldBricks = new List<DesignerBrick>();
        Selected.ForEach(x => oldBricks.Add(x)); // copy previous bricks to be removed/destroyed later
        DeselectAll();
        oldBricks.ForEach(x =>
        {
            CreateBrickAndSelect(x.Brick, (BrickType)_designerInputs.InputGetBrickType());
            Bricks.Remove(x);
            Destroy(x.gameObject);
        }); // to create brick of choice, copy values to current brick info, then create brick, then remove and destroy each of the old bricks

        // reset row and column
        GetCurrentBrickInfo().Row = _startRow;
        GetCurrentBrickInfo().Col = 0;
    }

    private void DeselectAll()
    {
        Selected.Clear();
        _selectedCursorManager.ReturnSelectedCursors();
    }

}
