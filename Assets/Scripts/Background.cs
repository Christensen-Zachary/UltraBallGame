using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Background : MonoBehaviour
{
    private float _leaveSidesOpenByPercent = 1 / 3f; // percent of the side that should not be overlapped by square. amount is for both sides combined

    private void Awake()
    {
        (float height, float width) = BGUtils.GetScreenSize();

        float scale;
        if (height < width)
        {
            scale = height;
            if ((width - height) < (_leaveSidesOpenByPercent * width))
            {
                scale = (1 - _leaveSidesOpenByPercent) * height;
            }
        }
        else
        {
            scale = width;
            if ((height - width) < (_leaveSidesOpenByPercent * height))
            {
                scale = (1 - _leaveSidesOpenByPercent) * width;
            }
        }
        transform.localScale = Vector2.one * scale;
        transform.localPosition = Vector3.zero;
    }

}
