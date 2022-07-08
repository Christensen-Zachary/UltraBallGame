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
    [field: SerializeField]
    public GameObject BrickPrefab7 { get; set; }
    [field: SerializeField]
    public GameObject BrickPrefab8 { get; set; }
    [field: SerializeField]
    public GameObject BrickPrefab9 { get; set; }
    [field: SerializeField]
    public GameObject BrickPrefab10 { get; set; }
    [field: SerializeField]
    public GameObject BrickPrefab11 { get; set; }

    private GameObject _advanceableParent;
    
    private CompositeCollider2D CompositeCollider2D { get; set; }

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

        CompositeCollider2D = GetComponent<CompositeCollider2D>();

        _brickParent = transform;

        CreateAdvanceableParent();
    }

    private void CreateAdvanceableParent()
    {
        _advanceableParent = new GameObject($"AdvanceableParent {System.Guid.NewGuid()}");
        _advanceableParent.transform.SetParent(_brickParent);
        _advanceableParent.transform.localPosition = Vector3.zero;
        
        // okay to set here because advanceService isnt used until after facBrick would have been ready to create the bricks
        _advanceService.AdvanceableParent = _advanceableParent;
    }

    // called during setup level
    public void DestroyBricks()
    {
        List<GameObject> bricks = new List<GameObject>();
        for (int i = 0; i < _brickParent.childCount; i++)
        {
            bricks.Add(_brickParent.GetChild(i).gameObject);
        }
        _advanceService.Advanceables.Clear();
        bricks.ForEach(x => Destroy(x));

        CreateAdvanceableParent();
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
            case BrickType.InvincibleTriangle0:
                obj = Instantiate(BrickPrefab7);
                break;
            case BrickType.InvincibleTriangle90:
                obj = Instantiate(BrickPrefab8);
                break;
            case BrickType.InvincibleTriangle180:
                obj = Instantiate(BrickPrefab9);
                break;
            case BrickType.InvincibleTriangle270:
                obj = Instantiate(BrickPrefab10);
                break;
            case BrickType.DirectionalBrick0:
                obj = Instantiate(BrickPrefab11);
                break;
            default:
                obj = Instantiate(BrickPrefab0);
                break;
        }
        
        obj.name = $"Brick {System.Guid.NewGuid()}";
        brick.ID = obj.name;
        if (obj.TryGetComponent<Advanceable>(out Advanceable advanceable1))
        {
            obj.transform.SetParent(_advanceableParent.transform);
        }
        else
        {
            obj.transform.SetParent(_brickParent);
        }
        obj.transform.localScale = Vector3.one * _grid.UnitScale;
        obj.transform.localPosition = _grid.GetPosition(brick.Col, brick.Row);

        Damageable damageable = obj.GetComponentInChildren<Damageable>();
        if (damageable != null)
        {
            damageable.FacBrick = this;
            damageable.WinService = _winService;
            damageable.MaxColorValue = MaxHealth;
            ThemeVisitor.Visit(damageable);
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

        if (obj.TryGetComponent(out ShrinkGrow shrinkGrow))
        {
            shrinkGrow.SetScales();
        }

        return obj;
    }

    public void EnableCompositeCollider()
    {
        CompositeCollider2D.generationType = CompositeCollider2D.GenerationType.Synchronous;
        CompositeCollider2D.GenerateGeometry();
    }

    public void DisableCompositeCollider()
    {
        CompositeCollider2D.generationType = CompositeCollider2D.GenerationType.Manual;
        CompositeCollider2D.GenerateGeometry();
    }
}
