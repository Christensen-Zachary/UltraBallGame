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


    [field: SerializeField]
    public GameObject DesignBrickPrefab { get; set; }

    private List<DesignBrick> DesignBricks { get; set; } = new List<DesignBrick>();
    private List<Ball> Balls { get; set; }
    private DesignBrick SelectedBrick { get; set; }

    private Grid _grid;
    private Camera _mainCamera;
    private bool _setHealth = false;
    private string _healthStr = "";
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

    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            Vector3 inputPos = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
            inputPos = new Vector3(inputPos.x, inputPos.y, 0);
            foreach (var brick in DesignBricks)
            {
                if (brick.ContainsPoint(inputPos))
                {
                    SetSelectedBrick(brick);
                    break;
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            _acceptCreateInput = true;
            //CreateDesignBrick();
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            CloneDesignBrick(SelectedBrick);
        }
        
        if (_acceptCreateInput)
        {
            _acceptCreateInput = false; // set to false to if input when input is given and else won't make true, then will remain false
            if (Input.GetKeyDown(KeyCode.Q))
            {
                CreateDesignBrick(BrickType.Square);
            }
            else if (Input.GetKeyDown(KeyCode.W))
            {
                CreateDesignBrick(BrickType.Triangle0);
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                CreateDesignBrick(BrickType.Triangle90);
            }
            else if (Input.GetKeyDown(KeyCode.R))
            {
                CreateDesignBrick(BrickType.Triangle180);
            }
            else if (Input.GetKeyDown(KeyCode.T))
            {
                CreateDesignBrick(BrickType.Triangle270);
            }
            else
            {
                _acceptCreateInput = true; // if no create brick input given, then try again
            }
        }

        if (DesignBricks.Count > 1) // check input for changing selected brick
        {
            if (Input.GetKeyDown(KeyCode.D))
            {
                for (int col = SelectedBrick.Col + 1; col < _grid.NumberOfDivisions; col++)
                {
                    DesignBrick newBrick = DesignBricks.Find(x => x.Row == SelectedBrick.Row && x.Col == col);
                    if (newBrick != null)
                    {
                        SetSelectedBrick(newBrick);
                        break;
                    }
                }
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                for (int col = SelectedBrick.Col - 1; col >= 0; col--)
                {
                    DesignBrick newBrick = DesignBricks.Find(x => x.Row == SelectedBrick.Row && x.Col == col);
                    if (newBrick != null)
                    {
                        SetSelectedBrick(newBrick);
                        break;
                    }
                }
            }
            else if (Input.GetKeyDown(KeyCode.W))
            {
                for (int row = SelectedBrick.Row - 1; row >= 0; row--)
                {
                    DesignBrick newBrick = DesignBricks.Find(x => x.Row == row && x.Col == SelectedBrick.Col);
                    if (newBrick != null)
                    {
                        SetSelectedBrick(newBrick);
                        break;
                    }
                }
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                for (int row = SelectedBrick.Row + 1; row < _grid.NumberOfDivisions; row++)
                {
                    DesignBrick newBrick = DesignBricks.Find(x => x.Row == row && x.Col == SelectedBrick.Col);
                    if (newBrick != null)
                    {
                        SetSelectedBrick(newBrick);
                        break;
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.Return))
            {
                SaveLevel();
                print($"Level saved");
            }
        }

        if (SelectedBrick != null)
        {
            if (Input.GetKeyDown(KeyCode.H))
            {
                print($"_setHealth = true");
                _setHealth = true;
            }

            if (_setHealth)
            {
                if (Input.GetKeyDown(KeyCode.Alpha0))
                {
                    _healthStr += "0";
                }
                else if (Input.GetKeyDown(KeyCode.Alpha1))
                {
                    _healthStr += "1";
                }
                else if (Input.GetKeyDown(KeyCode.Alpha2))
                {
                    _healthStr += "2";
                }
                else if (Input.GetKeyDown(KeyCode.Alpha3))
                {
                    _healthStr += "3";
                }
                else if (Input.GetKeyDown(KeyCode.Alpha4))
                {
                    _healthStr += "4";
                }
                else if (Input.GetKeyDown(KeyCode.Alpha5))
                {
                    _healthStr += "5";
                }
                else if (Input.GetKeyDown(KeyCode.Alpha6))
                {
                    _healthStr += "6";
                }
                else if (Input.GetKeyDown(KeyCode.Alpha7))
                {
                    _healthStr += "7";
                }
                else if (Input.GetKeyDown(KeyCode.Alpha8))
                {
                    _healthStr += "8";
                }
                else if (Input.GetKeyDown(KeyCode.Alpha9))
                {
                    _healthStr += "9";
                }

                if (Input.GetKeyDown(KeyCode.J))
                {
                    print($"_setHealth = false");
                    _setHealth = false;
                    try
                    {
                        SelectedBrick.SetHealth(Convert.ToInt32(_healthStr));
                        _healthStr = "";
                    }
                    catch
                    {
                        print($"Unable to convert {_healthStr} to int32");
                    }
                }
            }
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

    private void SetSelectedBrick(DesignBrick brick)
    {
        if (SelectedBrick != null)
        {
            SelectedBrick.Selected = false;
        }
        brick.Selected = true;
        SelectedBrick = brick;
    }

    private void SaveLevel()
    {
        Level level = new Level(_levelNumber, _grid.NumberOfDivisions, (from brick in DesignBricks select brick.Brick).ToList(), Balls);
        LevelService.SaveLevel(level);
        
    }
}
