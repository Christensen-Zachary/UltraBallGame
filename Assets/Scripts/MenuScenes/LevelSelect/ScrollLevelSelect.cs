using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Linq;

public class ScrollLevelSelect : MonoBehaviour
{
    [field: SerializeField]
    public ScrollLevelSelectUI ScrollLevelSelectUI { get; set; } // reference set in editor
    [field: SerializeField]
    public GameObject LevelButtonPrefab { get; set; } // reference set in editor 
    [field: SerializeField]
    public GameObject BackgroundPrefab { get; set; } // reference set in editor 
    [field: SerializeField]
    public Image HeaderFade { get; set; } // reference set in editor 
    [field: SerializeField]
    public Image Header { get; set; } // reference set in editor 
    [field: SerializeField]
    public Image HeaderEdge { get; set; } // reference set in editor 
    [field: SerializeField]
    public Image FooterFade { get; set; } // reference set in editor 
    [field: SerializeField]
    public Image Footer { get; set; } // reference set in editor 
    [field: SerializeField]
    public Image FooterEdge { get; set; } // reference set in editor 


    [field: SerializeField]
    public float HeightPadding { get; set; } = 0.1f;
    [field: SerializeField]
    public float HeightWidthRatio { get; set; } = 0.2f;

    
    public GameObject background;
    float height = 0f;
    float width = 0f;
    public float unitScale = 0f;
    Vector2 origin = Vector2.zero;
    public readonly int columnCount = 5;

    public int latestLevelUnlocked = 1;
    public readonly int highestLevel = 256;
    public SpriteRenderer backgroundSpriteRenderer;
    
   
    private void Awake() 
    {
        latestLevelUnlocked = ES3.Load<int>(BGStrings.ES_LEVELNUM, 1);
        

        (height, width) = BGUtils.GetScreenSize();

        // always make grid width a ratio to the screen height. Should fit on iphone and everything, then fit to height
        // put image over top of screen that is same color as background that fades out. Buttons will go underneath but appear to fade out
        background = Instantiate(BackgroundPrefab);
        background.name = "Background";
        // background.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.25f);
        background.transform.position = Vector3.zero;
        height = height * (1 - HeightPadding * 2);
        width = width * (1 - HeightPadding);//height * HeightWidthRatio;
        background.transform.localScale = new Vector3(width, height, 0);
        backgroundSpriteRenderer = background.GetComponent<SpriteRenderer>();

        unitScale = width /columnCount;
        // bottom left corners
        origin = new Vector2(-background.GetComponent<SpriteRenderer>().bounds.extents.x, -background.GetComponent<SpriteRenderer>().bounds.extents.y) + new Vector2(0.5f, 0.5f) * unitScale;
        

        // setup level buttons


        FooterFade.color = DivideColor(ThemeData.ThemeColors[ThemeItem.SuperBackground], 2);
        HeaderFade.color = DivideColor(ThemeData.ThemeColors[ThemeItem.SuperBackground], 2);//ThemeData.ThemeColors[ThemeItem.SuperBackground];
        FooterEdge.color = ThemeData.ThemeColors[ThemeItem.GameboardBorder];
        HeaderEdge.color = ThemeData.ThemeColors[ThemeItem.GameboardBorder];

        if (PlayerPrefs.GetInt(ToggleHDR.HDR_ENABLED_KEY, 1) == 1)
        {
            HeaderEdge.material.SetFloat("_Brightness", ThemeData.ThemeBorderBrightness / 12f);
            FooterEdge.material.SetFloat("_Brightness", ThemeData.ThemeBorderBrightness / 12f);
        }
        else
        {
            HeaderEdge.material.SetFloat("_Brightness", 3);
            FooterEdge.material.SetFloat("_Brightness", 3);
        }

        Header.color = ThemeData.ThemeColors[ThemeItem.SuperBackground];
        Footer.color = ThemeData.ThemeColors[ThemeItem.SuperBackground];
    }



 

    public Vector2 GetPosition(float col, float row)
    {
        return origin + new Vector2(col, row) * new Vector2(unitScale, unitScale);
    }

    private Color DivideColor(Color color, float divisor)
    {
        return new Color(color.r / divisor, color.g / divisor, color.b / divisor, color.a);
    }
  
    // only get average of values of the same sign, whichever sign there is the most of
    private float GetAverage(float[] arr)
    {
        int positiveCount = 0;
        int negativeCount = 0;
        for (int i = 0; i < arr.Length; i++)
        {
            if (arr[i] >= 0) positiveCount++;
            else if (arr[i] < 0) negativeCount++;
        }

        int numbersUsedCount = 0;
        float sum = 0;
        for (int i = 0; i < arr.Length; i++)
        {
            if (arr[i] == 0) continue;

            if (positiveCount >= negativeCount)
            {
                if (arr[i] > 0)
                {
                    sum += arr[i];
                    numbersUsedCount++; 
                }
            }
            else
            {
                if (arr[i] < 0)
                {
                    sum += arr[i];
                    numbersUsedCount++;
                }
            }
        }

        if (numbersUsedCount == 0) numbersUsedCount = 1; // prevent divide by zero
        return sum / numbersUsedCount;
    }

}
