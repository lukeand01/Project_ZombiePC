using MyBox;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Diagnostics;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    PlayerHandler handler;
    EntityStat stat;
    [SerializeField] GameObject graphic;
    LayerMask wallLayer;
    LayerMask flyLayer;
    LayerMask groundLayer;
    DashUnit _dashUnit;
    FlyUnit _flyUnit;
    private void Awake()
    {
        handler = GetComponent<PlayerHandler>();
        stat = GetComponent<EntityStat>();

        wallLayer |= (1 << 9);

        flyLayer |= (1 << 10);
        flyLayer |= (1 << 15);

        groundLayer |= (1 << 10);
    }

    private void Start()
    {
        _dashUnit = UIHandler.instance._AbilityUI.GetDashUnit;
        _flyUnit = UIHandler.instance._AbilityUI.GetFlyUnit;

        SetDash();



    }

    private void Update()
    {
        HandleActualRotation();
    }

    private void FixedUpdate()
    {
        HandleGiantCooldown();
        HandleDash();

        HandleGrounded();
        HandleFly();

        _dashUnit.ControlCannotUse(IsWallAhead());
    }

    public void ResetPlayerMovement()
    {
        ResetFly();

        flyUse_Total = flyUse_Initial;
        flyCooldwon_Total = flyCooldown_Initial;

    }

    #region MOVEMENT
    [SerializeField]Vector3 lastDir;
    //so a part of the 


    public void MovePlayer(Vector3 dirVector)
    {
        //the thing that disables it is the death plane
        
        if(dirVector != Vector3.zero)
        {
            lastDir = new Vector3(dirVector.x, 0, dirVector.y);
        }

        float currentSpeed = stat.GetTotalValue(StatType.Speed) * 0.3f ;

        float giantModifier = currentSpeed * currentGiantPassiveEffect;
       //if the player is ever falling he cannot move or do anything.

        float moveModifier = 1;

        Vector3 movement = new Vector3(dirVector.x, 0, dirVector.y) * (currentSpeed - giantModifier) * moveModifier;
        Vector3 fallSpeed = new Vector3(0, handler._rb.velocity.y , 0);
        handler._rb.velocity = movement + fallSpeed;
        //handler._rb.AddForce((movement + fallSpeed) * 200, ForceMode.Force);

        //maybe increase a force to create the effect.

    }

    #endregion

    #region ROTATION

    [Separator("ROTATION")]
    [SerializeField] Vector3 rotationVector;

    void HandleActualRotation()
    {
        //why do i care about rotationX
        /* THIS IS FOR ORTOGRPAHIC CAMERA
        
        Vector3 lookDirection = new Vector3(rotationDirX, 0, rotationDirZ);
        Quaternion targetRotation = Quaternion.LookRotation(lookDirection, Vector3.up);
        graphic.transform.rotation = Quaternion.RotateTowards(graphic.transform.rotation, targetRotation, Time.deltaTime * 1000);

        */

        if(rotationVector != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(rotationVector, Vector3.up);
            graphic.transform.rotation = Quaternion.RotateTowards(graphic.transform.rotation, targetRotation, Time.deltaTime * 1100);
        }
    }

    public void RotatePlayer(Vector3 dir)
    {
        rotationVector = dir;


        //rotationDirX = dir.y;
        //rotationDirZ = -dir.x;

        //Quaternion targetRotation = Quaternion.LookRotation(dir);
        //graphic.transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 1000 * Time.deltaTime);
    }

    #endregion

    #region ENEMY GIANT ESPECIAL PASSIVE

    float currentGiantPassiveEffect;

    float giantCooldownCurrent;
    float giantCooldownTotal;


    public void GiantInRange()
    {
        giantCooldownTotal = 0.5f;
        giantCooldownCurrent = giantCooldownTotal;

        currentGiantPassiveEffect = 0.6f;

        Debug.Log("giant in range");
    }
    
    void HandleGiantCooldown()
    {
        if(giantCooldownCurrent > 0)
        {
            giantCooldownCurrent -= Time.fixedDeltaTime;
        }
        else
        {
            currentGiantPassiveEffect = 0;
        }
    }


    #endregion

    #region DASH
    
    [SerializeField] Transform feet;
    [SerializeField] Transform head; //we shoot a raycast from both of these to tell if there is a wall ahead.

    [Separator("DASH")]
    [SerializeField] AudioClip audio_Dash;

    int dashTotal;
    int dashCurrent;
    float dashCooldownCurrent;
    float dashCooldownTotal;
    float dashCooldownReduction;
    float dashCooldownOriginal;


    void SetDash()
    {
        dashTotal = 1;

        dashCooldownOriginal = 1;
        dashCooldownTotal = dashCooldownOriginal;
        UIHandler.instance._AbilityUI.UpdateDashFill(dashCooldownCurrent, dashCooldownTotal);
    }



    public void Dash()
    {
        //dash in the last direction. if we hit a wall we stop earlier.
        if(lastDir == Vector3.zero)
        {
            return;
        }
        if(dashCurrent >= dashTotal)
        {

            return;
        }
        if(dashCooldownCurrent > 0)
        {

            return;
        }

       


        dashCurrent++;

        float reduction = dashCooldownOriginal * dashCooldownReduction;
        dashCooldownTotal = dashCooldownOriginal - reduction;
        dashCooldownCurrent = dashCooldownTotal;

        StartCoroutine(DashProcess());
    }

    IEnumerator DashProcess()
    {
        //a short period you cannot control the char and a short period
        //if there is a wall in front we stop it.

        GameHandler.instance._soundHandler.CreateSfx(audio_Dash);

        handler._playerController.block.AddBlock("Dash", BlockClass.BlockType.Partial);
        BDClass bdClass = new BDClass("Dash", BDType.Immune, 0);
        handler._entityStat.AddBD(bdClass);

        float startTime = Time.time;
        float dashTime = 0.08f;
        float dashSpeed = 120;


        //if this touches a wall then we call it to force it end.


        //if its side ways then we need to reduce it

        Vector3 dashSpeedDir = new Vector3(lastDir.x, 0, lastDir.z) * dashSpeed;

        if(lastDir.x != 0 && lastDir.z != 0)
        {
            dashSpeedDir *= 0.5f;
        }


        while (Time.time < startTime + dashTime && !IsWallAhead())
        {         
            Vector3 movement = dashSpeedDir;
            handler._rb.velocity = movement;
            yield return null;           
        }



        Debug.Log("ey");
        handler._rb.velocity = Vector3.zero;
        handler._playerController.block.RemoveBlock("Dash");
        handler._entityStat.RemoveBdWithID("Dash");

    }

    void CancelDash()
    {
        StopAllCoroutines();
        handler._rb.velocity = Vector3.zero;
        handler._playerController.block.RemoveBlock("Dash");
        handler._entityStat.RemoveBdWithID("Dash");
    }

    void ResetDash()
    {
        dashCurrent = 0;
    }

    void HandleDash()
    {
        if (dashCurrent <= 0) return;

        if(dashCooldownCurrent > 0)
        {
            dashCooldownCurrent -= Time.fixedDeltaTime;
            UIHandler.instance._AbilityUI.UpdateDashFill(dashCooldownCurrent, dashCooldownTotal);
        }
        else
        {
            ResetDash();
        }
    }

    public void AddDashCooldownReduction()
    {

        dashCooldownReduction = 0.4f;
    }
    public void RemoveDashCooldownReduction()
    {
        dashCooldownReduction = 0;
    }

    #endregion

    #region FLY

    [Separator("FLy")]
    [SerializeField] BoxCollider fly_Collider;
    [SerializeField] float flyUse_Initial;
    [SerializeField] float flyCooldown_Initial;

    [SerializeField]float flyUse_Current;
    float flyUse_Total;

    [SerializeField]float flyCooldown_Current;
    float flyCooldwon_Total;

    [SerializeField] bool isGrounded;


    //


    public bool hasFly { get { return flyUse_Total > 0; } }

    public void SetFlying(float flyUse, float flyCooldown)
    {
        flyUse_Total = flyUse;
        flyCooldwon_Total = flyCooldown;

        _flyUnit.gameObject.SetActive(true);
    }
    void HandleGrounded()
    {


        if (!hasFly)
        {
            isGrounded = false;
            fly_Collider.gameObject.SetActive(false);
            _flyUnit.gameObject.SetActive(false);
            return;
        }
        else
        {
            fly_Collider.gameObject.SetActive(true);
            _flyUnit.gameObject.SetActive(true);
        }



        if(flyCooldown_Current > 0)
        {
            flyCooldown_Current -= Time.fixedDeltaTime;
            _flyUnit.UpdateCooldown_Cooldown(flyCooldown_Current, flyCooldwon_Total);
            return;
        }
        _flyUnit.UpdateCooldown_Cooldown(0, flyUse_Total);

        Vector3 boxSize = fly_Collider.size;
        Vector3 boxCenter = fly_Collider.center;
        Vector3 boxWorldCenter = transform.TransformPoint(boxCenter);

        isGrounded = Physics.BoxCast(boxWorldCenter, boxSize / 2, Vector3.down, transform.rotation, 90, groundLayer);

        if(isGrounded && flyUse_Current > 0)
        {
            flyCooldown_Current = flyCooldwon_Total;
            flyUse_Current = 0;
            CancelFly();
        }

    }
    void HandleFly()
    {
        //what we do when we are flying.
        if (isGrounded) return;


        RaycastHit hit;

        bool IsDeathCollider = Physics.Raycast(transform.position, Vector3.down, out hit, 50, flyLayer);

        if (!IsDeathCollider) return;
        if (hit.collider == null) return;
        if (hit.collider.gameObject.layer == 10) return;


        if (flyUse_Total > flyUse_Current)
        {
            //then we can fly
            StartFly();
            flyUse_Current += Time.fixedDeltaTime;
            _flyUnit.UpdateCooldown_Use(flyUse_Current, flyUse_Total);
        }
        else
        {
            CancelFly();
        }

    }
    void ResetFly()
    {
        flyUse_Current = 0;
        flyCooldown_Current = 0;

        _flyUnit.gameObject.SetActive(false);
    }
    void StartFly()
    {

        handler._rb.useGravity = false;
    }
    void CancelFly()
    {

        handler._rb.useGravity = true;
    }

    #endregion

    bool IsWallAhead()
    {
        //either of those.
        bool isHeadWall = Physics.Raycast(head.position, lastDir, 2f, wallLayer);
        bool isFeetWall = Physics.Raycast(feet.position, lastDir, 2f, wallLayer);

        return isFeetWall || isHeadWall ;


    }


    private void OnCollisionEnter(Collision collision)
    {
        return;
        if (collision.collider.gameObject.layer == 9)
        {
            Debug.Log("wall");
            CancelDash();
        }
        
    }

    //because of the behavior i cant use

}
