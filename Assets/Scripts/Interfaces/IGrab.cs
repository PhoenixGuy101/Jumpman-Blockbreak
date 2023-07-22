using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGrab
{
    void Grabbed(GameObject parent);
    void LetGo();
}
