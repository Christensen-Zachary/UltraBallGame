using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class CSVPreviewGame : MonoBehaviour, ISetupLevel, IWaitingForPlayerInput
{
    [field: SerializeField]
    public ResourceLocator ResourceLocator { get; set; }

    [field: SerializeField]
    public GameState GameState { get; set; } // reference set in editor

    [field: SerializeField]
    public TextMeshProUGUI GameIDText { get; set; } // reference set in editor
    [field: SerializeField]
    public TextMeshProUGUI BeforeAfterText { get; set; } // reference set in editor
    [field: SerializeField]
    public TextMeshProUGUI TurnNumberText { get; set; } // reference set in editor

    private Grid _grid;
    private FacBrick _facBrick;
    private FacBall _facBall;
    private Player _player;
    private LevelService _levelService;

    private int _counter = 0;
    private bool _beforeOrAfter = true;
    private Vector2 _shotAngle = Vector2.zero;
    private float _shotPosition = 0;

    private readonly string FILE_PATH = "./gameOutput.csv";

    private void Awake() 
    {
        ResourceLocator.AddResource("ShowCSVSavesGame", this);

        _grid = ResourceLocator.GetResource<Grid>("Grid");
        _facBrick = ResourceLocator.GetResource<FacBrick>("FacBrick");
        _facBall = ResourceLocator.GetResource<FacBall>("FacBall");
        _player = ResourceLocator.GetResource<Player>("Player");
        _levelService = ResourceLocator.GetResource<LevelService>("Level");
    }


    public void SetupLevel()
    {
        _levelService.Balls.ForEach(x => _facBall.Create(x));
        _player.SetRadius();

        OrderCSV();
        LoadCSVSave();

        GameState.State = GState.WaitingForPlayerInput;
    }


    private void OrderCSV()
    {
        List<string> rows = new List<string>();
        string header;
        using (StreamReader sr = new StreamReader(FILE_PATH))
        {
            header = sr.ReadLine(); // read past header
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                rows.Add(line);
            }
        }

        rows = rows.OrderBy(x => x.Split(",")[0]).ToList();

        using (StreamWriter sw = new StreamWriter(FILE_PATH))
        {
            sw.WriteLine(header);
            rows.ForEach(x => sw.WriteLine(x));
        }

    }

    public void WaitingForPlayerInput()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            _counter++;
            LoadCSVSave();
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            _counter--;
            LoadCSVSave();
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            _beforeOrAfter = !_beforeOrAfter;
            LoadCSVSave();
        }
        else if (Input.GetKeyDown(KeyCode.Return))
        {
            _facBrick.EnableCompositeCollider();
        }

        
    }

    private void LoadCSVSave()
    {
        _facBrick.DestroyBricks();
        _facBrick.DisableCompositeCollider();

        string[] rowString;
        using (StreamReader sr = new StreamReader(FILE_PATH))
        {
            sr.ReadLine(); // read past headers
            for (int i = 0; i < _counter; i++) // read to line of counter
            {
                if (sr.ReadLine() == null) return; // prevent exception from counter going beyond file
            }
            string line = sr.ReadLine(); 
            if (line == null) return; // prevent exception from counter going beyond file
            rowString = line.Split(",");
            List<Brick> bricks = new List<Brick>();
            for (int i = 0; i < GameData.ROWS_ON_GAMEBOARD * 2; i += 2)
            {
                int index = i + 12 + (_beforeOrAfter ? 0 : GameData.ROWS_ON_GAMEBOARD * 2); // starts at 8, is however many rows long with 2 values per row
                // can not access gameData possibly, need method for conversion
                string rowTypes = rowString[index];
                string rowValues = rowString[index + 1];
                List<Brick> rowBricks = GameData.ConvertStringToBricks(rowTypes, rowValues, i / 2 + 1);
                rowBricks.ForEach(x => bricks.Add(x));
            }
            if (bricks.Count > 0) 
            {
                if (bricks.Where(x => Brick.IsDamageable(x.BrickType)).Count() > 0) 
                    _facBrick.MaxHealth = bricks.Where(x => Brick.IsDamageable(x.BrickType)).Max(x => x.Health);
            }
            bricks.ForEach(x => _facBrick.Create(x));
        }

        _facBrick.EnableCompositeCollider();
        _shotPosition = float.Parse(rowString[8]);
        _shotAngle = new Vector2(Mathf.Cos(float.Parse(rowString[7])), Mathf.Sin(float.Parse(rowString[7])));
        
        GameIDText.text = rowString[0];
        TurnNumberText.text = $"Turn {int.Parse(rowString[2])}";
        StartCoroutine(AimPreviewRoutine());
    }

    private IEnumerator AimPreviewRoutine()
    {
        yield return new WaitForSeconds(0.01f);
        if (_beforeOrAfter) // if before then show player aim
        {
            BeforeAfterText.text = "Before";
            _player.MovePlayer(_grid.GetPosition(_shotPosition, 0));
            _player.ShowAim(_shotAngle);
        }
        else // otherwise hide aim
        {
            BeforeAfterText.text = "After";
            _player.HideAim();
        }
    }
}
