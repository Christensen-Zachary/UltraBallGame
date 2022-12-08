using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NormalGame : MonoBehaviour, IGetState, IEmpty, ISetupLevel, IWaitingForPlayerInput, IMovingPlayer, IAiming, ISliderAiming, IFiring, IEndTurn, IGameOver, IWin, IOptionsPanel
{
    [field: SerializeField]
    public ResourceLocator ResourceLocator { get; set; }

    public GState State { get; set; } = GState.SetupLevel;
    private GState _stateBeforeOptions = GState.EmptyState;

    private Grid _grid;
    private Background _background;
    private Player _player;
    private GameInput _gameInput;
    private LevelService _levelService;
    private BrickFixCollision _brickFixCollision;
    private FacBrick _facBrick;
    private FacBall _facBall;
    private AdvanceService _advanceService;
    private EndTurnDestroyService _endTurnDestroyService;
    private EndTurnAttackService _endTurnAttackService;
    private GameUI _gameUI;
    private GameUISwitcher _gameUISwitcher;
    private GameUIComposition _gameUIComposition;
    private WinService _winService;

    private void Awake() 
    {
        ResourceLocator.AddResource("NormalGame", this);    

        _grid = ResourceLocator.GetResource<Grid>("Grid");
        _background = ResourceLocator.GetResource<Background>("Background");
        _player = ResourceLocator.GetResource<Player>("Player");
        _gameInput = ResourceLocator.GetResource<GameInput>("GameInput");
        _levelService = ResourceLocator.GetResource<LevelService>("Level");
        _brickFixCollision = ResourceLocator.GetResource<BrickFixCollision>("BrickFixCollision");
        _facBrick = ResourceLocator.GetResource<FacBrick>("FacBrick");
        _facBall = ResourceLocator.GetResource<FacBall>("FacBall");
        _advanceService = ResourceLocator.GetResource<AdvanceService>("AdvanceService");
        _endTurnDestroyService = ResourceLocator.GetResource<EndTurnDestroyService>("EndTurnDestroyService");
        _endTurnAttackService = ResourceLocator.GetResource<EndTurnAttackService>("EndTurnAttackService");
        _gameUI = ResourceLocator.GetResource<GameUI>("GameUI");
        _gameUISwitcher = ResourceLocator.GetResource<GameUISwitcher>("GameUISwitcher");
        _gameUIComposition = ResourceLocator.GetResource<GameUIComposition>("GameUIComposition");
        _winService = ResourceLocator.GetResource<WinService>("WinService");

    }

    public void Aiming()
    {
        if (_gameInput.StartFire())
        {
            _player.HideAim();
            _player.RunFire(_gameInput.GetFireDirection());
            if (_gameUISwitcher != null) _gameUISwitcher.StartFire();
            State = GState.Firing;
        }
        else if (_gameInput.EndAim())
        {
            _player.HideAim();
            State = GState.WaitingForPlayerInput;
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
        State = GState.EmptyState;
        StartCoroutine(EndTurnRoutine());
    }

    public void Firing()
    {
        if (_gameInput.ReturnFire() || _player.IsFireComplete())
        {
            _player.EndFire();
            State = GState.EndTurn;
        }
    }

    public void GameOver()
    {
        if (_gameUIComposition.ResetGame())
        {
            State = GState.SetupLevel;
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
            if (_gameUISwitcher != null) _gameUISwitcher.ShowMoveSlider(false);
            State = GState.WaitingForPlayerInput;
        }
        else if (_gameUIComposition.OpenOptions())
        {
            OpenOptions();
        }
        else
        {
            _player.MovePlayer(_gameInput.GetMovePosition());
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

            State = _stateBeforeOptions;
        }
        else if (_gameUIComposition.CloseOptionsPanel())
        {
            _gameUI.HideOptions();

            State = _stateBeforeOptions;
        }
        else if (_gameUIComposition.ResetGame())
        {
            State = GState.SetupLevel;
        }
        else if (_gameUIComposition.OpenMainMenu())
        {
            _gameUI.LoadMainMenu();
        }
    }

    public void SetupLevel()
    {
        State = GState.EmptyState;
        StartCoroutine(SetupLevelRoutine());
    }

    public void SliderAiming()
    {
        if (_gameUIComposition.StartFire())
        {
            _player.HideAim();
            _player.RunFire(_gameUI.GetFireDirection());
            _gameUISwitcher.StartFire();
            State = GState.Firing;
        }
        else if (_gameUIComposition.EndSliderAim())
        {
            _gameUISwitcher.ShowAimSlider(false);
            _player.HideAim();
            State = GState.WaitingForPlayerInput;
        }
        else if (_gameInput.TouchingGameboard())
        {
            Vector2 direction = _gameInput.GetFireDirection();
            _gameUI.SetSliderValue(Mathf.Clamp(Mathf.Rad2Deg * Mathf.Atan2(direction.y, direction.x), 1, 179));
            _player.ShowAim(_gameUI.GetFireDirection());
        }
        else
        {
            _player.ShowAim(_gameUI.GetFireDirection());
        }
    }

    public void WaitingForPlayerInput()
    {
        if (_gameInput.StartAim())
        {
            //_state = GState.Aiming;
            if (_gameUISwitcher != null) _gameUISwitcher.ShowAimSlider(true);
            State = GState.SliderAiming;
        }
        else if (_gameInput.StartMove())
        {
            if (_gameUISwitcher != null) _gameUISwitcher.ShowMoveSlider(true);
            State = GState.MovingPlayer;
        }
        else if (_gameUIComposition.OpenOptions())
        {
            OpenOptions();
        }
        else if (_gameUIComposition.StartSliderAim())
        {
            if (_gameUISwitcher != null) _gameUISwitcher.ShowAimSlider(true);
            State = GState.SliderAiming;
        }
        else if (_gameUIComposition.GiveExtraBalls())
        {
            if (_levelService.ExtraBallPowerUpCount > 0)
            {
                _levelService.ExtraBallPowerUpCount--;
                for (int i = 0; i < _levelService.Balls.Count; i++)
                {
                    _endTurnDestroyService.AddGameObject(
                        _facBall.Create(_levelService.Balls[i])
                    );
                    _levelService.BallCounter++;
                }
            }
        }
        else if (_gameUIComposition.GiveFloorBricks())
        {
            if (_levelService.FloorBricksPowerUpCount > 0)
            {
                _levelService.FloorBricksPowerUpCount--;
                AddFloorBricks(); 
            }
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
        return State;
    }


    private void AddFloorBricks()
    {
        int closestColumn = 0;
        float closestColumnDistance = 100;
        Vector2 playerPosition = new Vector2(_player.transform.position.x, 0);
        for (int i = 0; i < _grid.NumberOfDivisions; i++)
        {
            Vector2 position = new Vector2(_grid.GetPosition(i, 0).x, 0);
            if (Vector2.Distance(playerPosition, position) < closestColumnDistance)
            {
                closestColumn = i;
                closestColumnDistance = Vector2.Distance(playerPosition, position);
            }

        }
        _player.MovePlayer(new Vector2(_grid.GetPosition(closestColumn, 0).x, _player.transform.position.y));

        for (int i = 0; i < _grid.NumberOfDivisions; i++)
        {
            BrickType brickType = BrickType.Square;
            if (i == closestColumn - 1) brickType = BrickType.Triangle0;
            else if (i == closestColumn + 1) brickType = BrickType.Triangle90;

            if (i != closestColumn)
            {
                GameObject obj = _facBrick.Create(new Brick { BrickType = brickType, Col = i, Row = _grid.NumberOfDivisions - 1, Health = 250 }, new Type[] { typeof(Advanceable) });
                _endTurnDestroyService.AddGameObject(obj);
                obj.GetComponentInChildren<Damageable>()._doesCountTowardsWinning = false;
            }
        }
    }

    private void OpenOptions()
    {
        if (_gameUI != null) _gameUI.ShowOptions();

        _stateBeforeOptions = State;
        State = GState.OptionsPanel;
    }

    private IEnumerator EndTurnRoutine()
    {
        _levelService.BallCounter = _levelService.Balls.Count;

        yield return StartCoroutine(_endTurnAttackService.Attack());

        CreateNextRow();

        _facBrick.DisableCompositeCollider();
        yield return StartCoroutine(_advanceService.Advance());
        _facBrick.EnableCompositeCollider();

        _endTurnDestroyService.DestroyGameObjects();

        if (_player.Health <= 0)
        {
            if (_gameUI != null) _gameUI.HideGame();
            if (_gameUI != null) _gameUI.ShowGameOver();

            State = GState.GameOver;
        }
        else if (_winService.HasWon())
        {
            ES3.Save(BGStrings.ES_LEVELNUM, _levelService._levelNumber + 1);

            if (_gameUI != null) _gameUI.HideGame();
            if (_gameUI != null) _gameUI.ShowWin();

            State = GState.Win;
        }
        else
        {
            if (_gameUISwitcher != null) _gameUISwitcher.StartTurn();
            State = GState.WaitingForPlayerInput;
        }
    }

    private IEnumerator SetupLevelRoutine()
    {
        _endTurnAttackService.ResetAttackService();
        _endTurnDestroyService.DestroyGameObjects(); // this is important so that when the game is reset before the end of turn, then potential objects that had been added will be destroyed. Otherwise this service will cause an error if objects had been added
        _levelService.ResetLevelService();
        if (_gameUI != null) _gameUI.ShowGame();
        if (_gameUISwitcher != null) _gameUISwitcher.StartTurn();
        _player.Health = 100;
        _player.MovePlayer(_grid.GetPosition((_grid.NumberOfDivisions - 1) / 2f, 0));
        _facBrick.DestroyBricks();
        _facBall.DestroyBalls();
        _winService.NumberOfBricksDestroyed = 0;
        _winService.NumberOfBricksToWin = _levelService.Bricks.Where(x => Brick.IsDamageable(x.BrickType)).Count();

        _facBrick.MaxHealth = _levelService.Bricks.Select(x => x.Health).Max();

        for (int i = 0; i < _levelService.NumberOfDivisions - 1; i++)
        {
            CreateNextRow();
        }

        _levelService.Balls.ForEach(x => _facBall.Create(x));
        _player.SetRadius();

        _facBrick.DisableCompositeCollider();
        yield return StartCoroutine(_advanceService.Advance());
        _facBrick.EnableCompositeCollider();

        // change states first so variable will always be true until after state change to avoid race condition, unless this happens atomically then it doesn't matter
        State = GState.WaitingForPlayerInput;
    }

    private void CreateNextRow()
    {
        _levelService.GetNextRow().ForEach(x => { int row = x.Row; x.Row = --x.Row * -1 + (int)_grid.GameBoardHeight - 1; _facBrick.Create(x); x.Row = row; }); // subtract row so will advance down into position
    }
}