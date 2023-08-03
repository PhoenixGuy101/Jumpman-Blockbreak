using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowShooter : FreezeableFunctionality
{

    [SerializeField]
    private Vector2 arrowSpeed;
    [SerializeField]
    private float shootDelay = 2;
    private float shootTimer;
    [SerializeField]
    private GameObject arrowPrefab;

    private void Start()
    {
        shootTimer = shootDelay;
    }

    private void FixedUpdate()
    {
        if (shootTimer > 0 && !isFrozen) shootTimer -= Time.fixedDeltaTime;
        else if (!base.isFrozen)
        {
            ShootArrow(); //will have to trigger an animation instead, which then triggers the actual shooting of the arrow upon completion.
        }
    }

    private void ShootArrow()
    {
        GameObject instArrow = Instantiate(arrowPrefab, transform.position, Quaternion.identity);
        instArrow.TryGetComponent(out IProjectile iProj);
        if (iProj != null)
        {
            iProj.Setup(arrowSpeed);
        }
        shootTimer = shootDelay;
    }
}
