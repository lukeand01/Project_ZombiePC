using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GunUpgradeStationCanvas : MonoBehaviour
{
    [SerializeField] GameObject gunProgressHolder;
    [SerializeField] Image gunProgressBar;
    [SerializeField] GameObject debug_IsReadyText;

    public void UpdateGunProgress(float current, float total)
    {
        gunProgressHolder.SetActive(total > 0);
        gunProgressBar.fillAmount = current / total;
    }

    public void ControlDebugIsReady(bool isReady)
    {
        debug_IsReadyText.SetActive(isReady);
    }

}
