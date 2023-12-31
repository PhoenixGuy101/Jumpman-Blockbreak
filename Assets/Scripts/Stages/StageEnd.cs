using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageEnd : MonoBehaviour
{
    //this script is attached to the exits of stages: sends out an event to the GameManager that alerts it that the player is switching stages
    public delegate void StageExit(int[] index, Vector3[] playerSpawn);
    public static event StageExit OnStageExit;

    public delegate void LevelFinished();
    public static event LevelFinished OnPlayerReachingEnd;

    [SerializeField]
    private bool isLevelEnd;

    [SerializeField]
    private int[] stageIndex;   //array of the stages the exit connects to

    [SerializeField]
    private Vector3[] playerPositions;  //array of the positions that the player should be when they are switching to one of the stages listed in the stageIndex array

    private Rigidbody2D rb;
    private float revSpeed = 10;

    private void Start()
    {
        if (isLevelEnd)
        {
            rb = gameObject.GetComponent<Rigidbody2D>();
        }
    }

    private void FixedUpdate()
    {
        if (isLevelEnd)
        {
            rb.MoveRotation(rb.rotation + revSpeed * Time.fixedDeltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (isLevelEnd) OnPlayerReachingEnd();
            else OnStageExit(stageIndex, playerPositions);
        }
    }
}
