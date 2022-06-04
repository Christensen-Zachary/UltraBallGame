using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    [field: SerializeField]
    public ResourceLocator ResourceLocator { get; set; }
    private float _leaveSidesOpenByPercent = 1 - (2 / (1 + Mathf.Sqrt(5))); // percent of the side that should not be overlapped by square. amount is for both sides combined
    [field: SerializeField]
    public List<RectTransform> LPanels { get; set; }
    [field: SerializeField]
    public List<RectTransform> PPanels { get; set; }
    [field: SerializeField]
    public GameObject GameOverPanel { get; set; }
    [field: SerializeField]
    public GameObject WinPanel { get; set; }

    private float _height;
    private float _width;

    public bool ResetGame { get; set; } = false;
    private int _resetGameCounter = 0;

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
                LPanels[0].SetRight(_width - _width * _leaveSidesOpenByPercent / 2f);
                LPanels[1].SetLeft(_width - _width * _leaveSidesOpenByPercent / 2f);
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
                PPanels[0].SetBottom(_height - _height * _leaveSidesOpenByPercent / 2f);
                PPanels[1].SetTop(_height - _height * _leaveSidesOpenByPercent / 2f);
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
    }

    public void ActivateResetGame()
    {
        ResetGame = true;
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
