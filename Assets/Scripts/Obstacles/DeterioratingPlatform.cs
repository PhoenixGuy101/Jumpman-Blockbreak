using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeterioratingPlatform : MonoBehaviour, IObstacle, IDeteriorate
{
    //fields
    [SerializeField]
    [Range(1, 3)]
    private int leaveTotal = 3; //how many times the player can land and leave the platform before it is destroyed (editable in the inspector)
    private int leaveLeft;      //a tracker for how many times the player has left the platform
    private bool playerOn;      //a variable to confirm the player landed on the platform.

    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private Sprite healthySprite;
    [SerializeField]
    private Sprite damagedSprite;
    [SerializeField]
    private Sprite destroyedSprite;
    
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
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        switch (leaveTotal)
        {
            case 3:
                spriteRenderer.sprite = healthySprite;
                break;
            case 2:
                spriteRenderer.sprite = damagedSprite;
                break;
            case 1:
                spriteRenderer.sprite = destroyedSprite;
                break;
        }
        Debug.Log("Block Health: " + leaveLeft);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (collision.transform.position.y > gameObject.transform.position.y + 1f) playerOn = true;
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
        //Debug.Log("Block Health: " + leaveLeft);
        if (leaveLeft == 2) spriteRenderer.sprite = damagedSprite;
        else if (leaveLeft == 1) spriteRenderer.sprite = destroyedSprite;
        else
        {
            StartCoroutine(Crumble());
        }
    }

    private IEnumerator Crumble()
    {
        Color c = spriteRenderer.color;
        for (float alpha = 1f; alpha >= 0; alpha -= 0.2f)
        {
            c.a = alpha;
            spriteRenderer.color = c;
            yield return null;
        }
        Destroy(gameObject); //will need to disappear or play a crumbling animation.
    }
}
