using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelButton : MonoBehaviour
{
    public int row = 0;
    public int col = 0;
    public int levelNumber = -1;
    public GameObject upArrow;
    public GameObject rightArrow;
    public GameObject leftArrow;
    private float betweenSpriteOffset = 0.75f;


    public void Return()
    {
        transform.SetParent(null);
        transform.position = 100 * Vector2.one;
    }

    public void SetBetweenButtonUp()
    {
        HideArrows();
        upArrow.SetActive(true);
    }

    public void SetBetweenButtonLeft()
    {
        HideArrows();
        leftArrow.SetActive(true);
    }

    public void SetBetweenButtonRight()
    {
        HideArrows();
        rightArrow.SetActive(true);
    }

    public void HideArrows()
    {
        upArrow.SetActive(false);
        leftArrow.SetActive(false);
        rightArrow.SetActive(false);
    }
}
