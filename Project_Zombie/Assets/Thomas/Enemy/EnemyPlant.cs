using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPlant : EnemyBase
{
    [SerializeField] Transform[] _eyeArray;
    LayerMask _wallAndPlayerLayer;

    float _cooldown_Current;
    [SerializeField] float _cooldown_Total;
    [SerializeField] AudioClip _deathClip;

    bool isReady;

    protected override void AwakeFunction()
    {
        base.AwakeFunction();

        _wallAndPlayerLayer |= (1 << 3);
        _wallAndPlayerLayer |= (1 << 7);
        _wallAndPlayerLayer |= (1 << 9);

        _cooldown_Current = Random.Range(_cooldown_Total * 0.7f, _cooldown_Total * 1.2f);

        _touchDamage_Total = 2;

        SetStats(5);
    }


    protected override void UpdateFunction()
    {

        base.UpdateFunction();



        if (IsDead()) return;
        if (!isReady) return;


        if (IsPlayerInSight())
        {
            Shoot();
            RotatePlant();
        }

    }


    bool IsPlayerInSight()
    {
        //we do the 

        for (int i = 0; i < _eyeArray.Length; i++)
        {
            var item = _eyeArray[i];

            Vector3 targetPos = (PlayerHandler.instance.transform.position - item.position).normalized;
            Ray ray = new Ray(item.position, targetPos);

            if (Physics.Raycast(ray, out RaycastHit hit, 500, _wallAndPlayerLayer))
            {
                if (hit.collider.tag != "Player")
                {
                    Debug.Log("failure");
                    return false;
                }
            }
            else
            {
                Debug.Log("failure 1");
                return false;
            }

        }

        return true;
    }

    void RotatePlant()
    {
        Vector3 direction = PlayerHandler.instance.transform.position - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(direction.normalized);
        transform.rotation = Quaternion.SlerpUnclamped(transform.rotation, targetRotation, 10 * Time.deltaTime);
    }

    void Shoot()
    {

        if(_cooldown_Current > 0)
        {
            _cooldown_Current -= Time.deltaTime;    
            return;
        }
        Vector3 shootDir = PlayerHandler.instance.transform.position - transform.position;
        BulletScript bullet = GameHandler.instance._pool.GetBullet(ProjectilType.EnemySpit, head.transform);
        bullet.MakeEnemy();
        bullet.SetUp("EnemyPlant", shootDir);
        bullet.MakeSpeed(8, 0,0);
        bullet.MakeDamage(new DamageClass(30, DamageType.Physical, 0), 0, 0);

        _cooldown_Current = Random.Range(_cooldown_Total * 0.7f, _cooldown_Total * 1.2f);
    }

    public override void ResetEnemyForPool()
    {
        base.ResetEnemyForPool();

        StopAllCoroutines();

        isReady = false;

    }

    public override void Die(bool wasKilledByPlayer = true)
    {
        base.Die(wasKilledByPlayer);

        isReady = false;

        GameHandler.instance._soundHandler.CreateSfx_WithAudioClip(_deathClip, transform, 0.3f);
    }

    public override void SetStats(int round)
    {
        base.SetStats(round);

        Rise();
    }

    public void Rise()
    {
        transform.position += new Vector3(0, -10, 0);
        StartCoroutine(RiseProcess());
    }

    IEnumerator RiseProcess()
    {
        transform.DOMoveY(0, 1.5F).SetEase(Ease.Linear);
        yield return new WaitForSeconds(1.5F);

        isReady = true;
    }

    float _touchDamage_Total;
    float _touchDamage_Current;


    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer != 3) return;

        Debug.Log("this");

        if(_touchDamage_Current > _touchDamage_Total)
        {
            PlayerHandler.instance._playerResources.TakeDamage(new DamageClass(1, DamageType.Physical, 0));
            _touchDamage_Current = 0;
        }
        else
        {
            _touchDamage_Current += Time.deltaTime;
        }

    }

    private void OnTriggerExit(Collider other)
    {
        _touchDamage_Current = _touchDamage_Total;
    }
}
