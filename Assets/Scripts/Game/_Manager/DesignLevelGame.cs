using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesignLevelGame : MonoBehaviour, IWaitingForPlayerInput
{
    [field: SerializeField]
    public ResourceLocator ResourceLocator { get; set; }


    [field: SerializeField]
    private GameState GameState { get; set; } // reference set in editor

    private void Awake() 
    {
        ResourceLocator.AddResource("DesignLevelGame", this);
    }

    public void WaitingForPlayerInput()
    {
        
    }
}
