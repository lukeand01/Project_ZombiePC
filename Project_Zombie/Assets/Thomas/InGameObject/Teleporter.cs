using MyBox;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Timers;
using UnityEngine;

public class Teleporter : MonoBehaviour
{

    //if the player enter it starts charging.
    //once charged it sends the player to the right place.

    bool isPlayerInside;
    bool isTeleporting;
    bool hasBeenTeleportedTo;
    [SerializeField]bool hasCountedTimerAlready;

    public void ControlHasBeenTeleportedTo(bool hasBeenTeleported)
    {
        this.hasBeenTeleportedTo = hasBeenTeleported;
    }

    [SerializeField] float timeToTeleport_Current;
    [SerializeField] float timeToTeleport_Total;


    [SerializeField] int cooldownForTeleport_Total;
    [SerializeField] int cooldownForTeleport_Current;

    [SerializeField] Teleporter targetTeleporter; //teleport to this always;
    [SerializeField] bool shouldLockEnemies;


    //this thing still need a teleporter.

    //we will fade in.
    //teleport the player to the target but disable the player mvoement and graphic
    //then fade out
    //then we show the player together with a thunder.

    //

    private void FixedUpdate()
    {


        if(targetTeleporter != null)
        {
            if(targetTeleporter.isTimerRunning)
            {
             
                return;
            }

        }

        if (!isPlayerInside) return;
        if (hasBeenTeleportedTo) return;



        if (timeToTeleport_Total > timeToTeleport_Current)
        {
            timeToTeleport_Current += Time.fixedDeltaTime;
        }
        else
        {
            SendTeleport();
        }
    }

    void SendTeleport()
    {
        //lock the player
        //send information
        StartCoroutine(SendTeleportProcess(false));

    }


    IEnumerator SendTeleportProcess(bool sendToReturnPoint)
    {
        timeToTeleport_Current = 0;
        isTeleporting = true;
        PlayerHandler.instance._playerController.block.AddBlock("Teleporter", BlockClass.BlockType.Complete);

        if (shouldLockEnemies)
        {
            PlayerHandler.instance._entityEvents.OnLockPortals(true);
            Debug.Log("called it true");
        }

        
      
        float duration = 1;


        //we call the thunder and make the player disappear.

        yield return new WaitForSeconds(duration * 0.1f);

        PlayerHandler.instance.ControlGraphicHolderVisibility(false);

        yield return new WaitForSeconds(duration * 0.9f);

        //then we teleport to the teleport
        //and we start the process.
        if(targetTeleporter != null)
        {
            targetTeleporter.ControlHasBeenTeleportedTo(true);
        }

        ControlHasBeenTeleportedTo(false);

        if (sendToReturnPoint)
        {
            PlayerHandler.instance.transform.position = spotForReturn.transform.position;
        }
        else
        {
            PlayerHandler.instance.transform.position = targetTeleporter.transform.position;
        }


        yield return new WaitForSeconds(0.1f);



        yield return new WaitForSeconds(duration * 0.3f);

        PlayerHandler.instance.ControlGraphicHolderVisibility(true);
        PlayerHandler.instance._playerController.block.RemoveBlock("Teleporter");

        yield return new WaitForSeconds(duration * 0.7f);
     
        //this must count in the other being teleported.
        

        if(targetTeleporter != null)
        {
            targetTeleporter.CallTimer();
        }
        else
        {

        }
        
        if (!shouldLockEnemies)
        {
            PlayerHandler.instance._entityEvents.OnLockPortals(false);
            Debug.Log("called it false");
        }
    }

    public void CallTimer()
    {
        Debug.Log("here " + gameObject.name);

        if (hasCountedTimerAlready)
        {
            hasCountedTimerAlready = false;
            return;
        }

        if (timer_Total > 0)
        {
            if (timer_Current > 0)
            {
                StopTimer();
            }
            else
            {
                StartTimer();
            }

            hasCountedTimerAlready = true;

        }
    }

    void ReceiveTeleport()
    {
        //
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "Player") return;

        isPlayerInside = true;


    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag != "Player") return;

        isPlayerInside = false;
        hasBeenTeleportedTo = false;

        timeToTeleport_Current = 0;

    }


    #region TIMER BASED
    [Separator("TIMER BASED")]
    [SerializeField] Transform spotForReturn;
    [SerializeField] int timer_Total;
    [SerializeField] int timer_Current;

    public bool isTimerRunning { get { return timer_Current > 0; } }

    public bool HasTimer { get { return timer_Total > 0; } }
    


    Coroutine timerCoroutine;

    //when it willingly returns. it should go to return point
    //when it is forced should remove the timer.

    IEnumerator TimerProcess()
    {

        timer_Current = timer_Total;
        
        while (timer_Current > 0)
        {
            timer_Current -= 1;
            UIHandler.instance._playerUI.UpdateTimerForTeleport(timer_Current);
            yield return new WaitForSeconds(1);
        }

        StopTimer();
        StartCoroutine(SendTeleportProcess(true));

    }
    void StartTimer()
    {

        timer_Current = 0;
        UIHandler.instance._playerUI.ShowTimerForTeleport();
       timerCoroutine = StartCoroutine(TimerProcess());
    }
    void StopTimer()
    {

        UIHandler.instance._playerUI.HideTimerForTeleport();

        StopCoroutine(timerCoroutine);
    }


    #endregion
}


//so we have teleporter 1 
//and teleporter 2
//teleporter 1 always handles the logic and teleporter 2 justs exists if the player wants to return earlier.
//


//first goal is creating the teleporter with timer.
//second goal is to create a cooldown for the teleporter.