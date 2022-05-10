using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player : MonoBehaviour
{
    [field: SerializeField]
    private ResourceLocator ResourceLocator { get; set; }
    private Camera _mainCamera;

    private Grid _grid;
    private Aim _aim;
    private float _radius = 1;
    

    public List<Shootable> Shootables { get; private set; } = new List<Shootable>();
    public bool IsFireRunning { get; private set; } = true;
    void Awake()
    {
        ResourceLocator.AddResource("Player", this);

        _mainCamera = Camera.main;
        _grid = ResourceLocator.GetResource<Grid>("Grid");
        _aim = ResourceLocator.GetResource<Aim>("Aim");

        transform.localPosition = _grid.GetPosition((_grid.NumberOfDivisions - 1) / 2f, _grid.NumberOfDivisions - 1);
        transform.localScale = _grid.UnitScale * Vector2.one;
    }

    public void SetRadius()
    {
        Shootable shootable = Shootables.First();
        _radius = _grid.UnitScale * shootable.GetComponent<CircleCollider2D>().radius * shootable.transform.localScale.x;
    }

    
    public void ShowAim(Vector2 direction)
    {
        direction.Normalize();
        _aim.ShowPrediction(transform.position, direction, _radius);
    }

    public void HideAim()
    {
        _aim.HidePrediction();
    }

    public void RunFire(Vector2 direction)
    {
        direction.Normalize();
        StartCoroutine(Fire(direction));
    }

    private IEnumerator Fire(Vector2 direction)
    {
        IsFireRunning = true;
        foreach (var ball in Shootables)
        {
            ball.Fire(direction);
            yield return new WaitForSeconds(0.2f);
        }
        IsFireRunning = false;
    }

    public void EndFire()
    {
        StopAllCoroutines();
        IsFireRunning = false;
        foreach (var ball in Shootables)
        {
            ball.Return();
        }
    }


    public bool IsFireComplete()
    {
        return !Shootables.Any(x => x.IsReturned == false) && !IsFireRunning;
    }



    public void MovePlayer(Vector2 newPosition)
    {
        newPosition = new Vector2(newPosition.x, transform.position.y);
        transform.position = newPosition;
    }
}
