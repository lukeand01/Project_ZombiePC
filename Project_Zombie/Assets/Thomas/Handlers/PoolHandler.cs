using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PoolHandler : MonoBehaviour
{

    //we will use this to pool enemies and turretFly and maybe other stuff later.

    //also need other bullets to be put here as well.

    private void Awake()
    {
        CreateBulletPool();
        CreateEnemyPool();
        CreateSoundPool();
        CreateAreaDamagePool();
        CreateTurretFlyPool();
    }


    public void CompleteReset()
    {
        Audio_Reset();
        Bullet_Reset();
        DamageArea_Reset();
        Enemy_Reset();
        TurretFly_Reset();
    }

    #region BULLET
    [Separator("BULLET")]
    [SerializeField] BulletScript[] bulletScriptTemplateArray;
    [SerializeField] Transform bulletContainer;
    List<ObjectPool<BulletScript>> pool_Bullet_List  = new();
    int currentBulletIndex;
    //

    public BulletScript GetBullet(int index,Transform pos)
    {
        this.currentBulletIndex = index;
        BulletScript newBullet = pool_Bullet_List[index].Get();
        
        newBullet.transform.position = pos.position;
        newBullet.gameObject.SetActive(true);

        return newBullet;
    }

    void Bullet_Reset()
    {
        for (int i = 0; i < bulletContainer.childCount; i++)
        {
            var item = bulletContainer.GetChild(i).gameObject;
            item.SetActive(false);
        }
    }
    void CreateBulletPool()
    {


        for (int i = 0; i < bulletScriptTemplateArray.Length; i++)
        {
            pool_Bullet_List.Add(new ObjectPool<BulletScript>(Bullet_Create, null, Bullet_ReturnToPool, defaultCapacity: 300));
        }


    }

    //i should be able to set the difference for the turretFly.
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
        var bullet = Instantiate(bulletScriptTemplateArray[currentBulletIndex]);
        bullet.transform.SetParent(bulletContainer);
        bulletContainer.gameObject.name = "BulletContainer " + bulletContainer.childCount.ToString();
        return bullet;
    }

    public void Bullet_Release(int index, BulletScript bullet)
    {
        pool_Bullet_List[index].Release(bullet);
    }



    #endregion

    #region FLY TURRETS
    [Separator("FLY TURRETS")]
    [SerializeField] TurretFlying turretFlyTemplate;
    [SerializeField] Transform turretFlyContainer;
    private ObjectPool<TurretFlying> pool_TurretFly;


    public TurretFlying GetTurretFly(Transform pos)
    {
        TurretFlying newTurret = pool_TurretFly.Get();
        newTurret.transform.position = pos.position;
        newTurret.gameObject.SetActive(true);

        return newTurret;
    }

    void TurretFly_Reset()
    {
        for (int i = 0; i < turretFlyContainer.childCount; i++)
        {
            var item = turretFlyContainer.GetChild(i).gameObject;
            item.SetActive(false);
        }
    }
    void CreateTurretFlyPool()
    {
        pool_TurretFly = new ObjectPool<TurretFlying>(TurretFly_Create, null, TurretFly_ReturnToPool, defaultCapacity: 15);

    }

    //i should be able to set the difference for the turretFly.
    //like the material is given

    void TurretFly_ReturnToPool(TurretFlying turretFly)
    {
        turretFly.ResetToReturnToPool();
        turretFly.gameObject.SetActive(false);
        turretFly.transform.position = Vector3.zero;
        //i need to return this to the player as well.
    }

    TurretFlying TurretFly_Create()
    {
        var bullet = Instantiate(turretFlyTemplate);
        //
        bullet.transform.SetParent(turretFlyContainer);
        turretFlyContainer.gameObject.name = "TurretFlyContainer " + turretFlyContainer.childCount.ToString();
        return bullet;
    }

    public void TurretFly_Release(TurretFlying turretFly)
    {
        pool_TurretFly.Release(turretFly);
    }

    #endregion


    #region ENEMY
    [Separator("ENEMY")]
    [SerializeField] EnemyData[] enemyData_array;
    [SerializeField] Transform enemyContainer;
    List<EnemyBase> enemySpawnedList = new();
    Dictionary<EnemyData, ObjectPool<EnemyBase>> pool_Enemy_Dictionary = new();

    EnemyData targetData;
    void CreateEnemyPool()
    {

        foreach (var item in enemyData_array)
        {
            if(item.enemyModel == null)
            {
                Debug.Log("no model here " + item.name);
                continue;
            }

            pool_Enemy_Dictionary.Add(item, new ObjectPool<EnemyBase>(Enemy_Create, null, Enemy_ReturnToPool, defaultCapacity: 100));

        }
    }


    void Enemy_Reset()
    {
        for (int i = 0; i < enemyContainer.childCount; i++)
        {
            var item = enemyContainer.GetChild(i).gameObject;
            item.SetActive(false);
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
        newObject.transform.SetParent(enemyContainer);
        enemyContainer.gameObject.name = "EnemyContainer " + enemyContainer.childCount.ToString();
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

    #region AUDIO
    [Separator("SOUND")]
    [SerializeField] SoundUnit soundTemplate;
    [SerializeField] Transform soundContainer;
    ObjectPool<SoundUnit> pool_Sound;

    void Audio_Reset()
    {
        for (int i = 0; i < soundContainer.childCount; i++)
        {
            var item = soundContainer.GetChild(i).gameObject;
            item.SetActive(false);
        }
    }
    void CreateSoundPool()
    {
        pool_Sound = new ObjectPool<SoundUnit>(Sound_Create, null, Sound_ReturnToPool, defaultCapacity: 150);

    }

    public SoundUnit GetSound(Transform pos)
    {
        SoundUnit newBullet = pool_Sound.Get();
        newBullet.transform.position = pos.position;
        newBullet.gameObject.SetActive(true);



        return newBullet;
    }

    SoundUnit Sound_Create()
    {
        var sound = Instantiate(soundTemplate);
        sound.transform.SetParent(soundContainer);
        soundContainer.gameObject.name = "SoundContainer " + soundContainer.childCount.ToString();
        return sound;
    }

    void Sound_ReturnToPool(SoundUnit sound)
    {
        sound.ReturnToPool();
        sound.gameObject.SetActive(false);
        sound.transform.localPosition = Vector3.zero;
        //i need to return this to the player as well.
    }

    public void Sound_Release(SoundUnit sound)
    {
        pool_Sound.Release(sound);
    }
    #endregion

    #region DAMAGE AREA
    [Separator("DAMAGE AREA")]
    [SerializeField] AreaDamage damageAreaTemplate;
    [SerializeField] Transform damageContainer;
    ObjectPool<AreaDamage> pool_AreaDamage;

    void CreateAreaDamagePool()
    {
        pool_AreaDamage = new ObjectPool<AreaDamage>(AreaDamage_Create, null, AreaDamage_ReturnToPool, defaultCapacity: 150);

    }

    void DamageArea_Reset()
    {
        for (int i = 0; i < damageContainer.childCount; i++)
        {
            var item = damageContainer.GetChild(i).gameObject;
            item.SetActive(false);
        }
    }

    public AreaDamage GetAreaDamage(Transform pos)
    {
        AreaDamage newAreaDamage = pool_AreaDamage.Get();
        newAreaDamage.transform.position = pos.position;
        newAreaDamage.gameObject.SetActive(true);

        //
        

        return newAreaDamage;
    }

    AreaDamage AreaDamage_Create()
    {
        var areaDamage = Instantiate(damageAreaTemplate);
        areaDamage.transform.SetParent(damageContainer);
        damageContainer.gameObject.name = "DamageContainer " + damageContainer.childCount.ToString();
        return areaDamage;
    }

    void AreaDamage_ReturnToPool(AreaDamage areaDamage)
    {
        areaDamage.ResetForPool();
        areaDamage.transform.localPosition = Vector3.zero;
        //i need to return this to the player as well.
    }

    public void AreaDamage_Release(AreaDamage areaDamage)
    {
        pool_AreaDamage.Release(areaDamage);
    }

    #endregion
}
