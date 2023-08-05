using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PortalTransitionCycleAnnouncement : MonoBehaviour
{
    public TMP_Text cycleText;

    private void OnDisable()
    {

        if (GameManager.Instance.currentCycle != 0) cycleText.text = GameManager.Instance.currentCycle.ToString();
        else cycleText.text = null;
    }
}
