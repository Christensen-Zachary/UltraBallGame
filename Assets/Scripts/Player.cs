using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player : MonoBehaviour
{
    [field: SerializeField]
    private ResourceLocator ResourceLocator { get; set; }
    private Grid _grid;
    private Camera _mainCamera;

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

    private Vector3 GetMousePosition()
    {
        return _mainCamera.ScreenToWorldPoint(Input.mousePosition);
    }

    public bool StartAim()
    {
        return Input.GetMouseButtonDown(0) && _grid.Contains(GetMousePosition());
    }
    public bool EndAim()
    {
        return !_grid.Contains(GetMousePosition());
    }

    public void HideAim()
    {
        _aim.HidePrediction();
    }

    public void ShowAim()
    {
        Vector2 direction = GetMousePosition() - transform.position;
        direction.Normalize();
        _aim.ShowPrediction(transform.position, direction, _radius);
    }

    public bool StartFire()
    {
        return Input.GetMouseButtonUp(0) && _grid.Contains(GetMousePosition());
    }

    public void RunFire()
    {
        StartCoroutine(Fire());
    }

    private IEnumerator Fire()
    {
        IsFireRunning = true;
        foreach (var ball in Shootables)
        {
            ball.Fire(_aim.Direction);
            yield return new WaitForSeconds(0.25f);
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

    public bool ReturnFire()
    {
        return Input.GetKeyDown(KeyCode.Space);
    }

    public bool IsFireComplete()
    {
        return !Shootables.Any(x => x.IsReturned == false) && !IsFireRunning;
    }


    public bool StartMove()
    {
        return Input.GetKeyDown(KeyCode.M);
    }

    public bool EndMove()
    {
        return Input.GetKeyDown(KeyCode.M);
    }

    public Vector2 GetMovePosition()
    {
        return _mainCamera.ScreenToWorldPoint(Input.mousePosition);
    }

    public void MovePlayer(Vector2 newPosition)
    {
        newPosition = new Vector2(newPosition.x, transform.position.y);
        transform.position = newPosition;
    }
}
