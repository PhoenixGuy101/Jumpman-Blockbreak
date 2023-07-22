using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStage
{
    void InitializeStage();
    void LeaveStage();
    void PlayerDeath();
}
