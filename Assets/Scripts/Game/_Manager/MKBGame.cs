using UnityEngine;

public class MKBGame : MonoBehaviour, IWaitingForPlayerInput
{
    [field: SerializeField]
    public ResourceLocator ResourceLocator { get; set; }

    private GameInput _gameInput;
    private GameUIComposition _gameUIComposition;
    private GameUI _gameUI;
    private LevelService _levelService;
    private EndTurnDestroyService _endTurnDestroyService;
    private FacBall _facBall;



    private GameUISwitcher _gameUISwitcher;

    [field: SerializeField]
    private GameState GameState { get; set;} // reference set in editor

    [field: SerializeField]
    private FSMGameComposition FSMGameComposition { get; set;} // reference set in editor
    [field: SerializeField]
    private NormalGame NormalGame { get; set;} // reference set in editor

    private void Awake()
    {
        ResourceLocator.AddResource("MKBGame", this);

        _gameInput = ResourceLocator.GetResource<GameInput>("GameInput");
        _levelService = ResourceLocator.GetResource<LevelService>("Level");
        _facBall = ResourceLocator.GetResource<FacBall>("FacBall");
        _endTurnDestroyService = ResourceLocator.GetResource<EndTurnDestroyService>("EndTurnDestroyService");
        _gameUI = ResourceLocator.GetResource<GameUI>("GameUI");
        _gameUISwitcher = ResourceLocator.GetResource<GameUISwitcher>("GameUISwitcher");
        _gameUIComposition = ResourceLocator.GetResource<GameUIComposition>("GameUIComposition");
    }

    public void WaitingForPlayerInput()
    {
        if (_gameInput.StartAim())
        {
            //_state = GState.Aiming;
            if (_gameUISwitcher != null) _gameUISwitcher.ShowAimSlider(true);
            GameState.State = GState.Aiming;
        }
        else if (_gameInput.StartMove())
        {
            print("Starting move");
            GameState.State = GState.MovingPlayer;
        }
        else if (_gameUIComposition.OpenOptions())
        {
            if (_gameUI != null) _gameUI.ShowOptions();

            GameState.StateBeforeOptions = GameState.State;
            GameState.State = GState.OptionsPanel;
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
                NormalGame.AddFloorBricks(); 
            }
        }
        else if (_gameUIComposition.SetBallsOnFire())
        {
            if (_levelService.FireBallsPowerUpCount > 0)
            {
                NormalGame.SetBallsOnFire(); 
            }
        }
    }
}
