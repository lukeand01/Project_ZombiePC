using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretToBuy_Flying : ChestBase
{
    [SerializeField] int price;
    [SerializeField] GameObject graphic;
    


    public override void Interact()
    {

        if (!graphic.activeInHierarchy) return;

        if (!PlayerHandler.instance._playerResources.HasEnoughPoints(price)) return;



        TurretFlying turretFly =  GameHandler.instance._pool.GetTurretFly(transform);
        PlayerHandler.instance.AddTurretFly(turretFly);
        
        gameObject.SetActive(false);



        base.Interact();
    }

    public override void InteractUI(bool isVisible)
    {
        base.InteractUI(isVisible);
    }


    private void OnDestroy()
    {
        //Debug.Log("destroy turret?");
    }



}

//this needs a pool for itself.
//best solution