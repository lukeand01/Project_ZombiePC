using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalHandler : MonoBehaviour
{
    //this will be responsible for the spawning.
    //and for the chests. the references to them will be hold here.



    //spwan requires places. they must be visible for the player otherwise its a bit weird.
    //so they will be portals. they are a squire and they should do an effect when the enemy is about to be spwaned
    //they each have a cooldown where they can be used. but who gives order foir them to be used is the localhandler
    //the way it works is that there is a cooldown for a spawnwave. this spwan wave has a number of zombies it wants to spawn
    //also we need to be able to limit stronger spawns. for example there should be no more than one giant in the scene. and perpahs we would like for the giant spawn in particular to have a cooldown


    //RULES OF SPAWNING
    //must be somewhat close. most of the spawn should be reserved for the room the player currently it is.
    //the wave of spawning is 60% the player room and 40 % divided among the rooms connect to that room. if there is no rooms connected there is no division.
    //
    //

    //can we make this simple?
    //we get all the spawns that are close and accessible to the player
    //on cooldown we randomly send information to them and they store the selected fellas. 
    //they use that information when they can.
    //the choice of what kind of fella happens only here. 





    
    public static LocalHandler instance;
    [SerializeField] StageData _stageData;


    //we may give an ability chest with an enemy



    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);


    }

    private void Start()
    {
        if (_stageData == null) return;
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
        spawnTotal = 8; //a larger time before they start appearing.
        //spawnCurrent = spawnTotal;
        spawnCurrent = 0;
        round = 1;
        roundTotal = 25 * round;

        chestResourceTotal = Random.Range(35, 60);
        chestResourceCurrent = chestResourceTotal;

        roomArray[0].OpenRoom();
        ChestGunSpawn(true);

        ChestAbilitySet();
    }


    #region ROUND 
    public int round { get; private set; }

    float roundCurrent;
    float roundTotal;
  
    void HandleRound()
    {
        if(roundCurrent > 0)
        {
            roundCurrent -= Time.fixedDeltaTime;
        }
        else
        {
            ProgressStage();
            roundTotal = 25 * round;
            roundCurrent = roundTotal;
        }
    }

    public void ProgressStage()
    {
        round++;
        //get a new spawnlist.
        GetNewSpawnList();
    }

    #endregion

    #region ROOM
    [SerializeField] Room[] roomArray;
    List<Room> openRoomList = new();
    Dictionary<string, Room> openRoomDictionary = new();
    public void OpenRoom(Room room)
    {
        if (!openRoomDictionary.ContainsKey(room.id))
        {
            openRoomDictionary.Add(room.id, room);
            room.OpenRoom();
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
    [SerializeField]List<EnemyChanceSpawnClass> enemyChanceList = new();

    //every wave is spwaned


    [SerializeField]List<Portal> allowedPortal = new();

    float spawnCurrent;
    float spawnTotal;

    int spawnWaveQuantity; //this version is 0

    //so every level.
    //like this i will spawn too much.
    //i still need to spawn at the right fellas.


    void SpawnHandle()
    {
        if(spawnCurrent > 0)
        {
            spawnCurrent -= Time.fixedDeltaTime;
        }
        else
        {
            //then we spawn.
            spawnTotal = Random.Range(5, 8);         
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
            Debug.LogError("NO ALLOWED PORTAL");
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
    [SerializeField] Transform[] chestGunPosArray;

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
        chestGun.transform.localRotation = Quaternion.Euler(targetPos.localRotation.x, targetPos.localRotation.y, targetPos.localRotation.z);
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
            Debug.Log("chose one");
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