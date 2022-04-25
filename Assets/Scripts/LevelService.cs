using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

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

        int levelNum = ES3.Load<int>(BGStrings.ES_LEVELNUM, 1);
        Level level = ES3.Load<Level>($"{BGStrings.ES_LEVELNAME}{levelNum}", Level.GetDefault());

        NumberOfDivisions = level.NumberOfDivisions;
        Bricks = level.Bricks;
        Balls = level.Balls;
    }


    public List<Brick> GetNextRow()
    {
        RowCounter++;
        return Bricks.Where(x => x.Row == RowCounter - 1).ToList();
    }
}
