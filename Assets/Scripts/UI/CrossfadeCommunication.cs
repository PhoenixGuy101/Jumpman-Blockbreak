using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossfadeCommunication : MonoBehaviour
{
    private void OnEnable()
    {
        setCanPause(true);
    }
    private void OnDisable()
    {
        setCanPause(false);
    }
    public void setCanPause(bool state)
    {
        GameManager.Instance.canPauseProp = state;
    }
}
