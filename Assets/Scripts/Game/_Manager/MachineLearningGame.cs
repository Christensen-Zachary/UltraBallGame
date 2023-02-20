using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class MachineLearningGame : Agent, IWaitingForPlayerInput, IFiring, ICheckWinLose
{
    [field: SerializeField]
    public ResourceLocator ResourceLocator { get; set; }

    public float _winReward = 1f;
    public float _loseReward = -1f;
    public float _damageReward = 0.001f;

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
    private int _turnDamage = 0;

    private int _stepCounter = 0;
    private int _episodeCounter = 0;
    private float _totalRuntime = 0;

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

        //Time.timeScale = 3f;
    }

    private void Update()
    {
        _totalRuntime += Time.deltaTime;
    }

    public override void Initialize()
    {
        Academy.Instance.AutomaticSteppingEnabled = false;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        //print("CollectObservations");
        sensor.AddObservation(_player.Shootables.Count);
        
        List<Brick> bricks = GetGameboard();
        sensor.AddObservation(bricks.Where(x => Brick.IsDamageable(x.BrickType)).Sum(x => x.Health));

        sensor.AddObservation(_player.Health);
        sensor.AddObservation(bricks.Where(x => x.Row == 1).Count());

        for (int i = 0; i < 12; i++) // 12 columns
        {
            for (int j = 1; j < 15; j++) // 14 rows
            {
                Brick brick = bricks.Find(x => x.Col == i && x.Row == j);
                if (brick != null)
                {
                    sensor.AddObservation(new Vector3(((int)brick.BrickType + 1) / 12f, j / 14f, (i + 1) / 12f)); // divide by 12 to normalize between 0 and 1. Add 1 to col because min calue for col is 0. There are 12 cols, 14 rows, and 12 bricktypes
                }
                else
                {
                    sensor.AddObservation(new Vector3(0, j / 12f, (i + 1) / 12f));
                } 
            }
        }
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        //print("OnActionReceived");
        // start fire
        float angle = Mathf.Clamp(actions.ContinuousActions[0], 0, Mathf.PI);
        Vector2 fireDirection = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
        float shotPosition = Mathf.Clamp(actions.ContinuousActions[1], _player.LeftMostCol, _player.RightMostCol);

        _player.MovePlayer(_grid.GetPosition(shotPosition, 0));
        _player.RunFire(fireDirection);
        _damageCounter.StartTurn();

        _gameData.ShotAngle = angle;
        _gameData.ShotPosition = shotPosition;
        List<Brick> BeforeGameboard = GetGameboard();
        _gameData.BeforeGameboard = BeforeGameboard;

        GameState.State = GState.Firing;
    }

    private List<Brick> GetGameboard()
    {
        return _facBrick.GetBricks().Select(x => { Brick brick = new Brick(); x.CopySelfInto(brick); return brick; }).ToList();
    }

    public override void OnEpisodeBegin()
    {
        //print("OnEpisodeBegin");
        _episodeCounter++;

        print($"Avg time per step ({_totalRuntime / _stepCounter / 20f}) Avg time per episode ({_totalRuntime / _episodeCounter / 20f})");
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        actionsOut.ContinuousActions.Array[0] = Random.Range(_player.LeftMostCol, _player.RightMostCol);
        actionsOut.ContinuousActions.Array[1] = Random.Range(0, Mathf.PI);
    }

    public void WaitingForPlayerInput()
    {
        GameState.State = GState.EmptyState;

        // stepping environment
        //print("Stepping Environment");
        _stepCounter++;
        Academy.Instance.EnvironmentStep();
    }

    public void Firing()
    {
        _turnTimer += Time.deltaTime;

        if (_damageCounter.DamageCount > _turnDamage)
        {
            SetReward(_damageReward * (_damageCounter.DamageCount - _turnDamage));
            _turnDamage = _damageCounter.DamageCount;
        }

        if (_player.IsFireComplete() || _turnTimer > _maxTurnLength)
        {
            _turnTimer = 0;
            _turnDamage = 0;
            _player.EndFire();
            GameState.State = GState.EndTurn;
        }
    }

    public void CheckWinLose()
    {
        if (_player.Health <= 0 || _winService.HasWon())
        {
            if (_player.Health <= 0)
            {
                SetReward(_loseReward);
            }
            else if (_winService.HasWon())
            {
                SetReward(_winReward);
            }

            EndEpisode();
            _gameData.SaveGameToFile();
            GameState.State = GState.SetupLevel;
        }
        else
        {
            GameState.State = GState.WaitingForPlayerInput;
        }

    }
}
