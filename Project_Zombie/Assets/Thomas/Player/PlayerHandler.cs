using MyBox;
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

    public PlayerStatTracker _playerStatTracker {  get; private set; }

    public Rigidbody _rb {  get; private set; }
    
    public EntityEvents _entityEvents {  get; private set; }
    public EntityStat _entityStat { get; private set; }

    public Camera _cam { get; private set; }

    public int playerGunRollLevel { get; private set; }
    public int playerAbilityRollLevel {  get; private set; }


    [Separator("REF HOLDERS")]
    [SerializeField] ItemTierHolder itemHolderGun;
    [SerializeField] ItemTierHolder itemHolderResource;
    [SerializeField] AbilityTierHolder abilityHolder;

    [Separator("REF HOLDER FOR CITY")]
    [SerializeField] CityData hqHolder; //what if i require more than just level. such as quests or other places in certain levels


    LayerMask layerForBuilding;


    private void Update()
    {

        if(_cam == null)
        {
            _cam = Camera.main;
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            _playerResources.GainPoints(100);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            _playerResources.SpendPoints(100);
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            _playerAbility.ClearPassiveList();
        }
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            _playerAbility.DebugIncreaseLevel();
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            BDClass bd = new BDClass("Debug", StatType.Damage, 0.5f, 0, 0);
            bd.MakeTemp(15);
            _entityStat.AddBD(bd);

        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            BDClass bd = new BDClass("DebugSpeeed", StatType.Speed, 0f, -0.5f, 0);
            bd.MakeTemp(15);
            _entityStat.AddBD(bd);

        }


    }

    private void Start()
    {
        //but i need to 7update as the it happens.
        _entityEvents.eventUpdateStat += UpdateStatUI;



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
        //also in the end we need to make sure that it always return tot eh base

    }

    void UpdateStatUI(StatType stat, float value)
    {
        UIHandler.instance._pauseUI.UpdateStat(stat, value);
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

        itemHolderGun.ResetAllDivisions();
        itemHolderResource.ResetAllDivisions();
        abilityHolder.SetHolder();

        _playerController = GetComponent<PlayerController>();
        _playerMovement = GetComponent<PlayerMovement>();
        _playerInventory = GetComponent<PlayerInventory>();
        _playerCombat = GetComponent<PlayerCombat>();
        _playerResources = GetComponent<PlayerResources>();
        _playerAbility = GetComponent<PlayerAbility>();
        _playerStatTracker = GetComponent<PlayerStatTracker>();

        _entityEvents = GetComponent<EntityEvents>();
        _entityStat = GetComponent<EntityStat>();

        _rb = GetComponent<Rigidbody>();


        SetPlayerAbilityRollLevel(1);
        SetPlayerGunRollLevel(1);
       
        //layerForBuilding |= (1 << )
    }

    #region GETTING ITENS AND CHANCE LISTS
    public void SetPlayerAbilityRollLevel(int newValue)
    {
        playerAbilityRollLevel = newValue;
        abilityHolder.GenerateNewChanceListBasedInLevel(newValue);
        
    }
    public void SetPlayerGunRollLevel(int newValue)
    {
        playerGunRollLevel = newValue;
        itemHolderGun.GenerateNewChanceListBasedInLevel(newValue);
    }

    public List<ItemData> GetGunSpinningList()
    {
        return itemHolderGun.GetRandomListWithAmount(8);
    }
    public ItemData GetGunChosen()
    {
        return itemHolderGun.GetChosenItem();
    }

    public List<AbilityPassiveData> GetPassiveList()
    {
        return abilityHolder.GetPassiveChosenList(3);
    }



    #endregion


    public bool IsHoveringEmptySpace()
    {

        return false;
    }







}
