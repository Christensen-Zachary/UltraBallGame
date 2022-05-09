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

    private void Awake()
    {
        _grid = ResourceLocator.GetResource<Grid>("Grid");
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

        if (obj.TryGetComponent(out Damageable damageable))
        {
            damageable.Health = brick.Health;
        }

        return obj;
    }
}
