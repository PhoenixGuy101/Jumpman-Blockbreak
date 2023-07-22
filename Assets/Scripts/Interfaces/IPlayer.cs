using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayer
{
    void OnFriction(GameObject platform);
    void OffFriction();
    void UpdateFriction(float f);
}
