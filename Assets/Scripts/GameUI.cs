using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUI : MonoBehaviour
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

    private float _height;
    private float _width;

    public bool ResetGame { get; set; } = false;
    private int _resetGameCounter = 0;
    public bool NextLevel { get; set; } = false;
    private int _nextLevelCounter = 0;

    public bool OpenMainMenuPanel { get; set; }
    private int _openMainMenuPanelCounter = 0;

    public bool CloseMainMenuPanel { get; set; }
    private int _closeMainMenuPanelCounter = 0;

    public bool OpenMainMenu { get; set; }
    private int _openMainMenuCounter = 0;

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

    private void Update()
    {
        if (ResetGame)
        {
            if (_resetGameCounter++ > 0)
            {
                ResetGame = false;
                _resetGameCounter = 0;
            }
        }

        if (NextLevel)
        {
            if (_nextLevelCounter++ > 0)
            {
                NextLevel = false;
                _nextLevelCounter = 0;
            }
        }

        if (OpenMainMenuPanel)
        {
            if (_openMainMenuPanelCounter++ > 0)
            {
                OpenMainMenuPanel = false;
                _openMainMenuPanelCounter = 0;
            }
        }
        
        if (CloseMainMenuPanel)
        {
            if (_closeMainMenuPanelCounter++ > 0)
            {
                CloseMainMenuPanel = false;
                _closeMainMenuPanelCounter = 0;
            }
        }

        if (OpenMainMenu)
        {
            if (_openMainMenuCounter++ > 0)
            {
                OpenMainMenu = false;
                _openMainMenuCounter = 0;
            }
        }
    }

    public void ShowMainMenuOkayPanel()
    {
        MainMenuOkayPanel.SetActive(true);
        HideGame();
    }

    public void HideMainMenuOkayPanel()
    {
        MainMenuOkayPanel.SetActive(false);
        ShowGame();
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
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

        HideGameOver();
    }

    public void HideGame()
    {
        LPanels.ForEach(x => x.gameObject.SetActive(false));
        PPanels.ForEach(x => x.gameObject.SetActive(false));
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
}
