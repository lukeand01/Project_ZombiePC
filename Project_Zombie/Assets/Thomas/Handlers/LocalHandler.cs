using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalHandler : MonoBehaviour
{
    
    //the portals are doubling the initial and not adding the next ones.



    
    public static LocalHandler instance;

    LocalHandler_RoundHandler _roundHandler;

    [field: SerializeField] public StageData _stageData {  get; private set; }

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

        round = 1;
        GetNewSpawnList();
        StartLocalHandler();

        UIHandler.instance.ControlUI(false);
        PlayerHandler.instance._playerController.block.RemoveBlock("City");
    }

    private void FixedUpdate()
    {
        ChestGunHandle();
        ChestResourceHandle();
        ChestAbilityHandle();
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


    [SerializeField] bool useNewRoundSystem;

    #region ROUND 
    public int round { get; private set; }

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

    //every wave is spwaned

    public List<Portal> allowedPortal { get; private set; } = new();

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

        Debug.Log("add");

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

        Debug.Log("chest resource spawn");

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

    #region CHEST ABILITY

    [Separator("CHEST ABILITY")]
    [SerializeField] ChestAbility chestAbilityTemplate;

    float chestAbilityTotal;
    [SerializeField] float chestAbilityCurrent;

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

        UIHandler.instance.debugui.UpdateDEBUGUI(chestAbilityCurrent.ToString());

        if(chestAbilityCurrent > 0)
        {
            chestAbilityCurrent -= Time.fixedDeltaTime;
        }
        else
        {

            int random = Random.Range(0, allowedPortal.Count);
            allowedPortal[random].SetNextSpawnToCarryChest(chestAbilityTemplate);

            chestAbilityCurrent = chestAbilityTotal;
        }
    }

    //we get a random allowed

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

    #region POWER
    [Separator("POWER")]
    [SerializeField] List<PowerObject> powerRefList = new();

    float powerCurrent;
    float powerTotal;


    //power has to be a bit smarter
    //i have to give ammo every x turns. every 3 turns the last enemy 
    //

    void Power_Set()
    {
        chestAbilityTotal = 35;
        chestAbilityCurrent = chestAbilityTotal;
    }

    void Power_Handle()
    {
        if (allowedPortal.Count <= 0)
        {

            return;
        }

        if (powerCurrent > 0)
        {
            powerCurrent -= Time.fixedDeltaTime;
        }
        else
        {

            int random = Random.Range(0, allowedPortal.Count);
            allowedPortal[random].SetNextSpawnToCarryChest(chestAbilityTemplate);

            powerCurrent = powerTotal;
        }
    }

    public bool ShouldSpawnAmmoPower()
    {
        return true;
    }

    //it is isnt given by the 

    #endregion

    #region DROPS



    #endregion
}



//