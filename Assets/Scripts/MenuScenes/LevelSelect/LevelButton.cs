using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelButton : MonoBehaviour
{
    public int row = 0;
    public int col = 0;
    public int levelNumber = -1;



    public void Return()
    {
        transform.SetParent(null);
        transform.position = 100 * Vector2.one;


    }
}
