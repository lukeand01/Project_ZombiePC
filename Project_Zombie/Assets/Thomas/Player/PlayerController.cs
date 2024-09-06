using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    PlayerHandler handler;
    public BlockClass block { get; private set; }
    public KeyClass key { get; private set; }

    private void Awake()
    {
        handler = GetComponent<PlayerHandler>();

        block = new BlockClass();
        key = new KeyClass();

        layerForMouseHover_Enemy |= (1 << 6);
        layerForMouseHover_Ground |= (1 << 11);


    }

    private void Start()
    {
        //instead we will update thing only if necessary.so for now it will be done in those two sides.
        SetKeyBasedInSettings();

    }

    public void SetKeyBasedInSettings()
    {
        SettingsData settingData = GameHandler.instance._settingsData;
        key.CreateNewDictionaryFromSettings(settingData.keyList);
    }
    bool checkIfHoldingAbilityAfterPause;

    private void Update()
    {

        if (handler == null) return;
       


        if(Time.timeScale == 0)
        {
            checkIfHoldingAbilityAfterPause = true; 
        }

        if(checkIfHoldingAbilityAfterPause && Time.timeScale != 0)
        {
            if (!Input.GetKey(key.GetKey(KeyType.Ability1)))
            {
                handler._playerAbility.StopChargeAbilityActive(0);
            }
            if (!Input.GetKey(key.GetKey(KeyType.Ability2)))
            {
                handler._playerAbility.StopChargeAbilityActive(1);
            }
            if (!Input.GetKey(key.GetKey(KeyType.Ability3)))
            {
                handler._playerAbility.StopChargeAbilityActive(2);
            }

            checkIfHoldingAbilityAfterPause = false;
        }

        if (handler == null) return;




        if(handler._rb.velocity.y < -8)
        {
            return;
        }
        

        if (block.HasBlock(BlockClass.BlockType.Complete))
        {

            return;
        }



        InputPause();


        if (block.HasBlock(BlockClass.BlockType.Partial))
        {

            return;
        }

        if (handler._entityStat.isStunned)
        {
            return;
        }

        if (block.HasBlock(BlockClass.BlockType.OnlyCharge))
        {
            InputAbilityActive_Charge();
            return;
        }

        if (!block.HasBlock(BlockClass.BlockType.Combat))
        {                    
           InputAbilityActive_Charge();
           InputShoot();
           InputReload();
           InputSwap();
           InputAbilityActive();

        }
        

        InputMovement();

        if (!block.HasBlock(BlockClass.BlockType.Rotation))
        {
            InputRotation();
        }
        else
        {
            Debug.Log("has rotation block");
        }

        InputDash();
        InputInteract();
        InputEquipWindow();


    }


    void InputMovement()
    {
        Vector3 dir = Vector3.zero;

        if (Input.GetKey(key.GetKey(KeyType.MoveLeft)))
        {
            dir += Vector3.right;
        }
        if (Input.GetKey(key.GetKey(KeyType.MoveRight)))
        {
            dir += Vector3.left;
        }
        if (Input.GetKey(key.GetKey(KeyType.MoveDown)))
        {
            dir += Vector3.up;
        }
        if (Input.GetKey(key.GetKey(KeyType.MoveUp)))
        {
            dir += Vector3.down;
        }

        if(dir != Vector3.zero)
        {
            handler._entityEvents.OnHardInput();
        }

        handler._playerMovement.MovePlayer(dir);
    }


    Vector3 mouseDir;


    [SerializeField] Vector3 debugMousePos;
    [SerializeField] Vector3 debugDir;
    [SerializeField] GameObject debugGrpahic;
    void InputRotation()
    {
        /* FOR ORTOGRAPHIC VIEW
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        debugMousePos = mousePos;
        Vector3 dir = (mousePos - transform.position).normalized;
        debugDir = dir;
        handler._playerMovement.RotatePlayer(dir);
        mouseDir = dir;
        */


        Vector3 targetDirection = GetMouseDirection();

        if (targetDirection != Vector3.zero)
        {
            handler._playerMovement.RotatePlayer(targetDirection);
        }


    }


    void InputShoot()
    {
        if (Input.GetKey(key.GetKey(KeyType.Shoot)))
        {
            Vector3 shootDir = GetMouseDirection();

            if(shootDir != Vector3.zero)
            {
                handler._playerCombat.Shoot(shootDir);
            }

            handler._entityEvents.OnHardInput();
        }
        else
        {
            handler._playerCombat.ResetHoldShoot();
        }

        

    }

    void InputReload()
    {
        if (Input.GetKeyDown(key.GetKey(KeyType.Reload)))
        {
            handler._playerCombat.Reload();
            handler._entityEvents.OnHardInput();
        }
    }
    void InputSwap()
    {
        if (Input.GetKeyDown(key.GetKey(KeyType.SwapWeapon_1)))
        {
            handler._playerCombat.OrderSwapGun(1);
            handler._entityEvents.OnHardInput();
        }
        if (Input.GetKeyDown(key.GetKey(KeyType.SwapWeapon_2)))
        {
            handler._playerCombat.OrderSwapGun(2);
            handler._entityEvents.OnHardInput();
        }
    }

    void InputInteract()
    {
        if (Input.GetKeyDown(key.GetKey(KeyType.Interact)))
        {
            handler._playerInventory.InteractWithCurrentInteractable();
            
        }
    }

    void InputPause()
    {
        //
        if (Input.GetKeyDown(key.GetKey(KeyType.Pause)))
        {
           if(UIHandler.instance != null)
            {
                UIHandler.instance._pauseUI.CallPause();
            }
        }
    }

    void InputAbilityActive()
    {
        //perhaps we need to check only if its holding.
        if (Input.GetKeyDown(key.GetKey(KeyType.Ability1)))
        {
            handler._playerAbility.UseAbilityActive(0);
            handler._entityEvents.OnHardInput();
        }
        
        if (Input.GetKeyDown(key.GetKey(KeyType.Ability2)))
        {
            handler._playerAbility.UseAbilityActive(1);
            handler._entityEvents.OnHardInput();
        }
        



        if (Input.GetKeyDown(key.GetKey(KeyType.Ability3)))
        {
            handler._playerAbility.UseAbilityActive(2);
            handler._entityEvents.OnHardInput();
        }
        

    }

    //after pause we need to reconect it to the thing.
    void InputAbilityActive_Charge()
    {
        if (Input.GetKey(key.GetKey(KeyType.Ability1)))
        {
            handler._playerAbility.StartChargeAbilityActive(0);
        }
        
        if (Input.GetKeyUp(key.GetKey(KeyType.Ability1)))
        {
            handler._playerAbility.StopChargeAbilityActive(0);
        }


        if (Input.GetKey(key.GetKey(KeyType.Ability2)))
        {
            handler._playerAbility.StartChargeAbilityActive(1);
        }
        if (Input.GetKeyUp(key.GetKey(KeyType.Ability2)))
        {
            handler._playerAbility.StopChargeAbilityActive(1);
        }


        if (Input.GetKey(key.GetKey(KeyType.Ability3)))
        {
            handler._playerAbility.StartChargeAbilityActive(2);
        }
        if (Input.GetKeyUp(key.GetKey(KeyType.Ability3)))
        {
            handler._playerAbility.StopChargeAbilityActive(2);
        }
    }


    LayerMask layerForMouseHover_Enemy;
    LayerMask layerForMouseHover_Ground;
    [SerializeField] float debugAngleMouse;
    public Vector3 GetMouseDirection()
    {
        
        if (handler._cam == null)
        {

            return Vector3.zero;
        }

        

        Ray ray = handler._cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerForMouseHover_Enemy))
        {
            //actually i dont want hit.point i want the entity itself.

            // Calculate direction to rotate character
            Vector3 targetDirection = (hit.collider.transform.position - transform.position).normalized;
            targetDirection.y = 0f; // Ignore vertical component (if needed)

            // Rotate character towards mouse position         
            return targetDirection;
        }
        //and another that only hits the map and gives a point in the map.
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerForMouseHover_Ground))
        {
  

            //Vector3 offset = new Vector3(0,0,0);

            float angle = Vector3.SignedAngle(hit.point, transform.transform.forward, Vector3.up);
            if (angle < 0) angle += 360;


            debugAngleMouse = angle;
            //this is the angle


            Vector3 targetDirection = (hit.point - transform.position.normalized);
            targetDirection.y = 0f; 

            


            //here we will add the offsets.
            //to the right we add a bit to the offset
            //to the left we subtract a bit to the offset



            return targetDirection;
        }


        return Vector3.zero;
    }

    void InputDash()
    {
        if (Input.GetKeyDown(key.GetKey(KeyType.Dash)))
        {
            handler._playerMovement.Dash();
            handler._entityEvents.OnHardInput();
        }
    }

    void InputEquipWindow()
    {


        if (CityHandler.instance == null) return;


        if (Input.GetKeyDown(key.GetKey(KeyType.EquipWindow)))
        {
            UIHandler.instance._EquipWindowUI.CallUI();
        }

        

    }
}
