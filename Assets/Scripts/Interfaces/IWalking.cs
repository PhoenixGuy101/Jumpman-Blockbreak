using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWalking
{
    void OnWallBump();
    void OnPlayerHit(GameObject player);
}
