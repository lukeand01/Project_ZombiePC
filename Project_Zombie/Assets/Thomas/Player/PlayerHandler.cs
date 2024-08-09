using MyBox;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHandler : MonoBehaviour
{
    public static PlayerHandler instance {  get; private set; }
    public PlayerMovement _playerMovement {  get; private set; }
    public PlayerController _playerController { get; private set; }
    public PlayerCombat _playerCombat { get; private set; }

    public PlayerInventory _playerInventory { get; private set; }

    public PlayerResources _playerResources { get; private set; }

    public PlayerAbility _playerAbility { get; private set; }

    public PlayerParty _playerParty { get; private set; }

    public PlayerStatTracker _playerStatTracker {  get; private set; }

    public PlayerCamera _playerCamera {  get; private set; }

    public Rigidbody _rb {  get; private set; }
    
    public EntityEvents _entityEvents {  get; private set; }
    public EntityStat _entityStat { get; private set; }

    public Camera _cam { get; private set; }

    [Separator("BODY PARTS REF")]
    [SerializeField] GameObject graphicHolder;
    [SerializeField] Transform _head;
    [field:SerializeField]public Transform[] eyeArray { get; private set; }


    [SerializeField] DropData _dropData;

    private void FixedUpdate()
    {
        CallDebug();
    }

    public void CallDebug()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            _dropData.CallDrop();
        }
    }


    public void ControlGraphicHolderVisibility(bool isVisible)
    {
        graphicHolder.SetActive(isVisible);
    }


    //the chance for the
    //[Separator("REF HOLDERS")]
    //[SerializeField] ItemTierHolder itemHolderGun;
    //[SerializeField] ItemTierHolder itemHolderResource;
    //[SerializeField] AbilityTierHolder abilityHolder;




    //how should the ability work?
    //maybe actually what we can do is that the gun 

    //int he armory you see all the guns you can have, but unless you have already seen those guns they will remain a mystery.
    
    //from where i get the gun?
    //guns should have difference spawn chances.
    //


    private void Update()
    {

        if(_cam == null)
        {
            _cam = Camera.main;
        }

     
        


    }

    private void Start()
    {
        //but i need to 7update as the it happens.
        _entityEvents.eventUpdateStat += UpdateStatUI;




    }

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }


        DontDestroyOnLoad(gameObject);


        _playerController = GetComponent<PlayerController>();
        _playerMovement = GetComponent<PlayerMovement>();
        _playerInventory = GetComponent<PlayerInventory>();
        _playerCombat = GetComponent<PlayerCombat>();
        _playerResources = GetComponent<PlayerResources>();
        _playerAbility = GetComponent<PlayerAbility>();
        _playerStatTracker = GetComponent<PlayerStatTracker>();
        _playerParty = GetComponent<PlayerParty>();
        _playerCamera = GetComponent<PlayerCamera>();

        _entityEvents = GetComponent<EntityEvents>();
        _entityStat = GetComponent<EntityStat>();

        _rb = GetComponent<Rigidbody>();

       
        //layerForBuilding |= (1 << )
    }

    public void ResetPlayer()
    {
        //inform the gun to drop all guns
        //inform the stathandler to remove all bds
        //reset the passives.


        _entityStat.ResetEntityStat();
        _playerAbility.ResetPassiveAbilities();
        _playerCombat.ResetPlayerCombat();
        _playerStatTracker.ResetStatTracker();
        _playerResources.ResetPlayerResource();
        _playerCamera.ResetPlayerCamera();
        _playerMovement.ResetPlayerMovement();
        //also in the end we need to make sure that it always return tot eh base

    }

    void UpdateStatUI(StatType stat, float value)
    {
        UIHandler.instance._pauseUI.UpdateStat(stat, value);
    }


    #region ESPECIAL CONDITIONS
    //we will take care of:
    //doublepoints
    //next gunbox is free.
    //health regen a percent.
    //

    //i probably should put this in the thing.


    #endregion

    #region FLY TURRETS

    [SerializeField] List<TurretFlying> turretFlyList = new();

    public void AddTurretFly(TurretFlying _turretFly)
    {
        //in case we need to remove


        _turretFly.SetRefPoint(_head);
        _turretFly.SetUp_Ally(50, 50);
        _turretFly.SetUp();
        turretFlyList.Add(_turretFly);
    }

    public void RemoveTurretFly(string id)
    {
        for (int i = 0; i < turretFlyList.Count; i++)
        {
            if (turretFlyList[i].id == id)
            {
                turretFlyList.RemoveAt(i);
                return;
            }
        }
    }

    //need to warn to remove itself.

    #endregion



    public void TryToCallExplosionCameraEffect(Transform enemyPos, float callModifier)
    {
        float distance = Vector3.Distance(enemyPos.position, transform.position);

        if(distance <= 25)
        {
            Vector3 explosionDir = transform.position - enemyPos.position;
            explosionDir.Normalize();
            _playerCamera.CallCameraRotation(explosionDir, distance, callModifier );
        }
        else
        {
            Debug.Log("dont call explosion");
        }


    }

    public void PushPlayer(Vector3 pos, float strenght)
    {
        _rb.AddForce(pos * strenght, ForceMode.Impulse);
    }

    public void CallDeathByFalling()
    {
        //here we will get the player camera to do something.
        //we wait and then we call the screen

        StopAllCoroutines();
        StartCoroutine(DeathByFallingProcess());

    }

    IEnumerator DeathByFallingProcess()
    {
        _playerController.block.AddBlock("Falling", BlockClass.BlockType.Complete);

        _playerCamera.SetCamera(CameraPositionType.FallDeath, 2, 2);

        _rb.velocity = new Vector3(0, _rb.velocity.y, 0);


        yield return new WaitForSeconds(1.5f); //then we kill the player

        _playerResources.Die(true);

        _playerController.block.RemoveBlock("Falling");
    }

}

public enum EspecialConditionType
{
    PointsModifier,
    GunBoxPriceModifier,
    GatePriceModifier

}