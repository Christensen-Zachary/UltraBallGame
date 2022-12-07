using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class ThemeVisitor : MonoBehaviour
{
    [field: SerializeField]
    private ThemeData ThemeData { get; set; }

    public static void Visit(ThemeButtonImage themeButtonImage)
    {
        themeButtonImage.GetComponent<UnityEngine.UI.Image>().color = ThemeData.ThemeButtonImageColor;
    }

    public static void Visit(ThemeFontColor themeFontColor)
    {
        themeFontColor.TextMesh.color = ThemeData.ThemeFontColor;
    }

    public static void Visit(InvincibleBrick invincibleBrick)
    {
        invincibleBrick.GetComponent<SpriteRenderer>().color = ThemeData.ThemeColors[ThemeItem.InvincibleBrick];
    }
    public static void Visit(BrickNumber brickNumber)
    {
        brickNumber.TextMesh.color = ThemeData.BrickTextColor;
    }

    public static void Visit(Damageable damageable)
    {
        damageable.MaxColor = ThemeData.ThemeColors[ThemeItem.MaxDamage];
        damageable.MinColor = ThemeData.ThemeColors[ThemeItem.MinDamage];
    }

    public static void Visit(Player player)
    {

        switch (ThemeData.themeType)
        {
            case ThemeType.Theme3:
                player.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Player/JackOLantern");
                break;
            default:
                player.GetComponent<SpriteRenderer>().color = ThemeData.ThemeColors[ThemeItem.Player];
                break;
        }
    }

    public static void Visit(Aim aim)
    {
        aim.EndPredictionSprite.GetComponent<SpriteRenderer>().color = ThemeData.ThemeColors[ThemeItem.EndPrediction];
        aim.MidPredictionSprite.GetComponent<SpriteRenderer>().color = ThemeData.ThemeColors[ThemeItem.MidPrediction];
    }


    public static void Visit(Shootable shootable)
    {
        shootable.GetComponent<SpriteRenderer>().color = ThemeData.ThemeColors[ThemeItem.BasicBall];

        GameObject psReturn = Resources.Load<GameObject>($"ParticleSystems/Balls/psReturn{(int)ThemeData.themeType}");
        if (psReturn == null)
        {
            psReturn = Resources.Load<GameObject>($"ParticleSystems/Balls/psReturn0");
        }
        psReturn = Instantiate(psReturn);
        psReturn.transform.SetParent(shootable.transform);
        psReturn.transform.localScale = Vector3.one;
        psReturn.transform.localPosition = new Vector3(0, 0, 1);
        shootable.PSReturn = psReturn.GetComponent<ParticleSystem>();
        psReturn.SetActive(true);
    }

    public static void Visit(PlayerHealth playerHealth)
    {
        playerHealth.MaxHealthColor = ThemeData.ThemeColors[ThemeItem.PlayerMaxHealth];
        playerHealth.MinHealthColor = ThemeData.ThemeColors[ThemeItem.PlayerMinHealth];
    }

    public static void Visit(Background background)
    {
        background.GetComponent<SpriteRenderer>().color = ThemeData.ThemeColors[ThemeItem.Background];
    }
    public static void Visit(SuperBackground superBackground)
    {
        switch (ThemeData.themeType)
        {
            case ThemeType.JellyFish:
                SetSpriteSuperBackground("Sprites/Background/bg4", superBackground.gameObject);
                break;
            case ThemeType.VaporWave:
                SetSpriteSuperBackground("Sprites/Background/bg1", superBackground.gameObject);
                break;
            default:
                superBackground.GetComponent<SpriteRenderer>().color = ThemeData.ThemeColors[ThemeItem.SuperBackground];
                break;
        }

    }

    private static void SetSpriteSuperBackground(string spritePath, GameObject gameObject)
    {
        Sprite sprite = SetSprite(spritePath, gameObject);

        (float height, float width) = BGUtils.GetScreenSize();
        float spriteRatio = sprite.rect.height / sprite.rect.width;
        float screenRatio = height / width;

        if (spriteRatio > screenRatio) // sprite tall and narrow compared to screen. Fit to width
        {
            print("Fitting sprite to width");
            gameObject.transform.localScale = spriteRatio * 0.95f * width * Vector2.one;
        }
        else // sprite short and wide compared to screen. Fit to height
        {
            print("Fitting sprite to height");
            gameObject.transform.localScale = height * 0.95f * Vector2.one;
        }
    }

    private static Sprite SetSprite(string spritePath, GameObject gameObject)
    {
        Sprite sprite = Resources.Load<Sprite>(spritePath);
        gameObject.GetComponent<SpriteRenderer>().sprite = sprite;
        return sprite;
    }

    public static void Visit(ThemeText themeText)
    {
        if (ThemeData.ThemeFonts.ContainsKey(ThemeData.themeType)) themeText.SetFont(ThemeData.ThemeFonts[ThemeData.themeType]);
        else themeText.SetFont(ThemeData.ThemeFonts[ThemeType.Default]);
    }

    public static void Visit(ThemeGameBorder themeGameBorder)
    {
        switch (ThemeData.themeType)
        {
            case ThemeType.Default:
                themeGameBorder.GetComponent<SpriteRenderer>().color = ThemeData.ThemeColors[ThemeItem.GameBorder];
                themeGameBorder.GetComponent<SpriteRenderer>().sortingOrder = -101;
                break;
            case ThemeType.JellyFish:
                themeGameBorder.GetComponent<SpriteRenderer>().color = ThemeData.ThemeColors[ThemeItem.GameBorder];
                themeGameBorder.GetComponent<SpriteRenderer>().sortingOrder = -101;
                break;
            case ThemeType.VaporWave:
                themeGameBorder.GetComponent<SpriteRenderer>().color = ThemeData.ThemeColors[ThemeItem.GameBorder];
                themeGameBorder.GetComponent<SpriteRenderer>().sortingOrder = -101;
                break;
            case ThemeType.Theme2:
                themeGameBorder.GetComponent<SpriteRenderer>().color = ThemeData.ThemeColors[ThemeItem.GameBorder];
                themeGameBorder.GetComponent<SpriteRenderer>().sortingOrder = -101;
                break;
            case ThemeType.Theme3:
                themeGameBorder.GetComponent<SpriteRenderer>().color = ThemeData.ThemeColors[ThemeItem.GameBorder];
                themeGameBorder.GetComponent<SpriteRenderer>().sortingOrder = -101;
                break;
            case ThemeType.Retro:
                themeGameBorder.GetComponent<SpriteRenderer>().color = ThemeData.ThemeColors[ThemeItem.GameBorder];
                themeGameBorder.GetComponent<SpriteRenderer>().sortingOrder = -101;
                break;
            default:
                break;
        }
    }

}
