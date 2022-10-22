using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameUI : MonoBehaviour, IResetGame, INextLevel, IOpenMainMenu, ICloseMainMenuPanel, IOpenMainMenuPanel, IOpenOptions, ICloseOptionsPanel, IStartSliderAim, IEndSliderAim, IStartFireUI, IGiveExtraBalls, IGiveFloorBricks
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

    public Slider _aimSlider;

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
                Debug.LogError("HPanels.Count != 2");
            }
        }
        else // is portrait
        {
            ActivatePortrait();
            if (PPanels.Count == 2)
            {
                PPanels[0].SetBottom(_height - _height * Background.LEAVE_SIDES_OPEN_BY_PERCENT / 2f);
                PPanels[1].SetTop(_height - _height * Background.LEAVE_SIDES_OPEN_BY_PERCENT / 2f);

            }
            else
            {
                Debug.LogError("VPanels.Count != 2");
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
        value = Mathf.Clamp(value, 1, 179);
        _aimSlider.value = value;
    }
    

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void ActivateGiveFloorBricks()
    {
        GiveFloorBricks = true;
    }

    public void ActivateGiveExtraBalls()
    {
        GiveExtraBalls = true;
    }

    public void ActivateStartSliderAim()
    {
        StartSliderAim = true;
    }

    public void ActivateEndSliderAim()
    {
        EndSliderAim = true;
    }

    public void ActivateStartFire()
    {
        StartFire = true;
    }

    public void ActivateOpenOptionsPanel()
    {
        OpenOptions = true;
    }
    
    public void ActivateCloseOptionsPanel()
    {
        CloseOptionsPanel = true;
    }

    public void ActivateOpenMainMenuOkayPanel()
    {
        OpenMainMenuPanel = true;
    }

    public void ActivateOpenMainMenu()
    {
        OpenMainMenu = true;
    }

    public void ActivateCloseMainMenuPanel()
    {
        CloseMainMenuPanel = true;
    }

    public void ActivateResetGame()
    {
        ResetGame = true;
    }

    public void ActivateNextLevel()
    {
        NextLevel = true;
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

        Dim.SetActive(false);
        HideGameOver();
        OptionsPanel.SetActive(false);
    }

    public void HideGame()
    {
        Dim.SetActive(true);

        LPanels.ForEach(x => x.gameObject.SetActive(false));
        PPanels.ForEach(x => x.gameObject.SetActive(false));
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
