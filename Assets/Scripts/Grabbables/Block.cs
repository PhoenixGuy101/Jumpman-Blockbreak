using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour, IGrab, IObstacle
{
    private Rigidbody2D rb;
    private Collider2D coll;
    private Transform startParent;
    private Transform currParent;
    private Vector3 resetPos;

    private void OnDisable()
    {
        resetPos = gameObject.transform.position; //snapshot the respawn point
        currParent = transform.parent;  //snapshot the transform parent
    }
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        startParent = transform.parent;    //get a reference to the original parent of the Block, allowing it to free itself when let go
        currParent = startParent;
        resetPos = gameObject.transform.position;
    }
    void IGrab.Grabbed(GameObject parent)
    {
        Debug.Log("Grabbing");
        transform.parent = parent.transform;    //set the transform parent to the parent's transform.
        rb.velocity = Vector3.zero;             //ensure the block has no velocity from before it was grabbed
        rb.bodyType = RigidbodyType2D.Kinematic;//change the bodytype of the rigidbody so that it is affected by the player's movement while not just falling from gravity and other forces
        rb.simulated = false;                   //no longer simulated so the player doesn't run into the block as they move around
    }
    void IGrab.LetGo()
    {
        ResetPhysics(false);
    }

    void IObstacle.Reset()
    {
        gameObject.transform.position = resetPos;
        if (currParent != GameManager.Instance.stageArrayProp[GameManager.Instance.currStage].transform.GetChild(2)) ResetPhysics(true);
        else ResetPhysics(false);
    }

    private void ResetPhysics(bool posReset)
    {
        if (posReset) gameObject.transform.parent = currParent; //if given true, the block will have a parent being the currParent
        else gameObject.transform.parent = GameManager.Instance.stageArrayProp[GameManager.Instance.currStage].transform.GetChild(2); //if given false, the block will attach
                                                                                                                                      //itself to the current stage's obstacle GameObject
                                                                                                                                      //instead of its original parent
        transform.localScale = new Vector3(1, 1, 1);//in case if the block has been flipped around, flip it back to its normal state
        transform.rotation = Quaternion.identity;
        rb.bodyType = RigidbodyType2D.Dynamic;      //return the bodytype and the simulated to the original status
        rb.simulated = true;
    }
}
