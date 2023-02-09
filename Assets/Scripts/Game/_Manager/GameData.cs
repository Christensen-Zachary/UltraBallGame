using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions;

public class GameData : MonoBehaviour
{
    [field: SerializeField]
    private ResourceLocator ResourceLocator { get; set; }

    [field: SerializeField]
    public bool SaveDataOn { get; set; } = false;

    private Player _player;
    private DamageCounter _damageCounter;
    private LevelService _levelService;
    private WinService _winService;
    private FacBrick _facBrick;
    private PowerupManager _powerupManager;

    private string _gameID = "";
    private int _turnCount = 0;

    public float ShotAngle { get; set; }
    public float ShotPosition { get; set; }
    public List<Brick> BeforeGameboard { get; set; }

    private void Awake() 
    {
        ResourceLocator.AddResource("GameData", this);

        _player = ResourceLocator.GetResource<Player>("Player");
        _damageCounter = ResourceLocator.GetResource<DamageCounter>("DamageCounter");
        _levelService = ResourceLocator.GetResource<LevelService>("Level");
        _winService = ResourceLocator.GetResource<WinService>("WinService");
        _facBrick = ResourceLocator.GetResource<FacBrick>("FacBrick");
        _powerupManager = ResourceLocator.GetResource<PowerupManager>("PowerupManager");
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

    // maybe bricks moving down doesnt matter because gameboard will be understood by ai anyway
    // or else maybe when getting string, could make copy of bricks list and decrement rows by turn count
    // maybe it does matter because bricks past row 18 will not show up, and I don't want to default to full gameboard representations
    public void SaveTurnToFile()
    {
        if (!SaveDataOn) return;

        List<Brick> bricks = new List<Brick>();
        _facBrick.GetBricks().ForEach(x => {
            Brick brick = new Brick();
            x.CopySelfInto(brick);
            bricks.Add(brick);
        });

        bricks.ForEach(x => x.Row -= _turnCount);
        BeforeGameboard.ForEach(x => x.Row -= _turnCount);

        string beforeBricksString = ConvertBricksToString(BeforeGameboard);
        string afterBricksString = ConvertBricksToString(bricks);

        if (!File.Exists("./gameOutput.csv"))
        {
            using (StreamWriter sw = new StreamWriter("./gameOutput.csv", true))
            {
                sw.Write("GameID, LevelNumber, TurnNumber, BallCount, DamageDealt, BricksDestroyed, DamageTaken, ShotAngle, ShotPosition, ExtraBallsUsed, FloorBricksUsed, FireBallsUsed,");
                for (int i = 0; i < ROWS_ON_GAMEBOARD; i++)
                {
                    sw.Write($"BeforeRow{i+1}Types,BeforeRow{i+1}Values,");
                }
                for (int i = 0; i < ROWS_ON_GAMEBOARD; i++)
                {
                    if (i != 0) sw.Write(",");
                    sw.Write($"AfterRow{i+1}Types,AfterRow{i+1}Values");
                }
                sw.WriteLine("");
            }
        }

        int damageTaken = bricks.Where(x => x.Row == 1).Count(); // is snapshot of bricks before advance, so first row bricks will cause damage
        
        string dataString = $"{_gameID},{_levelService.LevelNumber},{_turnCount},{_player.Shootables.Count},{_damageCounter.TurnDamageString()},{damageTaken},{ShotAngle},{ShotPosition},{_powerupManager.UsedExtraBalls},{_powerupManager.UsedFloorBricks},{_powerupManager.UsedFireBalls},";
        dataString += $"{beforeBricksString}";
        if (beforeBricksString.Split(",").Count() / 2 < ROWS_ON_GAMEBOARD)
        {
            for (int i = 0; i < ROWS_ON_GAMEBOARD - (beforeBricksString.Split(",").Count() / 2); i++)
            {
                dataString += ",0,0";
            }
        }
        else if (beforeBricksString.Split(",").Count() / 2 == ROWS_ON_GAMEBOARD)
        {

        }
        else
        {
            Debug.LogError($"Given more before bricks than space when saving turn");
        }
        dataString += $",{afterBricksString}";
        if (afterBricksString.Split(",").Count() / 2 < ROWS_ON_GAMEBOARD)
        {
            for (int i = 0; i < ROWS_ON_GAMEBOARD - (afterBricksString.Split(",").Count() / 2); i++)
            {
                dataString += ",0,0";
            }
        }
        else if (afterBricksString.Split(",").Count() / 2 == ROWS_ON_GAMEBOARD)
        {

        }
        else
        {
            Debug.LogError($"Given more after bricks than space when saving turn");
        }

        using (StreamWriter sw = new StreamWriter("./gameOutput.csv", true))
        {
            sw.WriteLine(dataString);
        }
    }

    public void SaveGameToFile()
    {
        if (!SaveDataOn) return;

        string bricksString = ConvertBricksToString(_levelService.Bricks);

        if (!File.Exists("./fullGameOutput.csv"))
        {
            using (StreamWriter sw = new StreamWriter("./fullGameOutput.csv", true))
            {
                sw.Write("GameID,Won,NumberOfTurns,HealthLost,NumberOfBalls,");
                for (int i = 0; i <= MAX_ROWS - 1; i++) // maximum of 40 rows
                {
                    if (i != 0) sw.Write(",");
                    sw.Write($"Row{i+1}Types,Row{i+1}Values");
                }
                sw.WriteLine("");
            }
        }

        // Won, number of turns, health lost, number of balls, starting gameboard, _gameID
        string dataString = $"{_gameID},{(_player.Health > 0 && _winService.HasWon() ? 1 : 0)},{_turnCount},{_levelService.Health - _player.Health},{_player.Shootables.Count},{bricksString}";
        if (bricksString.Split(",").Count() < MAX_ROWS) // if less rows than max then pad with 0s
        {
            for (int i = 0; i < MAX_ROWS - (bricksString.Split(",").Count() / 2); i++)
            {
                dataString += ",0,0";
            }
        }
        using (StreamWriter sw = new StreamWriter("./fullGameOutput.csv", true))
        {
            sw.WriteLine(dataString);
        }

    }

    public static string ConvertBricksToString(List<Brick> bricks)
    {
        string brickData = "";

        // track data row by row
        for (int i = 1; i <= ROWS_ON_GAMEBOARD ; i++)
        {
            List<Brick> row = bricks.Where(x => x.Row == i).ToList();
            BigInteger brickType = 0;
            BigInteger brickValue = 0;
            for (int j = 0; j < COLS_ON_GAMEBOARD; j++)
            {
                Brick brick = row.Find(x => x.Col == j);
                if (brick != null)
                {
                    if (Brick.IsDamageable(brick.BrickType) && brick.Health <= 0) continue; // do not include destroyed bricks

                    brickType |= ((int)brick.BrickType + 1) * (BigInteger)Mathf.Pow(2, j * BITS_PER_BRICK); // must add 1 to brick type because starts at base 0 index with square
                    if (Brick.IsDamageable(brick.BrickType) && brick.Health > 0)
                    {
                        brickValue |= (BigInteger)brick.Health * (BigInteger)Mathf.Pow(2, j * BITS_PER_BRICK);
                    }
                }
            }
            brickData += $"{brickType.ToSafeString()},{brickValue.ToSafeString()}";
            if (i != ROWS_ON_GAMEBOARD) brickData += ",";
        }

        return brickData;
    }

    public static List<Brick> ConvertStringToBricks(string brickTypeString, string brickValueString, int row)
    {
        List<Brick> bricks = new List<Brick>();

        BigInteger brickTypes = BigInteger.Parse(brickTypeString);
        BigInteger brickValues = BigInteger.Parse(brickValueString);
        
        for (int i = 0; i < COLS_ON_GAMEBOARD; i++)
        {
            int typeBits = 0;
            int valueBits = 0;
            for (int j = 0; j < BITS_PER_BRICK; j++)
            {
                if (brickTypes / (BigInteger)Mathf.Pow(2, i * BITS_PER_BRICK + j) % 2 == 1)
                {
                    typeBits |= 1 << j;
                }

                if (brickValues / (BigInteger)Mathf.Pow(2, i * BITS_PER_BRICK + j) % 2 == 1)
                {
                    valueBits |= 1 << j;
                }
            }

            if (typeBits > 0) // found a brick
            {
                typeBits--; // -- because brick types are offset by 1 since square is 0
                
                bricks.Add(new Brick()
                {
                    Row = row,
                    Col = i,
                    Health = valueBits,
                    BrickType = (BrickType)typeBits
                });
            }
        }
    

        return bricks;
    }

    public void TestBrickStringConversion()
    {
            List<Brick> bricks = _facBrick.GetBricks();
            string brickString = ConvertBricksToString(bricks);
            List<string> brickStrings = brickString.Split(',').ToList();
            List<Brick> bricksFromString = new List<Brick>();
            for (int i = 0; i < brickStrings.Count; i += 2)
            {
                // i + 1 th row because rows start at 1 and count up
                ConvertStringToBricks(brickStrings[i], brickStrings[i+1], (i / 2) + 1).ForEach(x => bricksFromString.Add(x));
            }

            // for (int i = 0; i < bricks.Count; i++)
            // {
            //     print($"{bricks[i].Row},{bricks[i].Col},{bricks[i].BrickType} - {bricksFromString[i].Row},{bricksFromString[i].Col},{bricksFromString[i].BrickType}");
            // }

            for (int i = 0; i < bricks.Count; i++)
            {
                Assert.IsTrue(bricksFromString.Any(y => 
                {
                    return 
                    bricks[i].BrickType == y.BrickType && 
                    bricks[i].Row == y.Row && 
                    bricks[i].Col == y.Col && 
                    (!Brick.IsDamageable(bricks[i].BrickType) || bricks[i].Health == y.Health);
                }), $"{bricks[i].BrickType},{bricks[i].Row},{bricks[i].Col},{bricks[i].Health}");
            }
    }

    public static readonly int BITS_PER_BRICK = 9; // largest brick value is 270 so 9 bits needed for max value of 511
    public static readonly int MAX_ROWS = 40;
    public static readonly int COLS_ON_GAMEBOARD = 12;
    public static readonly int ROWS_ON_GAMEBOARD = 14;
}
