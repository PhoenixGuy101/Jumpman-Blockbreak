using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelLoadCallback : MonoBehaviour
{
    //This script exists to trigger an event as soon as start is done. Upon doing so, GameManager is able to disable inactive stages.
    public static event Action AfterStart;
    IEnumerator Start()
    {
        yield return null;
        if (AfterStart != null)
            AfterStart();
    }
}
