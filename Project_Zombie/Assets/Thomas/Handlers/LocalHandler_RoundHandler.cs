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

    float intervalTimerCurrent;
    [SerializeField]float intervalTimerTotal; //this is the time we are going to spwan an amount.

    int intervalQuantityTotal; //the number of times we will have interval spawns.
    [SerializeField]int intervalQuantityCurrent;
    

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
            RoundClass round = new RoundClass(item.data, item.MinRoundAllowedToTrigger, item.minRoundPassedPerTrigger);
            newRoundList.Add(round);
        }

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

        //at the start we check if there is a round already here.
   

        round++;

        Sprite roundSprite = null;

        if (roundData != null) roundSprite = roundData.roundSprite;


        UIHandler.instance._playerUI.UpdateRoundText_New(round, false, roundSprite);

        _handler.SetRound(round);

        _handler.GetNewSpawnList();

        yield return new WaitForSecondsRealtime(1);

        amountPerInterval = GetAmountToSpawnPerInterval(); //this amount of enemies per interval

        intervalQuantityTotal = GetQuantityOfSpawnIntervals(); //quantity of interval each round has.
        intervalQuantityCurrent = 0;

        intervalTimerTotal = GetIntervalBetweenSpawns(); //quantity of 
        intervalTimerCurrent = intervalTimerTotal * 0.7f;


        if(round % 5 == 0)
        {
            PlayerHandler.instance._playerCombat.RefreshAllReserveAmmo();
        }

        isTurnRunning = true;
    }

    RoundData roundData;
    void ClearRoundData()
    {
        if(roundData != null)
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
            if( item.MinRoundAllowedToTrigger > _handler.round)
            {
                Debug.Log("not enough round to call this");
                continue;
            }

            item.PassRound();

            int roll = Random.Range(0, 101);

            if(roll > item.chance)
            {
                if (item.CanTrigger())
                {
                    //then we announce that the next round will be this fella.
                    item.data.OnRoundStart();
                    roundData = item.data;
                    return;
                }
            }

        }


    }


    //but i also need a list for despawned fella. that once you receive you instantly assign to someone close.

    public void ReceiveDespawnedEnemy(EnemyBase enemy)
    {
        //we spawn the same fella at a possible list.
        int randomPortal = Random.Range(0, currentPortalCloseList.Count);
        currentPortalCloseList[randomPortal].OrderRespawn(enemy);

    }


    void SpawnEnemies()
    {

        if(intervalQuantityCurrent >= intervalQuantityTotal)
        {
            Debug.Log("can no longer spawn in this turn");
            return;
        }

        if (_handler.enemyChanceList.Count <= 0)
        {
            return;
        }

        //maybe we order the respawn through here.
        //

        List<EnemyData> chosenEnemyList = new();
        int safeBreak = 0;

        intervalQuantityCurrent++;


        int roll = Random.Range(0, 101);
        while (amountPerInterval > chosenEnemyList.Count)
        {
            safeBreak++;
            if (safeBreak > 1000)
            {
                //Debug.LogError("Safe break for choose enemies");
                return;
            }

            //check each fella against the raindom.
            int random = Random.Range(0, _handler.enemyChanceList.Count);
            EnemyChanceSpawnClass enemyChance = _handler.enemyChanceList[random];

            if (!_handler.CanStackEnemy(enemyChance))
            {
                Debug.Log("cannot add more of this fella " + enemyChance.data.enemyName);
                continue;
            }
            else
            {
                //Debug.Log("can add this fella");
            }

            if (enemyChance.totalChanceSpawn >= roll)
            {
                chosenEnemyList.Add(enemyChance.data);
                _handler.AddToStackDictionary(enemyChance);
                //we check if we should put in the dictionary
            }
            else
            {
                roll -= 10;
            }

        }

        Debug.Log("chosen enemy " + chosenEnemyList.Count);

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

        foreach (var item in enemyList)
        {
            int random = -1;
            if (currentPortalCloseList.Count == 0 && currentPortalNotFarEnoughList.Count > 0)
            {
                random = Random.Range(0, currentPortalNotFarEnoughList.Count);
                currentPortalNotFarEnoughList[random].OrderSpawn(item);
                return;
            }

            int roll = Random.Range(0, 101);


            if (roll >= 80 && currentPortalNotFarEnoughList.Count > 0)
            {
                //the its the not close enoguh list
                random = Random.Range(0, currentPortalNotFarEnoughList.Count);
                currentPortalNotFarEnoughList[random].OrderSpawn(item);
                return;
            }
            else if(currentPortalCloseList.Count > 0)
            {
                //then its the close list.
                random = Random.Range(0, currentPortalCloseList.Count);
                currentPortalCloseList[random].OrderSpawn(item);
                return;
            }


            Debug.Log("there was noone here");

            //if it ever gets here 
            random = Random.Range(0, _handler.allowedPortal.Count);
            _handler.allowedPortal[random].OrderSpawn(item);

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
        if(!isTurnRunning)
        {
           // Debug.Log("cant check because turn is not running");
            return;
        }

        if(round == 0)
        {
           // Debug.Log("round is 0 still");
            return;
        }

        if(currentTurnEnemySpawnedList.Count > 0)
        {

            return;
        }
        if(intervalQuantityTotal > intervalQuantityCurrent)
        {
            return;
        }

        if(intervalTimerTotal > intervalTimerCurrent)
        {
            //Debug.Log("its counting the interval yet");
            return;
        }


        if(currentTurnEnemySpawnedList.Count == 0)
        {
            //Debug.Log("this round is over");
            NextTurn();
        }
    }

    public void AddEnemy(EnemyData data)
    {
        Debug.Log("called");
        currentNumberForSpawnedEnemy += 1;
        currentTurnEnemySpawnedList.Add(data);
    }
    public void RemoveEnemy(EnemyData data)
    {
        currentNumberForSpawnedEnemy -= 1;
        for (int i = 0; i < currentTurnEnemySpawnedList.Count; i++)
        {
            if (currentTurnEnemySpawnedList[i].name == data.name)
            {
                currentTurnEnemySpawnedList.RemoveAt(i);
                return;
            }
        }
        if(currentTurnEnemySpawnedList.Count <= 0)
        {
            return;
        }

        currentTurnEnemySpawnedList.RemoveAt(0);
    }

    //i can make every enemy check how far it is. if there are perfoamcen problems then we will change

    #region GETTING VALUES FOR SPAWN

    //how many spawn inter5vals will be called per round
    int GetQuantityOfSpawnIntervals()
    {
      
        return 1;
        if(round >= 3)
        {
            return 4;
        }

        if(round > 5 && round <= 9)
        {
            return 6;
        }
        if(round > 10 && round <= 14)
        {
            return 7;
        }
        if(round > 15 && round <= 19)
        {
            return 8;
        }
        if(round > 20)
        {
            return 10;
        }
        return 3;
       
    }
    int GetAmountToSpawnPerInterval()
    {

        if (round <= 15)
        {
            // Exponential increase from round 1 to 15
            return Mathf.RoundToInt(5 * Mathf.Pow(2, (round - 1) / 14f * _handler._stageData.enemyPerIntervalModifier));
        }
        else
        {
            // Smooth transition and slower increase from round 16 to 25
            float initialEnemiesAt15 = 5 * Mathf.Pow(2, (15 - 1) / 14f * _handler._stageData.enemyPerIntervalModifier);
            return Mathf.RoundToInt(initialEnemiesAt15 + (50 - initialEnemiesAt15) * (1 - Mathf.Exp(-0.2f * (round - 15))));
        }

    }
    float GetIntervalBetweenSpawns()
    {

        if (round > 0 && round <= 5)
        {
            return 2;
        }

        if (round > 5 && round <= 10)
        {
            return 1;
        }
        if (round > 10 )
        {
            return 0.5f;
        }

        return 0;
    }

    #endregion


    //i will check each portal from here.

    [field:SerializeField] public List<Portal> currentPortalCloseList { get; private set; } = new();
    [field: SerializeField] public List<Portal> currentPortalNotFarEnoughList { get; private set; } = new();

    float portalCalculation_Current;
    float portalCalculation_Total;

    void CheckEveryAllowedPortalDistance()
    {
        //we check this list every 5 seconds.

        if(portalCalculation_Total > portalCalculation_Current)
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

            if(distance <= 40)
            {
                currentPortalCloseList.Add(item);
                continue;
            }

            if(distance > 40 && distance < 60)
            {
                currentPortalNotFarEnoughList.Add(item);
                continue;
            }
        }
    }

   

    //i dont have access to it though.
    //i cant get it through the data so where can i get it from?
    //


}

//