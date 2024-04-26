using DG.Tweening;
using MyBox;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GunSwapUnit : ButtonBase
{

    public int index {  get; private set; }

    [Separator("GUN SWAP")]
    [SerializeField] GameObject selected;
    [SerializeField] Image portrait;
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] GameObject empty;
    [SerializeField] GameObject highlight;

    ChestUI handler;
    public GunClass gun {  get; private set; }

    private void Awake()
    {
        
    }

    public void SetUp(GunClass gun, ChestUI handler, int index)
    {
        this.gun = gun;
        this.handler = handler;
        this.index = index + 1;


        selected.SetActive(false);

        empty.SetActive(gun.data == null);

        if(gun.data != null)
        {
            portrait.sprite = gun.data.itemIcon;
            nameText.text = gun.data.itemName;
        }
        else
        {
            portrait.sprite = null;
            nameText.text = "";
        }

    }

    public void Select()
    {
        StopAllCoroutines();
        StartCoroutine(HighlightProcess());

    }
    public void Unselect()
    {
        StopAllCoroutines();
        highlight.transform.DOKill();
        highlight.transform.DOScale(0.9f, 0.2f).SetUpdate(true);
    }


   

    IEnumerator HighlightProcess()
    {
        float timer = 0.3f;

        highlight.transform.DOKill();
        highlight.transform.DOScale(1.1f, timer).SetUpdate(true);
        yield return new WaitForSecondsRealtime(timer);
        highlight.transform.DOScale(1.07f, timer).SetUpdate(true);
        yield return new WaitForSecondsRealtime(timer);
        StartCoroutine(HighlightProcess());
    }



    public override void OnPointerClick(PointerEventData eventData)
    {
        if (gun.data == null) return;

        base.OnPointerClick(eventData);

        if(highlight.transform.localScale.x > 1)
        {
            handler.UnselectGunOwned();
        }
        else
        {
            handler.SelectGunOwned(this);
        }


        

    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        if (gun.data == null) return;

        base.OnPointerEnter(eventData);
        selected.SetActive(true);
        handler.HoverGunSwapUnit(this);

    }
    public override void OnPointerExit(PointerEventData eventData)
    {
        if (gun.data == null) return;

        base.OnPointerExit(eventData);
        selected.SetActive(false);
        handler.StopHover();
    }




    private void OnDisable()
    {
        selected.SetActive(false);
        highlight.transform.localScale = new Vector3(0.8f, 0.8f, 0);
    }
}
