using System.Collections;
using System.Collections.Generic;
using System.Net.Security;
using FastMobileBlurURP2023;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameUI : MonoBehaviour, IHorizontal, IVertical, IRandom, IReturnFire, IResetGame, INextLevel, IOpenMainMenu, ICloseMainMenuPanel, IOpenMainMenuPanel, IOpenOptions, ICloseOptionsPanel, IStartSliderAim, IEndSliderAim, IStartFireUI, IGiveExtraBalls, IGiveFloorBricks, ISetBallsOnFire
{
    [field: SerializeField]
    public ResourceLocator ResourceLocator { get; set; }
    [field: SerializeField]
    public List<RectTransform> LPanels { get; set; }
    [field: SerializeField]
    public List<RectTransform> PPanels { get; set; }
    [field: SerializeField]
    public GameObject GameOverPanel { get; set; }
    [field: SerializeField]
    public GameObject WinPanel { get; set; }
    [field: SerializeField]
    public GameObject MainMenuOkayPanel { get; set; }
    [field: SerializeField]
    public GameObject OptionsPanel { get; set; }
    
    [field: SerializeField]
    public GameObject OptionsButton { get; set; }

    [field: SerializeField]
    public GameObject Dim { get; set; }
    public Animator animator;

    public Slider _aimSlider;
    public Slider _movePlayerSlider; // used here for responsive design, otherwise is controlled by player class

    private float _height;
    private float _width;

    public bool ResetGame { get { if (_resetGame) { _resetGame = false; return true; } return false; } set { _resetGame = value; } }
    private bool _resetGame = false;

    public bool NextLevel { get { if (_nextLevel) { _nextLevel = false; return true; } return false; } set { _nextLevel = value; } }
    private bool _nextLevel = false;

    public bool OpenMainMenuPanel { get { if (_openMainMenuPanel) { _openMainMenuPanel = false; return true; } return false; } set { _openMainMenuPanel = value; } }
    private bool _openMainMenuPanel = false;

    public bool CloseMainMenuPanel { get { if (_closeMainMenuPanel) { _closeMainMenuPanel = false; return true; } return false; } set { _closeMainMenuPanel = value; } }
    private bool _closeMainMenuPanel = false;

    public bool OpenMainMenu { get { if (_openMainMenu) { _openMainMenu = false; return true; } return false; } set { _openMainMenu = value; } }
    private bool _openMainMenu = false;

    public bool OpenOptions { get { if (_openOptions) { _openOptions = false; return true; } return false; } set { _openOptions = value; } }
    private bool _openOptions = false;

    public bool CloseOptionsPanel { get { if (_closeOptions) { _closeOptions = false; return true; } return false; } set { _closeOptions = value; } }
    private bool _closeOptions = false;

    public bool StartSliderAim { get { if (_startSliderAim) { _startSliderAim = false; return true; } return false; } set {_startSliderAim = value; } }
    private bool _startSliderAim = false;

    public bool EndSliderAim { get { if (_endSliderAim) { _endSliderAim = false; return true; } return false; } set { _endSliderAim = value; } }
    private bool _endSliderAim = false;

    public bool StartFire { get { if (_startFire) { _startFire = false; return true; } return false; } set { _startFire = value; } }
    private bool _startFire = false;

    public bool GiveExtraBalls { get { if (_giveExtraBalls) { _giveExtraBalls = false; return true; } return false; } set { _giveExtraBalls = value; } }
    private bool _giveExtraBalls = false;

    public bool GiveFloorBricks { get { if (_giveFloorBricks) { _giveFloorBricks = false; return true; } return false; } set { _giveFloorBricks = value; } }
    private bool _giveFloorBricks = false;
    
    public bool GiveFireBalls { get { if (_giveFireBalls) { _giveFireBalls = false; return true; } return false; } set { _giveFireBalls = value; } }
    private bool _giveFireBalls = false;
    
    private bool PSetBallsOnFire { get { if (_setBallsOnFire) { _setBallsOnFire = false; return true; } return false; } set { _setBallsOnFire = value; } }
    private bool _setBallsOnFire = false;

    private bool PReturnFire { get { if (_returnFire) { _returnFire = false; return true; } return false; } set { _returnFire = value; } }
    private bool _returnFire = false;

    private bool PHorizontal { get { if (_horizontal) { _horizontal = false; return true; } return false; } set { _horizontal = value; } }
    private bool _horizontal = false;

    private bool PVertical { get { if (_vertical) { _vertical = false; return true; } return false; } set { _vertical = value; } }
    private bool _vertical = false;

    private bool PRandom { get { if (_random) { _random = false; return true; } return false; } set { _random = value; } }
    private bool _random = false;


    private void Awake()
    {
        ResourceLocator.AddResource("GameUI", this);

        RectTransform canvas = GetComponent<RectTransform>();
        float canvasScreenRatio = canvas.rect.height / BGUtils.GetScreenSize().height;

        (_height, _width) = BGUtils.GetScreenSize();
        _height *= canvasScreenRatio;
        _width *= canvasScreenRatio;
   
        //print($"CanvasRectHeight {canvas.rect.height} ScreenHeight {BGUtils.GetScreenSize().height} Height {height} Width {width} CanvasScreenRatio {canvasScreenRatio}");

        if (_height < _width) // is landscape
        {
            ActivateLandscape();
            if (LPanels.Count == 2)
            {
                LPanels[0].SetRight(_width - _width * Background.LEAVE_SIDES_OPEN_BY_PERCENT / 2f);
                LPanels[1].SetLeft(_width - _width * Background.LEAVE_SIDES_OPEN_BY_PERCENT / 2f);
            }
            else
            {
                Debug.LogError("LPanels.Count != 2");
            }
        }
        else // is portrait
        {
            ActivatePortrait();
            if (PPanels.Count == 2)
            {
                (float height, float width) = BGUtils.GetScreenSize();
                if (height / width < 1.9f)//(height - width) < (Background.LEAVE_SIDES_OPEN_BY_PERCENT * height))
                {
                    print($"Setting top to 0");
                    PPanels[0].SetTop(0);
                    PPanels[1].SetBottom(0);

                    
                    PPanels[0].SetBottom(_height - _height * Background.LEAVE_SIDES_OPEN_BY_PERCENT / 2.1f);
                    PPanels[1].SetTop(_height - _height * Background.LEAVE_SIDES_OPEN_BY_PERCENT / 2.1f);

                    Vector2 size = _aimSlider.GetComponent<RectTransform>().sizeDelta;
                    _aimSlider.GetComponent<RectTransform>().sizeDelta = new Vector2(290, size.y);

                    size = _movePlayerSlider.GetComponent<RectTransform>().sizeDelta;
                    _movePlayerSlider.GetComponent<RectTransform>().sizeDelta = new Vector2(290, size.y);

                    if (0.9f * width < height * (1 - Background.LEAVE_SIDES_OPEN_BY_PERCENT))
                    {
                        PPanels[0].SetBottom(465);
                        PPanels[1].SetTop(465);
                    }
                }
                else
                {
                    PPanels[0].SetBottom(_height - _height * Background.LEAVE_SIDES_OPEN_BY_PERCENT / 2f);
                    PPanels[1].SetTop(_height - _height * Background.LEAVE_SIDES_OPEN_BY_PERCENT / 1.7f);
                }
            }
            else
            {
                Debug.LogError("PPanels.Count != 2");
            }
        }
    }

    public Vector2 GetFireDirection()
    {
        float angle = Mathf.Deg2Rad * _aimSlider.value;// * 180f;
        return new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
    }

    public void SetSliderValue(float value)
    {
        value = Mathf.Clamp(value, 0, 180);
        _aimSlider.value = value;
    }
    

    public void LoadMainMenu()
    {
        StartCoroutine(LoadMainMenuCoroutine());
    }

    private IEnumerator SetRandomRoutine()
    {
        PRandom = true;
        yield return null;
        PRandom = false;
    }


    private IEnumerator SetVerticalRoutine()
    {
        PVertical = true;
        yield return null;
        PVertical = false;
    }

    private IEnumerator SetHorizontalRoutine()
    {
        PHorizontal = true;
        yield return null;
        PHorizontal = false;
    }

    private IEnumerator SetReturnFireRoutine()
    {
        PReturnFire = true;
        yield return null;
        PReturnFire = false;
    }

    private IEnumerator LoadMainMenuCoroutine()
    {
        animator.SetTrigger("Close");

        yield return new WaitForSeconds(MainMenuUI.SCENE_TRANSITION_WAIT_TIME);

        MainMenuUI.LoadMainMenu();
    }

    private IEnumerator SetGiveFireBalls()
    {
        PSetBallsOnFire = true;
        yield return null;
        PSetBallsOnFire = false;
    }

    public IEnumerator SetGiveFloorBricks()
    {
        _giveFloorBricks = true;
        yield return null;
        _giveFloorBricks = false;
    }

    public IEnumerator SetGiveExtraBalls()
    {
        _giveExtraBalls = true;
        yield return null;
        _giveExtraBalls = false;
    }

    public IEnumerator SetStartSliderAim()
    {
        _startSliderAim = true;
        yield return null;
        _startSliderAim = false;
    }

    public IEnumerator SetEndSliderAim()
    {
        _endSliderAim = true;
        yield return null;
        _endSliderAim = false;
    }

    public IEnumerator SetStartFire()
    {
        _startFire = true;
        yield return null;
        _startFire = false;
    }

    public IEnumerator SetOpenOptionsPanel()
    {
        _openOptions = true;
        yield return null;
        _openOptions = false;
    }

    public IEnumerator SetCloseOptionsPanel()
    {
        _closeOptions = true;
        yield return null;
        _closeOptions = false;
    }

    public IEnumerator SetOpenMainMenuOkayPanel()
    {
        _openMainMenuPanel = true;
        yield return null;
        _openMainMenuPanel = false;
    }

    public IEnumerator SetOpenMainMenu()
    {
        _openMainMenu = true;
        yield return null;
        _openMainMenu = false;
    }

    public IEnumerator SetCloseMainMenuOkayPanel()
    {
        _closeMainMenuPanel = true;
        yield return null;
        _closeMainMenuPanel = false;
    }

    public IEnumerator SetResetGame()
    {
        _resetGame = true;
        yield return null;
        _resetGame = false;
    }

    public IEnumerator SetNextLevel()
    {
        _nextLevel = true;
        yield return null;
        _nextLevel = false;
    }

    public void ActivateRandom()
    {
        if (!PRandom)
        {
            StartCoroutine(SetRandomRoutine());
        }
    }

    public void ActivateVertical()
    {
        if (!PVertical)
        {
            StartCoroutine(SetVerticalRoutine());
        }
    }

    public void ActivateHorizontal()
    {
        if (!PHorizontal)
        {
            StartCoroutine(SetHorizontalRoutine());
        }
    }

    public void ActivateReturnFire()
    {
        if (!PReturnFire)
        {
            StartCoroutine(SetReturnFireRoutine());
        }
    }

    public void ActivateGiveFireBalls()
    {
        if (!GiveFireBalls)
        {
            StartCoroutine(SetGiveFireBalls());
        }
    }


    public void ActivateGiveFloorBricks()
    {
        if (!GiveFloorBricks)
        {
            StartCoroutine(SetGiveFloorBricks());
        }
    }

    public void ActivateGiveExtraBalls()
    {
        if (!GiveExtraBalls)
        {
            StartCoroutine(SetGiveExtraBalls());
        }
    }

    public void ActivateStartSliderAim()
    {
        if (!StartSliderAim)
        {
            StartCoroutine(SetStartSliderAim());
        }
    }

    public void ActivateEndSliderAim()
    {
        if (!EndSliderAim)
        {
            StartCoroutine(SetEndSliderAim());
        }
    }

    public void ActivateStartFire()
    {
        if (!StartFire)
        {
            StartCoroutine(SetStartFire());
        }
    }

    public void ActivateOpenOptionsPanel()
    {
        if (!OpenOptions)
        {
            StartCoroutine(SetOpenOptionsPanel());
        }
    }

    public void ActivateCloseOptionsPanel()
    {
        if (!CloseOptionsPanel)
        {
            StartCoroutine(SetCloseOptionsPanel());
        }
    }

    public void ActivateOpenMainMenuOkayPanel()
    {
        if (!OpenMainMenuPanel)
        {
            StartCoroutine(SetOpenMainMenuOkayPanel());
        }
    }

    public void ActivateOpenMainMenu()
    {
        if (!OpenMainMenu)
        {
            StartCoroutine(SetOpenMainMenu());
        }
    }

    public void ActivateCloseMainMenuPanel()
    {
        if (!CloseMainMenuPanel)
        {
            StartCoroutine(SetCloseMainMenuOkayPanel());
        }
    }

    public void ActivateResetGame()
    {
        if (!ResetGame)
        {
            StartCoroutine(SetResetGame());
        }
    }

    public void ActivateNextLevel()
    {
        if (!NextLevel)
        {
            StartCoroutine(SetNextLevel());
        }
    }

    public void ActivateLandscape()
    {
        LPanels.ForEach(x => x.gameObject.SetActive(true));
        PPanels.ForEach(x => x.gameObject.SetActive(false));
    }

    public void ActivatePortrait()
    {
        LPanels.ForEach(x => x.gameObject.SetActive(false));
        PPanels.ForEach(x => x.gameObject.SetActive(true));
    }

    public void ShowGame()
    {
        if (_height < _width) // is landscape
        {
            ActivateLandscape();
        }
        else // is portrait
        {
            ActivatePortrait();
        }

        
        ActivateOverlay(false);
        HideGameOver();
        OptionsPanel.SetActive(false);
    }

    public void HideGame()
    {
        ActivateOverlay(true);

        LPanels.ForEach(x => x.gameObject.SetActive(false));
        PPanels.ForEach(x => x.gameObject.SetActive(false));
    }

    private void ActivateOverlay(bool activate)
    {
        BlurURP.Settings.IsActive = activate;
        Dim.SetActive(activate);
    }

    public void ShowOptions()
    {
        OptionsPanel.SetActive(true);
        HideGame();
    }

    public void HideOptions()
    {
        OptionsPanel.SetActive(false);
        ShowGame();
    }

    public void ShowMainMenuOkayPanel()
    {
        MainMenuOkayPanel.SetActive(true);
        OptionsPanel.SetActive(false);
        HideGame();
    }

    public void HideMainMenuOkayPanel()
    {
        MainMenuOkayPanel.SetActive(false);
        ShowGame();
    }

    public void ShowGameOver()
    {
        GameOverPanel.SetActive(true);
    }

    public void HideGameOver()
    {
        GameOverPanel.SetActive(false);
    }

    public void ShowWin()
    {
        WinPanel.SetActive(true);
    }

    public void HideWin()
    {
        WinPanel.SetActive(false);
    }

    public bool Horizontal()
    {
        return PHorizontal;
    }

    public bool Vertical()
    {
        return PVertical;
    }

    public bool Random()
    {
        return PRandom;
    }

    public bool ReturnFire()
    {
        return PReturnFire;
    }

    bool IResetGame.ResetGame()
    {
        return ResetGame;
    }

    bool IOpenMainMenu.OpenMainMenu()
    {
        return OpenMainMenu;
    }

    bool INextLevel.NextLevel()
    {
        return NextLevel;
    }


    bool ISetBallsOnFire.SetBallsOnFire()
    {
        return PSetBallsOnFire;
    }

    bool IGiveFloorBricks.GiveFloorBricks()
    {
        return GiveFloorBricks;
    }

    bool IGiveExtraBalls.GiveExtraBalls()
    {
        return GiveExtraBalls;
    }

    bool IStartFireUI.StartFire()
    {
        return StartFire;
    }

    bool IEndSliderAim.EndSliderAim()
    {
        return EndSliderAim;
    }

    bool IStartSliderAim.StartSliderAim()
    {
        return StartSliderAim;
    }

    bool ICloseOptionsPanel.CloseOptionsPanel()
    {
        return CloseOptionsPanel;
    }

    bool IOpenOptions.OpenOptions()
    {
        return OpenOptions;
    }

    bool IOpenMainMenuPanel.OpenMainMenuPanel()
    {
        return OpenMainMenuPanel;
    }

    bool ICloseMainMenuPanel.CloseMainMenuPanel()
    {
        return CloseMainMenuPanel;
    }

}
