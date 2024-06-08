using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalHandler_RoundHandler : MonoBehaviour
{
    //at the start of the game we show round and then start spawning.
    //


    //we need control how many fellas will spawn per time.
    //but they cant spawn all at the same time. there is an interval between the periods of spawn.
    //


    //we decided that every turn has an amount
    //1 - 5; 2 - 8; 3 - 15
    //and we decided every timer. the portals are random.
    //we also need the amount we are going to be spawning. every interval 2 people till 5 is spawned.

    //i dont want them to spawn at the same time. each should have some randomize.
    //what we can do is that its given at the same time, but in the moment that they receive they roll a random timer. 

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
    private void Awake()
    {
        _handler = GetComponent<LocalHandler>();
    }
    private void Update()
    {
        HandleTurn();
        CheckIfTurnEnded();
    }

    public void StartRound()
    {
        StartCoroutine(StartRoundProcess());
    }

    //

    IEnumerator StartRoundProcess()
    {
        UIHandler.instance._playerUI.OpenRound_New();
        UIHandler.instance._playerUI.UpdateRoundText_New(0, true);
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

        
        round++;


        UIHandler.instance._playerUI.UpdateRoundText_New(round, false);

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

    void SpawnEnemies()
    {

        //there is an amount of fellas that i want to spawn every round and an amount every interval.

        //i want to choose the fellas here.
        //i need to know the portals here.
        //

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


        foreach (var item in chosenEnemyList)
        {
            AddEnemy(item);
        }

        SendDataToPortals(chosenEnemyList);
        
    }

    void SendDataToPortals(List<EnemyData> enemyList)
    {
        //we will randomly send to all portals.
        List<Portal> availablePortalList = _handler.allowedPortal;
        int lastPortal = -1; //i will try to randomize a bit more so that i doesnt repeat the same way too much.



        foreach (var item in enemyList)
        {
           

            int random = Random.Range(0, availablePortalList.Count);

            int safeBreak = 0;

            while (random == lastPortal)
            {
                random = Random.Range(0, availablePortalList.Count);

                safeBreak++;

                if(safeBreak > 100)
                {
                    continue;
                }
            }

            availablePortalList[random].OrderSpawn(item);
            lastPortal = random;    


        }

    }


    List<EnemyData> GetChosenEnemyList()
    {
        return null;
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
           Debug.Log("enemies in scene");
            return;
        }
        if(intervalQuantityTotal > intervalQuantityCurrent)
        {
           Debug.Log("there are spawn intervals left");
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


    #region GETTING VALUES FOR SPAWN

    //how many spawn inter5vals will be called per round
    int GetQuantityOfSpawnIntervals()
    {
        //return 1;
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
            return 9;
        }
        return 3;
       
    }
    int GetAmountToSpawnPerInterval()
    {
        //return 1;

        if (round == 1) return 2;
        if (round > 1 && round <= 5) return 6;
        if(round > 10 && round <= 15) return 10;
        if (round > 15) return 15;

        return 1;

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


}

//