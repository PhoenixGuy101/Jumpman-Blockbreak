using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowShooter : MonoBehaviour, IFreezeable
{
    private IFreezeable freezeableInterface;
    private bool isFrozen;

    [SerializeField]
    private Vector2 arrowSpeed;
    [SerializeField]
    private float shootDelay = 2;
    private float shootTimer;
    [SerializeField]
    private GameObject arrowPrefab;

    void IFreezeable.Freeze()
    {
        isFrozen = true;
    }

    void IFreezeable.UnFreeze()
    {
        isFrozen = false;
    }

    private void Start()
    {
        freezeableInterface = gameObject.GetComponent<IFreezeable>();
        GameManager.Instance.freezableManager = freezeableInterface;
        shootTimer = shootDelay;
    }

    private void FixedUpdate()
    {
        if (shootTimer > 0 && !isFrozen) shootTimer -= Time.fixedDeltaTime;
        else if (!isFrozen)
        {
            ShootArrow(); //will have to trigger an animation instead, which then triggers the actual shooting of the arrow upon completion.
        }
    }

    private void OnDestroy()
    {
        GameManager.Instance.RemoveFreezable(freezeableInterface);
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
