using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    private float _leaveSidesOpenByPercent = 1 - (2 / (1 + Mathf.Sqrt(5))); // percent of the side that should not be overlapped by square. amount is for both sides combined
    [field: SerializeField]
    public List<RectTransform> LPanels { get; set; }
    [field: SerializeField]
    public List<RectTransform> PPanels { get; set; }

    private void Awake()
    {
        RectTransform canvas = GetComponent<RectTransform>();
        float canvasScreenRatio = canvas.rect.height / BGUtils.GetScreenSize().height;

        (float height, float width) = BGUtils.GetScreenSize();
        height *= canvasScreenRatio;
        width *= canvasScreenRatio;
   
        //print($"CanvasRectHeight {canvas.rect.height} ScreenHeight {BGUtils.GetScreenSize().height} Height {height} Width {width} CanvasScreenRatio {canvasScreenRatio}");

        if (height < width) // is landscape
        {
            ActivateLandscape();
            if (LPanels.Count == 2)
            {
                LPanels[0].SetRight(width - width * _leaveSidesOpenByPercent / 2f);
                LPanels[1].SetLeft(width - width * _leaveSidesOpenByPercent / 2f);
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
                PPanels[0].SetBottom(height - height * _leaveSidesOpenByPercent / 2f);
                PPanels[1].SetTop(height - height * _leaveSidesOpenByPercent / 2f);
            }
            else
            {
                Debug.LogError("VPanels.Count != 2");
            }
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
}
