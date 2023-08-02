using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//this class interacts with the parent enemy game object, telling it when it has hit a wall or the player
public class EnemyTrigger : MonoBehaviour
{
    //fields
    private GameObject parentObject;
    private IWalking parentInterface;

    // Start is called before the first frame update
    void Start()
    {
        parentObject = transform.parent.gameObject;
        parentObject.TryGetComponent(out IWalking pI);
        if (pI != null) parentInterface = pI;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) parentInterface.OnPlayerHit(collision.gameObject);
        
        else parentInterface.OnWallBump();
    }
}
