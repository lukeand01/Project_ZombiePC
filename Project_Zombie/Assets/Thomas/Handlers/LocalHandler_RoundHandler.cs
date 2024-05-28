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

    float intervalToSpawnCurrent;
    float intervalToSpawnTotal; //this is the time we are going to spwan an amount.

    int amountToSpawn = 0;
    int amountSpawned = 0; 

    [SerializeField] List<EnemyData> currentTurnEnemySpawnedList = new(); //all the enemies that were spawned in this turn. we wait to get to zero to remove it.

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

        yield return new WaitForSecondsRealtime(1.5f);

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

        yield return new WaitForSecondsRealtime(0.5f);

        
        round++;
        UIHandler.instance._playerUI.UpdateRoundText_New(round, false);

        _handler.SetRound(round);
        _handler.SetWaveQuantity(GetAmountToSpawnPerInterval());
      
        yield return new WaitForSecondsRealtime(1.5f);

        amountToSpawn = GetAmountToSpawnPerRound();
        amountSpawned = 0;

        intervalToSpawnTotal = GetIntervalBetweenSpawns();
        intervalToSpawnCurrent = 0;

        isTurnRunning = true;
    }

    void SpawnEnemies()
    {
        int quantityToSpawnPerInterval = GetAmountToSpawnPerInterval();

        amountSpawned += quantityToSpawnPerInterval;

        _handler.SetWaveQuantity(quantityToSpawnPerInterval);
        _handler.ChooseEnemiesAndSpawn();


        
    }

    void HandleTurn()
    {
        //everytime we meant to spawn we will get a new amount.

        if (!isTurnRunning) return;

        if (amountSpawned >= amountToSpawn)
        {
            //Debug.Log("must wait for enemies");
            intervalToSpawnCurrent = intervalToSpawnTotal;
            return;
        }
        if (round == 0) return;

        if (intervalToSpawnCurrent > intervalToSpawnTotal)
        {
            intervalToSpawnCurrent = 0;
            SpawnEnemies();
        }
        else
        {
            intervalToSpawnCurrent += Time.deltaTime;
            
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
           // Debug.Log("enemies in scene");
            return;
        }
        if(amountSpawned < amountToSpawn)
        {
           //Debug.Log("there are fellas left to spawn");
            return;
        }

        if(intervalToSpawnTotal > intervalToSpawnCurrent)
        {
            //Debug.Log("its counting the interval yet");
            return;
        }

        if(currentTurnEnemySpawnedList.Count <= 0 && amountSpawned >= amountToSpawn && round != 0)
        {
            //Debug.Log("this round is over");
            NextTurn();
        }
    }

    public void AddEnemy(EnemyData data)
    {
        currentTurnEnemySpawnedList.Add(data);
    }
    public void RemoveEnemy(EnemyData data)
    {
        Debug.Log("here");
        for (int i = 0; i < currentTurnEnemySpawnedList.Count; i++)
        {
            if (currentTurnEnemySpawnedList[i].name == data.name)
            {
                Debug.Log("found it");
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

    int GetAmountToSpawnPerRound()
    {
        return 1;

        int additional = 2;


        if(round > 5)
        {
            additional += 3;
        }
        if(round > 10)
        {
            additional += 3;
        }
        if(round > 15)
        {
            additional += 3;
        }
        if(round > 20)
        {
            additional += 4;
        }

        //20 + 9 = 29 * 2 = 58

        return 2 * (round + additional);
    }
    int GetAmountToSpawnPerInterval()
    {
        
        int value = round / 2;
        value = Mathf.Clamp(value, 4, 10);
        return value;

    }
    float GetIntervalBetweenSpawns()
    {
        if (round > 0 && round <= 5)
        {
            return 3;
        }

        if (round > 5 && round <= 10)
        {
            return 2;
        }
        if (round > 10 )
        {
            return 1.5f;
        }

        return 0;
    }

    #endregion


}
