using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacBrick : MonoBehaviour
{

    [field: SerializeField]
    public ResourceLocator ResourceLocator { get; set; }
    [field: SerializeField]
    private GameObject BrickPrefab { get; set; }

    private Grid _grid;
    private Transform _brickParent;
    private BrickFixCollision _brickFixCollision;

    public float MaxHealth { get; set; } = 10;

    private void Awake()
    {
        _grid = ResourceLocator.GetResource<Grid>("Grid");
        _brickFixCollision = ResourceLocator.GetResource<BrickFixCollision>("BrickFixCollision");
        ResourceLocator.AddResource("FacBrick", this);

        _brickParent = transform;
    }

    public GameObject Create(Brick brick)
    {
        GameObject obj;
        switch (brick.BrickType)
        {
            case BrickType.Square:
                obj = Instantiate(BrickPrefab);
                break;
            default:
                obj = Instantiate(BrickPrefab);
                break;
        }
        
        obj.name = $"Brick {System.Guid.NewGuid()}";
        obj.transform.SetParent(_brickParent);
        obj.transform.localScale = Vector3.one * _grid.UnitScale;
        obj.transform.localPosition = _grid.GetPosition(brick.Col, brick.Row);

        Damageable damageable = obj.GetComponentInChildren<Damageable>();
        if (damageable != null)
        {
            damageable.MaxColorValue = MaxHealth;
            damageable.SetColor(brick.Health);
            damageable.Health = brick.Health;
            damageable.BrickFixCollision = ResourceLocator.GetResource<BrickFixCollision>("BrickFixCollision");
        }

        if (obj.TryGetComponent(out BrickCollision brickCollision))
        {
            brickCollision.Col = brick.Col;
            brickCollision.Row = brick.Row;
            _brickFixCollision.Bricks.Add(brickCollision);
        }

        return obj;
    }
}
