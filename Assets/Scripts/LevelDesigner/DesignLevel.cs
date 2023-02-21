using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DesignLevel : MonoBehaviour
{
    [field: SerializeField]
    public ResourceLocator ResourceLocator { get; set; }
    private int _levelNumber = 1;
    private int _selectNumber = 0;

    private DesignerInputs _designerInputs = new DesignerInputs();


    [field: SerializeField]
    public GameObject DesignBrickPrefab { get; set; }

    private List<DesignBrick> DesignBricks { get; set; } = new List<DesignBrick>();
    private List<Ball> Balls { get; set; }
    private DesignBrick SelectedBrick { get; set; }
    private List<DesignBrick> SelectedBricks { get; set; } = new List<DesignBrick>();
    

    private Grid _grid;
    private NumberInputService _numberInputService;
    private Camera _mainCamera;
    private bool _setHealth = false;
    private bool _setLevel = false;
    private bool _acceptCreateInput = false;

    private void Awake()
    {
        _grid = ResourceLocator.GetResource<Grid>("Grid");
        _numberInputService = ResourceLocator.GetResource<NumberInputService>("NumberInputService");

        Balls = new List<Ball>();

        for (int i = 0; i < 5; i++)
        {
            Balls.Add(new Ball(1, 1.1f));
        }

        _mainCamera = Camera.main;
    }

    private void LoadLevel(int levelNumber)
    {
        Level level = LevelService.LoadLevel(levelNumber);
        
        DesignBricks.ForEach(x => Destroy(x.gameObject));
        DesignBricks.Clear();

        foreach (var brick in level.Bricks)
        {
            CreateDesignBrick(brick.BrickType);
            
            SelectedBrick.SetPosition(brick.Col, brick.Row);
            SelectedBrick.SetHealth(brick.Health);
        }
    }

    void Update()
    {
        LoadLevelRoutine();

        SetSelectedBrickByClick();

        CreateBrickRoutine();

        if (DesignBricks.Count > 1) // check input for changing selected brick
        {
            ChangeBricksByKeypad();

            SaveLevelRoutine();
        }

        if (SelectedBrick != null)
        {
            SetHealthRoutine();

            DeleteSelectedBrickRoutine();

            ChangeSelectedBrick();
        }

        MultiSelectRoutine();
    }

    private void MultiSelectRoutine()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            SelectedBrick.Selected = false;
        }
        if (Input.GetKey(KeyCode.LeftControl))
        {
            DesignBrick designBrick = GetBrickFromClick();
            if (designBrick != null)
            {
                if (SelectedBricks.Contains(designBrick))
                {
                    SelectedBricks.Remove(designBrick);
                    designBrick.Selected = false;
                }
                else
                {
                    SelectedBricks.Add(designBrick);
                    designBrick.Selected = true;
                    SelectedBrick = designBrick;
                }
            }

            if (Input.GetKeyDown(KeyCode.A))
            {
                SelectedBricks.Clear();
                DesignBricks.ForEach(x => { SelectedBricks.Add(x); x.Selected = true; });
            }

            if (Input.GetKeyDown(KeyCode.RightControl))
            {
                SelectedBricks.ForEach(x => x.Selected = false);
                SelectedBricks.Clear();
                SetSelectedBrick(DesignBricks.OrderByDescending(x => x.SelectNumber).First());
            }
        }
    }

    private void SaveLevelRoutine()
    {
        if (_designerInputs.InputSaveLevel())
        {
            SaveLevel();
            print($"Level saved");
        }
    }

    private void ChangeBricksByKeypad()
    {
        DesignBrick newBrick = null;
        if (_designerInputs.InputSetSelectedRight())
        {
            for (int col = SelectedBrick.Col + 1; col < _grid.NumberOfDivisions; col++)
            {
                newBrick = DesignBricks.Find(x => x.Row == SelectedBrick.Row && x.Col == col);
                if (newBrick != null)
                {
                    break;
                }
            }
        }
        else if (_designerInputs.InputSetSelectedLeft())
        {
            for (int col = SelectedBrick.Col - 1; col >= 0; col--)
            {
                newBrick = DesignBricks.Find(x => x.Row == SelectedBrick.Row && x.Col == col);
                if (newBrick != null)
                {
                    break;
                }
            }
        }
        else if (_designerInputs.InputSetSelectedUp())
        {
            for (int row = SelectedBrick.Row - 1; row >= 0; row--)
            {
                newBrick = DesignBricks.Find(x => x.Row == row && x.Col == SelectedBrick.Col);
                if (newBrick != null)
                {
                    break;
                }
            }
        }
        else if (_designerInputs.InputSetSelectedDown())
        {
            for (int row = SelectedBrick.Row + 1; row < _grid.NumberOfDivisions; row++)
            {
                newBrick = DesignBricks.Find(x => x.Row == row && x.Col == SelectedBrick.Col);
                if (newBrick != null)
                {
                    break;
                }
            }
        }

        if (newBrick != null) 
        {
            if (DesignBricks.Count > 0)
            {
                print($"Adding {newBrick.name} to DesignBricks");
                DesignBricks.Add(newBrick);
                newBrick.Selected = true;
                SelectedBrick = newBrick;
            }
            else
            {
                SetSelectedBrick(newBrick);
            }
        }
    }

    private void SetSelectedBrickByClick()
    {
        DesignBrick selectedBrick = GetBrickFromClick();

        if (selectedBrick != null && SelectedBricks.Count == 0)
        {
            SetSelectedBrick(selectedBrick);
        }
    }

    private DesignBrick GetBrickFromClick()
    {
        DesignBrick selectedBrick = null;
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 inputPos = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
            inputPos = new Vector3(inputPos.x, inputPos.y, 0);
            foreach (var brick in DesignBricks)
            {
                if (brick.ContainsPoint(inputPos))
                {
                    selectedBrick = brick;
                    break;
                }
            }
        }

        return selectedBrick;
    }

    private void CreateBrickRoutine()
    {
        if (_designerInputs.InputBeginCreate())
        {
            _acceptCreateInput = true;
        }
        else if (_designerInputs.InputCloneBrick())
        {
            CloneDesignBrick(SelectedBrick);
        }

        if (_acceptCreateInput)
        {
            _acceptCreateInput = false; // set to false to if input when input is given and else won't make true, then will remain false
            if (_designerInputs.InputSetSquare())
            {
                CreateDesignBrick(BrickType.Square);
            }
            else if (_designerInputs.InputSetTriangle0())
            {
                CreateDesignBrick(BrickType.Triangle0);
            }
            else if (_designerInputs.InputSetTriangle90())
            {
                CreateDesignBrick(BrickType.Triangle90);
            }
            else if (_designerInputs.InputSetTriangle180())
            {
                CreateDesignBrick(BrickType.Triangle180);
            }
            else if (_designerInputs.InputSetTriangle270())
            {
                CreateDesignBrick(BrickType.Triangle270);
            }
            else if (_designerInputs.InputSetInvincibleSquare())
            {
                CreateDesignBrick(BrickType.InvincibleSquare);
            }
            else if (_designerInputs.InputSetFirePowerup1())
            {
                CreateDesignBrick(BrickType.FirePowerup1);
            }
            else if (_designerInputs.InputSetDirectional0())
            {
                CreateDesignBrick(BrickType.DirectionalBrick0);
            }
            else if (_designerInputs.InputEvilBrick())
            {
                CreateDesignBrick(BrickType.EvilBrick);
            }
            else
            {
                _acceptCreateInput = true; // if no create brick input given, then try again
            }
        }
    }

    

    private void ChangeSelectedBrick()
    {
        if (Brick.IsDamageable(SelectedBrick.BrickType))
        {
            if (_designerInputs.InputSetSquare())
            {
                SelectedBrick.SetType(BrickType.Square);
            }
            else if (_designerInputs.InputSetTriangle0())
            {
                SelectedBrick.SetType(BrickType.Triangle0);
            }
            else if (_designerInputs.InputSetTriangle90())
            {
                SelectedBrick.SetType(BrickType.Triangle90);
            }
            else if (_designerInputs.InputSetTriangle180())
            {
                SelectedBrick.SetType(BrickType.Triangle180);
            }
            else if (_designerInputs.InputSetTriangle270())
            {
                SelectedBrick.SetType(BrickType.Triangle270);
            }
        }
        else if (Brick.IsInvincible(SelectedBrick.BrickType))
        {
            if (_designerInputs.InputSetSquare())
            {
                SelectedBrick.SetType(BrickType.InvincibleSquare);
            }
            else if (_designerInputs.InputSetTriangle0())
            {
                SelectedBrick.SetType(BrickType.InvincibleTriangle0);
            }
            else if (_designerInputs.InputSetTriangle90())
            {
                SelectedBrick.SetType(BrickType.InvincibleTriangle90);
            }
            else if (_designerInputs.InputSetTriangle180())
            {
                SelectedBrick.SetType(BrickType.InvincibleTriangle180);
            }
            else if (_designerInputs.InputSetTriangle270())
            {
                SelectedBrick.SetType(BrickType.InvincibleTriangle270);
            }
        }

        if (_designerInputs.InputSetInvincibleSquare())
        {
            if (Brick.IsInvincible(SelectedBrick.BrickType)) SelectedBrick.SetType(BrickType.Square);
            else SelectedBrick.SetType(BrickType.InvincibleSquare);
        }

        if (_designerInputs.InputSetFirePowerup1())
        {
            SelectedBrick.SetType(BrickType.FirePowerup1);
        }

        if (_designerInputs.InputSetDirectional0())
        {
            SelectedBrick.SetType(BrickType.DirectionalBrick0);
        }

        if (_designerInputs.InputEvilBrick())
        {
            SelectedBrick.SetType(BrickType.EvilBrick);
        }
    }

    

    private void LoadLevelRoutine()
    {
        
        if (_setLevel)
        {
            if (_designerInputs.InputLoadLevel())
            {
                _setLevel = false;

                int levelNumber = _numberInputService.GetNumber();
                LoadLevel(levelNumber);

                _numberInputService.ResetNumber();
            }
        }

        if (_designerInputs.InputLoadLevel() && !_setLevel)
        {
            _setLevel = true;
            _numberInputService.AcceptInput = true;
        }
    }

    private void DeleteSelectedBrickRoutine()
    {
        if (_designerInputs.InputDeletedSelectedBrick())
        {
            DeleteSelectedBrick();
        }
    }

    private void SetHealthRoutine()
    {
        if (_setHealth)
        {
            if (_designerInputs.InputEndSetHealth())
            {
                print($"_setHealth = false");
                _setHealth = false;
                
                int health = _numberInputService.GetNumber();
                if (SelectedBricks.Count > 0)
                {
                    print("Health set for many bricks");
                    if (health > 0) SelectedBricks.ForEach(x => x.SetHealth(health));
                }
                else
                {
                    print("Health set for one brick");
                    if (health > 0) SelectedBrick.SetHealth(health);
                }

                _numberInputService.ResetNumber();
            }
        }

        if (_designerInputs.InputStartSetHealth())
        {
            print($"_setHealth = true");
            _setHealth = true;
            _numberInputService.AcceptInput = true;
        }

    }


    private void CreateDesignBrick(BrickType brickType)
    {
        DesignBrick brick = Instantiate(DesignBrickPrefab).GetComponent<DesignBrick>();
        brick.name = $"DesignBrick {System.Guid.NewGuid()}";
        DesignBricks.Add(brick);
        brick.Initialize(ResourceLocator);
        brick.SetType(brickType);

        SetSelectedBrick(brick);
    }

    private void CloneDesignBrick(DesignBrick designBrick)
    {
        DesignBrick brick = Instantiate(DesignBrickPrefab).GetComponent<DesignBrick>();
        brick.name = $"DesignBrick {System.Guid.NewGuid()}";
        DesignBricks.Add(brick);
        brick.Initialize(ResourceLocator);

        brick.SetType(designBrick.BrickType);
        brick.SetPosition(designBrick.Col, designBrick.Row);
        brick.SetHealth(SelectedBrick.Health);

        SetSelectedBrick(brick);
    }

    private void DeleteSelectedBrick()
    {
        DesignBricks.Remove(SelectedBrick);
        Destroy(SelectedBrick.gameObject);

        SetSelectedBrick(DesignBricks.OrderByDescending(x => x.SelectNumber).First()); // get most recently selected brick
    }

    private void SetSelectedBrick(DesignBrick brick)
    {
        if (SelectedBrick != null)
        {
            SelectedBrick.Selected = false;
        }
        brick.Selected = true;
        brick.SelectNumber = _selectNumber++;
        SelectedBrick = brick;
    }

    private void SaveLevel()
    {
        Level level = new Level(_levelNumber, _grid.NumberOfDivisions, (from brick in DesignBricks select brick.Brick).ToList(), Balls, 10, 3, 5, 5);
        LevelService.SaveLevel(level);
        
    }
}
