using MyBox;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LocalHandler : MonoBehaviour
{

    //the portals are doubling the initial and not adding the next ones.




    public static LocalHandler instance;

    LocalHandler_RoundHandler _roundHandler;

    [field: SerializeField] public StageData _stageData { get; private set; }

    [SerializeField] bool debug_doNotSpawn;

    [SerializeField] Transform spawnPos;
    //we may give an ability chest with an enemy





    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        _roundHandler = GetComponent<LocalHandler_RoundHandler>();



    }

    private void Start()
    {
        if (_stageData == null)
        {

            return;
        }


        RoundSpawnModifier = 0;
        spawnCap = 100;

        GameHandler.instance._pool.CompleteReset();

        round = 1;
        GetNewSpawnList();
        StartLocalHandler();

        UIHandler.instance.ControlUI(false);
        PlayerHandler.instance._playerController.block.RemoveBlock("City");


        SpawnPaidInGameObjects();
    }

    private void FixedUpdate()
    {
        ChestGunHandle();
        ChestResourceHandle();
        ChestAbilityHandle();
        ChestAmmoHandle();
        Shrine_Handle();


        if (useNewRoundSystem)
        {

        }
        else
        {
            SpawnHandle();
            HandleRound();
        }


    }

    public void StartLocalHandler()
    {
        if (roomArray.Length == 0) return;


        PlayerHandler.instance._playerAbility.ResetPassiveAbilities();
        PlayerHandler.instance._playerAbility.UpdateActiveAbility();


        spawnTotal = 4; //a larger time before they start appearing.
        spawnWaveQuantity = 1;
        //spawnCurrent = spawnTotal;
        spawnCurrent = 0;

        shrineCurrent = 0;
        shrineTotal = Random.Range(130, 200);

        chestResourceTotal = Random.Range(35, 60);
        chestResourceCurrent = chestResourceTotal;


        ChestGunSpawn(true);

        ChestAbilitySet();
        ChestAmmoSet();
        roomArray[0].OpenRoom_Room();

        PlayerHandler.instance.transform.position = spawnPos.position;
        PlayerHandler.instance._playerCombat.ControlGunHolderVisibility(true);
        UIHandler.instance._MouseUI.ControlAppear(true);

        if (useNewRoundSystem)
        {
            _roundHandler.StartRound();
        }
        else
        {
            StartCoroutine(RoundStartProcess());
        }

    }

    public void GetEnemyAndSpawninAnotherPortal(EnemyBase enemy)
    {
        //i turned off this enemybase

        enemy.gameObject.SetActive(false);
        _roundHandler.ReceiveDespawnedEnemy(enemy);
        //then i will simply pass to another portal but instead of instatiating it will simply bring the fella to the right position and make it visible

    }


    [SerializeField] bool useNewRoundSystem;

    #region ROUND 
    [field: SerializeField] public int round { get; private set; }

    float roundCurrent;
    float roundTotal;

    bool roundHasStarted;


    public void SetRound(int newRound)
    {
        round = newRound;
    }
    public void SetWaveQuantity(int newValue)
    {
        spawnWaveQuantity = newValue;
    }

    IEnumerator RoundStartProcess()
    {
        UIHandler.instance._playerUI.CloseRound();


        round = 1;
        roundTotal = MyUtils.GetTimerForRoundTotal(round);
        
        roundCurrent = 0;
        roundHasStarted = false;
        yield return new WaitForSeconds(0.5f);
        UIHandler.instance._playerUI.OpenRound();
        UIHandler.instance._playerUI.UpdateRoundBar(roundCurrent, roundTotal);
        UIHandler.instance._playerUI.UpdateRoundText(round.ToString());
        yield return new WaitForSeconds(2);
        roundHasStarted = true;
    }

    void HandleRound()
    {

        if (!roundHasStarted) return;


        if(roundTotal > roundCurrent )
        {
            roundCurrent += Time.fixedDeltaTime;
        }
        else
        {
            ProgressStage();
            roundTotal = MyUtils.GetTimerForRoundTotal(round);

            roundCurrent = 0;
        }

        UIHandler.instance._playerUI.UpdateRoundBar(roundCurrent, roundTotal);
    }

    public void ProgressStage()
    {
        round++;
        //get a new spawnlist.
        UIHandler.instance._playerUI.UpdateRoundText(round.ToString());




        GetNewSpawnList();
    }

    #endregion
    
    #region ROOM
    [SerializeField] Room[] roomArray;
    List<Room> openRoomList = new();
    Dictionary<string, Room> openRoomDictionary = new();
    public void OpenRoom_LocalHandler(Room room, string fromwhjere)
    {
        if (!openRoomDictionary.ContainsKey(room.id))
        {
            openRoomDictionary.Add(room.id, room);
            //we must also liberated
            foreach (var item in room.portalList)
            {
                allowedPortal.Add(item);
            }

        }
    }
    #endregion

    #region SPAWN SYSTEM
    //at first that will be just one fella.
    //

    //goal of spawner
    //cannot spawn more because there are more spawners. player´s should not be afraid to open door because of it.
    //there is a timer for spawn. 
    //at the start we spawn 1 fella every 3 seconds.
    //at round 2 we spawn 2 fellas every 3 seeconds.
    //we keep spawning 

    //we put these fellas right at the start. but how to check on them? everytime a fella dies we check?
    //everytime a fella is spawned and the fella has a cap we attack an event to its death so we can remove it.
    public Dictionary<string, int> dictionaryForEnemiesWithSpawnCap { get; private set; } = new();
    [field:SerializeField] public List<EnemyChanceSpawnClass> enemyChanceList { get; private set; } = new();


    //is this the problem
    //every wave is spwaned

    [field:SerializeField] public List<Portal> allowedPortal { get; private set; } = new();

    float spawnCurrent;
    float spawnTotal;
    int amountSpawned;

    int spawnWaveQuantity; //this version is 0

    //there seems to be a disconnect 

    void SpawnHandle()
    {
        if (debug_doNotSpawn) return;
        if(spawnCurrent > 0)
        {
            spawnCurrent -= Time.fixedDeltaTime;
        }
        else
        {
            //then we spawn.
            float additionalTimer = 0;

            if(amountSpawned >= 5)
            {
                amountSpawned = 0;
                additionalTimer *= spawnTotal * 2;
                additionalTimer = Mathf.Clamp(additionalTimer, 3, 5);
            }
            else
            {
                amountSpawned++;
            }


            spawnTotal = MyUtils.GetSpawnTimerBasedInRound(round) + additionalTimer;
            spawnWaveQuantity = MyUtils.GetSpawnQuantityBasedInRound(round);
            spawnCurrent = spawnTotal;
            ChooseEnemiesAndSpawn();
        }
    }

    public void GetNewSpawnList()
    {
        if (_stageData == null) return;
        enemyChanceList = _stageData.GetCompleteSpawnList(round);
        spawnWaveQuantity = 1; //for now it will be just one.
    }

    public void ChooseEnemiesAndSpawn()
    {
        //a stage map will dictate the chances of any fellas appearing. the chances based with level.
        //we first get the list of only the fellas that we can spawn at the moment. we remove the max and min.
        //i cant spawn too much if there is too little things.

        if(enemyChanceList.Count <= 0)
        {
            return;
        }
        
        //maybe we order the respawn through here.
        //
        

        List<EnemyData> chosenEnemyList = new();
        int safeBreak = 0;

        int roll = Random.Range(0, 101);
        while(spawnWaveQuantity > chosenEnemyList.Count)
        {
            safeBreak++;
            if(safeBreak > 1000)
            {
                //Debug.LogError("Safe break for choose enemies");
                SpawnChosenEnemies(chosenEnemyList);
                break;
            }

            //check each fella against the raindom.
            int random = Random.Range(0, enemyChanceList.Count);
            EnemyChanceSpawnClass enemyChance = enemyChanceList[random];

            if (!CanStackEnemy(enemyChance))
            {
                Debug.Log("cannot add more of this fella " + enemyChance.data.enemyName);
                continue;
            }
            else
            {
                //Debug.Log("can add this fella");
            }

            if(enemyChance.totalChanceSpawn >= roll)
            {
                chosenEnemyList.Add(enemyChance.data);
                AddToStackDictionary(enemyChance);
                //we check if we should put in the dictionary
            }
            else
            {
                roll -= 10;
            }

        }



        foreach (var item in chosenEnemyList)
        {
           
            _roundHandler.AddEnemy(item);
        }

        SpawnChosenEnemies(chosenEnemyList);

    }

    public void SpawnChosenEnemies(List<EnemyData> enemyDataList)
    {
        //now we need to get the right spawners.
        //the spawner
        List<int> indexList = new();



        if(allowedPortal.Count <= 0)
        {
            //Debug.LogError("NO ALLOWED PORTAL");
            return;
        }


        foreach(EnemyData item in enemyDataList)
        {
            //we choose a random fella that wans teh last.

            if(indexList.Count >= enemyDataList.Count)
            {
                Debug.Log("then we cannot spawn anymore");
                //and we reduce the cooldown of the spawns

                break;
            }

            bool stopLoop = false;
            int safeBreake = 0;

            //i will check 

            while (!stopLoop)
            {
                int randomSpawner = Random.Range(0, allowedPortal.Count);
               stopLoop = !indexList.Contains(randomSpawner);

                safeBreake++;
                if(safeBreake > 1000)
                {
                    break;
                }

                if (!stopLoop)
                {
                    continue;
                }

                
                allowedPortal[randomSpawner].OrderSpawn(item);
                indexList.Add(randomSpawner);
            }
            
        }

    }

    public void EnemyDied(EnemyData data)
    {
        //we check if the enemy was in the list and we remove it. o
        if (dictionaryForEnemiesWithSpawnCap.ContainsKey(data.name))
        {
            //if we have a key. then we must remove a number of it.
            dictionaryForEnemiesWithSpawnCap[data.name] -= 1;

            if (dictionaryForEnemiesWithSpawnCap[data.name] <= 0)
            {
                dictionaryForEnemiesWithSpawnCap.Remove(data.name);
            }
        }

    }
    public void AddToStackDictionary(EnemyChanceSpawnClass enemySpawnClass)
    {
        if (enemySpawnClass.maxAllowedAtAnyTime == 0)
        {
            return;
        }


        if (dictionaryForEnemiesWithSpawnCap.ContainsKey(enemySpawnClass.data.name))
        {
            
            dictionaryForEnemiesWithSpawnCap[enemySpawnClass.data.name] += 1;
        }
        else
        {
            dictionaryForEnemiesWithSpawnCap.Add(enemySpawnClass.data.name, 1);
        }
    }

    public bool CanStackEnemy(EnemyChanceSpawnClass enemySpawnClass)
    {
        if (!dictionaryForEnemiesWithSpawnCap.ContainsKey(enemySpawnClass.data.name)) return true;

        Debug.Log("Curren stack " + dictionaryForEnemiesWithSpawnCap[enemySpawnClass.data.name] + " yo : " + enemySpawnClass.maxAllowedAtAnyTime);

        return dictionaryForEnemiesWithSpawnCap[enemySpawnClass.data.name] < enemySpawnClass.maxAllowedAtAnyTime;
    }

    public void RemoveEnemyFromSpawnList(EnemyData data)
    {
        _roundHandler.RemoveEnemy(data);
    }

    #endregion

    #region CHEST GUN
    [Separator("CHEST GUN")]
    [SerializeField] ChestGun chestGun; //this is the only one we need. we will be teleporting this to the right places.


    float chestGunCurrent;
    float chestGunTotal;

    void ChestGunHandle()
    {
        //if the chestgun is not visible then we count the cooldown

        if (chestGun.gameObject.activeInHierarchy) return;

       if(chestGunCurrent > 0)
        {
            chestGunCurrent -= Time.fixedDeltaTime;
        }
        else
        {
            ChestGunSpawn();
            chestGunTotal = 10;
            chestGunCurrent = chestGunTotal;
        }
    }
    public void ChestGunUse()
    {
        chestGun.gameObject.SetActive(false);
        chestGunTotal = 10;
        chestGunCurrent = chestGunTotal; 
    }

    void ChestGunSpawn(bool isFirst = false)
    {

        //put in a random position it can be the same.
        int min = 0;

        if (roomArray.Length <= 1) return;


        if (isFirst)
        {
            //then we dont spawn at the first level.
            min = 1;
        }



        Transform targetPos = null;
        int safeBreak = 0;
        while(targetPos == null)
        {
            int random = Random.Range(min, roomArray.Length);
            targetPos = roomArray[random].GetChestGunSpawnPos();
            safeBreak++;

            if(safeBreak > 1000)
            {
                Debug.LogError("safe break for gun chest");
                break;
            }
        }

        chestGun.transform.position = targetPos.position;
        chestGun.transform.localRotation = Quaternion.Euler(0,-90,0);
        chestGun.gameObject.SetActive(true);


    }

    #endregion

    #region CHEST RESOURCE
    //can never have more than just 3 things.
    [Separator("CHEST RESOURCE")]
    [SerializeField] ChestResource chestResourceTemplate;
    [SerializeField] Transform[] chestResourceSpawnPosArray;
    List<int> chestResourceSpawnIndexList = new(); //these are the fellas 

    float chestResourceCurrent;
    float chestResourceTotal;

    void ChestResourceHandle()
    {
        if(chestResourceCurrent > 0)
        {
            chestResourceCurrent -= Time.fixedDeltaTime;
        }
        else
        {
            ChestResourceSpawn();
            chestResourceTotal = Random.Range(40, 60);
            chestResourceCurrent = chestResourceTotal;
        }


    }

    void ChestResourceSpawn()
    {
        //i cannot use a position that has already been used.
        //otherwise will have two chests in the same poalce.

        //

        if (chestResourceSpawnPosArray.Length == 0) return;
        if (_stageData == null) return;

        ChestResource newObject = Instantiate(chestResourceTemplate);

        Transform targetPos = null;
        int index = 0;

        while(targetPos == null)
        {
            int random = Random.Range(0, chestResourceSpawnPosArray.Length);

            if (!chestResourceSpawnIndexList.Contains(random))
            {
                targetPos = chestResourceSpawnPosArray[random];
                chestResourceSpawnIndexList.Add(random);
                index = random;
            }

        }

        newObject.transform.position = targetPos.position;

        ItemClass item = _stageData.GetItem();
        newObject.SetUp(item, index);
    }


    //this is outdated and should be replaced with the pool system.
    public void ChestResourceDestroy(int index)
    {
        if(chestResourceSpawnIndexList.Count <= 0)
        {
            Debug.LogError("TRIED TO REMOVE A CHEST BUT THERE ARE NO MORE RESOURCE CHESTS"); 
            return;
        }

        if (!chestResourceSpawnIndexList.Contains(index))
        {
            return;
        }



        for (int i = 0; i < chestResourceSpawnIndexList.Count; i++)
        {          
            if(index == chestResourceSpawnIndexList[i])
            {
                Debug.Log("this was called");
                chestResourceSpawnIndexList.RemoveAt(i);
                return;
            }
        }

        Debug.Log("should not have arrived here. chest resource destroy " + index);
    }
    #endregion

    #region SPAWN Chest Ability, Ammo and Drop

    [Separator("CHEST ABILITY")]
    [SerializeField] ChestAbility chestAbilityTemplate;
    bool chestAbility_Ready;


    float chestAbilityTotal;
    float chestAbilityCurrent;

    //its better to check directly here than to keep in the portal.
    //or other way to get abilities.

    void ChestAbilitySet()
    {
        chestAbilityTotal = 3;
        chestAbilityCurrent = chestAbilityTotal;
    }

    void ChestAbilityHandle()
    {
        if(allowedPortal.Count <= 0)
        {

            return;
        }

        //UIHandler.instance.debugui.UpdateDEBUGUI(chestAbilityCurrent.ToString());

        if(chestAbilityCurrent > 0)
        {
            chestAbilityCurrent -= Time.fixedDeltaTime;
        }
        else
        {
            chestAbility_Ready = true;
            chestAmmoCurrent = chestAbilityTotal;
        }

        //then we check here and then for drop. io think the chest spawn system should be better.
        //because if i want to change how the spawn works this might not be as good.
        //instead, when it dies it checks the localhandler if it has anything for it. this


    }
    public ChestAbility GetChestAbility()
    {

        int roll = Random.Range(0, 101);

        if (chestAbility_Ready && roll > 92)
        {
            chestAbility_Ready = false;
            return chestAbilityTemplate;
        }
        else
        {
            return null;
        }

    }


    [Separator("CHEST AMMO")]
    [SerializeField] Chest_Ammo chest_Ammo_Template;
    bool chestAmmo_Ready;

    float chestAmmoTotal;
    float chestAmmoCurrent;

    void ChestAmmoSet()
    {
        chestAmmoTotal = 5;
        chestAmmoCurrent = 0;
    }

    void ChestAmmoHandle()
    {
        if (chestAmmoCurrent > 0)
        {
            chestAmmoCurrent -= Time.fixedDeltaTime;
        }
        else
        {

            chestAmmo_Ready = true;
            chestAmmoCurrent = chestAmmoTotal;
        }
    }

    public Chest_Ammo GetChestAmmo()
    {
        int roll = Random.Range(0, 101);

        if (chestAmmo_Ready &&  roll > 92)
        {
            chestAmmo_Ready = false;
            return chest_Ammo_Template;
        }
        else
        {
            return null;
        }
    }


    //[Separator("DROP")]

    //we get a random allowed
    //we check if we should spawn. how to decide? we roll for each fella. if none come up then we ignore it.
    //the chances are naturally low.

    #endregion

    #region SHRINE
    //shrines will spawn randomly around the map. they can appar in front of the player because they would do an animation
    //we randomly choose one out of three types of shrine and we inform the player what kind of shrine it is.
    //the quest
    [Separator("SHRINE")]
    [SerializeField] Shrine shrineTemplate;
    [SerializeField] Transform[] shrinePosArray;
    List<Shrine> shrineSpawnedList = new();
    List<int> shrineIndexList = new();
    float shrineCurrent;
    float shrineTotal;

   

    void Shrine_Handle()
    {
        if(shrineSpawnedList.Count >= 3)
        {
            shrineCurrent = 0;
            return;
        }
        if(shrinePosArray.Length == 0)
        {
            //Debug.Log("not enough in teh array");
            return;
        }

        if(shrineSpawnedList.Count >= shrinePosArray.Length)
        {
            shrineCurrent = 0;
            //Debug.Log("all positions filled");
            return;
        }

        if(shrineCurrent > shrineTotal)
        {
            Shrine_Spawn();
            shrineCurrent = 0;
        }
        else
        {
            shrineCurrent += Time.fixedDeltaTime;
        }

    }

    void Shrine_Spawn()
    {

        int roll = Random.Range(0, shrinePosArray.Length);
        int safeBreak = 0;

        while (shrineIndexList.Contains(roll))
        {
            roll = Random.Range(0, shrinePosArray.Length);

            safeBreak += 1;

            if(safeBreak > 1000)
            {
                Debug.Log("broke shrine spawn");
                return;
            }

            continue;
        }


        Shrine newObject = Instantiate(shrineTemplate);
       
        newObject.transform.position = shrinePosArray[roll].position + new Vector3(0,-6,0);
        newObject.SetIndex(shrineSpawnedList.Count);
        newObject.RaiseFromGround();
        shrineIndexList.Add(roll);
        shrineSpawnedList.Add(newObject);
        shrineTotal = Random.Range(130, 200);
    }

    public void Shrine_Remove(int index)
    {
        shrineSpawnedList.RemoveAt(index);
        shrineIndexList.RemoveAt(index);
    }


    #endregion

    #region DROPS



    #endregion

    #region ROUND TYPES LOGIC
    public bool IsBloodMoon { get; private set; }

    public void SetBloodMoonBool(bool isBloodMoon) => this.IsBloodMoon = isBloodMoon;

    public float RoundSpawnModifier { get; private set; } = 1;

    public void SetRoundSpawnModifier(float spawnModifier)
    {
        RoundSpawnModifier = spawnModifier;
    }

    public int spawnCap { get; private set; }
    public void SetSpawnCap(int spawnCap)
    {
        this.spawnCap = spawnCap;
    }

    public bool CanSpawnAnotherEnemy()
    {
        //we check the number of enemies present so we dont spawn too much accidently.
        return true;
    }


    #endregion

    #region ESPECIAL LIST
    public List<EnemyChanceSpawnClass> especialList { get; private set; } = new();
    public int preferenceForEspecialList { get; private set; }

    //everytime we are to spawn we check if there is anything in this list
    //

    public void SetEspecialList(List<EnemyChanceSpawnClass> especialList, int preferenceForEspecialList)
    {
        this.especialList = especialList;
        this.preferenceForEspecialList = preferenceForEspecialList;
    }
    public void ResetEspecialList()
    {
        especialList.Clear();
        preferenceForEspecialList = 0;
    }

    #endregion

    #region SPAWN Paid Objects (Sentry, Flying sentries, HealthFountain, AbilityBox,)

    //health fountains spawn at the start.
    //sentries spawn at start
    //flying sentries spawn at periods.
    //ability boxes 

    [Separator("PAID OBJECTS")]
    [SerializeField] HealthFountain[] _healthFountainArray;
    [SerializeField] TurretToBuy[] _turretBuyArray;
    [SerializeField] TurretToBuy_Flying[] _turretBuyFlyArray;
    [SerializeField] ChestAbility[] _chestAbilityArray;

    void SpawnPaidInGameObjects()
    {
        ResetPaidObjects();

        SpawnFountains();
        SpawnSentries();

        PlayerHandler.instance._entityEvents.eventPassedRound -= SpawnFlyingSentries;
        PlayerHandler.instance._entityEvents.eventPassedRound -= SpawnPaidAbilityBoxes;

        PlayerHandler.instance._entityEvents.eventPassedRound += SpawnFlyingSentries;
        PlayerHandler.instance._entityEvents.eventPassedRound += SpawnPaidAbilityBoxes;
    }

    void SpawnFountains()
    {
        //there are positions. we will enable a number of fountains
        List<int> indexList = new();

        int safeBreak = 0;

        int amount = Random.Range(2, 3);
        for (int i = 0; i < amount;)
        {
            safeBreak++;
            if(safeBreak > 1000)
            {
                break;
            }


            int random = Random.Range(0, _healthFountainArray.Length);

            if (!indexList.Contains(random))
            {
                _healthFountainArray[random].gameObject.SetActive(true);
                indexList.Add(random);
                i++;
            }

        }

    }

    void SpawnSentries()
    {
        List<int> indexList = new();

        int safeBreak = 0;

        int amount = Random.Range(2, 3);
        for (int i = 0; i < amount;)
        {
            safeBreak++;
            if (safeBreak > 1000)
            {
                break;
            }


            int random = Random.Range(0, _turretBuyArray.Length);

            if (!indexList.Contains(random))
            {
                _turretBuyArray[random].gameObject.SetActive(true);
                indexList.Add(random);
                i++;
            }

        }
    }

    void SpawnFlyingSentries()
    {
        //every turn we check this.

        int roll = Random.Range(0, 101);

        if (20 < roll) return;

        int quantityActive = 0;

        foreach (var item in _turretBuyFlyArray)
        {
            if (item == null) continue;

            if (item.gameObject.activeInHierarchy)
            {
                quantityActive++;
            }
        }

        if(quantityActive >= 2)
        {
            return;
        }

        //then we get a random fella to make it active.

        bool done = false;

        int safeBreak = 0;

        while (!done)
        {
            safeBreak++;

            if (safeBreak > 1000) break;

            int random = Random.Range(0, _turretBuyFlyArray.Length);

            if (_turretBuyFlyArray[random ] == null)
            {
                Debug.Log("this is the random i tried " + random);
                return;
            }

            if (!_turretBuyFlyArray[random].gameObject.activeInHierarchy)
            {
                done = true;
                _turretBuyFlyArray[random].gameObject.SetActive(true);
            }
        }


    }

    void SpawnPaidAbilityBoxes()
    {
        //every turn we check this.
        int roll = Random.Range(0, 101);


        if (30 < roll) return;



        int quantityActive = 0;

        foreach (var item in _chestAbilityArray)
        {
            if(item == null) continue;
            if (item.gameObject.activeInHierarchy)
            {
                quantityActive++;
            }
        }

        if (quantityActive >= 2)
        {
            return;
        }


        bool done = false;

        int safeBreak = 0;

        while (!done)
        {
            safeBreak++;

            if (safeBreak > 1000) break;

            int random = Random.Range(0, _chestAbilityArray.Length);

            if (_chestAbilityArray[random] == null) continue;

            if (!_chestAbilityArray[random].gameObject.activeInHierarchy)
            {
                done = true;
                _chestAbilityArray[random].gameObject.SetActive(true);
            }
        }

    }

    void ResetPaidObjects()
    {
        foreach (var item in _healthFountainArray)
        {
            item.gameObject.SetActive(false);
        }
        foreach (var item in _turretBuyArray)
        {
            item.gameObject.SetActive(false);
        }
        foreach (var item in _turretBuyFlyArray)
        {
            item.gameObject.SetActive(false);
        }
        foreach (var item in _chestAbilityArray)
        {
            item.gameObject.SetActive(false);
        }
    }

    #endregion
}




//