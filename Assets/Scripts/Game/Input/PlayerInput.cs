using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour, ITouchingGameboard, IStartFire, IGetFireDirection, IGetMousePosition, IStartAim, IEndAim, IReturnFire, IStartMove, IEndMove, IGetMovePosition
{
    [field: SerializeField]
    private ResourceLocator ResourceLocator { get; set; }
    private Grid _grid;
    private Player _player;
    private Camera _mainCamera;

    private void Awake()
    {
        ResourceLocator.AddResource("PlayerInput", this);

        _grid = ResourceLocator.GetResource<Grid>("Grid");
        _player = ResourceLocator.GetResource<Player>("Player");

        _mainCamera = Camera.main;
    }

    public bool StartFire()
    {
        return Input.GetMouseButtonUp(0) && _grid.Contains(GetMousePosition());
    }

    public Vector3 GetMousePosition()
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

   
    public bool ReturnFire()
    {
        return Input.GetKeyDown(KeyCode.Space);
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

    public Vector2 GetFireDirection()
    {
        Vector2 direction = GetMousePosition() - _player.transform.position;
        // without clamped y value, aiming left and underneath the player causes aim to aim far right. Clamp y to be positive to fix this
        direction = new Vector2(direction.x, Mathf.Clamp(direction.y, 0, Mathf.Infinity));
        return direction;
    }

    public bool TouchingGameboard()
    {
        return Input.GetMouseButton(0) && _grid.Contains(GetMousePosition());
    }
}
