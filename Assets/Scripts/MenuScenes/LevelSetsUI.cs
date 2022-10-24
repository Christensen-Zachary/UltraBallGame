using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSetsUI : MonoBehaviour
{
    [field: SerializeField]
    public GameObject LevelSetsButtonPrefab { get; set; }
    [field: SerializeField]
    public GameObject LevelSetsMenu{ get; set; }

    public Animator animator;

    private void Awake()
    {
        for (int i = 0; i < 6; i++)
        {
            // create button to access menu of levels
            GameObject obj = Instantiate(LevelSetsButtonPrefab);
            obj.SetActive(transform);
            obj.GetComponent<BtnLoadLevel>().SetText((i * 50 + 1).ToString() + " - " + ((i + 1) * 50).ToString());
            obj.transform.SetParent(LevelSetsMenu.transform);
            obj.transform.localScale = Vector3.one;
            
            System.Action<int> setListener = (capturedi) => { obj.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => OpenLevelSets(capturedi)); };
            setListener(i);
        }
    }



    public void OpenMainMenu()
    {
        StartCoroutine(OpenMainMenuCoroutine());
    }

    private void OpenLevelSets(int setNumber)
    {
        StartCoroutine(OpenLevelSetsCoroutine(setNumber));   
    }

    private IEnumerator OpenLevelSetsCoroutine(int setNumber)
    {
        animator.SetTrigger("Close");

        yield return new WaitForSeconds(1f);

        ES3.Save(BGStrings.ES_LEVELSETNUMBER, setNumber);
        LevelSelectUI.LoadLevelSelect();
    }

    private IEnumerator OpenMainMenuCoroutine()
    {
        animator.SetTrigger("Close");

        yield return new WaitForSeconds(1f);

        MainMenuUI.LoadMainMenu();
    }

    public static void LoadLevelSets()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("LevelSets");
    }

}
