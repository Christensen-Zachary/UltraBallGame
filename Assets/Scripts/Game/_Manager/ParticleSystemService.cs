using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSystemService : MonoBehaviour
{
    [field: SerializeField]
    public ResourceLocator ResourceLocator { get; set; }


    public GameObject FirePS; // set in prefab

    private void Awake() 
    {
        ResourceLocator.AddResource("ParticleSystemService", this);
    }
}
