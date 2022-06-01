using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
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
    private BrickFixCollision _brickFixCollision;
    private FacBrick _facBrick;
    private FacBall _facBall;
    private AdvanceService _advanceService;

    private bool _isRunningSetupLevel = false;

    void Start()
    {
        _player = ResourceLocator.GetResource<Player>("Player");
        _gameInput = ResourceLocator.GetResource<GameInput>("GameInput");
        _levelService = ResourceLocator.GetResource<LevelService>("Level");
        _brickFixCollision = ResourceLocator.GetResource<BrickFixCollision>("BrickFixCollision");
        _facBrick = ResourceLocator.GetResource<FacBrick>("FacBrick");
        _facBall = ResourceLocator.GetResource<FacBall>("FacBall");
        _advanceService = ResourceLocator.GetResource<AdvanceService>("AdvanceService");

        //Time.timeScale = 0.3f;
    }


    void Update()
    {
        //print($"state: {_state}");

        /*
         * Use effectors to create directional collisions to create 'doors' for balls to go through and get trapped for maximum fun zone
         * 
         */
        if (_state == GState.SetupLevel)
        {
            if (!_isRunningSetupLevel)
            {
                StartCoroutine(SetupLevel());
            }
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

    private IEnumerator SetupLevel()
    {
        _isRunningSetupLevel = true;

        _facBrick.MaxHealth = _levelService.Bricks.Select(x => x.Health).Max();

        for (int i = 0; i < _levelService.NumberOfDivisions - 1; i++)
        {
            _levelService.GetNextRow().ForEach(x => { x.Row--; _facBrick.Create(x); }); // subtract row so will advance down into position
        }
        //_brickFixCollision.SetProblemCorners();
        //StartCoroutine(_brickFixCollision.SetPolygonColliderPaths());
        //_brickFixCollision.SetPolygonColliderPaths();

        _levelService.Balls.ForEach(x => _facBall.Create(x));
        _player.SetRadius();

        yield return StartCoroutine(_advanceService.Advance());
        yield return null;

        // change states first so variable will always be true until after state change to avoid race condition, unless this happens atomically then it doesn't matter
        _state = GState.WaitingForPlayerInput;
        _isRunningSetupLevel = false;
    }
}
