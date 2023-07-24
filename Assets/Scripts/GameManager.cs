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
    private LevelStats levelStats;

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
    private int cycleTotal;
    private int cycleCurr;
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
        StageEnd.OnStageExit += StageExit;
        PlayerController.OnPlayerDeath += PlayerRespawn;
        LevelLoadCallback.AfterStart += SetStagesInactive;
        StageEnd.OnPlayerReachingEnd += ReachedEnd;
        InitLevel(); //initialize the level
    }

    private void OnDisable()
    {
        //disable all the listeners
        StageEnd.OnStageExit -= StageExit;
        PlayerController.OnPlayerDeath -= PlayerRespawn;
        LevelLoadCallback.AfterStart -= SetStagesInactive;
        StageEnd.OnPlayerReachingEnd -= ReachedEnd;
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

        if (currLevelIndex > 0) //if not the main menu, ignore the stage and level setup
        {
            levelStats = GameObject.FindGameObjectWithTag("Level").GetComponent<LevelStats>();
            cycleTotal = levelStats.levelCycleAmount;
            cycleCurr = cycleTotal;
            playerRespawnPos = levelStats.levelPlayerStart;

            stageArray = null; //reset stageArray
            StageSorter stageSorter = new StageSorter(); //initialize the stage sorter
            stageArray = GameObject.FindGameObjectsWithTag("Stage"); //add all the stages to the array
            Array.Sort(stageArray, stageSorter); //sort the stages in order by name using the stage sorter (stages should be named "Stage00, Stage01, Stage02" and so on
        
            cameraComponent = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();

            playerPrefab = GameObject.FindGameObjectWithTag("Player");
            playerController = playerPrefab.GetComponent<PlayerController>();
            playerController.playerJumpHeight = jumpHeight; //communicate to the player controller the set jump height and time
            playerController.playerJumpTime = jumpTime;

            if (stageArray.Length >= 1) InitStage(0); //initialize stage 0 if there's at least 1 stage present
        }
        
    }
    private void InitStage(int index)
    {
        stageArray[index].TryGetComponent(out IStage stageEnter);
        if (stageEnter != null) stageEnter.InitializeStage();
        stageArray[index].TryGetComponent(out StageManager stageManager);
        cameraComponent.transform.position = stageManager.stageCameraPos;
        cameraComponent.orthographicSize = stageManager.stageCameraSize;
        currStageIndex = index;
        //playerRespawnPos = playerPrefab.transform.position; //change the player's respawn position to the first location it is in when initializing the stage

        Debug.Log("Current Stage Index: " + currStageIndex);
        
    }
    private void StageExit(int[] index, Vector3[] position)   //method triggered by the OnStageExit event from the StageExitEntrance script attached to the Exit prefab
                                                              //interprets the info from the event to know how to change the stages.
    {
        Debug.Log(index[0] == currStageIndex);
        if (index[0] == currStageIndex) ChangeStage(index[1], index[0], position[1]);
            
        else ChangeStage(index[0], index[1], position[0]);

    }
    private void ChangeStage(int entering, int exiting, Vector3 playerPos)  //method that is called whenever the active stage changes.
    {
        playerPrefab.transform.position = playerPos;
        InitStage(entering);
        stageArray[exiting].TryGetComponent(out IStage exitStage);
        if (exitStage != null) exitStage.LeaveStage();
    }

    private void SetStagesInactive() //listens to an event from LevelLoadCallback that runs just after the start function has executed
                                    //This method makes all stages except for the first one in stageArray be inactive
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

    private void ReachedEnd()
    {
        cycleCurr--;
        Debug.Log("cycleCurr: " + cycleCurr);
        //update ui
        if (cycleCurr <= 0) WinLevel();
        else ChangeStage(0, currStage, playerRespawnPos);
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
        ChangeStage(0, currStage, playerRespawnPos);
        //playerPrefab.transform.position = playerRespawnPos;
        /*stageArray[currStageIndex].TryGetComponent(out IStage stageInterface);
        if (stageInterface != null)
        {
            stageInterface.PlayerDeath();
        }*/
        cycleCurr = cycleTotal;
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