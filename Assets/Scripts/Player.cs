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
    void Start()
    {
        ResourceLocator.AddResource("Player", this);

        _mainCamera = Camera.main;
        _grid = ResourceLocator.GetResource<Grid>("Grid");
        _aim = ResourceLocator.GetResource<Aim>("Aim");

        transform.localPosition = _grid.GetPosition((_grid.NumberOfDivisions - 1) / 2f, _grid.NumberOfDivisions - 1);
        transform.localScale = _grid.UnitScale * Vector2.one;

    }

    private Vector3 GetMousePosition()
    {
        return _mainCamera.ScreenToWorldPoint(Input.mousePosition);
    }

    public bool StartAim()
    {
        return Input.GetMouseButtonDown(0) && _grid.Contains(GetMousePosition());
    }

    public bool Fire()
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

}
