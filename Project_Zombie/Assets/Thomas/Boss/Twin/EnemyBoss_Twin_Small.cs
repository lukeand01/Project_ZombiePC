using DG.Tweening;
using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyBoss_Twin_Small : EnemyBoss
{

    EnemyBoss_Twin _mainBoss;

    [SerializeField] GameObject _attackUIHolder;
    [SerializeField] Image _attackUIFill;
    [SerializeField] BoxCollider _attackCollider;

    float _dust_Cooldown_Total;
    float _dust_Cooldown_Current;
    protected override void UpdateFunction()
    {

        if (_mainBoss == null) return;

        _agent.speed = 5 + _mainBoss._phaseLevel * 2;

        if (PlayerHandler.instance._entityStat.isStunned)
        {
            StopAgent();
            return;
        }

        if(IsActing)
        {
            return;
        }



        base.UpdateFunction();

        float distance = Vector3.Distance(PlayerHandler.instance.transform.position, transform.position);

        if(_bossData.attackRange > distance)
        {
            CallAutoAttack();
        }

        if(_dust_Cooldown_Total > _dust_Cooldown_Current)
        {
            _dust_Cooldown_Current += Time.deltaTime;
        }
        else
        {
            if (_bossData.attackRange * 1.3f <= distance)
            {
                CallDust();
                _dust_Cooldown_Current = 0;
            }
        }
      
    }


    public void SetTwinSmall(EnemyBoss_Twin mainBoss)
    {
        _mainBoss = mainBoss;   
        StartCoroutine(StartTwinSmallProcess());

    }

    IEnumerator StartTwinSmallProcess()
    {
        _graphicHolder.transform.localPosition = new Vector3(0, -10, 0);
        _graphicHolder.transform.DOLocalMoveY(0, 1.5f);

        yield return new WaitForSeconds(1.5f);




        ControlIsActing(false);
    }

    public override void CalculateAttack()
    {
        base.CalculateAttack();
    }

    public void CallAutoAttack()
    {
        //call animation

    }
    IEnumerator AutoAttackProcess()
    {
        yield return new WaitForSeconds(0.5f);

        //check for the player being cloes enough
        float distance = Vector3.Distance(PlayerHandler.instance.transform.position, transform.position);
        if (_bossData.attackRange > distance)
        {
            BDClass bd = new BDClass("Small_Stun", BDType.Stun, 0.5f);
            PlayerHandler.instance._playerResources.ApplyBD(bd);
            DamageClass damage = new DamageClass(20, DamageType.Physical, 0);
            PlayerHandler.instance._playerResources.TakeDamage(damage);
            StartCoroutine(DisappearProcess());
        }
        else
        {
            ControlIsActing(false);
        }


    }

    IEnumerator DisappearProcess()
    {
        //we create some dust and make it go down.
        ControlIsActing(true);

        _graphicHolder.transform.DOLocalMoveY(-10, 1.5f);

        yield return new WaitForSeconds(1.5f);

        gameObject.SetActive(false);

    }

    public void CallDust()
    {
        StopAgent();
        ControlIsActing(true);
        _attackUIHolder.gameObject.SetActive(true);
        _attackUIFill.DOFillAmount(1, 1.5f).SetEase(Ease.Linear).OnComplete(DoneDust);
    }

    void DoneDust()
    {
        ControlIsActing(false);
        _boxCollider.enabled = true;
        Invoke(nameof(DisabelDustCollider), 0.1f);
    }
    void DisabelDustCollider()
    {
        _boxCollider.enabled = false;
    }


    public override void TakeDamage(DamageClass damageRef)
    {
        _enemyCanvas.CreateShieldPopUp();
    }

}
