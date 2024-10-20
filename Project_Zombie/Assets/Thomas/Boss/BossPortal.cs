using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPortal : MonoBehaviour, IInteractable
{
    string _id;
    [SerializeField] InteractCanvas _interactCanvas;
    [SerializeField] Portal[] _portalArray;

    [SerializeField] ParticleSystem _portal;
    [SerializeField] Transform _spawnPoint;
    [SerializeField] Transform _presentationPoint;

    public Action eventBossEnded;
    public void OnBossEnded() => eventBossEnded?.Invoke();

    //when we interact we create a list.
    //each sigil has its graphic that i will give it here.

    bool hasStarted;

    public void SendEnemyToSpawn(List<EnemyData> enemyList)
    {
        //we give this list to the portals in case the boss wants to spawn something.

        if (_portalArray.Length == 0) return;
        if(enemyList.Count == 0) return;    


        int lastUsed = -1;
        int enemyIndex = 0;
        int safeBreak = 0;

        while (enemyList.Count > enemyIndex )
        {
            safeBreak++;

            if (safeBreak > 1000) break;

            int random = UnityEngine.Random.Range(0, _portalArray.Length);

            if(lastUsed == random)
            {
                continue;
            }

            _portalArray[random].Spawn(enemyList[enemyIndex]);

            enemyIndex++;
            lastUsed = random;
        }
    }

    private void Awake()
    {
        _id = MyUtils.GetRandomID();
    }

    public string GetInteractableID()
    {
        return _id;
    }

    public void Interact()
    {
        if (!PlayerHandler.instance._playerInventory.HasEnoughSigil()) return;

        PlayerHandler.instance._playerInventory.SpendSigil();

        EnemyData bossData = LocalHandler.instance.GetRandomBoss();

        if (bossData == null)
        {
            Debug.Log("no data ");
            return;
        }

        EnemyBoss bossModel = bossData.bossModel;

        if(bossModel == null)
        {
            Debug.Log("no model for boss");
            return;
        }

        //then we will begin the process here.
        //we will spawn at the spawn point.
        //raise it to the 
        hasStarted = true;

        StartCoroutine(SpawnBossProcess(bossData));
    }


    IEnumerator SpawnBossProcess(EnemyData data)
    {
        EnemyBoss newBoss = GameHandler.instance._pool.GetBoss(data, _spawnPoint.position);
        newBoss.transform.position = _spawnPoint.position;
        //newBoss.transform.position = _presentationPoint.position;


        GameHandler.instance._soundHandler.CreateSfx(SoundType.AudioClip_Boss_Start, null, 0.5f);
        newBoss.StartBossUI();

        _portal.gameObject.SetActive(true);
        _portal.Play();

        newBoss.transform.DOMove(_presentationPoint.position, 5).SetEase(Ease.Linear);

        yield return new WaitForSeconds(6);

        
        newBoss.SetBossPortal(this);
        newBoss.StartBoss();



        _portal.gameObject.SetActive(false);
    }

    public void EndBoss()
    {
        hasStarted = false;

    }

    public void KillAllEnemies()
    {
        for (int i = 0; i < _portalArray.Length; i++)
        {
            var item = _portalArray[i];

            item.ClearPortal();
        }

        OnBossEnded();

    }

    public void InteractUI(bool isVisible)
    {
        //_interactCanvas.gameObject.SetActive(isVisible);
        _interactCanvas.ControlInteractButton(isVisible);

    }

    public bool IsInteractable()
    {
        return !hasStarted;
    }

    
    //how do i spawn it? always from the middle of the room. there is a presentation and it spawns from teh center. there is a portal
    //who do i spawn?

    //how do sigils work? 
    //there are only two things that are relevant.
    //if there is one of each
    //and if there is three of the same
    //that defines the additional buffs.
    //for now i wont think about it

    //so the boss is always 

    //what does the player win from killing the boss?
    //complete missions for stuff
    //perma buff that can help get to higher
    //the player gain a passive that stacks infiintely and is only gained by killing the boss. which grants 5% damage, 5% health
    //gain 3 passive ability chest
    //gain a shit ton of resources (get some iron)
    //get some unique resources. onme of the only ways to get a body enhancer or blueprint
    //get bless and points (10 bless and 2000 points)
    //


}


//i need 3 to call it
//if there are three similar you get something else in the end. but you cant control the chances through it
//

//to use show