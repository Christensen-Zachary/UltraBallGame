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


    private void Awake()
    {
        ResourceLocator.AddResource("GameUISwitcher", this);
    }

    public void ShowMoveSlider(bool bit)
    {
        BtnStartMove.SetActive(!bit);
        
        BtnEndMove.SetActive(bit);
        MoveSlider.SetActive(bit);
    }

    public void StartFire()
    {
        BtnEndMove.SetActive(false);
        BtnStartMove.SetActive(false);
        MoveSlider.SetActive(false);

        BtnReturnBalls.SetActive(true);
    }
    
    public void StartTurn()
    {
        MoveSlider.SetActive(false);
        BtnReturnBalls.SetActive(false);

        BtnStartMove.SetActive(true);
    }

}
