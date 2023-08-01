using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : Projectile
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isMoving = false;
            collision.gameObject.TryGetComponent(out IDamageable pInterface);
            if (pInterface != null) pInterface.Die();
        }
        else
        {
            isMoving = false;
            Destroy(gameObject); //will need to play fade out animation
        }
    }
}
