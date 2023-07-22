using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerableParent : MonoBehaviour, IPowerable, IObstacle
{
    protected bool isPowered;   //determines the current powered state
    [SerializeField]
    protected bool startsPowered;   //inverts the powered/unpowered status: if it starts powered, when activated, it turns off essentially

    protected virtual void OnDisable()
    {
        Setup();
    }
    protected virtual void Powered()
    {
        isPowered = true;
    }

    protected virtual void UnPowered()
    {
        isPowered = false;
    }

    protected virtual void Setup()
    {

    }

    void IPowerable.Activate()
    {
        if (!startsPowered) Powered();
        else UnPowered();
    }

    void IPowerable.DeActivate()
    {
        if (startsPowered) Powered();
        else UnPowered();
    }

    void IObstacle.Reset()
    {
        Setup();
    }
}
