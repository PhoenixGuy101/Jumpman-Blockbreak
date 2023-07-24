using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEnd : MonoBehaviour
{
    //script that sends out event to signal the player has ended the level
    public delegate void LevelFinished();

    public static event LevelFinished OnPlayerReachingEnd;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) OnPlayerReachingEnd();
    }
}
