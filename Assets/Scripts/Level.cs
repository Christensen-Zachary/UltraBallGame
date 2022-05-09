using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class Level
{
    [field: SerializeField]
    public int LevelNum { get; set; }
    [field: SerializeField]
    public int NumberOfDivisions { get; set; } = 12;

    [field: SerializeField]
    public List<Brick> Bricks { get; set; } = new List<Brick>();
    [field: SerializeField]
    public List<Ball> Balls { get; set; } = new List<Ball>();


    public static Level GetDefault()
    {
        Level level = new Level()
        {
            LevelNum = 1,
            NumberOfDivisions = 12,
            Bricks = new List<Brick>(),
            Balls = new List<Ball>()
        };

        for (int row = 0; row < level.NumberOfDivisions - 2; row++)
        {
            for (int col = 0; col < level.NumberOfDivisions; col++)
            {
                if (col != level.NumberOfDivisions / 2 
                    && col != (level.NumberOfDivisions / 2) + 1 
                    && (row % 2 == 0 && col % 2 != 0 || row % 2 != 0 && col % 2 == 0)) 

                    level.Bricks.Add(new Brick(BrickType.Square, col, row, Random.Range(4, 25)));
            }
        }

        for (int i = 0; i < 5; i++)
        {
            level.Balls.Add(new Ball(1, 1.1f));
        }

        return level;
    }
}
