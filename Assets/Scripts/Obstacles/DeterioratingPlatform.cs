using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeterioratingPlatform : MonoBehaviour, IObstacle, IDeteriorate
{
    //fields
    [SerializeField]
    private int leaveTotal = 3; //how many times the player can land and leave the platform before it is destroyed (editable in the inspector)
    private int leaveLeft;      //a tracker for how many times the player has left the platform
    private int damageStatus = 0; //will be changed to be the sprite selection
    private bool playerOn;      //a variable to confirm the player landed on the platform.
    
    void IObstacle.Reset()
    {
        //This object only resets upon the player dying
    }

    void IDeteriorate.deteriorate()
    {
        LoseState();
    }
    
    // Start is called before the first frame update
    void Start()
    {
        leaveLeft = leaveTotal;
        playerOn = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerOn = true;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && playerOn)
        {
            playerOn = false;
            LoseState();
        }
    }

    private void LoseState()
    {
        leaveLeft--;
        if (leaveTotal / leaveLeft > .5) damageStatus = 1;
        else if (leaveLeft > 0) damageStatus = 2;
        else { 
            damageStatus = 3;
            Destroy(gameObject); //will need to play destroy animation
        }
        
    }
}
