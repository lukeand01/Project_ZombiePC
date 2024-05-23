using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatTrackerUnit : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI infoText;

    public void SetUp(StatTrackerType stat, float value)
    {
        infoText.text = stat.ToString() + ": " + value.ToString();
    }

}
