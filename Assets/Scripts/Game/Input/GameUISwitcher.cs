using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUISwitcher : MonoBehaviour
{
    [field: SerializeField]
    public ResourceLocator ResourceLocator { get; set; }
    [field: SerializeField]
    public GameObject BtnStartMove { get; set; }
    [field: SerializeField]
    public GameObject BtnEndMove { get; set; }
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
    public GameObject BtnExtraBalls { get; set; }
    [field: SerializeField]
    public GameObject BtnFloorBricks { get; set; }
    [field: SerializeField]
    public GameObject BtnFireBalls { get; set; }
    


    private void Awake()
    {
        ResourceLocator.AddResource("GameUISwitcher", this);
    }

    public void ShowAimSlider(bool bit)
    {
        BtnStartMove.SetActive(!bit);
        //BtnStartAim.SetActive(!bit);
        BtnExtraBalls.SetActive(!bit);
        BtnFloorBricks.SetActive(!bit);
        BtnFireBalls.SetActive(!bit);

        BtnEndAim.SetActive(bit);
        BtnFire.SetActive(bit);
        AimSlider.SetActive(bit);
    }

    public void ShowMoveSlider(bool bit)
    {
        BtnStartMove.SetActive(!bit);
        //BtnStartAim.SetActive(!bit);
        BtnExtraBalls.SetActive(!bit);
        BtnFloorBricks.SetActive(!bit);
        BtnFireBalls.SetActive(!bit);

        BtnEndMove.SetActive(bit);
        MoveSlider.SetActive(bit);
    }

    public void StartFire()
    {
        BtnStartMove.SetActive(false);
        //BtnStartAim.SetActive(false);
        BtnEndAim.SetActive(false);
        AimSlider.SetActive(false);
        BtnFire.SetActive(false);
        BtnExtraBalls.SetActive(false);
        BtnFloorBricks.SetActive(false);
        BtnFireBalls.SetActive(false);

        BtnReturnBalls.SetActive(true);
    }
    
    public void StartTurn()
    {
        BtnReturnBalls.SetActive(false);

        BtnStartMove.SetActive(true);
        //BtnStartAim.SetActive(true);
        BtnExtraBalls.SetActive(true);
        BtnFloorBricks.SetActive(true);
        BtnFireBalls.SetActive(true);
    }

}
