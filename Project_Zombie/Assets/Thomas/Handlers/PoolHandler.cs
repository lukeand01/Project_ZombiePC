using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PoolHandler : MonoBehaviour
{

    //we will use this to pool enemies and bullet and maybe other stuff later.



    private void Awake()
    {
        CreateBulletPool();
        CreateEnemyPool();
    }

    #region BULLET
    [Separator("BULLET")]
    [SerializeField] BulletScript bulletScriptTemplate;
    private ObjectPool<BulletScript> pool_Bullet;

    public BulletScript GetBullet(Transform pos)
    {
        BulletScript newBullet = pool_Bullet.Get();
        newBullet.transform.position = pos.position;
        newBullet.gameObject.SetActive(true);
        return newBullet;
    }

    void CreateBulletPool()
    {
        
        pool_Bullet = new ObjectPool<BulletScript>(Bullet_Create, null, Bullet_ReturnToPool, defaultCapacity: 150);

    }

    //i should be able to set the difference for the bullet.
    //like the material is given

    void Bullet_ReturnToPool(BulletScript bullet)
    {
        bullet.ResetToReturnToPool();
        bullet.gameObject.SetActive(false);
        bullet.transform.position = Vector3.zero;
        //i need to return this to the player as well.
    }

    BulletScript Bullet_Create()
    {
        var bullet = Instantiate(bulletScriptTemplate);
        return bullet;
    }

    public void Bullet_Release(BulletScript bullet)
    {
        pool_Bullet.Release(bullet);
    }



    #endregion


    #region ENEMY
    [Separator("ENEMY")]
    [SerializeField] EnemyData[] enemyData_array;
    Dictionary<EnemyData, ObjectPool<EnemyBase>> pool_Enemy_Dictionary = new();

    EnemyData targetData;
    void CreateEnemyPool()
    {

        foreach (var item in enemyData_array)
        {
            if(item.enemyModel == null)
            {
                Debug.Log("no model here");
                continue;
            }

            pool_Enemy_Dictionary.Add(item, new ObjectPool<EnemyBase>(Enemy_Create, null, Enemy_ReturnToPool, defaultCapacity: 100));

        }
    }

    public EnemyBase GetEnemy(EnemyData data, Vector3 pos)
    {

        targetData = data;

        EnemyBase newEnemy = pool_Enemy_Dictionary[data].Get();


        newEnemy.transform.position = pos;
        newEnemy.gameObject.SetActive(true);
        return newEnemy;
    }

    EnemyBase Enemy_Create()
    {   
        EnemyBase newObject = Instantiate(targetData.enemyModel);

        return newObject;
    }

    void Enemy_ReturnToPool(EnemyBase enemy)
    {
        enemy.ResetEnemyForPool();

    }

    public void Enemy_Release(EnemyData data, EnemyBase enemy)
    {
        pool_Enemy_Dictionary[data].Release(enemy);
    }

    #endregion

}
