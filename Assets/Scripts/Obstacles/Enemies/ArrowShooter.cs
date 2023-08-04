using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowShooter : FreezeableFunctionality, IObstacle
{

    [SerializeField]
    private Vector2 arrowSpeed;
    [SerializeField]
    private float shootDelay = 2;
    private float shootTimer;
    [SerializeField]
    private GameObject arrowPrefab;
    private Direction fireDirection;
    private Vector3 arrowTransform;
    private GameObject instArrow;

    void IObstacle.Reset()
    {

    }

    private void Start()
    {
        shootTimer = shootDelay;
    }

    private void FixedUpdate()
    {
        if (shootTimer > 0 && !isFrozen) shootTimer -= Time.fixedDeltaTime;
        else if (!base.isFrozen)
        {
            shootTimer = shootDelay;
            ShootArrow(); //will have to trigger an animation instead, which then triggers the actual shooting of the arrow upon completion.
            
        }
    }

    private void ShootArrow()
    {
        instArrow = Instantiate(arrowPrefab, arrowTransform, Quaternion.identity) as GameObject;
        instArrow.gameObject.TryGetComponent(out IProjectile iProj);
        if (iProj != null)
        {
            iProj.Setup(arrowSpeed);
        }
        //shootTimer = shootDelay;
    }

    private void SetDirection()
    {
        if (Mathf.Abs(arrowSpeed.x) > Mathf.Abs(arrowSpeed.y))
        {
            if (arrowSpeed.x > 0) fireDirection = Direction.right;
            else fireDirection = Direction.left;
        }
        else
        {
            if (arrowSpeed.y > 0) fireDirection = Direction.up;
            else fireDirection = Direction.down;
        }
    }

    private void SetArrowTransform()
    {
        switch (fireDirection)
        {
            case Direction.right:
                arrowTransform = transform.position + (1 * Vector3.right);
                break;
            case Direction.left:
                arrowTransform = transform.position - (1 * Vector3.right);
                break;
            case Direction.up:
                arrowTransform = transform.position + (1 * Vector3.up);
                break;
            case Direction.down:
                arrowTransform = transform.position - (1 * Vector3.up);
                break;
        }
    }
}
