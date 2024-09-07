using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthFountain : ChestBase
{
    //if you have enough money regen a percent of your health. 25%
    //then it goes to cooldown. the cooldown is 5 turns.
    //and the ui should show the amount required

    //this will also apply

    //we decide on the spawn, but it always need to have at least one health, but it should be random.

    [Separator("STAT FOUNTAIN")]
    [SerializeField] int price;
    [SerializeField] int roundsPerUse;
    [SerializeField] bool isBuff;
    [SerializeField] ParticleSystem _psHealth;
    [SerializeField] ParticleSystem _psBuff;
    [SerializeField] AudioClip _audioHealth;
    [SerializeField] AudioClip _audioBuff;
    int roundsPassed;

    bool CanUse { get { return roundsPassed >= roundsPerUse; } }

    //

    private void Start()
    {
        PlayerHandler.instance._entityEvents.eventPassedRound += PassedRound;

        roundsPassed = roundsPerUse;

    }

    public void SetUp(bool isBuff)
    {
        this.isBuff = isBuff;

        ControlPS();
    }

    void PassedRound()
    {
        if(roundsPassed < roundsPerUse)
        {
            roundsPassed++;
        }

        if (CanUse)
        {
            ControlPS();
        }
    }

    void ControlPS()
    {

        if (!CanUse)
        {
            _psBuff.gameObject.SetActive(false);
            _psHealth.gameObject.SetActive(false);
        }
        else
        {
            _psBuff.gameObject.SetActive(isBuff);
            _psHealth.gameObject.SetActive(!isBuff);

            if (isBuff)
            {
                _psBuff.Clear();
                _psBuff.Play();
            }
            else
            {
                _psHealth.Clear();
                _psHealth.Play();
            }
        }

        

    }
    


    public override void Interact()
    {

        if (!CanUse) return;

        if (!PlayerHandler.instance._playerResources.HasEnoughPoints(price)) return;


        if (isBuff)
        {
            //
            GiveRandomBuff();
            GameHandler.instance._soundHandler.CreateSfx(_audioBuff);
        }
        else
        {
            PlayerHandler.instance._playerResources.RestoreHealthBasedInPercent(0.25f);
            interactCanvas.ControlInteractButton(false);
            GameHandler.instance._soundHandler.CreateSfx(_audioHealth);
        }
        //heal the player and start the cooldowjn

        roundsPassed = 0;
        PlayerHandler.instance._playerResources.SpendPoints(price);


    }

    void GiveRandomBuff()
    {
        //damage, critchance, dodge chance, speed
        BDClass bd = GetRandomBdClass();
        PlayerHandler.instance._entityStat.AddBD(bd);

       FadeUI_New fadeUI = GameHandler.instance._pool.GetFadeUI(transform.position + new Vector3(0, 10, 0));
        //i want a warning in the top.
    }

    BDClass GetRandomBdClass()
    {
        int random = Random.Range(0, 4);


        switch (random)
        {
            case 0: //DAMAGE
                BDClass bd_Damage = new BDClass("Shrine_Damage", StatType.Damage, 0, 0, 0.15f);
                bd_Damage.MakeTemp(30);
                return bd_Damage;
            case 1: //CRIT CHANCE
                BDClass bd_CritChance = new BDClass("Shrine_CritChance", StatType.CritChance, 20, 0, 0);
                bd_CritChance.MakeTemp(30);
                return bd_CritChance;

            case 2: //DODGE
                BDClass bd_Dodge = new BDClass("Shrine_Dodge", StatType.Dodge, 15, 0, 0);
                bd_Dodge.MakeTemp(30);
                return bd_Dodge;
            case 3: //DODGE
                BDClass bd_Speed = new BDClass("Shrine_Speed", StatType.Speed, 0, 0.15f, 0);
                bd_Speed.MakeTemp(30);
                return bd_Speed;
        }


        return null;
    }


    public override void InteractUI(bool isVisible)
    {
        if (!CanUse) return;
        base.InteractUI(isVisible);
        interactCanvas.ControlPriceHolder(price);
    }

}


//