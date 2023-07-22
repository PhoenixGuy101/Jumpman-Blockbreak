using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillBox : MonoBehaviour
{
    [SerializeField]
    private Vector3 respawn;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.gameObject.TryGetComponent(out IDamageable pInterface);
            if (pInterface != null) pInterface.Die();
        }
        else if (collision.CompareTag("Block"))
        {
            collision.gameObject.TryGetComponent(out IObstacle oInterface);
            if (oInterface != null) oInterface.Reset();
        }
    }
}
