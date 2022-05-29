using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesignBrick : MonoBehaviour
{
    [field: SerializeField]
    public ResourceLocator ResourceLocator { get; set; }
    private bool _isInitialized = false;

    public bool Selected { get; set; } = false;

    public Brick Brick { get; set; } = new Brick();
    public int Row => Brick.Row;
    public int Col => Brick.Col;
    public int Health => Brick.Health;


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
            if (Input.GetKeyDown(KeyCode.W))
            {
                SetPosition(Brick.Col, Brick.Row - 1);
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                SetPosition(Brick.Col, Brick.Row + 1);
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                SetPosition(Brick.Col + 1, Brick.Row);
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                SetPosition(Brick.Col - 1, Brick.Row);
            }
        }
    }

    public void SetHealth(int health)
    {
        Brick.Health = health;
    }

    public void SetPosition(int col, int row)
    {
        transform.localPosition = _grid.GetPosition(col, row);
        Brick.Col = col;
        Brick.Row = row;
    }
    
    public bool ContainsPoint(Vector3 point)
    {
        return _sr.bounds.Contains(point);
    }
}
