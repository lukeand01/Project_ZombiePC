using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretToBuy : ChestBase
{

    
    [SerializeField] Turret _turret;
    [SerializeField] int price;

    Vector3 originalPos;


    private void Awake()
    {
        originalPos = _turret.transform.localPosition;
        _turret.transform.localPosition = new Vector3(0, -5, 0);
        _turret.ControlCannotShoot(true);
        _turret.gameObject.SetActive(false);
    }

    public override void Interact()
    {
        //check if the sentry is active. 
        //
        if (_turret.gameObject.activeInHierarchy) return;

        if (!PlayerHandler.instance._playerResources.HasEnoughPoints(price)) return;

        PlayerHandler.instance._playerResources.SpendPoints(price);
        StartSentry();
        interactCanvas.ControlInteractButton(false);

    }
    public override void InteractUI(bool isVisible)
    {
        if (_turret.gameObject.activeInHierarchy) return;
        interactCanvas.ControlInteractButton(isVisible);
        interactCanvas.ControlPriceHolder(price);
    }


    void StartSentry()
    {
        float duration = 1;
        _turret.gameObject.SetActive(true);
        _turret.transform.DOLocalMoveY(originalPos.y, duration);
        _turret.CallDurationToStart(duration);
        _turret.SetTurretBuy(this);
    }
    public void StopSentry()
    {
        float duration = 1;
        _turret.transform.DOLocalMoveY(-5, duration);
        _turret.ControlCannotShoot(true);
        Invoke(nameof(DisableSentry), duration);
    }

    void DisableSentry()
    {
        _turret.gameObject.SetActive(false);
    }

}
