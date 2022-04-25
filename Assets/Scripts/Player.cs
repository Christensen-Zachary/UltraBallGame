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
    
    private List<Shoot> _shoots = new List<Shoot>();
    [field: SerializeField]
    private GameObject ShootPrefab { get; set; }
    public bool IsFireRunning { get; private set; } = true;
    void Awake()
    {
        ResourceLocator.AddResource("Player", this);

        _mainCamera = Camera.main;
        _grid = ResourceLocator.GetResource<Grid>("Grid");
        _aim = ResourceLocator.GetResource<Aim>("Aim");

        transform.localPosition = _grid.GetPosition((_grid.NumberOfDivisions - 1) / 2f, _grid.NumberOfDivisions - 1);
        transform.localScale = _grid.UnitScale * Vector2.one;

        for (int i = 0; i < 10; i++)
        {
            GameObject shoot = Instantiate(ShootPrefab);
            shoot.name = $"Ball {i}";
            shoot.transform.SetParent(transform);
            shoot.transform.localScale = Vector3.one;
            _shoots.Add(shoot.GetComponent<Shoot>());
        }
        _shoots.ForEach(x => x.Return());
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
        _aim.ShowPrediction(transform.position, direction);
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
        foreach (var ball in _shoots)
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
        foreach (var ball in _shoots)
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
        return !_shoots.Any(x => x.IsReturned == false) && !IsFireRunning;
    }

}
