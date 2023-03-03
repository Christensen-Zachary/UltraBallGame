using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NormalGame : MonoBehaviour, IGetState, IEmpty, ISetupLevel, IWaitingForPlayerInput, IMovingPlayer, IAiming, ISliderAiming, IFiring, IEndTurn, ICheckWinLose, IGameOver, IWin, IOptionsPanel
{
    [field: SerializeField]
    public ResourceLocator ResourceLocator { get; set; }

    [field: SerializeField]
    private GameState GameState { get; set;} // reference set in editor

    private Grid _grid;
    private Background _background;
    private Player _player;
    private GameInput _gameInput;
    private LevelService _levelService;
    private FacBrick _facBrick;
    private FacBall _facBall;
    private AdvanceService _advanceService;
    private EndTurnDestroyService _endTurnDestroyService;
    private EndTurnAttackService _endTurnAttackService;
    private GameUI _gameUI;
    private GameUISwitcher _gameUISwitcher;
    private GameUIComposition _gameUIComposition;
    private WinService _winService;
    private DamageCounter _damageCounter;
    private GameData _gameData;
    private PowerupManager _powerupManager;
    private GameSettings _gameSettings;
    private BallCounter _ballCounter;
    private FastForward _fastForward;
    public BtnOptionsAnimation _btnOptionsAnimation; // reference set in editor

    private readonly float _dropInDuration = 0.5f;
    private bool _dropInDirection = true;
    private int _dropInStyle = 0;
    private int _dropInStyle2 = 0;
    

    private void Awake() 
    {
        ResourceLocator.AddResource("NormalGame", this);    

        _grid = ResourceLocator.GetResource<Grid>("Grid");
        _background = ResourceLocator.GetResource<Background>("Background");
        _player = ResourceLocator.GetResource<Player>("Player");
        _gameInput = ResourceLocator.GetResource<GameInput>("GameInput");
        _levelService = ResourceLocator.GetResource<LevelService>("Level");
        _facBrick = ResourceLocator.GetResource<FacBrick>("FacBrick");
        _facBall = ResourceLocator.GetResource<FacBall>("FacBall");
        _advanceService = ResourceLocator.GetResource<AdvanceService>("AdvanceService");
        _endTurnDestroyService = ResourceLocator.GetResource<EndTurnDestroyService>("EndTurnDestroyService");
        _endTurnAttackService = ResourceLocator.GetResource<EndTurnAttackService>("EndTurnAttackService");
        _gameUI = ResourceLocator.GetResource<GameUI>("GameUI");
        _gameUISwitcher = ResourceLocator.GetResource<GameUISwitcher>("GameUISwitcher");
        _gameUIComposition = ResourceLocator.GetResource<GameUIComposition>("GameUIComposition");
        _winService = ResourceLocator.GetResource<WinService>("WinService");
        _damageCounter = ResourceLocator.GetResource<DamageCounter>("DamageCounter");
        _gameData = ResourceLocator.GetResource<GameData>("GameData");
        _powerupManager = ResourceLocator.GetResource<PowerupManager>("PowerupManager");
        _gameSettings = ResourceLocator.GetResource<GameSettings>("GameSettings");
        _ballCounter = ResourceLocator.GetResource<BallCounter>("BallCounter");
        _fastForward = ResourceLocator.GetResource<FastForward>("FastForward");
    }

    public void Aiming()
    {
        if (_gameInput.StartFire())
        {
            _player.HideAim();
            _player.RunFire(_gameInput.GetFireDirection());
            if (_gameUISwitcher != null) _gameUISwitcher.StartFire();
            GameState.State = GState.Firing;
        }
        else if (_gameInput.EndAim())
        {
            _player.HideAim();
            GameState.State = GState.WaitingForPlayerInput;
        }
        else
        {
            _player.ShowAim(_gameInput.GetFireDirection());

            
        }
    }

    public void Empty()
    {
        throw new NotImplementedException();
    }

    public void EndTurn()
    {
        GameState.State = GState.EmptyState;
        StartCoroutine(EndTurnRoutine());
    }

    public void Firing()
    {
        _fastForward.AdvanceTimer();
        _fastForward.TryFastForward();
        

        if (_gameInput.ReturnFire() || _player.IsFireComplete())
        {
            _player.EndFire();

            _fastForward.Reset();
            
            GameState.State = GState.EndTurn;
        }
        else if (_gameUIComposition.Random())
        {
            _player.Shootables.ForEach(x => {
                x.RandomizeDirection();
            });
        }
        else if (_gameUIComposition.Horizontal())
        {
            _player.Shootables.ForEach(x => {
                x.HorizontalDirection();
            });
        }
        else if (_gameUIComposition.Vertical())
        {
            _player.Shootables.ForEach(x => {
                x.VerticalDirection();
            });
        }
    }

    public void GameOver()
    {
        if (_gameUIComposition.ResetGame())
        {
            GameState.State = GState.SetupLevel;
        }
        else if (_gameUIComposition.OpenMainMenu())
        {
            SceneManager.LoadScene("MainMenu");
        }
    }

    public void MovingPlayer()
    {
        if (_gameInput.EndMove())
        {
            GameState.State = GState.WaitingForPlayerInput;
        }
        else if (_gameUIComposition.OpenOptions())
        {
            OpenOptions();
        }
        else
        {
            _player.MovePlayer(_gameInput.GetMovePosition());
            _powerupManager.AdjustFloorBricks();
        }
    }

    public void OptionsPanel()
    {
        if (_gameUIComposition.OpenMainMenuPanel())
        {
            _gameUI.ShowMainMenuOkayPanel();
        }
        else if (_gameUIComposition.CloseMainMenuPanel())
        {
            _gameUI.HideMainMenuOkayPanel();

            GameState.State = GameState.StateBeforeOptions;
        }
        else if (_gameUIComposition.CloseOptionsPanel())
        {
            _gameUI.HideOptions();

            GameState.State = GameState.StateBeforeOptions;
        }
        else if (_gameUIComposition.ResetGame())
        {
            GameState.State = GState.SetupLevel;
        }
        else if (_gameUIComposition.OpenMainMenu())
        {
            _gameUI.LoadMainMenu();
        }
    }

    public void SetupLevel()
    {
        GameState.State = GState.EmptyState;
        StartCoroutine(SetupLevelRoutine());
    }

    public void SliderAiming()
    {
        if (_gameUIComposition.StartFire())
        {
            _player.HideAim();
            _player.RunFire(_gameUI.GetFireDirection());
            if (_gameUISwitcher != null) _gameUISwitcher.StartFire();
            _damageCounter.StartTurn();

            _gameData.ShotAngle = Mathf.Atan2(_gameUI.GetFireDirection().y, _gameUI.GetFireDirection().x);
            _gameData.ShotPosition = _player._movePlayerSlider.value;// transform.position.x;
            _gameData.BeforeGameboard = _facBrick.GetBricks().Select(x => { Brick brick = new Brick(); x.CopySelfInto(brick); return brick; }).ToList();

            GameState.State = GState.Firing;
            return;
        }
        else if (_gameUIComposition.EndSliderAim())
        {
            _gameUISwitcher.ShowAimSlider(false);
            _player.HideAim();
            GameState.State = GState.WaitingForPlayerInput;
            return;
        }
        else if (_gameInput.TouchingGameboard())
        {
            Vector2 direction = _gameInput.GetFireDirection();
            _gameUI.SetSliderValue(Mathf.Clamp(Mathf.Rad2Deg * Mathf.Atan2(direction.y, direction.x), 0, 180));
            _player.ShowAim(_gameUI.GetFireDirection());
        }
        
        _player.ShowAim(_gameUI.GetFireDirection());
        _powerupManager.AdjustFloorBricks();
    
    }

    public void WaitingForPlayerInput()
    {
        if (_gameInput.StartAim())
        {
            //_state = GState.Aiming;
            if (_gameUISwitcher != null) _gameUISwitcher.ShowAimSlider(true);
            GameState.State = GState.SliderAiming;
        }
        else if (_gameUIComposition.OpenOptions())
        {
            OpenOptions();
        }
        else if (_gameUIComposition.StartSliderAim())
        {
            if (_gameUISwitcher != null) _gameUISwitcher.ShowAimSlider(true);
            GameState.State = GState.SliderAiming;
        }
        else if (_gameUIComposition.GiveExtraBalls())
        {
            _powerupManager.UseExtraBalls();
        }
        else if (_gameUIComposition.GiveFloorBricks())
        {
            _powerupManager.UseFloorBricks();
        }
        else if (_gameUIComposition.SetBallsOnFire())
        {
            _powerupManager.UseFirePowerup();
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            if (Time.timeScale == 1)
            {
                print("TimeScale is now 3");
                Time.timeScale = 3;
            }
            else
            {
                print("TimeScale is now 1");
                Time.timeScale = 1;
            }

            // _gameData.TestBrickStringConversion();
            
        }
    }

    public void Win()
    {
        if (_gameUIComposition.NextLevel())
        {   
            SceneManager.LoadScene("Game");
        }
    }

    public GState GetState()
    {
        return GameState.State;
    }

    private void OpenOptions()
    {
        if (_gameUI != null) _gameUI.ShowOptions();

        GameState.StateBeforeOptions = GameState.State;
        GameState.State = GState.OptionsPanel;
    }

    public IEnumerator EndTurnRoutine()
    {

        int ballsToAdd = _damageCounter.GetAddBallCount();
        _damageCounter.EndTurn(); // called before saving data because stores values for string that is returned 

        for (int i = 0; i < ballsToAdd; i++)
        {
            _facBall.Create(_levelService.Balls.First());
            _levelService.CurrentBalls.Add(_levelService.Balls.First());
        }

        _gameData.AdvanceTurn();
        yield return StartCoroutine(_gameData.SaveTurnToFile());

        _powerupManager.EndTurnPowerupManager();

        if (_gameUISwitcher != null) _gameUISwitcher.EndFire();

        _ballCounter.Count = _levelService.CurrentBalls.Count;

        yield return StartCoroutine(_endTurnAttackService.Attack());

        _facBrick.DisableCompositeCollider();
        yield return StartCoroutine(_advanceService.Advance());
        //CreateNextRow();
        List<Brick> bricks = _levelService.GetNextRow();
        if (bricks.Count > 0)
        {
            yield return StartCoroutine(CreateNextRowWithDropIn(bricks));
            yield return new WaitForSeconds(_dropInDuration); // wait to allow last bricks to complete since there is no delay after last brick
        }
        _facBrick.EnableCompositeCollider();

        _endTurnDestroyService.DestroyGameObjects();

        GameState.State = GState.CheckWinLose;
    }

    private IEnumerator SetupLevelRoutine()
    {
        if (_btnOptionsAnimation != null) _btnOptionsAnimation.Hide();

        _powerupManager.EndTurnPowerupManager();

        _gameData.ResetGameData();

        _damageCounter.ResetCounters();
        _endTurnAttackService.ResetAttackService();
        _endTurnDestroyService.DestroyGameObjects(); // this is important so that when the game is reset before the end of turn, then potential objects that had been added will be destroyed. Otherwise this service will cause an error if objects had been added
        _levelService.ResetLevelService();
        if (_gameUI != null) _gameUI.ShowGame();
        if (_gameUISwitcher != null) _gameUISwitcher.StartTurn();
        _player.Health = _levelService.Health;
        _player.MovePlayer(_grid.GetPosition((_grid.NumberOfDivisions - 1) / 2f, 0));
        _facBrick.DestroyBricks();
        _facBall.DestroyBalls();
        _winService.NumberOfBricksDestroyed = 0;
        _winService.NumberOfBricksToWin = _levelService.Bricks.Where(x => Brick.IsDamageable(x.BrickType)).Count();

        _facBrick.MaxHealth = _levelService.Bricks.Where(x => Brick.IsDamageable(x.BrickType)).Select(x => x.Health).Max();

        // put rowCount into variable to ensure type conversion on different machines
        float rowCount = (float)_levelService.NumberOfDivisions * Background.BACKGROUND_RATIO;
        
        _facBrick.DisableCompositeCollider();
        _dropInStyle = UnityEngine.Random.Range(0, 6);
        _dropInStyle2 = UnityEngine.Random.Range(0, 2);
        for (int i = 0; i < rowCount - 1; i++)
        {
            //CreateNextRow();
            yield return StartCoroutine(CreateNextRowWithDropIn(_levelService.GetNextRow()));
        }
        _dropInStyle2 = 0; // reset to normal style to later rows always enter from top
        yield return new WaitForSeconds(_dropInDuration); // wait to allow last bricks to complete since there is no delay after last brick
        _facBrick.EnableCompositeCollider(); // enable after waiting so bricks have settled

        _levelService.Balls.ForEach(x => _facBall.Create(x));
        _player.SetRadius();

        if (_btnOptionsAnimation != null) StartCoroutine(_btnOptionsAnimation.FadeIn());
        // change states first so variable will always be true until after state change to avoid race condition, unless this happens atomically then it doesn't matter
        GameState.State = GState.WaitingForPlayerInput;
    }

    private IEnumerator CreateNextRowWithDropIn(List<Brick> row)
    {
        float topScreenYPos = _grid.UnitScale + Mathf.Abs(Camera.main.ScreenToWorldPoint(new Vector3(0, BGUtils.GetScreenSize().height, 0)).y);
        List<BrickData> brickDatas = new List<BrickData>();
        row.ForEach(x =>
        {
            GameObject brick = _facBrick.Create(x);
            brickDatas.Add(brick.GetComponent<BrickData>());
        });

        yield return StartCoroutine(DropRowIntoPosition(brickDatas, topScreenYPos));
    }

    private IEnumerator DropRowIntoPosition(List<BrickData> row, float topYPosition)
    {
        // ensure bricks are ordered by column
        row = row.OrderBy(x => x.Brick.Col).ToList();
        
        if (_dropInStyle == 1) row.Reverse();
        else if (_dropInStyle == 2) row.Shuffle();
        else if (_dropInStyle == 3)
        {
            List<BrickData> temp = new();
            int startColIndex = _levelService.NumberOfDivisions / 2 - 1; // assumes even number of columns
            for (int i = startColIndex; i >= 0; i--)
            {
                BrickData brick = row.Find(x => x.Brick.Col == i);
                if (brick != null) temp.Add(brick);
            }
            for (int i = startColIndex + 1; i < _levelService.NumberOfDivisions; i++)
            {
                BrickData brick = row.Find(x => x.Brick.Col == i);
                if (brick != null) temp.Add(brick);
            }
            row.Clear();
            temp.ForEach(x => row.Add(x));
        }
        else if (_dropInStyle == 4)
        {
            List<BrickData> temp = new();
            int endColIndex = _levelService.NumberOfDivisions / 2 - 1; // assumes even number of columns
            for (int i = 0; i <= endColIndex; i++)
            {
                BrickData brick = row.Find(x => x.Brick.Col == i);
                if (brick != null) temp.Add(brick);
            }
            for (int i = _levelService.NumberOfDivisions - 1; i > endColIndex; i--)
            {
                BrickData brick = row.Find(x => x.Brick.Col == i);
                if (brick != null) temp.Add(brick);
            }
            row.Clear();
            temp.ForEach(x => row.Add(x));
        }
        else if (_dropInStyle == 5 && (_dropInDirection = !_dropInDirection)) row.Reverse();

        // move bricks to start position all together and track positions
        List<(BrickData brick, Vector2 start, Vector2 end)> bricksStartAndEnd = new();
        for (int i = 0; i < row.Count; i++)
        {
            Vector3 endPosition = row[i].transform.position;
            if (_dropInStyle2 == 0) row[i].transform.position = new Vector3(row[i].transform.position.x, topYPosition, 0);
            else row[i].transform.position = _grid.GetPosition(_levelService.NumberOfDivisions / 2f, _levelService.NumberOfDivisions * Background.BACKGROUND_RATIO - 1);
             

            bricksStartAndEnd.Add((row[i], row[i].transform.position, endPosition));
        }

        // move bricks in staggered formation
        for (int i = 0; i < bricksStartAndEnd.Count; i++)
        {
            if (i != row.Count - 1) 
            {
                StartCoroutine(DropIntoPosition(bricksStartAndEnd[i].brick.transform, bricksStartAndEnd[i].start, bricksStartAndEnd[i].end));
                yield return new WaitForSeconds(_dropInDuration / _levelService.NumberOfDivisions);
            }
            else 
            {
                StartCoroutine(DropIntoPosition(bricksStartAndEnd[i].brick.transform, bricksStartAndEnd[i].start, bricksStartAndEnd[i].end));
                // no delay between rows
            }
        }
    }

    private IEnumerator DropIntoPosition(Transform obj, Vector2 from, Vector2 to)
    {
        float timer = 0;
        while (timer < _dropInDuration)
        {
            timer += Time.deltaTime;

            //obj.position = new Vector3(obj.position.x, Mathf.Lerp(from.y, to.y, 0.5f * Mathf.Cos(Mathf.PI * timer / _dropInDuration + Mathf.PI) + 0.5f), 0);
            obj.position = Vector3.Lerp(from, to, 0.5f * Mathf.Cos(Mathf.PI * timer / _dropInDuration + Mathf.PI) + 0.5f);

            yield return null;
        }
        
        obj.position = new Vector3(to.x, to.y, 0);
    }

    private void CreateNextRow()
    {
        _levelService.GetNextRow().ForEach(x => { _facBrick.Create(x); });
    }

    public void CheckWinLose()
    {
        if (_player.Health <= 0 || _winService.HasWon()) _gameData.SaveGameToFile();

        if (_player.Health <= 0)
        {
            if (_gameUI != null) _gameUI.HideGame();
            if (_gameUI != null) _gameUI.ShowGameOver();

            GameState.State = GState.GameOver;
        }
        else if (_winService.HasWon())
        {
            ES3.Save(BGStrings.ES_LEVELNUM, _levelService._levelNumber + 1);

            if (_gameUI != null) _gameUI.HideGame();
            if (_gameUI != null) _gameUI.ShowWin();

            GameState.State = GState.Win;
        }
        else
        {
            if (_gameUISwitcher != null) _gameUISwitcher.StartTurn();
            if (_btnOptionsAnimation != null) StartCoroutine(_btnOptionsAnimation.FadeIn());
            GameState.State = GState.WaitingForPlayerInput;
        }
    }
}