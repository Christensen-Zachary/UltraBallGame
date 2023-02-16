using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettings : MonoBehaviour
{
    [field: SerializeField]
    public ResourceLocator ResourceLocator { get; set; }

    public GameType gameType;
    public GameUIType gameUIType;
    public InputSource inputSource;

    private void Awake()
    {
        ResourceLocator.AddResource("GameSettings", this);
    }
}
