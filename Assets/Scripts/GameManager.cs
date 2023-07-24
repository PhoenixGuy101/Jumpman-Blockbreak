using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : Singleton<GameManager>
{
    //fields
    #region Levels&StagesProperties
    private int currLevelIndex;
    private int numLevelsTotal;

    private GameObject[] stageArray; //an array of all the stages in one level
    public GameObject[] stageArrayProp //property to access stageArray
    {
        get { return stageArray; }
    }
    private int currStageIndex; //current stage the player is in
    public int currStage        //property to access stageArray
    {
        get { return currStageIndex; }
    }
    #endregion

    #region PlayerStuff
    private Camera cameraComponent;

    private GameObject playerPrefab;
    private PlayerController playerController;
    private Vector3 playerRespawnPos;   //where the player will respawn if they die
    [Header("Player and Jumping")]
    [SerializeField]
    private float jumpHeight;   //jump height set in the inspector
    [SerializeField]
    private float jumpTime;     //jump time set in the inspector
    private float playTime;
    private int playerDeaths;
    #endregion

    #region UIProperties
    [Header("UI")]
    [SerializeField]
    private GameObject pauseMenu;
    [SerializeField]
    private GameObject winMenu;
    [SerializeField]
    private GameObject nextLvlBtn;
    [SerializeField]
    private TMP_Text timePlayedText;
    [SerializeField]
    private TMP_Text deathNumText;
    #endregion

    #region StartingMethods
    public override void Awake()
    {
        base.Awake();
        Physics2D.gravity = new Vector2(0.0f, (-2 * jumpHeight) / Mathf.Pow(jumpTime, 2));  //calculate gravity based on the jump height and time
    }

    private void OnEnable()
    {
        //listen for a variety of events
        StageEnd.OnStageExit += ChangeStage;
        PlayerController.OnPlayerDeath += PlayerRespawn;
        LevelLoadCallback.AfterStart += ActiveStageReset;
        StageEnd.OnPlayerReachingEnd += WinLevel;
        InitLevel(); //initialize the level
    }

    private void OnDisable()
    {
        //disable all the listeners
        StageEnd.OnStageExit -= ChangeStage;
        PlayerController.OnPlayerDeath -= PlayerRespawn;
        LevelLoadCallback.AfterStart -= ActiveStageReset;
        StageEnd.OnPlayerReachingEnd -= WinLevel;
    }

    private void Start()
    {
        numLevelsTotal = SceneManager.sceneCountInBuildSettings; //get the total amount of levels
    }

    private void Update()
    {
        if(!winMenu.activeInHierarchy) playTime += Time.deltaTime;
    }

    private void LoadLevel(int levelIndexToLoad)
    {
        SceneManager.sceneLoaded += OnLevelLoaded;

        SceneManager.LoadScene(levelIndexToLoad);
    }

    private void OnLevelLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnLevelLoaded;

        InitLevel();
    }
    #endregion

    #region Levels&Stages
    private void InitLevel()
    {
        HidePauseMenu();
        HideWinMenu();
        playTime = 0;
        playerDeaths = 0;

        currLevelIndex = SceneManager.GetActiveScene().buildIndex; //set the level index to the currently loaded one

        if (currLevelIndex > 0) //if not the main menu, ignore the stage setup
        {
            stageArray = null; //reset stageArray
            StageSorter stageSorter = new StageSorter(); //initialize the stage sorter
            stageArray = GameObject.FindGameObjectsWithTag("Stage"); //add all the stages to the array
            Array.Sort(stageArray, stageSorter); //sort the stages in order by name using the stage sorter (stages should be named "Stage00, Stage01, Stage02" and so on
        
            cameraComponent = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();

            playerPrefab = GameObject.FindGameObjectWithTag("Player");
            playerController = playerPrefab.GetComponent<PlayerController>();
            playerController.playerJumpHeight = jumpHeight; //communicate to the player controller the set jump height and time
            playerController.playerJumpTime = jumpTime;

            if (stageArray.Length >= 1) InitStage(-1); //initialize stage 0 (using -1 as a placeholder argument) if there's at least 1 stage present
        }
        
    }
    private void InitStage(int index)
    {
        if (index == -1) //this is done when a level is initialized, and therefore it guarantee sets the stage up in the 0 index of the stageArray
        {
            stageArray[0].TryGetComponent(out StageManager stageManager);       //get the stageManager script attached to the stage GameObject prefab
            cameraComponent.transform.position = stageManager.stageCameraPos;   //the stageManager script gives values for the camera position and size 
            cameraComponent.orthographicSize = stageManager.stageCameraSize;
            currStageIndex = 0;
        }
        else if (index != currStageIndex) //same as above, but it now changes the stage when the current stageArray index isn't identical to the stage GameObject
                                          //index the player is moving to, instead of working with only stageArray index 0
        {
            stageArray[index].TryGetComponent(out StageManager stageManager);
            cameraComponent.transform.position = stageManager.stageCameraPos;
            cameraComponent.orthographicSize = stageManager.stageCameraSize;
            currStageIndex = index;
        }
        playerRespawnPos = playerPrefab.transform.position; //change the player's respawn position to the first location it is in when initializing the stage

        Debug.Log("Current Stage Index: " + currStageIndex);
        
    }
    private void ChangeStage(int[] index, Vector3[] position)   //method that focuses on the exiting and entering of new stages; it is triggered by the OnStageExit
                                                                //event from the StageExitEntrance script attached to the Exit prefab
    {
        playerPrefab.transform.position = index[0] == currStageIndex ? position[1] : position[0]; //test to see where the player needs to be moved to based on
                                                                                                  //the current stage index, and the indices provided by the Exit GameObject:
                                                                                                  //if the player is in stage 1 and crosses the exit btwn 1 and 2, this
                                                                                                  //line will put the player onto the starting position in stage 2
        Debug.Log(index[0] == currStageIndex);
        if (index[0] == currStageIndex) //if the current stage index is the first one listed in the entrance/exit script, continue here, which will set up the other stage
                                        //listed to be entered, while the current/first stage listed will be considered left by the player, thus telling the stage manager
                                        //to act as expected
        {
            stageArray[index[1]].TryGetComponent(out IStage stageEnter);
            if (stageEnter != null) stageEnter.InitializeStage();
            InitStage(index[1]);
            stageArray[index[0]].TryGetComponent(out IStage stageExit);
            if (stageExit != null) stageExit.LeaveStage();
        }
        else //opposite of the above if statement
        {
            stageArray[index[0]].TryGetComponent(out IStage stageEnter);
            if (stageEnter != null) stageEnter.InitializeStage();
            InitStage(index[0]);
            stageArray[index[1]].TryGetComponent(out IStage stageExit);
            if (stageExit != null) stageExit.LeaveStage();
        }
        
    }

    private void ActiveStageReset() //runs at the beginning of a level being loaded, just after the start function has run as it listens to an event
                                    //that tells it to do so. This method makes all stages except for the first one (which the player is in)
                                    //in stageArray be inactive
    {
        if (stageArray.Length > 1)
        {
            for (int i = 1; i < stageArray.Length; i++)
            {
                stageArray[i].TryGetComponent(out IStage stageInterface);
                if (stageInterface != null)
                {
                    stageInterface.LeaveStage();
                }
            }
        }
    }
    #endregion
    private void WinLevel()
    {
        ShowWinMenu();
        Pause();
    }

    private bool HasNextLevel()
    {
        return currLevelIndex + 1 < numLevelsTotal;
    }

    public void ReplayLevel()
    {
        Debug.Log("ReplayLevel");

        LoadLevel(currLevelIndex);

        InitLevel();
    }

    private void LoadNextLevel()
    {
        Debug.Log("LoadingNextLevel");

        if (HasNextLevel())
        {
            LoadLevel(currLevelIndex + 1);
        }
        else LoadLevel(0);
    }

    private void QuitGame()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
        Application.Quit();
    }

    private void PlayerRespawn()
    {
        playerPrefab.transform.position = playerRespawnPos;
        stageArray[currStageIndex].TryGetComponent(out IStage stageInterface);
        if (stageInterface != null)
        {
            stageInterface.PlayerDeath();
        }
        playerDeaths++;
    }

    #region UIStuff

    private void Pause()
    {
        Time.timeScale = 0.0f;
    }

    private void UnPause()
    {
        Time.timeScale = 1.0f;
    }

    public void TogglePauseMenu()
    {
        if (!winMenu.activeInHierarchy) { 
            if (!pauseMenu.activeInHierarchy) ShowPauseMenu();
            else HidePauseMenu();
        }
    }

    private void ShowPauseMenu()
    {
        Pause();
        pauseMenu.SetActive(true);
        playerController.playerInput.SwitchCurrentActionMap("UI");
        Cursor.visible = true;
    }

    private void HidePauseMenu()
    {
        pauseMenu.SetActive(false);
        if (playerController != null) playerController.playerInput.SwitchCurrentActionMap("Player");
        Cursor.visible = false;
        UnPause();
    }

    private void ShowWinMenu()
    {
        winMenu.SetActive(true);
        TMP_Text nextLvlText = nextLvlBtn.GetComponentInChildren<TMP_Text>();
        if (HasNextLevel())
        {
            nextLvlText.text = "Next Level";
        }
        else
        {
            nextLvlText.text = "Main Menu";
        }
        
        timePlayedText.text = "Time: " + GetCurrentTime();
        deathNumText.text = "Deaths: " + playerDeaths;
        //nextLvlBtn.SetActive(HasNextLevel()); //not necessary, as the button is now set to go to the menu if there are no more levels left
        playerController.playerInput.SwitchCurrentActionMap("UI");
        Cursor.visible = true;
    }

    private void HideWinMenu()
    {
        winMenu.SetActive(false);
        if (playerController != null) playerController.playerInput.SwitchCurrentActionMap("Player");
        Cursor.visible = false;
    }

    #region ButtonMethods
    public void OnQuitBtnClick()
    {
        QuitGame();
    }

    public void OnResumeBtnClick()
    {
        HidePauseMenu();
    }

    public void OnReplayBtnClick()
    {
        ReplayLevel();
    }

    public void OnNextLvlBtnClick()
    {
        LoadNextLevel();
    }
    #endregion
    #endregion

    private string GetCurrentTime()
    {
        int minutes = Mathf.FloorToInt(playTime) / 60;
        int seconds = Mathf.FloorToInt(playTime) % 60;
        string minText;
        string secText;
        if (minutes <= 0) minText = "00";
        else if (minutes < 10) minText = "0" + minutes;
        else minText = minutes.ToString();
        if (seconds <= 0) secText = "00";
        else if (seconds < 10) secText = "0" + seconds;
        else secText = seconds.ToString();
        return minText + ":" + secText;
    }
}
