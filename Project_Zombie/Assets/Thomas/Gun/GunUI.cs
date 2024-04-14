using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GunUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI currentAmmoText;
    [SerializeField] TextMeshProUGUI reserveAmmoText;
    [SerializeField] Image reloadFillImage;
    [SerializeField] Image gunPortrait;

    public void UpdateGunPortrait(Sprite icon)
    {
        gunPortrait.sprite = icon;  
    }

    public void UpdateAmmoGun(int current, int reserve)
    {
        currentAmmoText.text = current.ToString();
        reserveAmmoText.text = reserve.ToString();


        if(reserve == -1)
        {
            reserveAmmoText.text = "?";
        }

        if(reserve == 0)
        {
            reserveAmmoText.color = Color.red;
        }
        else
        {
            reserveAmmoText.color = Color.white;
        }

        if (current == 0)
        {
            currentAmmoText.color = Color.red;
        }
        else
        {
            currentAmmoText.color = Color.white;
        }


    }

    public void UpdateReloadFill(float current, float total)
    {

        reloadFillImage.gameObject.SetActive(total > 0);
        reloadFillImage.fillAmount = current / total;
    }
}
