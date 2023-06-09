using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
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
    [field: SerializeField]
    public GameObject BrickPrefab12 { get; set; }
    [field: SerializeField]
    public GameObject BrickPrefab13 { get; set; }

    private GameObject _advanceableParent;
    
    private CompositeCollider2D CompositeCollider2D { get; set; }

    private Grid _grid;
    private Transform _brickParent;
    private AdvanceService _advanceService;
    private WinService _winService;
    private EndTurnDestroyService _endTurnDestroyService;
    private EndTurnAttackService _endTurnAttackService;
    private DamageCounter _damageCounter;

    public float MaxHealth { get; set; } = 10;

    private void Awake()
    {
        _grid = ResourceLocator.GetResource<Grid>("Grid");
        _advanceService = ResourceLocator.GetResource<AdvanceService>("AdvanceService");
        _winService = ResourceLocator.GetResource<WinService>("WinService");
        _endTurnDestroyService = ResourceLocator.GetResource<EndTurnDestroyService>("EndTurnDestroyService");
        _endTurnAttackService = ResourceLocator.GetResource<EndTurnAttackService>("EndTurnAttackService");
        _damageCounter = ResourceLocator.GetResource<DamageCounter>("DamageCounter");

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
        List<GameObject> bricks = GetCurrentBricks();
        _advanceService.Advanceables.Clear();
        bricks.ForEach(x => Destroy(x));

        CreateAdvanceableParent();
    }

    private List<GameObject> GetCurrentBricks()
    {
        List<GameObject> bricks = new List<GameObject>();
        for (int i = 0; i < _brickParent.childCount; i++)
        {
            bricks.Add(_brickParent.GetChild(i).gameObject);
        }

        return bricks;
    }

    public List<Brick> GetBricks()
    {
        IEnumerable<Brick> brickData = _brickParent.GetComponentsInChildren<BrickData>().Select(x => x.Brick);
        if (brickData == null) return new List<Brick>();
        else return brickData.ToList();
    }

    public List<BrickData> GetBrickDatas()
    {
        IEnumerable<BrickData> brickData = _brickParent.GetComponentsInChildren<BrickData>();
        if (brickData == null) return new List<BrickData>();
        else return brickData.ToList();
    }

    public GameObject Create(Brick brick, params Type[] removeBehaviours)
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
            case BrickType.FirePowerup1:
                obj = Instantiate(BrickPrefab6);
                break;
            case BrickType.FirePowerup2:
                obj = Instantiate(BrickPrefab13);
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
                if (brick.Health == 90 || brick.Health == 180 || brick.Health == 270)
                        obj.transform.Rotate(new Vector3(0, 0, brick.Health));
                break;
            case BrickType.EvilBrick:
                obj = Instantiate(BrickPrefab12);
                break;
            default:
                obj = Instantiate(BrickPrefab0);
                break;
        }
        
        BrickData brickData = obj.AddComponent<BrickData>();
        brickData.Brick = new Brick(); // keep brick object accessible for later
        brick.CopySelfInto(brickData.Brick);
        obj.name = $"Brick {System.Guid.NewGuid()}";
        brick.ID = obj.name;
        if (obj.TryGetComponent<Advanceable>(out Advanceable advanceable1))
        {
            if (removeBehaviours.Contains(typeof(Advanceable)))
            {
                obj.transform.SetParent(_brickParent);
                Destroy(advanceable1);
            }
            else
            {
                obj.transform.SetParent(_advanceableParent.transform);
            }
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
            damageable.DamageCounter = _damageCounter;
            damageable.WinService = _winService;
            damageable._brickData = brickData;
            damageable.MaxColorValue = MaxHealth;
            ThemeVisitor.Visit(damageable);
            damageable.SetColor(brick.Health);
            damageable.Health = brick.Health;

        }

        if (obj.TryGetComponent(out Advanceable advanceable) && !removeBehaviours.Contains(typeof(Advanceable)))
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

        if (obj.TryGetComponent(out DestroyTurnAfterHit destroyTurnAfterHit))
        {
            destroyTurnAfterHit._endTurnDestroyService = _endTurnDestroyService;
        }

        if (obj.TryGetComponent(out EvilBrick evilBrick))
        {
            evilBrick.Radius = _grid.UnitScale / 2f;
            evilBrick._endTurnAttackService = _endTurnAttackService;
            evilBrick._grid = _grid;
            _endTurnAttackService.Attacks.Add(evilBrick);
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
