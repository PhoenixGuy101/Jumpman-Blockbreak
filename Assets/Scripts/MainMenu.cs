using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private void Start()
    {
        GameManager manager = GameManager.FindObjectOfType<GameManager>();
        if (manager != null)
        Destroy(GameManager.Instance.gameObject);   //get rid of the gamemanager to prevent conflicting canvases
        Cursor.visible = true;
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

    public void OnPlayGameClick()
    {
        PlayGame();
    }

    public void OnQuitGameClick()
    {
        QuitGame();
    }
}
