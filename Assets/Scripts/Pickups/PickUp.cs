using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    //fields
    [SerializeField]
    private PickUpType type;
    public PickUpType pickType
    {
        get { return type; }
    }
    [SerializeField]
    private float effectDuration = 5;
    public float pickUpEffectDuration
    {
        get { return effectDuration; }
    }
}
