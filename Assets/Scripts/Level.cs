using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class Level
{
    public int LevelNum { get; set; }
    public int NumberOfDivisions { get; set; } = 12;

    public List<Brick> Bricks { get; set; }
    public List<Ball> Balls { get; set; }


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
                if (col != level.NumberOfDivisions / 2 && col != (level.NumberOfDivisions / 2) + 1 && (row % 2 == 0 && col % 2 != 0 || row % 2 != 0 && col % 2 == 0)) level.Bricks.Add(new Brick(col, row, 200));   
                
            }
        }

        for (int i = 0; i < 5; i++)
        {
            level.Balls.Add(new Ball(1, 1.1f));
        }

        return level;
    }
}
