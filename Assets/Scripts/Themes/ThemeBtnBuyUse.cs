using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ThemeBtnBuyUse : MonoBehaviour
{
    public bool _unlocked = false;
    public TextMeshProUGUI _text;

    public ThemeType _themeType = ThemeType.Default;
    
    public Animator animator;

    private void Awake()
    {
        if (_unlocked)
        {
            _text.text = "Use";
        }
        else
        {
            _text.text = "Buy\n$4.99";
        }

        animator = GameObject.FindObjectOfType<ThemeUI>().animator;
    }



    public void BuyUse()
    {
        StartCoroutine(BuyUseRoutine());
    }

    public IEnumerator BuyUseRoutine()
    {
        if (_unlocked)
        {
            animator.GetComponent<Image>().color = Color.black; // set to black to avoid color clash on return to different theme
            animator.SetTrigger("Close");

            yield return new WaitForSeconds(MainMenuUI.SCENE_TRANSITION_WAIT_TIME);

            ES3.Save(BGStrings.ES_THEMETYPE, _themeType);
            ES3.Save(BGStrings.ES_THEMETYPEB4PREVIEW, _themeType);

            PlayerPrefs.SetInt("hasReturnedFromThemeSwap", 1); // set to true
            SceneManager.LoadScene("ThemeSelect");
        }
        else
        {

        }
    }
}
