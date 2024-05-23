using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResourceGatherCanvas : MonoBehaviour
{

    [SerializeField] GameObject holder;
    [SerializeField] TextMeshProUGUI resourceQuantityText;
    [SerializeField] Image resourceProgressBar;


    public void UpdateResourceGather(int quantity, float current, float total)
    {
        holder.SetActive(true);

        resourceQuantityText.text = quantity.ToString();
        resourceProgressBar.fillAmount = current / total;
    }
    public void Close()
    {
        holder.SetActive(false);
    }

}
