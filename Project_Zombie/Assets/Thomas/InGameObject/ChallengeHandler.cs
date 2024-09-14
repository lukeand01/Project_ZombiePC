using MyBox;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using Unity.VisualScripting;
using UnityEngine;

public class ChallengeHandler : MonoBehaviour, IInteractable
{
    //this will take doors
    //and portals.
    //it will trigger for everyone to stop spawning, then tell these reference portal to start spawning his stuff.
    //then hte game simply resumes. the enemies outside need to wait outside.
    //this needs to take different challenges here.

    [SerializeField] Room _room;
    Door[] _doorArray;
    List<Portal> _portalList = new();
    [SerializeField] ChallengeComponent _challengeComponent;
    [SerializeField] InteractCanvas _interactCanvas;



    string id;

    bool isRunning;

    private void Awake()
    {
        id = MyUtils.GetRandomID();
    }

    private void Start()
    {
        _doorArray = _room.GetDoorArray();
        _portalList = _room.GetPortalList();


    }
    private void Update()
    {
        if (isRunning)
        {
            _challengeComponent.HandleChallengeComponent();
            HandleSpawn();
        }
    }

    [ContextMenu("START CHALLENGE")]
    public void StartChallenge()
    {
        
        UpdateEnemyArray();
        UpdateEnemyStackDictionary();
        UpdateEnemyValues();

        PlayerHandler.instance._entityEvents.OnLockEnemies(true);


        //close all doors
        for (int i = 0; i < _doorArray.Length; i++)
        {
            var item = _doorArray[i];
            item.CloseDoor_Challenge();
        }

        PlayerHandler.instance._entityEvents.OnLockPortals(true);


        _challengeComponent.StartChallengeComponent();

        isRunning = true;
        //then we trigger teh respective stuff.

    }
    public void WinChallenge()
    {
       
        EndChallenge();
    }
    public void LoseChallenge()
    {
        EndChallenge();
    }

    void EndChallenge()
    {
        _challengeComponent.GiveReward();
        _challengeComponent.StopChallengeComponent();


        for (int i = 0; i < _doorArray.Length; i++)
        {
            var item = _doorArray[i];
            item.OpenDoor_Challenge();
            
        }

        for (int i = 0; i < _portalList.Count; i++)
        {
            var item = _portalList[i];
            item.RemoveChallenge();
        }

        isRunning = false;

        PlayerHandler.instance._entityEvents.OnLockEnemies(false);
        PlayerHandler.instance._entityEvents.OnLockPortals(false);
    }

    


  


    #region ENEMY SPAWNING

    [SerializeField] EnemyChanceSpawnClass[] _enemySpawnChanceArray;
    List<EnemyChanceSpawnClass> _updatedEnemyList = new();
    Dictionary<string, int> _enemyStackDictionary = new(); //

    int amountPerInterval;

    float intervalTimer_Total;
    float intervalTimer_Current;

    [Separator("DEBUG")]
    [SerializeField] List<EnemyData> lastChosenListDebug = new();

    void UpdateEnemyValues()
    {
        int round = LocalHandler.instance.round;
        amountPerInterval = MyUtils.GetAmountToSpawnPerInterval(round, 1);
        intervalTimer_Total = MyUtils.GetIntervalBetweenSpawns(round);

        intervalTimer_Current = 0;

        //we will update the lsit as well.
        //we need to reduce the list

        _updatedEnemyList = GetUpdatedEnemyList();

    }

    List<EnemyChanceSpawnClass> GetUpdatedEnemyList()
    {
        List<EnemyChanceSpawnClass> newList = new();

        for (int i = 0; i < _enemySpawnChanceArray.Length; i++)
        {
            var item = _enemySpawnChanceArray[i];

            if(item.GetChanceToSpawn() > 0)
            {
                newList.Add(item);
            }

        }

        return newList;
    }

    protected void SpawnWave()
    {
        //we send one wave of list
        //we choose the fellas here. and send them to the random portals in the room.
        //

        if(_updatedEnemyList.Count <= 0)
        {

            Debug.Log("no updated list");
            return;
        }

        List<EnemyData> enemyList = new();

        //this will help with variety of portals.

        int safeBreak = 0;

        while(amountPerInterval > enemyList.Count )
        {
            safeBreak++;
            if(safeBreak > 1000)
            {
                Debug.Log("safe break for spawn wave");
                return;
            }

           
            int random = Random.Range(0, _updatedEnemyList.Count);
            EnemyChanceSpawnClass enemyChanceClass = _updatedEnemyList[random];

            if (!CanStack(enemyChanceClass))
            {
                return;
            }


            int roll = Random.Range(0, 101);

            if(enemyChanceClass.GetChanceToSpawn() > roll)
            {
                enemyList.Add(enemyChanceClass.data);
            }

        }

        lastChosenListDebug = enemyList;

        SendDataToPortals(enemyList);

    }
    protected void HandleSpawn()
    {
        //we decide how long tilla a spawn happens.
        if(_enemySpawnChanceArray.Length <= 0)
        {
            return;
        }

        if(intervalTimer_Current > intervalTimer_Total)
        {
            SpawnWave();
            intervalTimer_Current = 0;
        }
        else
        {
            intervalTimer_Current += Time.deltaTime;
        }

    }
    void UpdateEnemyArray()
    {
        //thios always come first and we just give the round for it

        int round = LocalHandler.instance.round;

        for (int i = 0; i < _enemySpawnChanceArray.Length; i++)
        {
            var item = _enemySpawnChanceArray[i];

            item.UpdateRound(round);
        }

    }

    void UpdateEnemyStackDictionary()
    {
        _enemyStackDictionary.Clear();

        for (int i = 0; i < _updatedEnemyList.Count; i++)
        {
            var item = _updatedEnemyList[i];

            if (item.GetMaxAllowedToSpawn() > 0)
            {
                //then we add here. otherwise we dont. simple.
                _enemyStackDictionary.Add(item.data.name, 0);
            }
        }


    }

    bool CanStack(EnemyChanceSpawnClass enemySpawnClass)
    {
        if (!_enemyStackDictionary.ContainsKey(enemySpawnClass.data.name))
        {
            //Debug.Log("not contains");
            return true;
        }


        return _enemyStackDictionary[enemySpawnClass.data.name] < enemySpawnClass.GetMaxAllowedToSpawn();
    }

    public void SendDataToPortals(List<EnemyData> enemyList)
    {



        int lastPortalindex = -1;

        int safeBreak = 0;

        for (int i = 0; i < enemyList.Count;)
        {
            safeBreak++;
            if(safeBreak > 1000)
            {
                Debug.Log("safe break");
                return;
            }


            var item = enemyList[i];

            int random = Random.Range(0, _portalList.Count);

            if(random == lastPortalindex)
            {
                Debug.Log("the same as random ");
                continue;
            }

            _portalList[random].AddChallenge(item);
            i++;

        }
    }

    #endregion

    #region INTERACT
    public string GetInteractableID()
    {
        return id;
    }

    public void Interact()
    {

        StartChallenge();
    }

    public void InteractUI(bool isVisible)
    {
        _interactCanvas.gameObject.SetActive(isVisible);
        _interactCanvas.ControlInteractButton(isVisible);
    }

    public bool IsInteractable()
    {
        return !isRunning;
    }
    #endregion





}
