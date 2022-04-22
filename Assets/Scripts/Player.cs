using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [field: SerializeField]
    private ResourceLocator ResourceLocator { get; set; }
    private Grid _grid;
    private Camera _mainCamera;

    private Aim _aim;
    
    [field: SerializeField]
    private GameObject ShootPrefab { get; set; }
    private List<Shoot> _shoots = new List<Shoot>();
    void Start()
    {
        ResourceLocator.AddResource("Player", this);

        _mainCamera = Camera.main;
        _grid = ResourceLocator.GetResource<Grid>("Grid");
        _aim = ResourceLocator.GetResource<Aim>("Aim");

        transform.localPosition = _grid.GetPosition((_grid.NumberOfDivisions - 1) / 2f, _grid.NumberOfDivisions - 1);
        transform.localScale = _grid.UnitScale * Vector2.one;

        for (int i = 0; i < 5; i++)
        {
            GameObject shoot = Instantiate(ShootPrefab);
            shoot.transform.SetParent(transform);
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

    public bool StartFire()
    {
        return Input.GetMouseButtonUp(0) && _grid.Contains(GetMousePosition());
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

    public IEnumerator Fire()
    {
        foreach (var ball in _shoots)
        {
            ball.Fire(_aim.Direction);
            yield return new WaitForSeconds(0.25f);
        }      
    }

    public void EndFire()
    {
        StopAllCoroutines();
        foreach (var ball in _shoots)
        {
            ball.Return();
        }
    }

}
