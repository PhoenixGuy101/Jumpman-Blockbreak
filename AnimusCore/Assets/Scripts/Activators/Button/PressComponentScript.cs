using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressComponentScript : MonoBehaviour
{
    private IActivator activator;
    private int objectsInTrigger;   //keeps track of the number of all objects currently holding down the button
    private bool isPressed;
    private GameObject childObj;
    private Rigidbody2D childRb;
    private Vector3 velocity;

    private void OnEnable()
    {
        objectsInTrigger = 0;
    }

    private void Start()
    {
        GameObject btnParent = transform.parent.gameObject; //set a reference to the press component's parent object
        activator = btnParent.GetComponent<IActivator>();   //get a reference to the Button script's IActivator interface
        objectsInTrigger = 0;
        isPressed = false;                                  //start off not pressed
        childObj = transform.GetChild(0).gameObject;        //get a reference to the child object which will function as the moving pressure plate
        childRb = childObj.GetComponent<Rigidbody2D>();     //get a reference to the child's rigidbody so movement is possible
        velocity = new Vector3(0, 6f, 0);                      //set a velocity for the pressure plate via a Vector 2
    }

    private void FixedUpdate()
    {
        if (isPressed && childObj.transform.localPosition.y > -1) //moves the pressure plate down if the button is pressed and hasn't reached 1 unit down
        {
            //childRb.MovePosition(childRb.position - velocity * Time.fixedDeltaTime);
            childObj.transform.localPosition -= velocity * Time.fixedDeltaTime;

        }
        else if (!isPressed && childObj.transform.localPosition.y < 0)  //moves the pressure plate up if the button isn't pressed and hasn't reached its starting position
        {
            //childRb.MovePosition(childRb.position + velocity * Time.fixedDeltaTime);
            childObj.transform.localPosition += velocity * Time.fixedDeltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("Block")) //accepts the player or blocks as objects that can keep the button pressed
        {
            if (objectsInTrigger == 0 && activator != null)
            {
                buttonPress(); //causes the button press if this is the first object on the button and there is a IActivator interface in the parent object
            }
            objectsInTrigger++; //adds to the count of objects on the button
        }
        
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if ((other.CompareTag("Player") || other.CompareTag("Block")) && isActiveAndEnabled)
        {
            objectsInTrigger--; //remove from the count of objects on the button
            if (objectsInTrigger == 0 && activator != null)
            {
                buttonRelease();    //release once there's no objects on the button's trigger
            }
        }
    }

    private void buttonPress()
    {
        activator.AcceptPower();//tell the parent object to activate
        isPressed = true;       //the button is now pressed
    }

    private void buttonRelease()
    {
        activator.LosePower();  //tell the parent object to de-activate
        isPressed = false;      //the button is no longer pressed
    }
}
