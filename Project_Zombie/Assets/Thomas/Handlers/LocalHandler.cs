using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalHandler : MonoBehaviour
{
    
    //the portals are doubling the initial and not adding the next ones.



    
    public static LocalHandler instance;
    [field: SerializeField] public StageData _stageData {  get; private set; }

    [SerializeField] bool debug_doNotSpawn;

    [SerializeField] Transform spawnPos;
    //we may give an ability chest with an enemy



    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);


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
        SpawnHandle();
        HandleRound();
    }

    public void StartLocalHandler()
    {
        if (roomArray.Length == 0) return;


        spawnTotal = 4; //a larger time before they start appearing.
        //spawnCurrent = spawnTotal;
        spawnCurrent = 0;


        chestResourceTotal = Random.Range(35, 60);
        chestResourceCurrent = chestResourceTotal;

        
        ChestGunSpawn(true);

        ChestAbilitySet();
        roomArray[0].OpenRoom_Room();

        PlayerHandler.instance.transform.position = spawnPos.position;

        StartCoroutine(RoundStartProcess());
    }




    #region ROUND 
    public int round { get; private set; }

    float roundCurrent;
    float roundTotal;

    bool roundHasStarted;

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
    //all we will do is decided how much should spawn in this instant. we are talking about a x amount every random time.
    //at first that will be just one fella.
    //

    Dictionary<int, EnemyData> dictionaryForEnemiesWithSpawnCap = new();
    List<EnemyChanceSpawnClass> enemyChanceList = new();

    //every wave is spwaned


    [SerializeField]List<Portal> allowedPortal = new();

    float spawnCurrent;
    float spawnTotal;

    int spawnWaveQuantity; //this version is 0

    //so every level.
    //like this i will spawn too much.
    //i still need to spawn at the right fellas.


    //


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
            spawnTotal = MyUtils.GetTimerBasedInRound(round);        
            spawnCurrent = spawnTotal;
            ChooseEnemies();
        }
    }

    void GetNewSpawnList()
    {
        if (_stageData == null) return;
        enemyChanceList = _stageData.GetCompleteSpawnList(round);
        spawnWaveQuantity = 1; //for now it will be just one.
    }

    void ChooseEnemies()
    {
        //a stage map will dictate the chances of any fellas appearing. the chances based with level.
        //we first get the list of only the fellas that we can spawn at the moment. we remove the max and min.
        //i cant spawn too much if there is too little things.

        if(enemyChanceList.Count <= 0)
        {
            return;
        }
        
        List<EnemyData> chosenEnemyList = new();
        int safeBreak = 0;

        int roll = Random.Range(0, 101);
        while(spawnWaveQuantity > chosenEnemyList.Count)
        {
            safeBreak++;
            if(safeBreak > 1000)
            {
                Debug.LogError("Safe break for choose enemies");
                break;
            }

            //check each fella against the raindom.
            int random = Random.Range(0, enemyChanceList.Count);
            EnemyChanceSpawnClass enemyChance = enemyChanceList[random];
            if(enemyChance.totalChanceSpawn >= roll)
            {
                chosenEnemyList.Add(enemyChance.data);
            }
            else
            {
                roll -= 10;
            }

        }

        SpawnChosenEnemies(chosenEnemyList);

    }

    void SpawnChosenEnemies(List<EnemyData> enemyDataList)
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

            while (!stopLoop)
            {
                int randomSpawner = Random.Range(0, allowedPortal.Count);
               stopLoop = !indexList.Contains(randomSpawner);

                safeBreake++;
                if(safeBreake > 1000)
                {
                    break;
                }
                allowedPortal[randomSpawner].Spawn(item);
                indexList.Add(randomSpawner);
            }
            
        }

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

        if (roomArray.Length == 0) return;


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
    float chestAbilityCurrent;

    void ChestAbilitySet()
    {
        chestAbilityTotal = 35;
        chestAbilityCurrent = chestAbilityTotal;
    }

    void ChestAbilityHandle()
    {
        if(allowedPortal.Count <= 0)
        {

            return;
        }

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
}
public class SpawnWaveClass
{
    //this has information regarding how many fellas we should spawn.


}



//