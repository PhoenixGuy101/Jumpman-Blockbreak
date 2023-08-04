using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowShooter : FreezeableFunctionality, IObstacle, IFreezeable
{

    [SerializeField]
    private Vector2 arrowSpeed;
    [SerializeField]
    private float arrowLifeSpan = 10;
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
        if (!isFrozen) ReloadDelay();
    }

    private void OnEnable()
    {
        StageEnd.OnPlayerReachingEnd += ReloadDelay;
    }

    private void OnDisable()
    {
        StageEnd.OnPlayerReachingEnd -= ReloadDelay;
    }

    protected override void Start()
    {
        base.Start();
        ReloadDelay();
        SetDirection();
        SetArrowTransform();
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
        instArrow = Instantiate(arrowPrefab, arrowTransform, Quaternion.identity) as GameObject;
        instArrow.gameObject.TryGetComponent(out IProjectile iProj);
        if (iProj != null)
        {
            iProj.Setup(arrowSpeed, arrowLifeSpan);
        }
        ReloadDelay();
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
                arrowTransform = new Vector3(gameObject.transform.position.x + 1, gameObject.transform.position.y, gameObject.transform.position.z);
                break;
            case Direction.left:
                arrowTransform = new Vector3(gameObject.transform.position.x - 1, gameObject.transform.position.y, gameObject.transform.position.z);
                break;
            case Direction.up:
                arrowTransform = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 1, gameObject.transform.position.z);
                break;
            case Direction.down:
                arrowTransform = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - 1, gameObject.transform.position.z);
                break;
        }
        Debug.Log("arrowTransform: " + arrowTransform);
    }

    private void ReloadDelay()
    {
        shootTimer = shootDelay;
    }
}
