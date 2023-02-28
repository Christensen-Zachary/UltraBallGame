using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FacBall : MonoBehaviour
{
    [field: SerializeField]
    public ResourceLocator ResourceLocator { get; set; }
    [field: SerializeField]
    private GameObject BallPrefab { get; set; }

    private Grid _grid;
    private Player _player;
    private LevelService _levelService;
    private Transform _ballParent;

    private List<Shootable> _shootablePool = new List<Shootable>();

    private void Awake()
    {
        _grid = ResourceLocator.GetResource<Grid>("Grid");
        _player = ResourceLocator.GetResource<Player>("Player");
        _levelService = ResourceLocator.GetResource<LevelService>("Level");

        ResourceLocator.AddResource("FacBall", this);

        _ballParent = _player.transform;
    }

    public void DestroyBalls()
    {

        List<Shootable> shootables = new List<Shootable>();
        _player.Shootables.ForEach(x => shootables.Add(x));
        shootables.ForEach(x => DestroyBall(x));
    }

    public void DestroyBall(Shootable shootable)
    {
        _player.Shootables.Remove(shootable);
        _shootablePool.Add(shootable);
    }

    public GameObject Create(Ball ball)
    {
        Shootable shootable;
        GameObject obj;
        if (_shootablePool.Count > 0)
        {
            shootable = _shootablePool.First();
            _shootablePool.Remove(shootable);
            obj = shootable.gameObject;
        }
        else
        {
            obj = Instantiate(BallPrefab);
            obj.name = $"Ball {System.Guid.NewGuid()}";
            obj.transform.SetParent(_ballParent);
            obj.transform.localScale = ball.Size * Vector3.one;

            shootable = obj.GetComponent<Shootable>();
        }

        shootable._levelService = _levelService;
        shootable.Damage = ball.Damage;
        shootable.Return();
        _player.Shootables.Add(shootable);

        return obj;
    }
}
