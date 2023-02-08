using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowCSVSavesGame : MonoBehaviour, ISetupLevel
{
    [field: SerializeField]
    public ResourceLocator ResourceLocator { get; set; }

    private void Awake() 
    {
        ResourceLocator.AddResource("ShowCSVSavesGame", this);    
    }


    public void SetupLevel()
    {
        
    }
}
