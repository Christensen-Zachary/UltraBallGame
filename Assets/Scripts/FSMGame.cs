using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GState
{
    SetupLevel,
    WaitingForPlayerInput,
    Aiming,
    Firing
}


public class FSMGame : MonoBehaviour
{
    [field: SerializeField]
    public ResourceLocator ResourceLocator { get; set; }
    private GState _state = GState.SetupLevel;

    private Player _player;
    private LevelService _levelService;
    private FacBrick _facBrick;
    private FacBall _facBall;

    void Start()
    {
        _player = ResourceLocator.GetResource<Player>("Player");
        _levelService = ResourceLocator.GetResource<LevelService>("Level");
        _facBrick = ResourceLocator.GetResource<FacBrick>("FacBrick");
        _facBall = ResourceLocator.GetResource<FacBall>("FacBall");
    }

    
    void Update()
    {
        if (_state == GState.SetupLevel)
        {
            for (int i = 0; i < _levelService.NumberOfDivisions - 1; i++)
            {
                List<Brick> row = _levelService.GetNextRow();
                row.ForEach(x => _facBrick.Create(x));
            }
            _levelService.Balls.ForEach(x => _facBall.Create(x));

            _state = GState.WaitingForPlayerInput;
        }
        else if (_state == GState.WaitingForPlayerInput)
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
                _player.RunFire();
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
            if (_player.ReturnFire() || _player.IsFireComplete())
            {
                _player.EndFire();
                _state = GState.WaitingForPlayerInput;
            }
        }
    }
}
