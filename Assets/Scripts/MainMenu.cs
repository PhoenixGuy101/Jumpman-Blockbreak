using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    //fields
    [SerializeField]
    private GameObject TitleScreen;
    [SerializeField]
    private GameObject CreditsScreen;
    private string Url = "http://desdenova.bandcamp.com/";
    [SerializeField]
    private GameObject LevelSelectScreen;

    private void Start()
    {
        GameManager manager = GameManager.FindObjectOfType<GameManager>();
        if (manager != null)
        Destroy(GameManager.Instance.gameObject);   //get rid of the gamemanager to prevent conflicting canvases
        Cursor.visible = true;
        TitleScreen.SetActive(true);
        CreditsScreen.SetActive(false);
        LevelSelectScreen.SetActive(false);
    }
    #region MainMenu
    private void PlayGame()
    {
        SceneManager.LoadScene(1);
    }

    private void QuitGame()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
        Application.Quit();
    }
    private void ViewCredits()
    {
        CreditsScreen.SetActive(true);
        TitleScreen.SetActive(false);
        LevelSelectScreen.SetActive(false);
    }
    private void ViewLevels()
    {
        LevelSelectScreen.SetActive(true);
        CreditsScreen.SetActive(false);
        TitleScreen.SetActive(false);
    }

    #region BtnClicks
    public void OnPlayGameClick()
    {
        PlayGame();
    }

    public void OnQuitGameClick()
    {
        QuitGame();
    }

    public void OnCreditsBtnClick()
    {
        ViewCredits();
    }
    public void OnLevelSelectBtnClick()
    {
        ViewLevels();
    }
    #endregion
    #endregion

    #region CreditsMenu
    private void ReturnToMenu()
    {
        TitleScreen.SetActive(true);
        CreditsScreen.SetActive(false);
        LevelSelectScreen.SetActive(false);
    }

    private void OpenBandcampUrl()
    {
        Application.OpenURL(Url);
    }


    #region BtnClicks
    public void OnMainMenuClick()
    {
        ReturnToMenu();
    }

    public void OnBandcampClick()
    {
        OpenBandcampUrl();
    }
    #endregion
    #endregion

    #region LevelSelect
    #region PlayLevel
    private void PlayLevel(int index)
    {
        SceneManager.LoadScene(index);
    }
    #endregion
    #region BtnClick
    public void OnLvl2Click()
    {
        PlayLevel(2);
    }
    public void OnLvl3Click()
    {
        PlayLevel(3);
    }
    public void OnLvl4Click()
    {
        PlayLevel(4);
    }
    public void OnLvl5Click()
    {
        PlayLevel(5);
    }
    public void OnLvl6Click()
    {
        PlayLevel(6);
    }
    public void OnLvl7Click()
    {
        PlayLevel(7);
    }
    public void OnLvl8Click()
    {
        PlayLevel(8);
    }
    #endregion
    #endregion
}
