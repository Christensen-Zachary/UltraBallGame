using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    void Start()
    {
        for (int i = 0; i < 3; i++)
        {
            LevelService.SaveLevel(Level.GetRandom());
        }
    }

}
