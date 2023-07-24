using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelStats : MonoBehaviour
{
    [SerializeField]
    private int playNum;
    public int levelCycleAmount
    {
        get { return playNum; }
    }
    [SerializeField]
    private Vector3 playerStart;
    public Vector3 levelPlayerStart
    {
        get { return playerStart; }
    }
}