using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ThemePreviewUI : MonoBehaviour
{
    private ThemeType _b4Theme = ThemeType.Default;

    public Animator animator;

    private void Awake()
    {
        _b4Theme = ES3.Load<ThemeType>(BGStrings.ES_THEMETYPEB4PREVIEW);
    }

    private IEnumerator Start() 
    {
        yield return null;
        ES3.Save(BGStrings.ES_THEMETYPE, _b4Theme);    
    }



    public void OpenThemeSelect()
    {
        StartCoroutine(OpenThemeSelectRoutine());
    }

    public IEnumerator OpenThemeSelectRoutine()
    {
        animator.GetComponent<Image>().color = Color.black; // set to black to avoid color clash on return to different theme
        animator.SetTrigger("Close");

        yield return new WaitForSeconds(MainMenuUI.SCENE_TRANSITION_WAIT_TIME);

        ES3.Save(BGStrings.ES_THEMETYPE, _b4Theme);

        PlayerPrefs.SetInt("hasReturnedFromThemeSwap", 1); // set to true
        SceneManager.LoadScene("ThemeSelect");
    }
}
