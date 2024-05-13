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


    private void Awake()
    {
        handler = GetComponent<PlayerHandler>();
        stat = GetComponent<EntityStat>();

        wallLayer |= (1 << 9);

       
    }

    private void Start()
    {
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
    }


    #region MOVEMENT
    Vector3 lastDir;

    public void MovePlayer(Vector3 dirVector)
    {

        if(dirVector != Vector3.zero)
        {
            lastDir = dirVector;
        }

        float currentSpeed = stat.GetTotalValue(StatType.Speed);
        float giantModifier = currentSpeed * currentGiantPassiveEffect;




        float moveModifier = 1;

        Vector3 movement = new Vector3(dirVector.x, 0, dirVector.y) * (currentSpeed - giantModifier) * moveModifier;

        handler._rb.velocity = movement;
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

        Quaternion targetRotation = Quaternion.LookRotation(rotationVector, Vector3.up);
        graphic.transform.rotation = Quaternion.RotateTowards(graphic.transform.rotation, targetRotation, Time.deltaTime * 700);


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
        Debug.Log("giant in range");
        giantCooldownTotal = 0.5f;
        giantCooldownCurrent = giantCooldownTotal;

        currentGiantPassiveEffect = 0.9f;
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

    int dashTotal;
    int dashCurrent;
    float dashCooldownCurrent;
    float dashCooldownTotal;
    float dashCooldownReduction;
    float dashCooldownOriginal;


    void SetDash()
    {
        dashTotal = 1;

        dashCooldownOriginal = 4;
        dashCooldownTotal = dashCooldownOriginal;
        UIHandler.instance.AbilityUI.UpdateDashFill(dashCooldownCurrent, dashCooldownTotal);
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


        handler._playerController.block.AddBlock("Dash", BlockClass.BlockType.Partial);
        BDClass bdClass = new BDClass("Dash", BDType.Immune, 0);
        handler._entityStat.AddBD(bdClass);

        float startTime = Time.time;
        float dashTime = 0.2f;
        float dashSpeed = 45;



        while (Time.time < startTime + dashTime && !IsWallAhead())
        {
            Vector3 movement = new Vector3(lastDir.x, 0, lastDir.y) * dashSpeed;
            handler._rb.velocity = movement;

            yield return null;           
        }

        handler._playerController.block.RemoveBlock("Dash");
        handler._entityStat.RemoveBdWithID("Dash");

        yield return null;
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
            UIHandler.instance.AbilityUI.UpdateDashFill(dashCooldownCurrent, dashCooldownTotal);
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


    bool IsWallAhead()
    {
        //either of those.
        bool isHeadWall = Physics.Raycast(head.position, lastDir, 1.5f, wallLayer);
        bool isFeetWall = Physics.Raycast(feet.position, lastDir, 1.5f, wallLayer);



        return isFeetWall || isHeadWall ;


    }
}
