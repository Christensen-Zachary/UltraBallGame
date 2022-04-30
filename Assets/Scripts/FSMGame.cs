using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GState
{
    SetupLevel,
    WaitingForPlayerInput,
    MovingPlayer,
    Aiming,
    Firing
}


public class FSMGame : MonoBehaviour
{
    [field: SerializeField]
    public ResourceLocator ResourceLocator { get; set; }
    private GState _state = GState.SetupLevel;

    private Player _player;
    private GameInput _gameInput;
    private LevelService _levelService;
    private FacBrick _facBrick;
    private FacBall _facBall;

    void Start()
    {
        _player = ResourceLocator.GetResource<Player>("Player");
        _gameInput = ResourceLocator.GetResource<GameInput>("GameInput");
        _levelService = ResourceLocator.GetResource<LevelService>("Level");
        _facBrick = ResourceLocator.GetResource<FacBrick>("FacBrick");
        _facBall = ResourceLocator.GetResource<FacBall>("FacBall");
    }

    
    void Update()
    {
        // print($"state: {_state}");

        /*
         * Use effectors to create directional collisions to create 'doors' for balls to go through and get trapped for maximum fun zone
         * 
         */
        if (_state == GState.SetupLevel)
        {
            for (int i = 0; i < _levelService.NumberOfDivisions - 1; i++)
            {
                _levelService.GetNextRow().ForEach(x => _facBrick.Create(x));
            }
            _levelService.Balls.ForEach(x => _facBall.Create(x));
            _player.SetRadius();

            _state = GState.WaitingForPlayerInput;
        }
        else if (_state == GState.WaitingForPlayerInput)
        {
            if (_gameInput.StartAim())
            {
                _state = GState.Aiming;
            }
            else if (_gameInput.StartMove())
            {
                _state = GState.MovingPlayer;
            }
        }
        else if (_state == GState.MovingPlayer)
        {
            if (_gameInput.EndMove())
            {
                _state = GState.WaitingForPlayerInput;
            }
            else
            {
                _player.MovePlayer(_gameInput.GetMovePosition());
            }
        }
        else if (_state == GState.Aiming)
        {
            if (_gameInput.StartFire())
            {
                _player.HideAim();
                _player.RunFire(_gameInput.GetFireDirection());
                _state = GState.Firing;
            }
            else if (_gameInput.EndAim())
            {
                _player.HideAim();
                _state = GState.WaitingForPlayerInput;
            }
            else
            {
                _player.ShowAim(_gameInput.GetFireDirection());
            }
        }
        else if (_state == GState.Firing)
        {
            if (_gameInput.ReturnFire() || _player.IsFireComplete())
            {
                _player.EndFire();
                _state = GState.WaitingForPlayerInput;
            }
        }
    }
}
