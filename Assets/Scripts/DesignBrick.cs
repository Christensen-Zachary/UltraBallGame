using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesignBrick : MonoBehaviour
{
    [field: SerializeField]
    public ResourceLocator ResourceLocator { get; set; }
    [field: SerializeField]
    public BrickNumber BrickNumber { get; set; }
    private bool _isInitialized = false;

    public bool Selected { get; set; } = false;
    public int SelectNumber { get; set; } = 0;

    public Brick Brick { get; set; } = new Brick();
    public int Row => Brick.Row;
    public int Col => Brick.Col;
    public int Health => Brick.Health;
    public BrickType BrickType => Brick.BrickType;


    private Grid _grid;
    private SpriteRenderer _sr;

    private void Awake()
    {
        _sr = GetComponent<SpriteRenderer>();
    }

    public void Initialize(ResourceLocator resourceLocator)
    {
        ResourceLocator = resourceLocator;
        _grid = ResourceLocator.GetResource<Grid>("Grid");
        transform.localScale = _grid.UnitScale * Vector3.one;
        SetPosition(0, 0);
        _isInitialized = true;
    }


    private void Update()
    {
        if (_isInitialized && Selected)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                SetPosition(Brick.Col, Brick.Row - 1);
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                SetPosition(Brick.Col, Brick.Row + 1);
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                SetPosition(Brick.Col + 1, Brick.Row);
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                SetPosition(Brick.Col - 1, Brick.Row);
            }
        }
    }

    public void SetHealth(int health)
    {
        Brick.Health = health;
        BrickNumber.SetNumber(health);
    }

    public void SetPosition(int col, int row)
    {
        transform.localPosition = _grid.GetPosition(col, row);
        Brick.Col = col;
        Brick.Row = row;
    }

    public void SetType(BrickType brickType)
    {
        Brick.BrickType = brickType;

        switch (brickType)
        {
            case BrickType.Square:
                _sr.sprite = Resources.Load<Sprite>("Sprites/Square");
                BrickNumber.Show();
                break;
            case BrickType.Triangle0:
                _sr.sprite = Resources.Load<Sprite>("Sprites/Triangle0");
                BrickNumber.Show();
                break;
            case BrickType.Triangle90:
                _sr.sprite = Resources.Load<Sprite>("Sprites/Triangle90");
                BrickNumber.Show();
                break;
            case BrickType.Triangle180:
                _sr.sprite = Resources.Load<Sprite>("Sprites/Triangle180");
                BrickNumber.Show();
                break;
            case BrickType.Triangle270:
                _sr.sprite = Resources.Load<Sprite>("Sprites/Triangle270");
                BrickNumber.Show();
                break;
            case BrickType.InvincibleSquare:
                _sr.sprite = Resources.Load<Sprite>("Sprites/Square");
                BrickNumber.Hide();
                break;
            case BrickType.FirePowerup:
                _sr.sprite = Resources.Load<Sprite>("Sprites/PNG/sun");
                BrickNumber.Hide();
                break;
            default:
                break;
        }
    }
    
    public bool ContainsPoint(Vector3 point)
    {
        return _sr.bounds.Contains(point);
    }
}
