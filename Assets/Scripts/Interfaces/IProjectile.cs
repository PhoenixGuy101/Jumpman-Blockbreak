using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IProjectile
{
    void Setup(Vector2 velocity, float lifeSpan);
}
