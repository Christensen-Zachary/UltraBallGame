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


    [field: SerializeField]
    public GameObject DesignBrickPrefab { get; set; }

    private List<DesignBrick> DesignBricks { get; set; } = new List<DesignBrick>();
    private List<Ball> Balls { get; set; }
    private DesignBrick SelectedBrick { get; set; }
    private List<DesignBrick> SelectedBricks { get; set; } = new List<DesignBrick>();
    

    private Grid _grid;
    private Camera _mainCamera;
    private bool _setHealth = false;
    private string _healthStr = "";
    private bool _setLevel = false;
    private string _levelStr = "";
    private bool _acceptCreateInput = false;

    private void Awake()
    {
        _grid = ResourceLocator.GetResource<Grid>("Grid");

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
        if (InputSaveLevel())
        {
            SaveLevel();
            print($"Level saved");
        }
    }

    private void ChangeBricksByKeypad()
    {
        DesignBrick newBrick = null;
        if (InputSetSelectedRight())
        {
            for (int col = SelectedBrick.Col + 1; col < _grid.NumberOfDivisions; col++)
            {
                newBrick = DesignBricks.Find(x => x.Row == SelectedBrick.Row && x.Col == col);
                if (newBrick != null)
                {
                    SetSelectedBrick(newBrick);
                    break;
                }
            }
        }
        else if (InputSetSelectedLeft())
        {
            for (int col = SelectedBrick.Col - 1; col >= 0; col--)
            {
                newBrick = DesignBricks.Find(x => x.Row == SelectedBrick.Row && x.Col == col);
                if (newBrick != null)
                {
                    SetSelectedBrick(newBrick);
                    break;
                }
            }
        }
        else if (InputSetSelectedUp())
        {
            for (int row = SelectedBrick.Row - 1; row >= 0; row--)
            {
                newBrick = DesignBricks.Find(x => x.Row == row && x.Col == SelectedBrick.Col);
                if (newBrick != null)
                {
                    SetSelectedBrick(newBrick);
                    break;
                }
            }
        }
        else if (InputSetSelectedDown())
        {
            for (int row = SelectedBrick.Row + 1; row < _grid.NumberOfDivisions; row++)
            {
                newBrick = DesignBricks.Find(x => x.Row == row && x.Col == SelectedBrick.Col);
                if (newBrick != null)
                {
                    SetSelectedBrick(newBrick);
                    break;
                }
            }
        }
    }

    private void SetSelectedBrickByClick()
    {
        DesignBrick selectedBrick = GetBrickFromClick();

        if (selectedBrick != null && DesignBricks.Count == 0)
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
        if (InputBeginCreate())
        {
            _acceptCreateInput = true;
        }
        else if (InputCloneBrick())
        {
            CloneDesignBrick(SelectedBrick);
        }

        if (_acceptCreateInput)
        {
            _acceptCreateInput = false; // set to false to if input when input is given and else won't make true, then will remain false
            if (InputSetSquare())
            {
                CreateDesignBrick(BrickType.Square);
            }
            else if (InputSetTriangle0())
            {
                CreateDesignBrick(BrickType.Triangle0);
            }
            else if (InputSetTriangle90())
            {
                CreateDesignBrick(BrickType.Triangle90);
            }
            else if (InputSetTriangle180())
            {
                CreateDesignBrick(BrickType.Triangle180);
            }
            else if (InputSetTriangle270())
            {
                CreateDesignBrick(BrickType.Triangle270);
            }
            else if (InputSetInvincibleSquare())
            {
                CreateDesignBrick(BrickType.InvincibleSquare);
            }
            else if (InputSetFirePowerup())
            {
                CreateDesignBrick(BrickType.FirePowerup);
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
            if (InputSetSquare())
            {
                SelectedBrick.SetType(BrickType.Square);
            }
            else if (InputSetTriangle0())
            {
                SelectedBrick.SetType(BrickType.Triangle0);
            }
            else if (InputSetTriangle90())
            {
                SelectedBrick.SetType(BrickType.Triangle90);
            }
            else if (InputSetTriangle180())
            {
                SelectedBrick.SetType(BrickType.Triangle180);
            }
            else if (InputSetTriangle270())
            {
                SelectedBrick.SetType(BrickType.Triangle270);
            }
        }
        else if (Brick.IsInvincible(SelectedBrick.BrickType))
        {
            if (InputSetSquare())
            {
                SelectedBrick.SetType(BrickType.InvincibleSquare);
            }
            else if (InputSetTriangle0())
            {
                SelectedBrick.SetType(BrickType.InvincibleTriangle0);
            }
            else if (InputSetTriangle90())
            {
                SelectedBrick.SetType(BrickType.InvincibleTriangle90);
            }
            else if (InputSetTriangle180())
            {
                SelectedBrick.SetType(BrickType.InvincibleTriangle180);
            }
            else if (InputSetTriangle270())
            {
                SelectedBrick.SetType(BrickType.InvincibleTriangle270);
            }
        }
        
        if (InputSetInvincibleSquare())
        {
            if (Brick.IsInvincible(SelectedBrick.BrickType)) SelectedBrick.SetType(BrickType.Square);
            else SelectedBrick.SetType(BrickType.InvincibleSquare);
        }
        
        if (InputSetFirePowerup())
        {
            SelectedBrick.SetType(BrickType.FirePowerup);
        }
    }

    private static bool InputSetFirePowerup()
    {
        return Input.GetKeyDown(KeyCode.U);
    }

    private static bool InputSetInvincibleSquare()
    {
        return Input.GetKeyDown(KeyCode.Y);
    }

    private void LoadLevelRoutine()
    {
        

        if (_setLevel)
        {
            if (Input0())
            {
                _levelStr += "0";
            }
            else if (Input1())
            {
                _levelStr += "1";
            }
            else if (Input2())
            {
                _levelStr += "2";
            }
            else if (Input3())
            {
                _levelStr += "3";
            }
            else if (Input4())
            {
                _levelStr += "4";
            }
            else if (Input5())
            {
                _levelStr += "5";
            }
            else if (Input6())
            {
                _levelStr += "6";
            }
            else if (Input7())
            {
                _levelStr += "7";
            }
            else if (Input8())
            {
                _levelStr += "8";
            }
            else if (Input9())
            {
                _levelStr += "9";
            }
            else if (InputLoadLevel())
            {
                _setLevel = false;
                try
                {
                    int levelNumber = Convert.ToInt32(_levelStr);
                    _levelStr = "";
                    LoadLevel(levelNumber);
                }
                catch
                {
                    print($"Could not convert {_levelStr} to a number");
                }
            }
        }

        if (InputLoadLevel() && !_setLevel)
        {
            _setLevel = true;
        }
    }

    private static bool InputLoadLevel()
    {
        return Input.GetKeyDown(KeyCode.L);
    }

    private void DeleteSelectedBrickRoutine()
    {
        if (InputDeletedSelectedBrick())
        {
            DeleteSelectedBrick();
        }
    }

    private static bool InputDeletedSelectedBrick()
    {
        return Input.GetKeyDown(KeyCode.D);
    }

    private static bool InputSaveLevel()
    {
        return Input.GetKeyDown(KeyCode.Return);
    }

    private static bool InputSetSelectedDown()
    {
        return Input.GetKeyDown(KeyCode.Keypad2);
    }

    private static bool InputSetSelectedUp()
    {
        return Input.GetKeyDown(KeyCode.Keypad5);
    }

    private static bool InputSetSelectedLeft()
    {
        return Input.GetKeyDown(KeyCode.Keypad1);
    }

    private static bool InputSetSelectedRight()
    {
        return Input.GetKeyDown(KeyCode.Keypad3);
    }

    private static bool InputSetTriangle270()
    {
        return Input.GetKeyDown(KeyCode.T);
    }

    private static bool InputSetTriangle180()
    {
        return Input.GetKeyDown(KeyCode.R);
    }

    private static bool InputSetTriangle90()
    {
        return Input.GetKeyDown(KeyCode.E);
    }

    private static bool InputSetTriangle0()
    {
        return Input.GetKeyDown(KeyCode.W);
    }

    private static bool InputSetSquare()
    {
        return Input.GetKeyDown(KeyCode.Q);
    }

    private static bool InputCloneBrick()
    {
        return Input.GetKeyDown(KeyCode.C);
    }

    private static bool InputBeginCreate()
    {
        return Input.GetKeyDown(KeyCode.Space);
    }

    private void SetHealthRoutine()
    {
        if (InputStartSetHealth())
        {
            print($"_setHealth = true");
            _setHealth = true;
        }

        if (_setHealth)
        {
            if (Input0())
            {
                _healthStr += "0";
            }
            else if (Input1())
            {
                _healthStr += "1";
            }
            else if (Input2())
            {
                _healthStr += "2";
            }
            else if (Input3())
            {
                _healthStr += "3";
            }
            else if (Input4())
            {
                _healthStr += "4";
            }
            else if (Input5())
            {
                _healthStr += "5";
            }
            else if (Input6())
            {
                _healthStr += "6";
            }
            else if (Input7())
            {
                _healthStr += "7";
            }
            else if (Input8())
            {
                _healthStr += "8";
            }
            else if (Input9())
            {
                _healthStr += "9";
            }

            if (InputEndSetHealth())
            {
                print($"_setHealth = false");
                _setHealth = false;
                try
                {
                    if (SelectedBricks.Count > 0)
                    {
                        SelectedBricks.ForEach(x => x.SetHealth(Convert.ToInt32(_healthStr)));
                    }
                    else
                    {
                        SelectedBrick.SetHealth(Convert.ToInt32(_healthStr));
                    }
                    
                    _healthStr = "";
                }
                catch
                {
                    print($"Unable to convert {_healthStr} to int32");
                }
            }
        }
    }

    private static bool InputEndSetHealth()
    {
        return Input.GetKeyDown(KeyCode.J);
    }

    private static bool Input9()
    {
        return Input.GetKeyDown(KeyCode.Alpha9);
    }

    private static bool Input8()
    {
        return Input.GetKeyDown(KeyCode.Alpha8);
    }

    private static bool Input7()
    {
        return Input.GetKeyDown(KeyCode.Alpha7);
    }

    private static bool Input6()
    {
        return Input.GetKeyDown(KeyCode.Alpha6);
    }

    private static bool Input5()
    {
        return Input.GetKeyDown(KeyCode.Alpha5);
    }

    private static bool Input4()
    {
        return Input.GetKeyDown(KeyCode.Alpha4);
    }

    private static bool Input3()
    {
        return Input.GetKeyDown(KeyCode.Alpha3);
    }

    private static bool Input2()
    {
        return Input.GetKeyDown(KeyCode.Alpha2);
    }

    private static bool Input1()
    {
        return Input.GetKeyDown(KeyCode.Alpha1);
    }

    private static bool Input0()
    {
        return Input.GetKeyDown(KeyCode.Alpha0);
    }

    private static bool InputStartSetHealth()
    {
        return Input.GetKeyDown(KeyCode.H);
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
        Level level = new Level(_levelNumber, _grid.NumberOfDivisions, (from brick in DesignBricks select brick.Brick).ToList(), Balls);
        LevelService.SaveLevel(level);
        
    }
}
