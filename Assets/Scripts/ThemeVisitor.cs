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
    Theme1
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

    public static Dictionary<ThemeColor, CustomColor> ThemeColors { get; private set; } = new Dictionary<ThemeColor, CustomColor>() {
        // default colors, are overwritten in GetThemeColors
        { ThemeColor.Player, CustomColor.Orange },
        { ThemeColor.Background, CustomColor.Grey },
        { ThemeColor.MaxDamage, CustomColor.DarkGreen },
        { ThemeColor.MinDamage, CustomColor.LightGreen },
        { ThemeColor.PlayerMaxHealth, CustomColor.Green },
        { ThemeColor.PlayerMinHealth, CustomColor.Red },
        { ThemeColor.MidPrediction, CustomColor.LightYellow },
        { ThemeColor.EndPrediction, CustomColor.LightYellow },
        { ThemeColor.BasicBall, CustomColor.Orange },
        { ThemeColor.Button, CustomColor.Brown }
    };

    public static void GetThemeColors(ThemeType themeType)
    {
        switch (themeType)
        {
            case ThemeType.Theme1:
                ThemeColors[ThemeColor.Player] = CustomColor.DarkGreen;
                ThemeColors[ThemeColor.MaxDamage] = CustomColor.LightRed;
                ThemeColors[ThemeColor.MinDamage] = CustomColor.Brown;
                ThemeColors[ThemeColor.MidPrediction] = CustomColor.LightGreen;
                ThemeColors[ThemeColor.EndPrediction] = CustomColor.LightGreen;
                ThemeColors[ThemeColor.Button] = CustomColor.DarkGreen;
                break;
        }
    }

    public static void Visit(Damageable damageable)
    {
        damageable.MaxColor = GetColor(ThemeColors[ThemeColor.MaxDamage]);
        damageable.MinColor = GetColor(ThemeColors[ThemeColor.MinDamage]);
    }

    public static void Visit(Player player)
    {
        player.GetComponent<SpriteRenderer>().color = GetColor(ThemeColors[ThemeColor.Player]);
    }

    public static void Visit(Aim aim)
    {
        aim.EndPredictionSprite.GetComponent<SpriteRenderer>().color = GetColor(ThemeColors[ThemeColor.EndPrediction]);
        aim.MidPredictionSprite.GetComponent<SpriteRenderer>().color = GetColor(ThemeColors[ThemeColor.MidPrediction]);
    }

    public static void Visit(Background background)
    {
        background.GetComponent<SpriteRenderer>().color = GetColor(ThemeColors[ThemeColor.Background]);
    }

    public static void Visit(Shootable shootable)
    {
        shootable.GetComponent<SpriteRenderer>().color = GetColor(ThemeColors[ThemeColor.BasicBall]);
    }

    public static void Visit(PlayerHealth playerHealth)
    {
        playerHealth.MaxHealthColor = GetColor(ThemeColors[ThemeColor.MaxDamage]);
        playerHealth.MinHealthColor = GetColor(ThemeColors[ThemeColor.MinDamage]);
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
                return new Color(
                    Convert.ToInt32("0B", 16) / 256f,
                    Convert.ToInt32("72", 16) / 256f,
                    Convert.ToInt32("6D", 16) / 256f);
            case CustomColor.LightGreen:
                return new Color(
                    Convert.ToInt32("D5", 16) / 256f,
                    Convert.ToInt32("FC", 16) / 256f,
                    Convert.ToInt32("B4", 16) / 256f);
            case CustomColor.LightRed:
                return new Color(
                    Convert.ToInt32("FF", 16) / 256f,
                    Convert.ToInt32("91", 16) / 256f,
                    Convert.ToInt32("A8", 16) / 256f);
            case CustomColor.Brown:
                return new Color(
                    Convert.ToInt32("4B", 16) / 256f,
                    Convert.ToInt32("4B", 16) / 256f,
                    Convert.ToInt32("4B", 16) / 256f);
            case CustomColor.LightYellow:
                return new Color(
                    Convert.ToInt32("F1", 16) / 256f,
                    Convert.ToInt32("DD", 16) / 256f,
                    Convert.ToInt32("89", 16) / 256f);
            case CustomColor.Orange:
                return new Color(
                    Convert.ToInt32("FF", 16) / 256f,
                    Convert.ToInt32("A5", 16) / 256f,
                    Convert.ToInt32("00", 16) / 256f);
            default:
                return Color.white;
        }

    }




}
