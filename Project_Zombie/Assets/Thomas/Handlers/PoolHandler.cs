using MyBox;
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
        CreateDashEffectPool();
        CreateFadeUIPool();
        CreateChestPool();
        CreateTrapPool();
        CreateBossPool();
    }


    public void CompleteReset()
    {
        Audio_Reset();
        Bullet_Reset();
        DamageArea_Reset();
        Enemy_Reset();
        TurretFly_Reset();
        PS_Reset();
        FadeUI_Reset();
        Chest_Reset();
        Trap_Reset();
        Boss_Reset();
    }

    #region BULLET
    [Separator("BULLET")]
    [SerializeField] BulletScript[] bulletScriptTemplateArray;
    [SerializeField] Transform bulletContainer;
    List<ObjectPool<BulletScript>> pool_Bullet_List  = new();
    int currentBulletIndex;
    
    public BulletScript GetBullet(ProjectilType indexType,Transform pos)
    {
        currentBulletIndex = (int)indexType;
        BulletScript newBullet = pool_Bullet_List[(int)indexType].Get();
        
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

    #region BOSS
    [Separator("BOSS")]
    [SerializeField] EnemyData[] bossData_array;
    [SerializeField] Transform bossContainer;
    List<EnemyBase> bossSpawnedList = new();
    Dictionary<EnemyData, ObjectPool<EnemyBoss>> pool_Boss_Dictionary = new();

    EnemyData targetBossData;
    void CreateBossPool()
    {

        for (int i = 0; i < bossData_array.Length; i++)
        {
            var item = bossData_array[i];

            if (item.enemyModel == null)
            {
                Debug.Log("no model here " + item.name);
                continue;
            }

            pool_Boss_Dictionary.Add(item, new ObjectPool<EnemyBoss>(Boss_Create, null, Boss_ReturnToPool, defaultCapacity: 100));
        }

    }


    void Boss_Reset()
    {
        for (int i = 0; i < bossContainer.childCount; i++)
        {
            var item = bossContainer.GetChild(i).gameObject;
            item.SetActive(false);
        }
    }

    public EnemyBoss GetBoss(EnemyData data, Vector3 pos)
    {
        targetBossData = data;

        EnemyBoss newBoss = pool_Boss_Dictionary[data].Get();

        newBoss.transform.position = pos;
        newBoss.gameObject.SetActive(true);

        return newBoss;
    }

    EnemyBoss Boss_Create()
    {
        EnemyBoss newObject = Instantiate(targetBossData.bossModel);
        newObject.transform.SetParent(bossContainer);
        bossContainer.gameObject.name = "BossContainer " + bossContainer.childCount.ToString();
        return newObject;
    }

    void Boss_ReturnToPool(EnemyBoss boss)
    {
        boss.ResetForPool();

    }

    public void Boss_Release(EnemyData data, EnemyBoss boss)
    {

        pool_Boss_Dictionary[data].Release(boss);
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

    #region PS EFFECT
    [Separator("PARTICLE SYSTEM")]
    [SerializeField] PSScript[] psTemplate;
    [SerializeField] Transform psContainer;
    Dictionary<PSType, PSScript> ps_Ref_Dictionary = new();
    Dictionary<PSType, ObjectPool<PSScript>> pool_Ps_Dictionary = new();
    PSScript psTarget;

    void CreateDashEffectPool()
    {
        List<PSType> psRefList = new() 
        {
            PSType.Dash_01,
            PSType.Blood_01,
            PSType.GroundCrack_01,
            PSType.BlackWholeForBossCollection_01,
            PSType.Dash_02
        };


        for (int i = 0; i < psTemplate.Length; i++)
        {
            var item = psTemplate[i];

            if (item == null)
            {
                Debug.Log("no model here " + item.name);
                continue;
            }

            pool_Ps_Dictionary.Add(psRefList[i], new ObjectPool<PSScript>(PS_Create, null, PS_ReturnToPool, defaultCapacity: 100));
            ps_Ref_Dictionary.Add(psRefList[i], psTemplate[i]);
        }


    }

    void PS_Reset()
    {
        for (int i = 0; i < psContainer.childCount; i++)
        {
            var item = psContainer.GetChild(i).gameObject;

            if (item == null) return;

            item.SetActive(false);
        }
    }

    public PSScript GetPS(PSType _type, Transform pos)
    {
        PSScript refPS = ps_Ref_Dictionary[_type];
        psTarget = refPS;

        PSScript newPS = pool_Ps_Dictionary[_type].Get();     
        newPS.transform.position = pos.position;
        newPS.gameObject.SetActive(true);

        return newPS;
    }


    PSScript PS_Create()
    {
        var ps = Instantiate(psTarget);
        ps.transform.SetParent(psContainer);
        psContainer.gameObject.name = "PsContainer " + psContainer.childCount.ToString();
        return ps;
    }


    void PS_ReturnToPool(PSScript ps)
    {
        ps.ResetForPool();
        ps.transform.localPosition = Vector3.zero;
        ps.transform.SetParent(psContainer);
        psContainer.gameObject.name = "PsContainer " + psContainer.childCount.ToString();
        //i need to return this to the player as well.
    }



    public void PS_Release(PSType _psType, PSScript ps)
    {
        pool_Ps_Dictionary[_psType].Release(ps);
    }



    #endregion

    #region FADE UI

    //i will put this because i will be creating a lot of these fellas.
    [Separator("FADE UI")]
    [SerializeField] FadeUI_New fadeUITemplate;
    [SerializeField] Transform fadeContainer;
    ObjectPool<FadeUI_New> pool_FadeUI;

    void CreateFadeUIPool()
    {
        pool_FadeUI = new ObjectPool<FadeUI_New>(FadeUI_Create, null, FadeUI_ReturnToPool, defaultCapacity: 150);

    }

    void FadeUI_Reset()
    {
        for (int i = 0; i < fadeContainer.childCount; i++)
        {
            var item = damageContainer.GetChild(i).gameObject;
            item.SetActive(false);
        }
    }

    public FadeUI_New GetFadeUI(Vector3 pos)
    {
        FadeUI_New newFade = pool_FadeUI.Get();
        newFade.transform.position = pos;
        newFade.gameObject.SetActive(true);
        return newFade;
    }

    FadeUI_New FadeUI_Create()
    {
        var newFade = Instantiate(fadeUITemplate);
        newFade.transform.SetParent(fadeContainer);
        fadeContainer.gameObject.name = "FadeContainer " + fadeContainer.childCount.ToString();
        return newFade;
    }

    void FadeUI_ReturnToPool(FadeUI_New fadeUI)
    {
        fadeUI.ResetForPool();
        fadeUI.transform.localPosition = Vector3.zero;
        fadeUI.transform.SetParent(fadeContainer);
        //i need to return this to the player as well.
    }

    public void FadeUI_Release(FadeUI_New fadeUI)
    {
        pool_FadeUI.Release(fadeUI);
    }



    #endregion

    #region CHEST
    [Separator("Chests")]
    [SerializeField] ChestBase[] chestBaseTemplateArray;
    [SerializeField] Transform chestContainer;
    Dictionary<ChestType, ChestBase> chest_Ref_Dictionary = new();
    Dictionary<ChestType, ObjectPool<ChestBase>> pool_Chest_Dictionary = new();
    ChestBase chestTarget;

    void CreateChestPool()
    {
        List<ChestType> chestRefList = new()
        {
            ChestType.ChestGun,
            ChestType.ChestAbility,
            ChestType.ChestResource,
            //ChestType.ChestShrine,
        };


        for (int i = 0; i < chestBaseTemplateArray.Length; i++)
        {
            var item = chestBaseTemplateArray[i];

            if (item == null)
            {
                Debug.Log("no model here for chest" + item.name);
                continue;
            }

            pool_Chest_Dictionary.Add(chestRefList[i], new ObjectPool<ChestBase>(Chest_Create, null, Chest_ReturnToPool, defaultCapacity: 100));
            chest_Ref_Dictionary.Add(chestRefList[i], chestBaseTemplateArray[i]);
        }


    }

    void Chest_Reset()
    {
        for (int i = 0; i < chestContainer.childCount; i++)
        {
            var item = chestContainer.GetChild(i).gameObject;

            if (item == null) return;

            item.SetActive(false);
        }
    }

    public ChestBase GetChest(ChestType _type, Transform pos)
    {
        ChestBase refChest = chest_Ref_Dictionary[_type];
        chestTarget = refChest;

        ChestBase newChest = pool_Chest_Dictionary[_type].Get();
        newChest.transform.position = pos.position;
        newChest.gameObject.SetActive(true);

        return newChest;
    }


    ChestBase Chest_Create()
    {
        var chest = Instantiate(chestTarget);
        chest.transform.SetParent(chestContainer);
        chestContainer.gameObject.name = "ChestContainer " + chestContainer.childCount.ToString();
        return chest;
    }


    void Chest_ReturnToPool(ChestBase ps)
    {
        ps.ResetForPool();
        ps.transform.localPosition = Vector3.zero;
        ps.transform.SetParent(chestContainer);
        chestContainer.gameObject.name = "ChestContainer " + chestContainer.childCount.ToString();
        //i need to return this to the player as well.
    }



    public void Chest_Release(ChestType _type, ChestBase chest)
    {
        pool_Chest_Dictionary[_type].Release(chest);
    }


    #endregion

    #region TRAP

    [Separator("Traps")]
    [SerializeField] TrapBase[] trapTemplateArray;
    [SerializeField] Transform trapContainer;
    Dictionary<TrapType, TrapBase> trap_Ref_Dictionary = new();
    Dictionary<TrapType, ObjectPool<TrapBase>> pool_Trap_Dictionary = new();
    TrapBase trapTarget;

    void CreateTrapPool()
    {
        List<TrapType> trapRefList = new()
        {
            TrapType.BearTrap
            //ChestType.ChestShrine,
        };


        for (int i = 0; i < trapTemplateArray.Length; i++)
        {
            var item = trapTemplateArray[i];

            if (item == null)
            {
                Debug.Log("no model here for chest" + item.name);
                continue;
            }

            pool_Trap_Dictionary.Add(trapRefList[i], new ObjectPool<TrapBase>(Trap_Create, null, Trap_ReturnToPool, defaultCapacity: 100));
            trap_Ref_Dictionary.Add(trapRefList[i], trapTemplateArray[i]);
        }


    }

    void Trap_Reset()
    {
        for (int i = 0; i < trapContainer.childCount; i++)
        {
            var item = trapContainer.GetChild(i).gameObject;

            if (item == null) return;

            item.SetActive(false);
        }
    }

    public TrapBase GetTrap(TrapType _type, Transform pos)
    {
        TrapBase refTrap = trap_Ref_Dictionary[_type];
        trapTarget = refTrap;

        TrapBase newTrap = pool_Trap_Dictionary[_type].Get();
        newTrap.transform.position = pos.position;
        newTrap.gameObject.SetActive(true);

        return newTrap;
    }


    TrapBase Trap_Create()
    {
        var trap = Instantiate(trapTarget);
        trap.transform.SetParent(chestContainer);
        trapContainer.gameObject.name = "TrapContainer " + trapContainer.childCount.ToString();
        return trap;
    }


    void Trap_ReturnToPool(TrapBase trap)
    {
        trap.ResetForPool();
        trap.transform.localPosition = Vector3.zero;
        trap.transform.SetParent(trapContainer);
        trapContainer.gameObject.name = "TrapContainer " + trapContainer.childCount.ToString();
        //i need to return this to the player as well.
    }



    public void Trap_Release(TrapType _type, TrapBase trap)
    {
        pool_Trap_Dictionary[_type].Release(trap);
    }


    #endregion


}


//we use this to call different particle systems
public enum PSType 
{ 
    Dash_01,
    Blood_01,
    GroundCrack_01,
    BlackWholeForBossCollection_01,
    Dash_02

}

public enum ProjectilType
{
    PlayerRegular = 0,
    EnemySpit = 1,
    FlyingSwords = 2,
    DarkOrb = 3,
    Skull_Seeker = 4
}
