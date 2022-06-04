using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System.IO;
using System;

public class LevelService : MonoBehaviour
{

    public int _levelNumber = -1;
    public int NumberOfDivisions { get; set; } = 12;

    public List<Brick> Bricks { get; set; }
    public List<Ball> Balls { get; set; }
    public int RowCounter { get; private set; } = 0;
    [field: SerializeField]
    public ResourceLocator ResourceLocator { get; set; }

    private void Awake()
    {
        ResourceLocator.AddResource("Level", this);

        int levelNum = ES3.Load<int>(BGStrings.ES_LEVELNUM, 1);
        if (_levelNumber >= 0) levelNum = _levelNumber;
        Level level = LoadLevel(levelNum);//_levelNumber);//ES3.Load<Level>($"{BGStrings.ES_LEVELNAME}{levelNum}", Level.GetDefault());

        if (level == null)
        {
            Application.Quit(-1);
        }

        NumberOfDivisions = level.NumberOfDivisions;
        Bricks = level.Bricks;
        Balls = level.Balls;
    }

    public void ResetLevelService()
    {
        RowCounter = 0;
    }

    public List<Brick> GetNextRow()
    {
        RowCounter++;
        return Bricks.Where(x => x.Row == RowCounter - 1).ToList();
    }


    private static readonly string NUMBEROFDIVISIONS_DELIMITER = "'";
    private static readonly string BRICK_TYPES_DELIMITER = "~";
    private static readonly string BRICK_TYPE_DELIMITER = ":";
    private static readonly string BRICK_DELIMITER = ";";
    private static readonly string BRICK_PARAMS_DELIMITER = ",";

    public static Level LoadLevel(int levelNumber)
    {
        FieldInfo field = typeof(UnityTest).GetField($"LEVEL{levelNumber}");
        string levelString = field.GetValue(null) as string;
        if (levelString == null)
        {
            print($"levelString was NULL");
            return null;
        }
        
        Level level = new Level() { LevelNum = levelNumber };

        try
        {
            level.NumberOfDivisions = Convert.ToInt32(levelString.Split(NUMBEROFDIVISIONS_DELIMITER)[0]);
        }
        catch (Exception ex)
        {
            print($"Could not get [0] from NUMBEROFDIVISIONS split {ex.Message}");
            return null;
        }

        try
        {
            string str = levelString.Split(NUMBEROFDIVISIONS_DELIMITER)[1];
        }
        catch (Exception ex)
        {
            print($"Could not get [1] from NUMBEROFDIVISIONS split {ex.Message}");
            return null;
        }

        try
        {
            string[] str = levelString.Split(NUMBEROFDIVISIONS_DELIMITER)[1].Split(BRICK_TYPES_DELIMITER);
        }
        catch (Exception ex)
        {
            print($"Could not get string[] from NUMBEROFDIVISIONS split BRICKTYPES split {ex.Message}");
            return null;
        }

        foreach (var item in levelString.Split(NUMBEROFDIVISIONS_DELIMITER)[1].Split(BRICK_TYPES_DELIMITER))
        {
            if (item != "")
            {
                BrickType type;
                try
                {
                    type = (BrickType)Convert.ToInt32(item.Split(BRICK_TYPE_DELIMITER)[0]);
                }
                catch (Exception ex)
                {
                    print($"Could not get [0] from BRICKTYPE split {ex.Message}");
                    return null;
                }

                try
                {
                    string[] str = item.Split(BRICK_TYPE_DELIMITER)[1].Split(BRICK_DELIMITER);
                }
                catch (Exception ex)
                {
                    print($"Could not get [1] from BRICKTYPE split BRICK_DELIMITER split {ex.Message}");
                    foreach (var item1 in item.Split(BRICK_TYPE_DELIMITER))
                    {
                        print($"{item1}");
                    }
                    return null;
                }

                foreach (var item1 in item.Split(BRICK_TYPE_DELIMITER)[1].Split(BRICK_DELIMITER))
                {
                    if (item1 != "")
                    {

                        level.Bricks.Add(new Brick(type,
                            Convert.ToInt32(item1.Split(BRICK_PARAMS_DELIMITER)[0]),
                            Convert.ToInt32(item1.Split(BRICK_PARAMS_DELIMITER)[1]),
                            Convert.ToInt32(item1.Split(BRICK_PARAMS_DELIMITER)[2])
                        ));
                    }
                }
            }
        }

            // balls added manually because they are not currently saved and loaded
            for (int i = 0; i < 20; i++)
            {
                level.Balls.Add(new Ball(1, 0.8f));
            }

            return level;
    }

    public static void SaveLevel(Level level)
    {
        using (StreamWriter sw = new StreamWriter("levelOutput.txt", true))
        {
            sw.Write($"{level.NumberOfDivisions}{NUMBEROFDIVISIONS_DELIMITER}");

            foreach (var brickType in level.Bricks.Select(x => x.BrickType).Distinct().ToArray())
            {
                sw.Write($"{(int)brickType}{BRICK_TYPE_DELIMITER}");
                foreach (var brick in level.Bricks.Where(x => x.BrickType == brickType))
                {
                    sw.Write($"{brick.Col}{BRICK_PARAMS_DELIMITER}{brick.Row}{BRICK_PARAMS_DELIMITER}{brick.Health}{BRICK_DELIMITER}");
                }
                sw.Write(BRICK_TYPES_DELIMITER);
            }

            sw.Write("\n");
        }
    }
}
