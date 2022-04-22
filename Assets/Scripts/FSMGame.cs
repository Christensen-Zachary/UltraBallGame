using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GState
{
    WaitingForPlayerInput,
    Aiming,
    Firing
}


public class FSMGame : MonoBehaviour
{
    [field: SerializeField]
    public ResourceLocator ResourceLocator { get; set; }
    private GState _state = GState.WaitingForPlayerInput;

    private Player _player;

    void Start()
    {
        _player = ResourceLocator.GetResource<Player>("Player");

    }

    
    void Update()
    {
        if (_state == GState.WaitingForPlayerInput)
        {
            if (_player.StartAim())
            {
                _state = GState.Aiming;
            }
        }
        else if (_state == GState.Aiming)
        {
            if (_player.StartFire())
            {
                _player.HideAim();
                StartCoroutine(_player.Fire());
                _state = GState.Firing;
            }
            else if (_player.EndAim())
            {
                _player.HideAim();
                _state = GState.WaitingForPlayerInput;
            }
            else
            {
                _player.ShowAim();
            }
        }
        else if (_state == GState.Firing)
        {
            if (_player.StartAim())
            {
                _player.EndFire();
                _state = GState.WaitingForPlayerInput;
            }
        }
    }
}
