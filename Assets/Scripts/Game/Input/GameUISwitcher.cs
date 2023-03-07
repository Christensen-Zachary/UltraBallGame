using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUISwitcher : MonoBehaviour
{
    [field: SerializeField]
    public ResourceLocator ResourceLocator { get; set; }
    [field: SerializeField]
    public GameObject MoveSlider { get; set; }
    [field: SerializeField]
    public GameObject BtnReturnBalls { get; set; }
    [field: SerializeField]
    public GameObject AimSlider { get; set; }
    [field: SerializeField]
    public GameObject BtnStartAim { get; set; }
    [field: SerializeField]
    public GameObject BtnEndAim { get; set; }
    [field: SerializeField]
    public GameObject BtnFire { get; set; }
    [field: SerializeField]
    public BtnFadeAnimation ExtraBallsAnimator { get; set; }
    [field: SerializeField]
    public BtnFadeAnimation FloorBricksAnimator { get; set; }
    [field: SerializeField]
    public BtnFadeAnimation FireBallsAnimator { get; set; }
    [field: SerializeField]
    public BtnFadeAnimation OptionsAnimator { get; set; }
    [field: SerializeField]
    public GameObject BtnVertical { get; set; }
    [field: SerializeField]
    public GameObject BtnHorizontal { get; set; }
    [field: SerializeField]
    public GameObject BtnRandom { get; set; }
    


    private void Awake()
    {
        ResourceLocator.AddResource("GameUISwitcher", this);
    }

    public void ShowAimSlider(bool bit)
    {
        DoFade(ExtraBallsAnimator, !bit);
        DoFade(FloorBricksAnimator, !bit);
        DoFade(FireBallsAnimator, !bit);
        DoFade(OptionsAnimator, !bit);
        
        BtnEndAim.SetActive(bit);
        BtnFire.SetActive(bit);
        MoveSlider.SetActive(bit);
    }

    private void DoFade(BtnFadeAnimation btnFadeAnimation, bool bit)
    {
        if (bit) btnFadeAnimation.FadeIn();   
        else btnFadeAnimation.FadeOut();
    }

    public void StartFire()
    {
        BtnEndAim.SetActive(false);
        MoveSlider.SetActive(false);
        BtnFire.SetActive(false);

        BtnVertical.SetActive(true);
        BtnHorizontal.SetActive(true);
        BtnRandom.SetActive(true);
        BtnReturnBalls.SetActive(true);
    }
    
    public void EndFire()
    {
        BtnVertical.SetActive(false);
        BtnHorizontal.SetActive(false);
        BtnRandom.SetActive(false);
        BtnReturnBalls.SetActive(false);
    }

    public void StartTurn()
    {
        BtnReturnBalls.SetActive(false);
        
        DoFade(OptionsAnimator, true);
        DoFade(ExtraBallsAnimator, true);
        DoFade(FloorBricksAnimator, true);
        DoFade(FireBallsAnimator, true);
    }

    public void HideGameButtons()
    {
        OptionsAnimator.Hide();
        ExtraBallsAnimator.Hide();
        FloorBricksAnimator.Hide();
        FireBallsAnimator.Hide();
    }

}
