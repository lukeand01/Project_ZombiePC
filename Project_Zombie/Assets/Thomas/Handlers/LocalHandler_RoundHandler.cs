
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LocalHandler_RoundHandler : MonoBehaviour
{

    //first lets change the timer for calculatio and put it in the 



    LocalHandler _handler;

    int round;

    bool isTurnRunning;

    [SerializeField] int amountPerInterval;

    [SerializeField]float intervalTimerCurrent;
    [SerializeField] float intervalTimerTotal; //this is the time we are going to spwan an amount.

    int intervalQuantityTotal; //the number of times we will have interval spawns.
    [SerializeField] int intervalQuantityCurrent;


    [SerializeField] List<EnemyData> currentTurnEnemySpawnedList = new(); //all the enemies that were spawned in this turn. we wait to get to zero to remove it.
    public int currentNumberForSpawnedEnemy;

    List<RoundClass> newRoundList = new();


    private void Awake()
    {
        _handler = GetComponent<LocalHandler>();

        portalCalculation_Total = 1;


        List<RoundClass> roundList = _handler._stageData.especialRoundList;

        foreach (var item in roundList)
        {
            RoundClass round = new RoundClass(item.data, item.MinimalRoundToTrigger, item.RoundsPassedPerTrigger);
            newRoundList.Add(round);
        }

        //i want to simulate teh calculation ehre.



    }
    private void Update()
    {
        HandleTurn();
        CheckIfTurnEnded();
    }
    private void FixedUpdate()
    {
        CheckEveryAllowedPortalDistance();


    }

    public void StartRound()
    {


        StartCoroutine(StartRoundProcess());
    }

    IEnumerator StartRoundProcess()
    {
        
        UIHandler.instance._playerUI.OpenRound_New();
        UIHandler.instance._playerUI.UpdateRoundText_New(0, true, null);


        round = 0;

        yield return new WaitForSecondsRealtime(1f);

        NextTurn();

    }

    public void NextTurn()
    {
        StopAllCoroutines();
        StartCoroutine(NextRoundProcess());

    }

    IEnumerator NextRoundProcess()
    {
        isTurnRunning = false;

        yield return new WaitForSecondsRealtime(0.2f);

        ClearRoundData();
        CheckIfShouldChangeRoundData();

        //we are going to pass the round a new visual
        PlayerHandler.instance._playerAbility.PassedRound();
        PlayerHandler.instance._entityEvents.OnPassedRound();
        //at the start we check if there is a round already here.


        round++;

        Sprite roundSprite = null;

        if (roundData != null) roundSprite = roundData.roundSprite;


        UIHandler.instance._playerUI.UpdateRoundText_New(round, false, roundSprite);

        _handler.SetRound(round);

        _handler.GetNewSpawnList();
        SetDictionaryForStacking();

        yield return new WaitForSecondsRealtime(1);

        amountPerInterval = MyUtils.GetAmountToSpawnPerInterval(round, _handler.RoundSpawnModifier); //this amount of enemies per interval

        intervalQuantityTotal = MyUtils.GetQuantityOfSpawnIntervals(round); //quantity of interval each round has.
        intervalQuantityCurrent = 0;

        intervalTimerTotal = MyUtils.GetIntervalBetweenSpawns(round); //quantity of 
        intervalTimerCurrent = intervalTimerTotal * 0.7f;


        if (round % 5 == 0)
        {
            PlayerHandler.instance._playerCombat.RefreshAllReserveAmmo();
        }

       
        isTurnRunning = true;

        yield return new WaitForSeconds(2);

        //UIHandler.instance._playerUI.UpdateRoundText_New(round, true, roundSprite); //we are doing this to force it, hopefully fix the problem
    }

    RoundData roundData;
    void ClearRoundData()
    {
        if (roundData != null)
        {
            roundData.OnRoundEnd();
            roundData = null;
        }

    }

    void CheckIfShouldChangeRoundData()
    {
        //i need to create a list for progressing the fella.


        foreach (var item in newRoundList)
        {
            if (item.MinimalRoundToTrigger > _handler.round)
            {

                continue;
            }

            item.PassRound();

            int roll = Random.Range(0, 101);

            if (item.chance > roll)
            {
                if (item.CanTrigger())
                {
                    //Debug.Log("can trigger");
                    //then we announce that the next round will be this fella.
                    item.data.OnRoundStart();
                    roundData = item.data;
                    return;
                }
                else
                {
                    //Debug.Log("cannot trigger");
                }
            }

        }


    }


    //but i also need a list for despawned fella. that once you receive you instantly assign to someone close.


    /// <summary>
    /// I RECEIVE ANY ENEMIES THAT ARE TOO FAR FROM THE PLAYER HERE
    /// </summary>
    public void ReceiveDespawnedEnemy(EnemyBase enemy)
    {
        //this here we spawn directly.
        //what might be happening


        if(currentPortalCloseList.Count > 0)
        {
            int randomPortal = Random.Range(0, currentPortalCloseList.Count);
            currentPortalCloseList[randomPortal].OrderRespawn(enemy);
            return;
        }

        if(currentPortalNotFarEnoughList.Count > 0)
        {
            int randomPortal = Random.Range(0, currentPortalNotFarEnoughList.Count);
            currentPortalNotFarEnoughList[randomPortal].OrderRespawn(enemy);
            return;
        }
        
        

        


        //the problem is that we might not have the current portal close list.
        //if thats the case we check for the next.
    }

    //i need to learn how we are dealing with the respawn
    //actually i wont do that. i will stop their movement, but i will not despawn them, so i need a way to force them to no not despawn.
    void SpawnEnemies()
    {

        if (intervalQuantityCurrent >= intervalQuantityTotal)
        {
            Debug.Log("can no longer spawn in this turn");
            return;
        }

        if (_handler.enemyChanceList.Count <= 0)
        {
            Debug.Log("this?");
            return;
        }




        List<EnemyData> chosenEnemyList = new();
        int safeBreak = 0;

        intervalQuantityCurrent++;

        bool triedEspecialList = false;

        int roll = Random.Range(0, 101);


        Debug.Log("handler " + _handler.enemyChanceList.Count);

        while (amountPerInterval > chosenEnemyList.Count)
        {
            safeBreak++;
            if (safeBreak > 2000)
            {
                Debug.LogError("Safe break for choose enemies ");

                break;
            }

            //check each fella against the raindom.

            EnemyChanceSpawnClass enemyChance = null;


            //for especial lists we ignore stack enemy.
            //how to do this? maybe we can add an additional fella.

            int especialRandom = Random.Range(0, 101);

            if (_handler.especialList.Count > 0 && _handler.preferenceForEspecialList > especialRandom && !triedEspecialList)
            {
                //then we are going to check this fella here.
                //but if it fails next time we dont check this.
                int random = Random.Range(0, _handler.especialList.Count);
                enemyChance = _handler.especialList[random];

            }
            else
            {
                int random = Random.Range(0, _handler.enemyChanceList.Count);
                enemyChance = _handler.enemyChanceList[random];
            }

            //Debug.Log("enemy chance list " + _handler.enemyChanceList.Count);
            triedEspecialList = false;



            if (!CanStack(enemyChance))
            {

                //Debug.Log("cannot add more of this fella " + enemyChance.data.enemyName);
                if (_handler.preferenceForEspecialList > especialRandom) triedEspecialList = true;
                continue;
            }
            else
            {
                //Debug.Log("can add this fella");
            }


            float chanceToSpawn = enemyChance.GetChanceToSpawn();
            if (chanceToSpawn >= roll && chanceToSpawn > 0)
            {
                chosenEnemyList.Add(enemyChance.data);
                TryToAddFromDictionary(enemyChance.data.name);
                //we check if we should put in the dictionary
            }
            else
            {
                roll -= 2;
            }

        }


        foreach (var item in chosenEnemyList)
        {
            AddEnemy(item);
        }


        SendDataToPortals(chosenEnemyList);

    }

    void SendDataToPortals(List<EnemyData> enemyList)
    {
        //we will randomly send to all portals.

        //am i not sending the data?




        for (int i = 0; i < enemyList.Count;)
        {
            var item = enemyList[i];

            int random = -1;
            if (currentPortalCloseList.Count == 0 && currentPortalNotFarEnoughList.Count > 0)
            {
                random = Random.Range(0, currentPortalNotFarEnoughList.Count);
                if (currentPortalNotFarEnoughList[random].gameObject.activeInHierarchy)
                {
                    currentPortalNotFarEnoughList[random].OrderSpawn(item);
                    i++;
                }
              
                // Debug.Log("no currentportal");
                continue;
            }

            int roll = Random.Range(0, 101);

            if (roll >= 80 && currentPortalNotFarEnoughList.Count > 0)
            {
                //the its the not close enoguh list
                random = Random.Range(0, currentPortalNotFarEnoughList.Count);
                if (currentPortalNotFarEnoughList[random].gameObject.activeInHierarchy)
                {
                    currentPortalNotFarEnoughList[random].OrderSpawn(item);
                    i++;
                }
                
                //Debug.Log("chosen currentportal not far enough");
                continue;
            }
            else if (currentPortalCloseList.Count > 0)
            {
                //then its the close list.
                // Debug.Log("current portal close list");
                random = Random.Range(0, currentPortalCloseList.Count);
                if (currentPortalCloseList[random].gameObject.activeInHierarchy)
                {
                    currentPortalCloseList[random].OrderSpawn(item);
                    i++;
                }
                
                continue;
            }


            // Debug.Log("there was noone here");

            //if it ever gets here 
            random = Random.Range(0, _handler.allowedPortal.Count);
            if (_handler.allowedPortal[random].gameObject.activeInHierarchy)
            {
                _handler.allowedPortal[random].OrderSpawn(item);
                i++;
            }
            
        }


    }


    void HandleTurn()
    {
        //everytime we meant to spawn we will get a new amount.


        if (!isTurnRunning) return;

        if (intervalQuantityCurrent >= intervalQuantityTotal)
        {
            //Debug.Log("must wait for enemies");
            intervalTimerCurrent = intervalTimerTotal;
            return;
        }
        if (round == 0) return;



        if (intervalTimerCurrent > intervalTimerTotal)
        {
            intervalTimerCurrent = 0;
            SpawnEnemies();
        }
        else
        {
            intervalTimerCurrent += Time.deltaTime;

        }

    }

    void CheckIfTurnEnded()
    {
        if (!isTurnRunning)
        {
            // Debug.Log("cant check because turn is not running");
            return;
        }

        if (round == 0)
        {
            // Debug.Log("round is 0 still");
            return;
        }

        if (currentTurnEnemySpawnedList.Count > 0)
        {

            return;
        }

        if (intervalQuantityTotal > intervalQuantityCurrent)
        {
            return;
        }

        if (intervalTimerTotal > intervalTimerCurrent)
        {
            //Debug.Log("its counting the interval yet");
            return;
        }

        Debug.Log("checking if turn ended " + currentTurnEnemySpawnedList.Count);
        if (currentTurnEnemySpawnedList.Count == 0)
        {
            //Debug.Log("this round is over");
            NextTurn();
        }
    }

    public void AddEnemy(EnemyData data)
    {
        currentNumberForSpawnedEnemy += 1;
        currentTurnEnemySpawnedList.Add(data);
    }
    public void RemoveEnemy(EnemyData data)
    {
        currentNumberForSpawnedEnemy -= 1;

        TryToRemoveFromDictionary(data.name);

        for (int i = 0; i < currentTurnEnemySpawnedList.Count; i++)
        {
            if (currentTurnEnemySpawnedList[i].name == data.name)
            {
                currentTurnEnemySpawnedList.RemoveAt(i);
                return;
            }
        }
        if (currentTurnEnemySpawnedList.Count <= 0)
        {
            return;
        }

        currentTurnEnemySpawnedList.RemoveAt(0);
    }

    //i can make every enemy check how far it is. if there are perfoamcen problems then we will change

    #region GETTING VALUES FOR SPAWN

    //how many spawn inter5vals will be called per round


    #endregion


    //i will check each portal from here.
    #region CHECKING FOR PORTALS IN DISTANCE

    [field: SerializeField] public List<Portal> currentPortalCloseList { get; private set; } = new();
    [field: SerializeField] public List<Portal> currentPortalNotFarEnoughList { get; private set; } = new();

    float portalCalculation_Current;
    float portalCalculation_Total;

    void CheckEveryAllowedPortalDistance()
    {
        //we check this list every 5 seconds.

        if (portalCalculation_Total > portalCalculation_Current)
        {
            portalCalculation_Current += Time.fixedDeltaTime;
            return;
        }
        else
        {
            portalCalculation_Current = 0;
        }

        Transform playerTransform = PlayerHandler.instance.transform;
        currentPortalCloseList.Clear();
        currentPortalNotFarEnoughList.Clear();

        foreach (var item in _handler.allowedPortal)
        {
            float distance = Vector3.Distance(playerTransform.position, item.transform.position);

            if (distance <= 40)
            {
                currentPortalCloseList.Add(item);
                continue;
            }

            if (distance > 40 && distance < 60)
            {
                currentPortalNotFarEnoughList.Add(item);
                continue;
            }
        }
    }

    #endregion


    #region FOR STACKING 
    Dictionary<string, int> dictionaryForEnemyStacking = new();

    //every round we reset this

    void SetDictionaryForStacking()
    {
        //we clear it and we check
        dictionaryForEnemyStacking.Clear();
        //then we create a new list from it

        foreach (var item in _handler.enemyChanceList)
        {
            if(item.GetMaxAllowedToSpawn() > 0)
            {
                dictionaryForEnemyStacking.Add(item.data.name, 0);
                //Debug.Log("Placed this here " + item.data.name);
            }
        }


    }

    bool CanStack(EnemyChanceSpawnClass enemySpawnClass)
    {
        if (!dictionaryForEnemyStacking.ContainsKey(enemySpawnClass.data.name))
        {
            //Debug.Log("not contains");
            return true;
        }


        return dictionaryForEnemyStacking[enemySpawnClass.data.name] < enemySpawnClass.GetMaxAllowedToSpawn();
    }

    void TryToAddFromDictionary(string key)
    {
        if (dictionaryForEnemyStacking.ContainsKey(key))
        {
            int value = dictionaryForEnemyStacking[key] + 1;
            dictionaryForEnemyStacking[key] = value;
            
            //Debug.Log("key 2 " + key);
            //Debug.Log("added this fella " + dictionaryForEnemyStacking[key]);
        }
        else
        {
            //there is nothing to add here.
        }
    }
    void TryToRemoveFromDictionary(string key)
    {
        if (dictionaryForEnemyStacking.ContainsKey(key))
        {
            int value = dictionaryForEnemyStacking[key] - 1;
            value = Mathf.Clamp(value, 0, 999);
            dictionaryForEnemyStacking[key] = value;
        }
        else
        {
            
        }
    }


    #endregion

    //i dont have access to it though.
    //i cant get it through the data so where can i get it from?
    //


}

//