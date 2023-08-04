using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeableFunctionality : MonoBehaviour, IFreezeable
{
    protected bool isFrozen;
    private IFreezeable freezeInterface;

    void IFreezeable.Freeze()
    {
        isFrozen = true;
    }

    void IFreezeable.UnFreeze()
    {
        isFrozen = false;
    }
    void IFreezeable.Remove()
    {
        Destroy(gameObject);
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        freezeInterface = gameObject.GetComponent<IFreezeable>();
        GameManager.Instance.freezableManager = freezeInterface;
    }

    private void OnDestroy()
    {
        GameManager.Instance.RemoveFreezable(freezeInterface);
    }
}
