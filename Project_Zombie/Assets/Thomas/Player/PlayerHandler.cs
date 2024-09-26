using DG.Tweening;
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
    public EntityAnimation _entityAnimation { get; private set; }
    public EntityMeshRend _entityMeshRend { get; private set; }

    public Camera _cam { get; private set; }

    [Separator("BODY PARTS REF")]
    [SerializeField] GameObject graphicHolder;
    [SerializeField] Transform _head;


    public GameObject GetGraphicHolder { get { return graphicHolder; } }

    [field:SerializeField]public Transform[] eyeArray { get; private set; }


    [SerializeField] DropData _dropData;

    private void FixedUpdate()
    {

        UIHandler.instance._playerUI.ControlBlind(_entityStat.isBlind);

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
    //maybe actually what we can do is that the gun_Perma 

    //int he armory you see all the guns you can have, but unless you have already seen those guns they will remain a mystery.
    
    //from where i get the gun_Perma?
    //guns should have difference spawn chances.
    //


    private void Update()
    {

        if(_cam == null)
        {
            _cam = Camera.main;
        }



        if (Input.GetKeyDown(KeyCode.U))
        {
            DebugGhostBD();
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            DebugSummonGhostOrb();
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

        _cam = Camera.main;

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
        _entityAnimation = GetComponent<EntityAnimation>();
        _entityMeshRend = GetComponent<EntityMeshRend>();

        _rb = GetComponent<Rigidbody>();


        _entityAnimation.SetAnimationID("Player");

        DOTween.Init();

        //layerForBuilding |= (1 << )
    }

    public void ResetPlayer()
    {
        //inform the gun_Perma to drop all guns
        //inform the stathandler to remove all bds
        //reset the passives.


        _entityStat.ResetEntityStat();
        _playerAbility.ResetPassiveAbilities();
        _playerCombat.ResetPlayerCombat();
        _playerStatTracker.ResetStatTracker();
        _playerResources.ResetPlayerResource();       
        _playerMovement.ResetPlayerMovement();
        _entityAnimation.ResetPlayerAnimation();
        _playerCamera.ResetPlayerCamera();
        //also in the end we need to make sure that it always return tot eh base


        abilityRoll_Cost = 0;
    }


    public void SetPlayerOutOfCity()
    {
        _entityAnimation.ControlWeight(2, 1);

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
        


    }

    public void PushPlayer(Vector3 dir, float strenght)
    {
        if (_playerResources.isDead) return;
        _rb.AddForce(dir * strenght, ForceMode.Impulse);
    }


    #region COLOR FOR DAMAGE

    [Separator("COLOR FOR DAMAGE")]
    [SerializeField] Color color_Physical;
    [SerializeField] Color color_Magical;
    [SerializeField] Color color_Plasma;
    [SerializeField] Color color_Corrupt;
    [SerializeField] Color color_Pure;
    public Color GetColorForDamageType(DamageType _type)
    {
        switch (_type)
        {
            case DamageType.Physical:
                return color_Physical;
            case DamageType.Magical:
                return color_Magical;
            case DamageType.Plasma:
                return color_Plasma;
            case DamageType.Corrupt:
                return color_Corrupt;
            case DamageType.Pure:
                return color_Pure;

        }

        return Color.black;
    }

    #endregion

    #region COLOR FOR ABILITY TYPE
    [Separator("COLOR FOR ABILITY TPE")]
    [SerializeField] Color color_BoneOfDeath;
    [SerializeField] Color color_SoulOfAnger;
    [SerializeField] Color color_EyeOfWisdom;

    public Color GetColorForAbilityCoinType(AbilityCoinType _type)
    {
        switch (_type)
        {
            case AbilityCoinType.Bone_Of_Death:
                return color_BoneOfDeath;
            case AbilityCoinType.Soul_Of_Anger:
                return color_SoulOfAnger;
            case AbilityCoinType.Eye_Of_Wisdom:
                return color_EyeOfWisdom;
        }

        return Color.black;
    }
    #endregion


    #region COST FOR ABILITY ROLL
    public int abilityRoll_Cost { get; private set; } = 0;

    public void AbilityRoll_Add()
    {
        abilityRoll_Cost += 1;
    }


    #endregion


    [ContextMenu("GHOST BD")]
    public void DebugGhostBD()
    {
        BDClass bd_Slow = new BDClass("Ghost_Slow", StatType.Speed, 0, -0.2f, 0);
        bd_Slow.MakeShowInUI();
        bd_Slow.MakeTemp(2);
        bd_Slow.MakeStack(5, true);
        _entityStat.AddBD(bd_Slow);

        return;
        BDClass bd_Blind = new BDClass("Ghost_Blind", BDType.Blind, 5);
        bd_Slow.MakeShowInUI();
        bd_Slow.MakeTemp(5);
        bd_Slow.MakeStack(0, true);
        _entityStat.AddBD(bd_Blind);
    }

    public void DebugSummonGhostOrb()
    {
        AreaDamage areaDamage = GameHandler.instance._pool.GetAreaDamage(transform.transform);
        Vector3 playerPosition = transform.position;
        DamageClass newDamage = new DamageClass(50, DamageType.Physical, 0);
        areaDamage.SetUp_Continuously(playerPosition, 5, 3, 5, newDamage, 3, 1, AreaDamageVSXType.Meteor);
        areaDamage.MakeDelayShowInUI();
    }
}

public enum EspecialConditionType
{
    PointsModifier,
    GunBoxPriceModifier,
    GatePriceModifier

}