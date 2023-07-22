using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : ActivatorParent, IActivator
{
    protected virtual void Start()
    {
        Setup();
    }
    
    protected override void ActivatePowerables()
    {
        base.ActivatePowerables();
        Debug.Log("On");
    }
    protected override void DeActivatePowerables()
    {
        base.DeActivatePowerables();
        Debug.Log("Off");
    }

    protected override void Setup()
    {
        TryGetComponent(out IActivator iAct);
        iAct.LosePower();//since this object starts off unless something presses its pressure plate trigger, it starts off not giving any power
        //The juice of the operations happen in the PressComponentScript which is attached to the pressure plate child object of the button
    }
}
