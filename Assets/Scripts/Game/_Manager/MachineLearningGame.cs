using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class MachineLearningGame : MonoBehaviour, IWaitingForPlayerInput, IFiring, ICheckWinLose
{
    [field: SerializeField]
    public ResourceLocator ResourceLocator { get; set; }

    private Grid _grid;
    private Player _player;
    private DamageCounter _damageCounter;
    private FacBrick _facBrick;
    private WinService _winService;
    private Background _background;
    private GameData _gameData;


    [field: SerializeField]
    public GameState GameState { get; set; }

    private readonly float _maxTurnLength = 30f;
    private float _turnTimer = 0;

    private void Awake()
    {
        ResourceLocator.AddResource("MachineLearningGame", this);

        _grid = ResourceLocator.GetResource<Grid>("Grid");
        _player = ResourceLocator.GetResource<Player>("Player");
        _damageCounter = ResourceLocator.GetResource<DamageCounter>("DamageCounter");
        _facBrick = ResourceLocator.GetResource<FacBrick>("FacBrick");
        _winService = ResourceLocator.GetResource<WinService>("WinService");
        _background = ResourceLocator.GetResource<Background>("Background");
        _gameData = ResourceLocator.GetResource<GameData>("GameData");

        Time.timeScale = 3f;
    }


    public void WaitingForPlayerInput()
    {
        // start fire
        float angle = Random.Range(0, Mathf.PI);
        Vector2 fireDirection = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
        float shotPosition = Random.Range(_player.LeftMostCol, _player.RightMostCol);
        
        _player.MovePlayer(_grid.GetPosition(shotPosition, 0));
        _player.RunFire(fireDirection);
        _damageCounter.StartTurn();

        _gameData.ShotAngle = angle;
        _gameData.ShotPosition = shotPosition;
        List<Brick> BeforeGameboard = _facBrick.GetBricks().Select(x => { Brick brick = new Brick(); x.CopySelfInto(brick); return brick; }).ToList();
        _gameData.BeforeGameboard = BeforeGameboard;

        GameState.State = GState.Firing;
    }

    public void Firing()
    {
        _turnTimer += Time.deltaTime;
        if (_player.IsFireComplete() || _turnTimer > _maxTurnLength)
        {
            _turnTimer = 0;
            _player.EndFire();
            GameState.State = GState.EndTurn;
        }
    }

    public void CheckWinLose()
    {
        if (_player.Health <= 0 || _winService.HasWon())
        {
            _gameData.SaveGameToFile();
            GameState.State = GState.SetupLevel;
        }
        else
        {
            GameState.State = GState.WaitingForPlayerInput;
        }

    }
}
