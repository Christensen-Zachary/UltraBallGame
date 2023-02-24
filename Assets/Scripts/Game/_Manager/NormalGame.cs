using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    private float _firingTimer = 0;
    private readonly float _timeToFastForward = 18f; // seconds
    private readonly int _maxBallsActiveToTriggerFastForward = 6;
    private bool _fastForwardActive = false;
    public Animator _fastForwardAnimator; // set in editor


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
        _firingTimer += Time.deltaTime;
        if (!_fastForwardActive && _firingTimer > _timeToFastForward)
        {
            if (_player.Shootables.Count(x => !x.IsReturned) <= _maxBallsActiveToTriggerFastForward || _firingTimer > _timeToFastForward * 2f)
            {
                if (_fastForwardAnimator != null) _fastForwardAnimator.SetTrigger("blink");
                _fastForwardActive = true;
                if (_gameSettings.timeScale == 1) Time.timeScale = 2f;
            }
        }

        if (_gameInput.ReturnFire() || _player.IsFireComplete())
        {
            _player.EndFire();

            if (_fastForwardActive)
            {
                if (_gameSettings.timeScale == 1) Time.timeScale = 1f;
                _fastForwardActive = false;
            }
            _firingTimer = 0;
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

        CreateNextRow();

        _facBrick.DisableCompositeCollider();
        yield return StartCoroutine(_advanceService.Advance());
        _facBrick.EnableCompositeCollider();

        _endTurnDestroyService.DestroyGameObjects();

        GameState.State = GState.CheckWinLose;
    }

    private IEnumerator SetupLevelRoutine()
    {
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
        for (int i = 0; i < rowCount; i++)
        {
            CreateNextRow();
        }

        _levelService.Balls.ForEach(x => _facBall.Create(x));
        _player.SetRadius();

        _facBrick.DisableCompositeCollider();
        yield return StartCoroutine(_advanceService.Advance());
        _facBrick.EnableCompositeCollider();

        // change states first so variable will always be true until after state change to avoid race condition, unless this happens atomically then it doesn't matter
        GameState.State = GState.WaitingForPlayerInput;
    }

    private void CreateNextRow()
    {
        //_levelService.GetNextRow().ForEach(x => { int row = x.Row; x.Row = --x.Row * -1 + (int)_grid.GameBoardHeight - 1; _facBrick.Create(x); x.Row = row; }); // subtract row so will advance down into position
        _levelService.GetNextRow().ForEach(x => { _facBrick.Create(x); }); // subtract row so will advance down into position
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
            GameState.State = GState.WaitingForPlayerInput;
        }
    }
}