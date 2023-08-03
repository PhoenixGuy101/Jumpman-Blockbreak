using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingEnemy : MonoBehaviour, IDamageable, IWalking, IFreezeable
{
    //fields
    private Rigidbody2D rb;

    private IFreezeable freezeableInterface;

    [SerializeField]
    private bool enemyMoves = true;
    [SerializeField]
    private bool facingRight = true;
    [SerializeField]
    private float moveSpeed = 7.0f;        //speed moving across the X axis
    [SerializeField]
    private float acceleration = 5.0f;      //acceleration to acheive target speed
    [SerializeField]
    private float decceleration = 7.0f;    //decceleration to slow down to target speed, or to slow down if they're going too fast
    [SerializeField]
    [Range(0.0f, 1.0f)]
    private float velPower = 1.0f;   //the power of the acceleration and decceleration
    private float targetVelocity;
    private float speedDif;
    private float accelRate;
    private float movement;

    [ExecuteInEditMode] private void OnValidate()
    {
        Turn();
    }

    void IDamageable.Die()
    {

    }
    
    /* IWalking is an interface that is interacted with via a child gameobject containing a trigger box
     * This way, the trigger box child object handles the actual triggers and communicates with
     * the enemy parent gameobject to execute upon those triggers.
     */

    void IWalking.OnPlayerHit(GameObject player)
    {
        AttackPlayer(player);
    }

    void IWalking.OnWallBump()
    {
        WallBump();
    }

    void IFreezeable.Freeze()
    {
        enemyMoves = false;
    }

    void IFreezeable.UnFreeze()
    {
        enemyMoves = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        freezeableInterface = gameObject.GetComponent<IFreezeable>();
        GameManager.Instance.freezableManager = freezeableInterface;
    }

    private void FixedUpdate()
    {
        if (enemyMoves)
        {
            targetVelocity = facingRight ? moveSpeed : -moveSpeed;
            speedDif = targetVelocity - rb.velocity.x;
            accelRate = (Mathf.Abs(rb.velocity.x) <= moveSpeed) ? acceleration : decceleration;
            movement = Mathf.Pow(Mathf.Abs(speedDif) * accelRate, velPower) * Mathf.Sign(speedDif);
            rb.AddForce(movement * Vector2.right, ForceMode2D.Force);
        }
    }

    private void OnDestroy()
    {
        GameManager.Instance.RemoveFreezable(freezeableInterface);
    }


    private void WallBump()
    {
        facingRight = facingRight ? false : true;
        Turn();
    }

    private void Turn()
    {
        if (facingRight == false)
        {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
        else if (facingRight == true)
        {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
    }
    private void AttackPlayer(GameObject p)
    {
        p.TryGetComponent(out IDamageable pDamage);
        if (pDamage != null)
        {
            enemyMoves = false;
            FinishAttackPlayer(pDamage); //should be an event to trigger attack animation
        }
    }
    private void FinishAttackPlayer(IDamageable damInterface) //plays once the attack animation is done.
    {
        damInterface.Die();
    }
}