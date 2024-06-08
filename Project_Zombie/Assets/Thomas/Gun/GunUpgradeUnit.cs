using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GunUpgradeUnit : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI titleText;
    [SerializeField] TextMeshProUGUI descriptionText;

    public void SetUp(string title, string description)
    {
        titleText.text = title;
        descriptionText.text = description;
    }
}
