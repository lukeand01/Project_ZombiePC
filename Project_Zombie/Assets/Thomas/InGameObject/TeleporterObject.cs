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
        hasBeenTeleportedTo = hasBeenTeleported;   
    }

    private void FixedUpdate()
    {
        DetectIfPlayerShouldBeTeleported();
    }

    private void Awake()
    {
        rotationSpeed_Current = rotationSpeed_Base;
    }

    private void Start()
    {
        PlayerHandler.instance._entityEvents.eventEntityStunned += DetectIfPlayerGotStunned;

    }


    [Separator("TIME FOR THE TELEPORT TO WORK")]
    [SerializeField] float timeToTeleport_Total;
    [SerializeField] float timeToTeleport_Current;

    


    #region DETECT
    void DetectIfPlayerGotStunned()
    {
        if (isPlayerInside)
        {

            //effect die off rather than just disappear.

        }
    }

    bool IsAbleToTeleport()
    {
        if (isTeleporting) return false;
        if (!isPlayerInside) return false;
        if (hasBeenTeleportedTo)
        {
            Debug.Log("has been teleported to");
            return false;
        }
        if (isLocked) return false;
        if (IsOnCooldown) return false;
        if(connectedTeleporter != null) if (connectedTeleporter.IsOnTimer) return false;
        if (merchantPortal != null) if (merchantPortal.isOnCooldown) return false;


        if (PlayerHandler.instance._entityStat.isStunned) return false;

        return true;
    }

    void DetectIfPlayerShouldBeTeleported()
    {

        if (!isPlayerInside)
        {
            ControlEffect(false);
            
        }

        if (!IsAbleToTeleport())
        {

            timeToTeleport_Current = 0;

            rotationSpeed_Current = rotationSpeed_Base;

            if (rotatingRb.angularVelocity.y > 0)
            {
                rotatingRb.AddTorque(new Vector3(0, -0.2f, 0), ForceMode.Impulse);
            }


            return;
        }

        HandleRotation();

       
        //

        if (timeToTeleport_Current > timeToTeleport_Total * 0.7f)
        {
            ControlEffect(true);
        }

        if (timeToTeleport_Total > timeToTeleport_Current)
        {
            timeToTeleport_Current += Time.fixedDeltaTime;


        }
        else
        {

            Teleport();
        }

    }

    #endregion

    #region ROTATION

    [Separator("ROTATION")]
    [SerializeField] Rigidbody rotatingRb;
    [SerializeField] float rotationSpeed_Base;
    [SerializeField] float rotationSpeed_IncrementModifier;
    [SerializeField] float rotationSpeed_Max;

    float rotationSpeed_Current; //

    void HandleRotation()
    {
        rotatingRb.AddTorque(new Vector3(0, rotationSpeed_Current, 0), ForceMode.Force);
        
        

        rotationSpeed_Current += Time.fixedDeltaTime * rotationSpeed_IncrementModifier;
        rotationSpeed_Current = Mathf.Clamp(rotationSpeed_Current, 0, rotationSpeed_Max);
    }


    #endregion

    #region ESPECIAL EFFECTS
    [SerializeField] ParticleSystem[] effectObjects_Common;
    [SerializeField] ParticleSystem soonToTeleportEffectHolder;
    [SerializeField] GameObject thunderEffectHolder;
    bool areEffectsVisible;

    void ControlEffect(bool isVisible)
    {
        if(areEffectsVisible != isVisible)
        {
            foreach (var item in effectObjects_Common)
            {
                item.gameObject.SetActive(isVisible);
            }

            areEffectsVisible = isVisible;
        }


        
    }


    #endregion

    #region SOUND EFFECTS
    [Separator("SOUND")]
    [SerializeField] AudioSource charge_AudioSource;
    [SerializeField] AudioClip thunder_AudioClip;


    void StartChargeAudio()
    {
        charge_AudioSource.Play();
    }
    void StopChargeAudio()
    {
        charge_AudioSource.Stop();
    }


    void CallThunderAudio()
    {
        GameHandler.instance._soundHandler.CreateSfx(SoundType.AudioClip_ThunderTeleporter);
    }

    #endregion

    #region TELEPORTING FUNCTIONS

    [Separator("TELEPORTING VARIABLES")]
    [SerializeField] TeleporterObject connectedTeleporter;
    [SerializeField] Transform wayOutTransform; //you leave to a place that is not a teleporter.
    [SerializeField] Portal_Merchant merchantPortal;
    [SerializeField] UnityEvent eventOnTeleportedHere;
    [SerializeField] bool lockAllEnemies;

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
        if(merchantPortal != null)
        {
            merchantPortal.TeleportToHere(this);
            return;
        }

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
            PlayerHandler.instance._entityEvents.OnLockPortals(true);
        }
    }


    //there is a period where you can move
    //there is another where you cannot
    //the generator sound starts shortly after entering the generator
    //it should signal when teh player can no longer move
    //soon to move should as sonn as the player can no longer move.

    IEnumerator TeleportProcess()
    {
        //here we actually start teleporting.
        //the player becomes untouchable.


        isTeleporting = true;


        PlayerHandler.instance._playerController.block.AddBlock("Teleport", BlockClass.BlockType.Complete);
        
        StartCoroutine(ThunderProcess());

        //then we raise the window, and whents taht done we call
        yield return StartCoroutine(GameHandler.instance.LowerCurtainProcess_Teleport(2));

        PlayerHandler.instance.ControlGraphicHolderVisibility(true);

        //here we actually take the fella back.
        TeleportToPlace();

        StartCoroutine(GameHandler.instance.RaiseCurtainProcess_Teleport(2));
        yield return new WaitForSeconds(1);

        PlayerHandler.instance._playerController.block.RemoveBlock("Teleport");


        //then we lower it.

        CallCooldown();

        if (connectedTeleporter != null)
        {
            connectedTeleporter.CallTimer();
            connectedTeleporter.CallEvents();
            connectedTeleporter.CallLock();
        }
        
        if(merchantPortal != null)
        {
            //we call on itself.
            CallTimer();
        }

      

        if (lockAllEnemies)
        {
            PlayerHandler.instance._entityEvents.OnLockPortals(false);
        }

        isAlreadyCalled = false;
        isTeleporting = false;
    }

    //make the hunder be instant.
    //turn off the graphicholder
    //

    IEnumerator ThunderProcess()
    {
        GameHandler.instance._soundHandler.CreateSfx_WithAudioClip(thunder_AudioClip);
        thunderEffectHolder.gameObject.SetActive(true);
        PlayerHandler.instance.ControlGraphicHolderVisibility(false);
        yield return new WaitForSeconds(0.2f);

        thunderEffectHolder.gameObject.SetActive(false);
        //then we need to turn off the player
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
            if(timer_Current <= 5)
            {
                GameHandler.instance._soundHandler.CreateSfx(SoundType.AudioClip_OutOfTimeTimer);
            }
            timer_Current -= 1;
            UIHandler.instance._playerUI.UpdateTimerForTeleport(timer_Current);
            yield return new WaitForSeconds(1.15f);
        }

        //we get here and inform something.
        //force any dialogue to end
        UIHandler.instance._DialogueUI.CloseDialogue();

        if (merchantPortal)
        {
            //teleport to this place.
            //i want to 

            merchantPortal.TeleportBack();
            yield break;
        }

        

        Debug.Log("got here");
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
        if (hasBeenTeleportedTo) return;

        isPlayerInside = true;
        StartChargeAudio();
        soonToTeleportEffectHolder.gameObject.SetActive(true);

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag != "Player") return;

        isPlayerInside = false;
        hasBeenTeleportedTo = false;
        timeToTeleport_Current = 0;
        StopChargeAudio();
        soonToTeleportEffectHolder.gameObject.SetActive(false);
    }
    #endregion

}
