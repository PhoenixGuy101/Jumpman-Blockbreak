using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeTest : MonoBehaviour, IFreezeable
{
    //fields
    private IFreezeable freezeInterface;

    void IFreezeable.Freeze()
    {
        Debug.Log("Freeze");
    }
    void IFreezeable.UnFreeze()
    {
        Debug.Log("UnFreeze");
    }
    
    // Start is called before the first frame update
    void Start()
    {
        freezeInterface = gameObject.GetComponent<IFreezeable>();
        GameManager.Instance.freezableManager = freezeInterface;
    }
    private void OnDestroy()
    {
        GameManager.Instance.RemoveFreezable(freezeInterface);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
