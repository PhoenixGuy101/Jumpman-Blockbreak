using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundedCheck
{
    private GameObject source;
    private Collider2D coll;
    private Rigidbody2D rb;
    private Vector3 size;
    private float d;

    public GroundedCheck(GameObject s, Rigidbody2D rigid, float distance)
    {
        source = s;
        coll = s.GetComponent<Collider2D>();
        size = new Vector3(coll.bounds.size.x, coll.bounds.size.y, coll.bounds.size.z);
        rb = rigid;
        d = distance;
        LayerMask lm = s.layer;
    }
    
    //the only method that is used outside of instantiation: used to test if the game object passed in is on the ground or not.
    public bool TestForGrounded()
    {
        if (Physics2D.BoxCast(coll.bounds.center, size, 0.0f, Vector2.down, d, ~(1 << source.layer)) && rb.velocity.y <= 0) return true;

        else return false;
    }
}
