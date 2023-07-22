using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivatorParent : MonoBehaviour, IActivator, IObstacle
{
    //fields
    [SerializeField]
    protected GameObject[] controlledPowerables; //An array of all the game objects the activator will power

    protected virtual void ActivatePowerables()
    {
        foreach (GameObject i in controlledPowerables) //go through each game object in the controlledPowerables array and turn them on
        {
            i.TryGetComponent(out IPowerable powerable);
            if (powerable != null)
            {
                powerable.Activate();
            }
        }
    }

    protected virtual void DeActivatePowerables()
    {
        foreach (GameObject i in controlledPowerables)  //go through each game object in the controlledPowerables array and turn them off
        {
            i.TryGetComponent(out IPowerable powerable);
            if (powerable != null)
            {
                powerable.DeActivate();
            }
        }
    }

    protected virtual void Setup()
    {

    }

    protected virtual void ResetActivator()
    {

    }

    void IActivator.AcceptPower()//the interface method that enables the activator and its powerables to turn on
    {
        ActivatePowerables();
    }

    void IActivator.LosePower() //the interface method that enables the activator and its powerables to turn off
    {
        DeActivatePowerables();
    }

    void IObstacle.Reset()
    {
        ResetActivator();
    }

}
