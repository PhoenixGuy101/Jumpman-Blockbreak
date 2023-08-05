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

    private void Start()
    {
        GameManager manager = GameManager.FindObjectOfType<GameManager>();
        if (manager != null)
        Destroy(GameManager.Instance.gameObject);   //get rid of the gamemanager to prevent conflicting canvases
        Cursor.visible = true;
        TitleScreen.SetActive(true);
        CreditsScreen.SetActive(false);
    }

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
    }

    private void ReturnToMenu()
    {
        TitleScreen.SetActive(true);
        CreditsScreen.SetActive(false);
    }

    private void OpenBandcampUrl()
    {
        Application.OpenURL(Url);
    }

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

    public void OnMainMenuClick()
    {
        ReturnToMenu();
    }

    public void OnBandcampClick()
    {
        OpenBandcampUrl();
    }
}
