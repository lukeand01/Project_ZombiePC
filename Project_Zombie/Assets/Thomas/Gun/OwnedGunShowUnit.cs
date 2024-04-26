using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class OwnedGunShowUnit : MonoBehaviour
{
    [SerializeField] Image icon;
    [SerializeField] GameObject selected;
    [SerializeField] TextMeshProUGUI ammoText;
    [SerializeField] GameObject holder;

    private void Awake()
    {
        holder.SetActive(false);
    }
    public void SetUp(GunClass gun)
    {
        icon.sprite = gun.data.itemIcon;
        selected.SetActive(false);
        ammoText.text = gun.ammoCurrent.ToString();
        holder.SetActive(true);
    }

    public void Select()
    {
        selected.SetActive(true);
    }
    public void Unselect()
    {
        selected.SetActive(false);
    }
    public void UpdateAmmo(int ammo)
    {
        ammoText.text = ammo.ToString();    
    }

}
