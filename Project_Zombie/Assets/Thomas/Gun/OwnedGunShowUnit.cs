using MyBox;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OwnedGunShowUnit : ButtonBase
{
    [Separator("OWNED GUN PART")]
    [SerializeField] Image icon;
    [SerializeField] GameObject selected;
    [SerializeField] TextMeshProUGUI ammoText;
    [SerializeField] GameObject holder;

    GunClass gun;

    private void Awake()
    {
        holder.SetActive(false);
    }
    public void SetUp(GunClass gun)
    {
        this.gun = gun;
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

    bool isPause = false;
    bool isEnd = false;


    private void Update()
    {
        isPause = UIHandler.instance._pauseUI.IsPauseOn();
        isEnd = UIHandler.instance._EndUI.IsEnd();

        if (!isPause && !isEnd)
        {
            selected.SetActive(false);
        }
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);

        if (isPause || isEnd)
        {
            selected.SetActive(true);
            UIHandler.instance._pauseUI.DescribeGun(gun, transform);
        }
    }
    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);

        if (isPause || isEnd)
        {
            selected.SetActive(false);
            UIHandler.instance._pauseUI.StopDescription();
        }
    }
    private void OnDisable()
    {
        selected.SetActive(false);
    }

}
