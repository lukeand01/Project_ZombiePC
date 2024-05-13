using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    }


    private void Update()
    {
        if (handler == null) return;
    
        if (block.HasBlock(BlockClass.BlockType.Complete))
        {

            return;
        }



        InputPause();


        if (block.HasBlock(BlockClass.BlockType.Partial)) return;

        if (handler._entityStat.isStunned)
        {
            return;
        }

        if (!block.HasBlock(BlockClass.BlockType.Combat))
        {
            InputShoot();
            InputReload();
            InputSwap();
            InputAbilityActive();
        }

        InputMovement();
        InputRotation();
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


        Vector3 targetDirection = getMouseDirection();

        if (targetDirection != Vector3.zero)
        {
            handler._playerMovement.RotatePlayer(targetDirection);
        }


    }


    void InputShoot()
    {
        if (Input.GetKey(key.GetKey(KeyType.Shoot)))
        {
            Vector3 shootDir = getMouseDirection();

            if(shootDir != Vector3.zero)
            {
                handler._playerCombat.Shoot(shootDir);
            }

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
        }
    }
    void InputSwap()
    {
        if (Input.GetKeyDown(key.GetKey(KeyType.SwapWeapon)))
        {
            handler._playerCombat.OrderSwapGun();
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
        if (Input.GetKeyDown(key.GetKey(KeyType.Ability1)))
        {
            handler._playerAbility.UseAbilityActive(0);
        }
        if (Input.GetKeyDown(key.GetKey(KeyType.Ability2)))
        {
            handler._playerAbility.UseAbilityActive(1);
        }
        if (Input.GetKeyDown(key.GetKey(KeyType.Ability3)))
        {
            handler._playerAbility.UseAbilityActive(2);
        }

    }
    Vector3 getMouseDirection()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            // Calculate direction to rotate character
            Vector3 targetDirection = (hit.point - transform.position).normalized;
            targetDirection.y = 0f; // Ignore vertical component (if needed)

            // Rotate character towards mouse position
            

            return targetDirection;
        }


        return Vector3.zero;
    }

    void InputDash()
    {
        if (Input.GetKeyDown(key.GetKey(KeyType.Dash)))
        {
            handler._playerMovement.Dash();
        }
    }

    void InputEquipWindow()
    {

        if (CityHandler.instance == null) return;

        if (Input.GetKeyDown(key.GetKey(KeyType.EquipWindow)))
        {
            UIHandler.instance.EquipWindowUI.CallUI();
        }

        

    }
}
