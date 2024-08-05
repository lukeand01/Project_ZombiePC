using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TeleporterObject : MonoBehaviour
{
    //it can either send to a spot or send to a teleporter.
    //


    //FEATURES:
    //can teleport to a teleporter.
    //can teleport to a place.
    //does a presentation on teleport.
    //teleport has a cooldown, both sides.
    //teleport has a timer, that is run by the last teleporter, at the end it does something, but using the teleporter forces it earlier.
    //

    bool isPlayerInside;//So we know when to run the progress.
    bool hasBeenTeleportedTo;//this we use so the player doesnt trigger it after being teleported.
    bool isTimerRunning;
    bool isAlreadyCalled; //we use this for especial cases.
    bool isLocked;
    public bool isTeleporting {  get; private set; }

    public void ControlHasBeenTeleported(bool hasBeenTeleported)
    {
        this.hasBeenTeleportedTo = hasBeenTeleported;   
    }

    private void FixedUpdate()
    {
        DetectIfPlayerShouldBeTeleported();
    }

    
    [Separator("TIME FOR THE TELEPORT TO WORK")]
    [SerializeField] float timeToTeleport_Total;
    [SerializeField] float timeToTeleport_Current;

    void DetectIfPlayerShouldBeTeleported()
    {
        if (isTeleporting) return;
        if (!isPlayerInside) return;
        if (hasBeenTeleportedTo) return;
        if (isLocked) return;
        if (IsOnCooldown) return;
        if (connectedTeleporter.IsOnTimer) return;


        if (timeToTeleport_Total > timeToTeleport_Current)
        {

            timeToTeleport_Current += Time.fixedDeltaTime;
        }
        else
        {

            Teleport();
        }

    }


    #region TELEPORTING FUNCTIONS

    [Separator("TELEPORTING VARIABLES")]
    [SerializeField] TeleporterObject connectedTeleporter;
    [SerializeField] Transform wayOutTransform; //you leave to a place that is not a teleporter.
    [SerializeField] UnityEvent eventOnTeleportedHere;
    [SerializeField] bool lockAllEnemies;


    //if there is a wayout it will use it instead of teh telerpoter. always
    //but it always inform the other teleporter that it can be used.
    //if either of them has cooldown, the cooldown is activated.
    //if you use a teleporter that does not have timer, nothing happens
    //if you go to a teleporter that has tiomer, it starts the timer
    //if you use a teleporter that has timer, it spots the timer.

    //it still calling the teleporter.
    //and is allowed the teleport to go between the timer.


    void Teleport()
    {
        //teleport to another teleporter.

        timeToTeleport_Current = 0;

        if(isTimerRunning)
        {
            StopTimer();
            isAlreadyCalled = true;
        }
        
        StartCoroutine(TeleportProcess());
    }

    void TeleportToPlace()
    {
        if (wayOutTransform != null)
        {
            PlayerHandler.instance.transform.position = wayOutTransform.transform.position;
        }
        else
        {
            connectedTeleporter.ControlHasBeenTeleported(true);
            PlayerHandler.instance.transform.position = connectedTeleporter.transform.position;
        }
    }

    public void CallTimer()
    {
        //

        if(timer_Total > 0 && !isAlreadyCalled)
        {
            //if itself.            

            if (isTimerRunning)
            {
                //if its running we will make it stop running
                StopTimer();
                Debug.Log("stop timer " + gameObject.name);
            }
            else
            {
                //if its not then we will start.
                StartTimer();
                Debug.Log("start timer " + gameObject.name);
            }


        }

      




    }

    void CallCooldown()
    {
        if (cooldownForTeleport_Total > 0)
        {
            //start cooldown.

        }
    }

    void CallEvents()
    {
        eventOnTeleportedHere.Invoke();
    }

    void CallLock()
    {
        if (lockAllEnemies)
        {
            PlayerHandler.instance._entityEvents.OnLockEntity(true);
        }
    }

    IEnumerator TeleportProcess()
    {

        isTeleporting = true;

        yield return StartCoroutine(LowerCurtainProcess());

        //here we actually take the fella back.
        TeleportToPlace();

        yield return StartCoroutine(RaiseCurtainProcess());

        CallCooldown();
        connectedTeleporter.CallTimer();
        connectedTeleporter.CallEvents();
        connectedTeleporter.CallLock();

        if (lockAllEnemies)
        {
            PlayerHandler.instance._entityEvents.OnLockEntity(false);
        }

        isAlreadyCalled = false;
        isTeleporting = false;
    }

    IEnumerator LowerCurtainProcess()
    {
        float duration = 1;

        PlayerHandler.instance._playerController.block.AddBlock("Teleporter", BlockClass.BlockType.Complete);

        StartCoroutine(GameHandler.instance.LowerCurtainProcess_Simple(1));

        //we call the thunder and make the player disappear.

        yield return new WaitForSeconds(duration * 0.1f);

        PlayerHandler.instance.ControlGraphicHolderVisibility(false);

        yield return new WaitForSeconds(duration * 0.9f);
    }

    IEnumerator RaiseCurtainProcess()
    {

        

        float duration = 1;

        yield return new WaitForSeconds(0.1f);


        StartCoroutine(GameHandler.instance.RaiseCurtainProcess_Simple(1));

        yield return new WaitForSeconds(duration * 0.3f);

        PlayerHandler.instance.ControlGraphicHolderVisibility(true);
        PlayerHandler.instance._playerController.block.RemoveBlock("Teleporter");

        yield return new WaitForSeconds(duration * 0.7f);
    }


    #endregion


    #region COOLDOWN
    [Separator("COOLDOWN")]
    [SerializeField] int cooldownForTeleport_Total;
    [SerializeField] int cooldownForTeleport_Current;

    public bool IsOnCooldown { get { return cooldownForTeleport_Current > 0; } }

    #endregion

    #region TIMER
    [Separator("TIMER BASED")]
    [SerializeField] int timer_Total;
    [SerializeField] int timer_Current;

    public bool IsOnTimer { get {  return timer_Current > 0; } }

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
            yield return new WaitForSeconds(1.2f);
        }


       if(isTimerRunning) Teleport();

    }
    void StartTimer()
    {       
        UIHandler.instance._playerUI.ShowTimerForTeleport();
        timerCoroutine = StartCoroutine(TimerProcess());
        isTimerRunning = true;
    }
    void StopTimer()
    {
        timer_Current = 0;
        isTimerRunning = false;
        UIHandler.instance._playerUI.HideTimerForTeleport();

        if(timerCoroutine != null)
        {
            StopCoroutine(timerCoroutine);
        }
       
    }

    #endregion


    #region COLLISION

    //
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
    #endregion

}
