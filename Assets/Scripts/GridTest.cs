using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridTest : MonoBehaviour
{
    public GameObject _testSprite;
    [field: SerializeField]
    public ResourceLocator ResourceLocator { get; set; }
    void Start()
    {
        Grid grid = ResourceLocator.GetResource<Grid>("Grid");

        for (int i = 0; i < grid.NumberOfDivisions; i++)
        {
            for (int j = 0; j < grid.NumberOfDivisions; j++)
            { 
                GameObject obj = Instantiate(_testSprite);
                obj.transform.localPosition = grid.GetPosition(i, j);
            }
        }
    }


}
