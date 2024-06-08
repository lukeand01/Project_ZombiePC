using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DebugUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI debugUI;


    public void UpdateDEBUGUI(string update)
    {
        debugUI.text = update;
    }
}
