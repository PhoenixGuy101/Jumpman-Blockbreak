using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : PowerableParent, IObstacle
{
    //fields
    Rigidbody2D rb;
    [SerializeField]
    protected Vector3[] pathPositions;  //enable a collection of locations for the platform to reach, set in the Inspector
    [SerializeField]
    protected float speed = 5;  //Set movement speed for the platform
    private float velocity;     //rough velocity based on the speed and direction
    public float platformVelocity   //property accessed by the player to get add the platform's velocity to their movement
    {
        get { return velocity; }
    }
    protected int currMoveGoal; //integer that tracks the platform's movement progress
    protected int prevMoveGoal;
    protected float platTravelDist;
    protected float travelTime;
    protected float t;          //the time the platform has spent moving from one position to the next
    protected List<IPlayer> playersOnPlat;

    void IObstacle.Reset()
    {
        Setup();
    }

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playersOnPlat = new List<IPlayer>();
        Setup();
    }

    protected override void Setup()
    {
        prevMoveGoal = 0;
        currMoveGoal = 1;                      //set the move goal to the second index in the pathPositions array
        transform.position = pathPositions[prevMoveGoal]; //ensure the platform starts at the beginning state
        t = 0.0f;
        SetVelocity();  //set the velocity of the platform
    }

    protected virtual void FixedUpdate()
    {
        if (isPowered)
        {
            if (pathPositions.Length > 1)
            {
                MovePlatform(); //call to move the platform
                if (transform.position == pathPositions[currMoveGoal]) //occurs once the platform has reached the end goal
                {
                    SetNextMoveGoal();  //change to the next movement goal
                    
                }
            }
        }
    }

    protected virtual void MovePlatform()
    {
        //use a MoveTowards method within the rigidbody MovePosition method to essentially tween the platform from one position to the next
        rb.MovePosition(Vector3.MoveTowards(pathPositions[prevMoveGoal], pathPositions[currMoveGoal], t));
        t += Time.fixedDeltaTime * speed;
    }

    protected virtual void SetNextMoveGoal()
    {
        t = 0;
        prevMoveGoal = currMoveGoal;
        currMoveGoal++;
        if (currMoveGoal >= pathPositions.Length) currMoveGoal = 0; //if the current movement goal is bigger than the index length in the path positions, set the goal back to the start.
        SetVelocity();  //update the velocity
    }
    
    protected virtual void SetVelocity()
    {
        //based on the x axis distance between movement goals and positions, update and change the velocity
        if (!isPowered) velocity = 0;
        else
        {
            platTravelDist = Mathf.Sqrt(Mathf.Pow(pathPositions[currMoveGoal].x - pathPositions[prevMoveGoal].x, 2) + Mathf.Pow(pathPositions[currMoveGoal].y - pathPositions[prevMoveGoal].y, 2));
            travelTime = platTravelDist / speed;
            velocity = (pathPositions[currMoveGoal].x - pathPositions[prevMoveGoal].x) / travelTime;
            Debug.Log(velocity);
            foreach (IPlayer player in playersOnPlat)
            {
                player.UpdateFriction(platformVelocity);
            }
        }
    }

    protected override void Powered()
    {
        base.Powered();
        SetVelocity();
    }

    protected override void UnPowered()
    {
        base.UnPowered();
        SetVelocity();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //when the player lands on the platform/enters the trigger collision ontop the platform, change the player's physics material's friction coefficient via IFriction interface
        if (collision.CompareTag("Player"))
        {
            collision.TryGetComponent(out IPlayer target);
            
            if (target != null)
            {
                playersOnPlat.Add(target);
                target.OnFriction(gameObject);
                target.UpdateFriction(platformVelocity);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //when the player leaves the platform/the trigger collision ontop the platform, revert the player's physics material's friction coefficient via IFriction interface
        if (collision.CompareTag("Player"))
        {
            collision.TryGetComponent(out IPlayer target);
            
            if (target != null)
            {
                target.UpdateFriction(0);
                playersOnPlat.Remove(target);
                target.OffFriction();
            }
        }
    }
}
