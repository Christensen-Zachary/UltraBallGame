using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameData : MonoBehaviour
{
    [field: SerializeField]
    private ResourceLocator ResourceLocator { get; set; }

    private Player _player;
    private DamageCounter _damageCounter;
    private LevelService _levelService;
    private WinService _winService;
    private FacBrick _facBrick;

    private string _gameID = "";
    private int _turnCount = 0;

    public float ShotAngle { get; set; }
    public float ShotPosition { get; set; }

    private void Awake() 
    {
        ResourceLocator.AddResource("GameData", this);

        _player = ResourceLocator.GetResource<Player>("Player");
        _damageCounter = ResourceLocator.GetResource<DamageCounter>("DamageCounter");
        _levelService = ResourceLocator.GetResource<LevelService>("Level");
        _winService = ResourceLocator.GetResource<WinService>("WinService");
        _facBrick = ResourceLocator.GetResource<FacBrick>("FacBrick");
    }

    public void ResetGameData()
    {
        _gameID = System.Guid.NewGuid().ToString();
        _turnCount = 0;
    }

    public void AdvanceTurn()
    {
        _turnCount++;
    }

    public void SaveTurnToFile()
    {
        List<Brick> bricks = _facBrick.GetBricks();
        // print($"Returned bricks length {bricks.Count}");
        // bricks.Select(x => x.Row).Distinct().ToList().ForEach(x => print($"Row {x} found"));
        // for (int i = 1; i < 19; i++)
        // {
        //     print($"Number of bricks in row {i} {bricks.Where(x => x.Row == i).Count()}");
        // }
        int damageTaken = bricks.Where(x => x.Row == 1).Count(); // is snapshot of bricks before advance, so first row bricks will cause damage
        // turn number, ball count, damage dealt, bricks destroyed, damage taken, shot angle, shot position, before gameboard, resulting gameboard, _gameID
        print("turn number, ball count, damage dealt, bricks destroyed, damage taken, shot angle, shot position, before gameboard, resulting gameboard, _gameID");
        string dataString = $"{_turnCount},{_player.Shootables.Count},{_damageCounter.TurnDamageString()},{damageTaken},{ShotAngle},{ShotPosition},{_gameID}";
        print(dataString);
    }

    public void SaveGameToFile()
    {
        // Won, number of turns, health lost, number of balls, starting gameboard, _gameID
        List<Brick> bricks = _levelService.Bricks;

        string dataString = $"{(_winService.HasWon() ? 1 : 0)},{_turnCount},{_levelService.Health - _player.Health},{_player.Shootables.Count},{_gameID}";
        print(dataString);
    }
}
