using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal_Merchant : MonoBehaviour
{

    TeleporterObject _lastTeleporter;
    //cooldown?
    [SerializeField] Transform _teleportPlace;
    [SerializeField] Merchant _merchant;
    public bool isOnCooldown { get; private set; }

    public void TeleportToHere(TeleporterObject lastTeleporter)
    {
        _lastTeleporter = lastTeleporter;
        PlayerHandler.instance.transform.position = _teleportPlace.position;
        _merchant.SetUp();


        //lock all enmies.

    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player") return;

        TeleportBack();
    }


    public void TeleportBack()
    {
        //teleport to another teleporter.
        StopAllCoroutines();
        StartCoroutine(TeleportProcess());
    }

    IEnumerator TeleportProcess()
    {
        //here we actually start teleporting.
        //the player becomes untouchable.
    

        PlayerHandler.instance._playerController.block.AddBlock("Teleport", BlockClass.BlockType.Complete);

       //another sound here.

        //then we raise the window, and whents taht done we call
        yield return StartCoroutine(GameHandler.instance.LowerCurtainProcess_Teleport(2));

        if (_lastTeleporter != null)
        {
            _lastTeleporter.CallTimer();
            _lastTeleporter.ControlHasBeenTeleported(true);
        }
        PlayerHandler.instance.transform.position = _lastTeleporter.transform.position;


        StartCoroutine(GameHandler.instance.RaiseCurtainProcess_Teleport(2));
        yield return new WaitForSeconds(1);

        PlayerHandler.instance._playerController.block.RemoveBlock("Teleport");


        //then we lower it.

        CallCooldown();

        

        PlayerHandler.instance._entityEvents.OnLockPortals(false);
        

    }

    void CallCooldown()
    {
        //we handle the cooldown here.
    }

}
