using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ThemeColor
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
    private void Awake()
    {
        ES3.Save<ThemeType>(BGStrings.ES_THEMETYPE, ThemeType.Default);
        GetThemeColors(ES3.Load<ThemeType>(BGStrings.ES_THEMETYPE, ThemeType.Default));
    }

    public static Dictionary<ThemeColor, Color> ThemeColors { get; private set; } = new Dictionary<ThemeColor, Color>() {
        // default colors, are overwritten in GetThemeColors
        { ThemeColor.Player, GetColor(CustomColor.Orange) },
        { ThemeColor.Background, GetColor(CustomColor.Grey) },
        { ThemeColor.MaxDamage, GetColor(CustomColor.DarkGreen) },
        { ThemeColor.MinDamage, GetColor(CustomColor.LightGreen) },
        { ThemeColor.PlayerMaxHealth, GetColor(CustomColor.Green) },
        { ThemeColor.PlayerMinHealth, GetColor(CustomColor.Red) },
        { ThemeColor.MidPrediction, GetColor(CustomColor.LightYellow) },
        { ThemeColor.EndPrediction, GetColor(CustomColor.LightYellow) },
        { ThemeColor.BasicBall, GetColor(CustomColor.Orange) },
        { ThemeColor.Button, GetColor(CustomColor.Brown) }
    };

    public static void GetThemeColors(ThemeType themeType)
    {
        switch (themeType)
        {
            case ThemeType.Theme1:
                SetThemeColor(ThemeColor.Player, GetColor(CustomColor.DarkGreen));
                SetThemeColor(ThemeColor.MaxDamage, GetColor(CustomColor.LightRed));
                SetThemeColor(ThemeColor.MinDamage, GetColor(CustomColor.Brown));
                SetThemeColor(ThemeColor.MidPrediction, GetColor(CustomColor.LightGreen));
                SetThemeColor(ThemeColor.EndPrediction, GetColor(CustomColor.LightGreen));
                SetThemeColor(ThemeColor.Button, GetColor(CustomColor.DarkGreen));
                break;
            case ThemeType.JellyFish:
                SetThemeColor(ThemeColor.Player, GetColor(CustomColor.Orange));
                SetThemeColor(ThemeColor.BasicBall, GetColor(CustomColor.Orange));
                SetThemeColor(ThemeColor.Background, GetColor(CustomColor.Grey));
                SetThemeColor(ThemeColor.MaxDamage, GetColor(CustomColor.DarkGreen));
                SetThemeColor(ThemeColor.MinDamage, GetColor(CustomColor.LightGreen));
                SetThemeColor(ThemeColor.PlayerMaxHealth, GetColor(CustomColor.Green));
                SetThemeColor(ThemeColor.PlayerMinHealth, GetColor(CustomColor.Red));
                SetThemeColor(ThemeColor.MidPrediction, GetColor(CustomColor.LightYellow));
                SetThemeColor(ThemeColor.EndPrediction, GetColor(CustomColor.LightYellow));
                SetThemeColor(ThemeColor.Button, GetColor(CustomColor.Brown));
                break;
        }
    }

    private static void SetThemeColor(ThemeColor themeColor, Color color)
    {
        ThemeColors[themeColor] = color;
    }

    public static void Visit(Damageable damageable)
    {
        damageable.MaxColor = ThemeColors[ThemeColor.MaxDamage];
        damageable.MinColor = ThemeColors[ThemeColor.MinDamage];
    }

    public static void Visit(Player player)
    {
        player.GetComponent<SpriteRenderer>().color = ThemeColors[ThemeColor.Player];
    }

    public static void Visit(Aim aim)
    {
        aim.EndPredictionSprite.GetComponent<SpriteRenderer>().color = ThemeColors[ThemeColor.EndPrediction];
        aim.MidPredictionSprite.GetComponent<SpriteRenderer>().color = ThemeColors[ThemeColor.MidPrediction];
    }

    public static void Visit(Background background)
    {
        background.GetComponent<SpriteRenderer>().color = ThemeColors[ThemeColor.Background];
    }

    public static void Visit(Shootable shootable)
    {
        shootable.GetComponent<SpriteRenderer>().color = ThemeColors[ThemeColor.BasicBall];
    }

    public static void Visit(PlayerHealth playerHealth)
    {
        playerHealth.MaxHealthColor = ThemeColors[ThemeColor.MaxDamage];
        playerHealth.MinHealthColor = ThemeColors[ThemeColor.MinDamage];
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
