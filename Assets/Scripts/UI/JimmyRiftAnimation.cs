using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JimmyRiftAnimation : MonoBehaviour
{
    private void OnDisable()
    {
        GameManager.Instance.WinLevel();
    }
}
