using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class MachineLearningGame : MonoBehaviour, IWaitingForPlayerInput, IFiring, ICheckWinLose
{
    [field: SerializeField]
    public ResourceLocator ResourceLocator { get; set; }


    private Player _player;
    private DamageCounter _damageCounter;
    private FacBrick _facBrick;
    private WinService _winService;
    private Background _background;

    [field: SerializeField]
    public GameState GameState { get; set; }

    private void Awake()
    {
        ResourceLocator.AddResource("MachineLearningGame", this);


        _player = ResourceLocator.GetResource<Player>("Player");
        _damageCounter = ResourceLocator.GetResource<DamageCounter>("DamageCounter");
        _facBrick = ResourceLocator.GetResource<FacBrick>("FacBrick");
        _winService = ResourceLocator.GetResource<WinService>("WinService");
        _background = ResourceLocator.GetResource<Background>("Background");
    }


    public void WaitingForPlayerInput()
    {
        // start fire
        float angle = Random.Range(0, Mathf.PI);
        Vector2 fireDirection = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
        float shotPosition = _player.transform.position.x - _background.transform.position.x;
        

        _player.RunFire(fireDirection);
        _damageCounter.StartTurn();

        //_gameData.ShotAngle = Mathf.Atan2(_gameUI.GetFireDirection().y, _gameUI.GetFireDirection().x);
        //_gameData.ShotPosition = _player._movePlayerSlider.value;// transform.position.x;
        List<Brick> BeforeGameboard = _facBrick.GetBricks().Select(x => { Brick brick = new Brick(); x.CopySelfInto(brick); return brick; }).ToList();

        GameState.State = GState.Firing;
    }

    public void Firing()
    {
        if (_player.IsFireComplete())
        {
            _player.EndFire();
            GameState.State = GState.EndTurn;
        }
    }

    public void CheckWinLose()
    {
        if (_player.Health <= 0 || _winService.HasWon())
        {
            // _gameData.SaveGameToFile();
            GameState.State = GState.SetupLevel;
        }
        else
        {
            GameState.State = GState.WaitingForPlayerInput;
        }

    }
}
