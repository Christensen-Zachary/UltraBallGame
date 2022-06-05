using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacBrick : MonoBehaviour
{

    [field: SerializeField]
    public ResourceLocator ResourceLocator { get; set; }
    [field: SerializeField]
    private GameObject BrickPrefab0 { get; set; }
    [field: SerializeField]
    private GameObject BrickPrefab1 { get; set; }
    [field: SerializeField]
    private GameObject BrickPrefab2 { get; set; }
    [field: SerializeField]
    private GameObject BrickPrefab3 { get; set; }
    [field: SerializeField]
    private GameObject BrickPrefab4 { get; set; }
    [field: SerializeField]
    public GameObject BrickPrefab5 { get; set; }
    [field: SerializeField]
    public GameObject BrickPrefab6 { get; set; }

    private Grid _grid;
    private Transform _brickParent;
    private BrickFixCollision _brickFixCollision;
    private AdvanceService _advanceService;
    private WinService _winService;


    public float MaxHealth { get; set; } = 10;

    private void Awake()
    {
        _grid = ResourceLocator.GetResource<Grid>("Grid");
        _brickFixCollision = ResourceLocator.GetResource<BrickFixCollision>("BrickFixCollision");
        _advanceService = ResourceLocator.GetResource<AdvanceService>("AdvanceService");
        _winService = ResourceLocator.GetResource<WinService>("WinService");
        ResourceLocator.AddResource("FacBrick", this);

        _brickParent = transform;
    }

    public void DestroyBricks()
    {
        List<GameObject> bricks = new List<GameObject>();
        for (int i = 0; i < _brickParent.childCount; i++)
        {
            bricks.Add(_brickParent.GetChild(i).gameObject);
        }
        _advanceService.Advanceables.Clear();
        bricks.ForEach(x => Destroy(x));
    }

    public GameObject Create(Brick brick)
    {
        GameObject obj;
        switch (brick.BrickType)
        {
            case BrickType.Square:
                obj = Instantiate(BrickPrefab0);
                break;
            case BrickType.Triangle0:
                obj = Instantiate(BrickPrefab1);
                break;
            case BrickType.Triangle90:
                obj = Instantiate(BrickPrefab2);
                break;
            case BrickType.Triangle180:
                obj = Instantiate(BrickPrefab3);
                break;
            case BrickType.Triangle270:
                obj = Instantiate(BrickPrefab4);
                break;
            case BrickType.InvincibleSquare:
                obj = Instantiate(BrickPrefab5);
                break;
            case BrickType.FirePowerup:
                obj = Instantiate(BrickPrefab6);
                break;
            default:
                obj = Instantiate(BrickPrefab0);
                break;
        }
        
        obj.name = $"Brick {System.Guid.NewGuid()}";
        brick.ID = obj.name;
        obj.transform.SetParent(_brickParent);
        obj.transform.localScale = Vector3.one * _grid.UnitScale;
        obj.transform.localPosition = _grid.GetPosition(brick.Col, brick.Row);

        Damageable damageable = obj.GetComponentInChildren<Damageable>();
        if (damageable != null)
        {
            damageable.WinService = _winService;
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

        if (obj.TryGetComponent(out Advanceable advanceable))
        {
            advanceable._grid = _grid;
            advanceable._advanceService = _advanceService;
            _advanceService.Advanceables.Add(advanceable);
        }

        if (obj.TryGetComponent(out FirePowerup firePowerup))
        {
            firePowerup.EndTurnDestroyService = ResourceLocator.GetResource<EndTurnDestroyService>("EndTurnDestroyService");
        }


        return obj;
    }
}
