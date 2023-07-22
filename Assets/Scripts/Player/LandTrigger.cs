using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandTrigger : MonoBehaviour  //The purpose of this class is to attach to a trigger that tells the player what
                                          //ARTIFICIAL friction to apply based on the friction coefficient value of the
                                          //physics material on the ground's rigidbody
{
    public delegate void Landing(Collider2D collision); //set up a delegate for the player landing
    public static event Landing OnLanding;              //utilize the delegate for an event

    private void OnTriggerEnter2D(Collider2D collision) //trigger for whenever the player is on a new object/ground
    {
        if (OnLanding != null) OnLanding(collision);    //activate the event and send over the collision
    }
}
