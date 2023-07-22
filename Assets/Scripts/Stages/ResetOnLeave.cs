using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetOnLeave : MonoBehaviour
{
    [SerializeField]
    private bool willReset;
    public bool willResetOnLeave
    {
        get { return willReset; }
    }
}
