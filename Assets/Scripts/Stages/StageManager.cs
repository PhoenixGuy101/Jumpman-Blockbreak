using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour, IStage
{
    //this script is part of the Stage prefab, and it contains the properties and methods needed to keep track of stages, and shift between them
    [SerializeField]
    private int index; //not really used
    public int stageIndex
    {
        get { return index; }
    }
    
    [SerializeField]
    private Vector3 cameraPos; //the camera's position when the stage is switched to
    public Vector3 stageCameraPos
    {
        get { return cameraPos; }
    }
    [SerializeField]
    private float cameraSize; //the camera's size when the stage is switched to
    public float stageCameraSize
    {
        get { return cameraSize; }
    }

    private GameObject obstacleChild;   //the obstacle child object, which serves as a folder for all obstacle objects in the stage

    void IStage.InitializeStage() //when the player enters: set the obstacles to active
    {
        if (!obstacleChild.activeSelf)
        {
            obstacleChild.SetActive(true);
        }
    }

    void IStage.LeaveStage() //when the player leaves, diable the obstacles and reset some
    {
        foreach (Transform child in obstacleChild.transform)
        {
            child.gameObject.TryGetComponent(out ResetOnLeave leave); //reset the obstacle only if its ResetOnLeave script has willResetOnLeave true
            if (leave != null && leave.willResetOnLeave)
            {
                child.gameObject.TryGetComponent(out IObstacle obs);
                if (obs != null)
                {
                    obs.Reset();
                }
            }
        }


        if (obstacleChild.activeSelf)   //disable the obstacles
        {
            obstacleChild.SetActive(false);
        }
    }
    
    void IStage.PlayerDeath() //run when the player dies, resets the obstacles in the stage
    {
        foreach(Transform child in obstacleChild.transform)
        {
            child.gameObject.TryGetComponent(out IObstacle obs);
            if (obs != null)
            {
                obs.Reset();
            }
        }
    }

    private void OnEnable() //get a reference to the obstacle child object
    {
        obstacleChild = gameObject.transform.GetChild(2).gameObject;
    }
}
