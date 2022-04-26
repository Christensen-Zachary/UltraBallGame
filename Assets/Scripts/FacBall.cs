using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacBall : MonoBehaviour
{
    [field: SerializeField]
    public ResourceLocator ResourceLocator { get; set; }
    [field: SerializeField]
    private GameObject BallPrefab { get; set; }

    private Grid _grid;
    private Player _player;
    private Transform _ballParent;

    private void Awake()
    {
        _grid = ResourceLocator.GetResource<Grid>("Grid");
        _player = ResourceLocator.GetResource<Player>("Player");

        ResourceLocator.AddResource("FacBall", this);

        _ballParent = _player.transform;
    }

    public GameObject Create(Ball ball)
    {
        GameObject obj = Instantiate(BallPrefab);
        obj.name = $"Ball {System.Guid.NewGuid()}";
        obj.transform.SetParent(_ballParent);
        obj.transform.localScale = ball.Size * Vector3.one;
        
        if (obj.TryGetComponent(out Shootable shootable))
        {
            shootable.Damage = ball.Damage;
            shootable.Return();
            _player.Shootables.Add(shootable);
        }

        return obj;
    }
}
