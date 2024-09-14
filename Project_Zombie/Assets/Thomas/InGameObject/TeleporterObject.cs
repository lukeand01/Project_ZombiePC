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
        if (hasBeenTeleportedTo) return false;
        if (isLocked) return false;
        if (IsOnCooldown) return false;
        if (connectedTeleporter.IsOnTimer) return false;

        if (PlayerHandler.instance._entityStat.isStunned) return false;

        return true;
    }

    void DetectIfPlayerShouldBeTeleported()
    {

        if (!isPlayerInside)
        {
            ControlEffect(false);
            StopChargeAudio();
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

        if(cooldownForTeleport_Current == 0)
        {
            //start playing
            StartChargeAudio();
        }

        if (timeToTeleport_Current > timeToTeleport_Total * 0.7f)
        {
            ControlEffect(true);
        }

        if (timeToTeleport_Total > timeToTeleport_Current)
        {
            timeToTeleport_Current += Time.fixedDeltaTime;

            //we start playing
            //i will tell by teleport timer actually.

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
        
        


        Debug.Log("rotating this fella");

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
            PlayerHandler.instance._entityEvents.OnLockPortals(false);
        }

        isAlreadyCalled = false;
        isTeleporting = false;
    }

    IEnumerator LowerCurtainProcess()
    {
        float duration = 1.5f;

        PlayerHandler.instance._playerController.block.AddBlock("Teleporter", BlockClass.BlockType.Complete);

        StartCoroutine(GameHandler.instance.LowerCurtainProcess_Teleport());

        //we call the thunder and make the player disappear.
        //start
        soonToTeleportEffectHolder.gameObject.SetActive(true);
        soonToTeleportEffectHolder.Play();

        yield return new WaitForSeconds(duration * 0.2f);


        thunderEffectHolder.gameObject.SetActive(true);
        PlayerHandler.instance.ControlGraphicHolderVisibility(false);
        //PlayerHandler.instance.TryToCallExplosionCameraEffect(transform, 0.2f);

        yield return new WaitForSeconds(duration * 0.8f);

        thunderEffectHolder.gameObject.SetActive(false);

        soonToTeleportEffectHolder.Stop();
        soonToTeleportEffectHolder.gameObject.SetActive(false);
    }

    IEnumerator RaiseCurtainProcess()
    {

        
        float duration = 1;

        yield return new WaitForSeconds(0.1f);


        StartCoroutine(GameHandler.instance.RaiseCurtainProcess_Teleport());

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
