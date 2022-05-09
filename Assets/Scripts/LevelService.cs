using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System.IO;
using System;

public class LevelService : MonoBehaviour
{


    public int NumberOfDivisions { get; set; } = 12;

    public List<Brick> Bricks { get; set; }
    public List<Ball> Balls { get; set; }
    public int RowCounter { get; private set; } = 0;
    [field: SerializeField]
    public ResourceLocator ResourceLocator { get; set; }

    private void Awake()
    {
        ResourceLocator.AddResource("Level", this);

        //int levelNum = ES3.Load<int>(BGStrings.ES_LEVELNUM, 1);
        Level level = LoadLevel(0);//ES3.Load<Level>($"{BGStrings.ES_LEVELNAME}{levelNum}", Level.GetDefault());

        if (level == null)
        {
            Application.Quit(-1);
        }

        NumberOfDivisions = level.NumberOfDivisions;
        Bricks = level.Bricks;
        Balls = level.Balls;
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

            foreach (var item in levelString.Split(NUMBEROFDIVISIONS_DELIMITER)[1].Split(BRICK_TYPES_DELIMITER))
            {
                if (item != "")
                {
                    BrickType type = (BrickType)Convert.ToInt32(item.Split(BRICK_TYPE_DELIMITER)[0]);
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
            for (int i = 0; i < 5; i++)
            {
                level.Balls.Add(new Ball(1, 1.1f));
            }

            return level;
        }
        catch
        {
            print("Error loading level");
            return null;
        }
    }

    public static void SaveLevel(Level level)
    {
        using (StreamWriter sw = new StreamWriter("levelOutput.txt"))
        {
            BrickType[] brickTypes = level.Bricks.Select(x => x.BrickType).Distinct().ToArray();

            sw.Write($"{level.NumberOfDivisions}{NUMBEROFDIVISIONS_DELIMITER}");

            foreach (var brickType in brickTypes)
            {
                sw.Write($"{(int)brickType}{BRICK_TYPE_DELIMITER}");
                foreach (var brick in level.Bricks.Where(x => x.BrickType == brickType))
                {
                    sw.Write($"{brick.Col}{BRICK_PARAMS_DELIMITER}{brick.Row}{BRICK_PARAMS_DELIMITER}{brick.Health}{BRICK_DELIMITER}");
                }
                sw.Write("~");
            }
        }
    }
}