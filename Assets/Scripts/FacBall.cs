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
    private Transform _ballParent;

    private void Awake()
    {
        _grid = ResourceLocator.GetResource<Grid>("Grid");
        ResourceLocator.AddResource("FacBall", this);

        _ballParent = transform;
    }

    public GameObject Create(Ball ball)
    {
        GameObject obj = Instantiate(BallPrefab);
        obj.name = $"Ball {System.Guid.NewGuid()}";
        obj.transform.SetParent(_ballParent);
        obj.transform.localScale = Vector3.one * _grid.UnitScale * ball.Size;
        
        if (obj.TryGetComponent(out Shoot shoot))
        {
            shoot.Damage = ball.Damage;
            shoot.Return();
        }

        return obj;
    }
}
