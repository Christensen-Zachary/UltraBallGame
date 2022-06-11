using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ThemeItem
{
    Player,
    Background,
    MaxDamage,
    MinDamage,
    PlayerMaxHealth,
    PlayerMinHealth,
    MidPrediction,
    EndPrediction,
    BasicBall,
    Camera,
    Button
}

public enum ThemeType
{
    Default,
    Theme1,
    JellyFish
}

public enum CustomColor
{
    Black,
    Grey,
    DarkGreen,
    LightGreen,
    LightRed,
    Brown,
    LightYellow,
    Orange,
    Green,
    Red
}

public class ThemeVisitor : MonoBehaviour
{
    [field: SerializeField]
    public TMPro.TMP_FontAsset Font1 { get; set; } // set in prefab
    [field: SerializeField]
    public TMPro.TMP_FontAsset Font2 { get; set; } // set in prefab

    private static ThemeType themeType;
    private void Awake()
    {
        ES3.Save<ThemeType>(BGStrings.ES_THEMETYPE, ThemeType.JellyFish);
        themeType = ES3.Load<ThemeType>(BGStrings.ES_THEMETYPE, ThemeType.Default);
        SetThemeType(themeType);
        
        ThemeFonts = new Dictionary<ThemeType, TMPro.TMP_FontAsset>()
        {
            { ThemeType.Default, Font1 },
            { ThemeType.JellyFish, Font2 }
        };

    }

    public static Dictionary<ThemeItem, Color> ThemeColors { get; private set; } = new Dictionary<ThemeItem, Color>() {
        // default colors, are overwritten in GetThemeColors
        { ThemeItem.Player, GetColor(CustomColor.Orange) },
        { ThemeItem.Background, GetColor(CustomColor.Grey) },
        { ThemeItem.MaxDamage, GetColor(CustomColor.DarkGreen) },
        { ThemeItem.MinDamage, GetColor(CustomColor.LightGreen) },
        { ThemeItem.PlayerMaxHealth, GetColor(CustomColor.Green) },
        { ThemeItem.PlayerMinHealth, GetColor(CustomColor.Red) },
        { ThemeItem.MidPrediction, GetColor(CustomColor.LightYellow) },
        { ThemeItem.EndPrediction, GetColor(CustomColor.LightYellow) },
        { ThemeItem.BasicBall, GetColor(CustomColor.Orange) },
        { ThemeItem.Camera, Color.black },
        { ThemeItem.Button, GetColor(CustomColor.Brown) }
    };

    public static Dictionary<ThemeType, TMPro.TMP_FontAsset> ThemeFonts { get; private set; }
    public static void SetThemeType(ThemeType themeType)
    {
        switch (themeType)
        {
            case ThemeType.Theme1:
                SetThemeColor(ThemeItem.Player, GetColor(CustomColor.DarkGreen));
                SetThemeColor(ThemeItem.MaxDamage, GetColor(CustomColor.LightRed));
                SetThemeColor(ThemeItem.MinDamage, GetColor(CustomColor.Brown));
                SetThemeColor(ThemeItem.MidPrediction, GetColor(CustomColor.LightGreen));
                SetThemeColor(ThemeItem.EndPrediction, GetColor(CustomColor.LightGreen));
                SetThemeColor(ThemeItem.Button, GetColor(CustomColor.DarkGreen));
                break;
            case ThemeType.JellyFish:
                SetThemeColor(ThemeItem.Player, ConvertToColor(0x3e, 0xa1, 0xb6)); // moonstone
                SetThemeColor(ThemeItem.BasicBall, ConvertToColor(0x3e, 0xa1, 0xb6));
                SetThemeColor(ThemeItem.Camera, ConvertToColor(9, 28, 42));
                //SetThemeColor(ThemeColor.Background, ConvertToColor(19, 56, 85)); // space cadet
                SetThemeColor(ThemeItem.Background, Color.black);
                SetThemeColor(ThemeItem.MaxDamage, ConvertToColor(187, 144, 200)); // lenurple
                SetThemeColor(ThemeItem.MinDamage, ConvertToColor(239, 216, 236)); // piggy pink
                SetThemeColor(ThemeItem.PlayerMaxHealth, ConvertToColor(187, 144, 200));
                SetThemeColor(ThemeItem.PlayerMinHealth, ConvertToColor(239, 216, 236)); 
                SetThemeColor(ThemeItem.MidPrediction, ConvertToColor(107, 102, 158)); // Dark blue grey
                SetThemeColor(ThemeItem.EndPrediction, ConvertToColor(107, 102, 158));
                SetThemeColor(ThemeItem.Button, GetColor(CustomColor.Brown));
                break;
        }
    }

    private static void SetThemeColor(ThemeItem themeItem, Color color)
    {
        ThemeColors[themeItem] = color;
    }

    public static void Visit(Damageable damageable)
    {
        damageable.MaxColor = ThemeColors[ThemeItem.MaxDamage];
        damageable.MinColor = ThemeColors[ThemeItem.MinDamage];
    }

    public static void Visit(Player player)
    {
        player.GetComponent<SpriteRenderer>().color = ThemeColors[ThemeItem.Player];
    }

    public static void Visit(Aim aim)
    {
        aim.EndPredictionSprite.GetComponent<SpriteRenderer>().color = ThemeColors[ThemeItem.EndPrediction];
        aim.MidPredictionSprite.GetComponent<SpriteRenderer>().color = ThemeColors[ThemeItem.MidPrediction];
    }

    public static void Visit(Background background)
    {
        background.GetComponent<SpriteRenderer>().color = ThemeColors[ThemeItem.Background];
    }

    public static void Visit(Shootable shootable)
    {
        shootable.GetComponent<SpriteRenderer>().color = ThemeColors[ThemeItem.BasicBall];
    }

    public static void Visit(PlayerHealth playerHealth)
    {
        playerHealth.MaxHealthColor = ThemeColors[ThemeItem.MaxDamage];
        playerHealth.MinHealthColor = ThemeColors[ThemeItem.MinDamage];
    }

    public static void Visit(MainCamera mainCamera)
    {
        mainCamera.GetComponent<Camera>().backgroundColor = ThemeColors[ThemeItem.Camera];
    }

    public static void Visit(ThemeText themeText)
    {
        themeText.SetFont(ThemeFonts[themeType]);
        themeText.SetFontColor(Color.white);
    }

    public static Color GetColor(CustomColor customColor)
    {
        switch (customColor)
        {
            case CustomColor.Green:
                return Color.green;
            case CustomColor.Red:
                return Color.red;
            case CustomColor.Black:
                return Color.black;
            case CustomColor.Grey:
                return new Color(0.1f, 0.1f, 0.2f);
            case CustomColor.DarkGreen:
                return ConvertToColor(0x0B, 0x72, 0x6D);
            case CustomColor.LightGreen:
                return ConvertToColor(0xD5, 0xFC, 0xD4);
            case CustomColor.LightRed:
                return ConvertToColor(0xFF, 0x91, 0xA8);
            case CustomColor.Brown:
                return ConvertToColor(0x4B, 0x4B, 0x4B);
            case CustomColor.LightYellow:
                return ConvertToColor(0xF1, 0xDD, 0x89);
            case CustomColor.Orange:
                return ConvertToColor(0xFF, 0xA5, 0x00);
            default:
                return Color.white;
        }

    }

    private static Color ConvertToColor(int r, int g, int b)
    {
        return new Color(r / 256f, g / 256f, b / 256f);
    }


}
