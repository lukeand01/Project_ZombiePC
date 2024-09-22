using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.UIElements;

[CreateAssetMenu(menuName = "Item / Gun / TempGun")]
public class ItemGunData : ItemData
{
    //basically every basic gun_Perma that behaves normally will take from this.

    [Range(1, 20)] public int bulletPerShot = 1;
    public float bulletOffset;
    [SerializeField] StatClass[] gunBaseStat;
    public GameObject gunModel;
    public BulletScript bulletTemplate;
    [field:SerializeField] public List<BulletBehavior> bulletBehaviorList { get; private set; } = new(); //the amount of stuff that a single bullet from this will do. like setting people on fire and dealing damage.
    public bool canHoldDownButtonToKeepShooting;
    [field:SerializeField] public int goThroughPower;
    [field: SerializeField] public MouseUIType mouseImageType;
    [field: SerializeField] public AnimationState_UpperBody animationType;

    [Separator("DAMAGE LIST")]
    [SerializeField] List<DamageTypeClass> damageList = new(); //
    public List<DamageTypeClass> GetDamageList { get { return damageList; } }

    [Separator("SOUNDS")]
     [SerializeField]AudioClip audio_Shoot;
     [SerializeField]AudioClip audio_Reload;

    [Separator("STORE")]
    [SerializeField] bool store;
    [field:SerializeField] public bool isTemp { get; private set; }
    [field: SerializeField] public RequirementToUnluckClass requirementToUnluck { get; private set; }


    [field: SerializeField] public int additionalRollRequired { get; private set; }//if its found then we need to roll again.

    


    public List<string> GetStringPriceList()
    {
        List<ResourceClass> resourceList = requirementToUnluck.requiredResourceList;
        List<string> stringList = new();


        foreach (var item in resourceList)
        {
            string text = "";

            text += item.data.itemName + ": ";
            text += item.quantity.ToString();

            stringList.Add(text);
        }

        return stringList;
    }

    [field:SerializeField] public int storeIndex {  get; private set; }

    public void SetIndex(int index)
    {
        storeIndex = index;
    }


    public AudioClip Get_Audio_Reload { get { return audio_Reload; } }

    //the reload is more complex buecause it needs to persist while the player is reloading and stop if the player is no longer reloading.

    [Separator("ONLY TEMP GUNS")]
    [SerializeField] bool hasBeenFound;

    public bool HasBeenFound { get { return hasBeenFound; } }

    public void SetHasBeenFound(bool hasBeenFound)
    {
        this.hasBeenFound = hasBeenFound;
    }

    private void Awake()
    {
        
    }

    private void OnEnable()
    {
        FormStatList();

        if (animationType == AnimationState_UpperBody.Idle)
        {
            if (isTemp)
            {
                animationType = AnimationState_UpperBody.Rifle;
            }
            else
            {
                animationType = AnimationState_UpperBody.Pistol;
            }
        }
    }

    [ContextMenu("Form Stat List")]
    public void FormStatList()
    {
        if (gunBaseStat.Length <= 0)
        {
            gunBaseStat = new StatClass[] {
            new StatClass(StatType.Damage, 0),
            new StatClass(StatType.Pen, 0),
            new StatClass(StatType.CritChance, 0),
            new StatClass(StatType.CritDamage, 0),
            new StatClass(StatType.ReloadSpeed, 0),
            new StatClass(StatType.Magazine, 0),
            new StatClass(StatType.FireRate, 0)};
        }
    }

    public StatClass[] GetGunStatList() => gunBaseStat;

    //butllet per shot is _value
    //also we should be able to get damage from it
    //i can just put those variables in the gun_Perma? but then i would need to double check everytime.

    IDamageable playerDamageable; //terrible solution, but it works for now.

    public virtual void Shoot(GunClass gun, string ownerId, BulletScript bulletTemplate, Vector3 gunDir, List<BulletBehavior> newBulletBehaviorList)
    {
        //the shoot behavior.
        //we can change and replcae the behavior so its better for the class to keep it.

        Transform gunPointPosition = gun.gunPoint;



        if(playerDamageable == null)
        {
            playerDamageable = PlayerHandler.instance._playerResources;
        }

        int goThroughPower = gun.GoThroughPower_Total;


        for (int i = 0; i < gun.bulletPerShot; i++)
        {


            float spread = Random.Range(-bulletOffset, bulletOffset);
            Vector3 direction = Quaternion.Euler(0f, spread, 0f) * gunDir;



            BulletScript newBullet = GameHandler.instance._pool.GetBullet(ProjectilType.PlayerRegular, gunPointPosition);


            newBullet.SetUp(ownerId, direction);

            //
            
            newBullet.MakeDamage(gun._DamageClass, 0, 0);
            newBullet.MakePlayerDamageableRef(playerDamageable);
            newBullet.MakeSpeed(50, 0, 0);
            newBullet.MakeCollision(goThroughPower);



            newBullet.MakeBulletBehavior(newBulletBehaviorList);
        }


        //GameHandler.instance._soundHandler.CreateSfx_WithAudioClip(audio_Shoot);

        //i need to receive
    }

    public virtual void CallCharge(ItemClass item)
    {
        //we do nothing here.
    }

    public float GetValue(StatType stat)
    {
        foreach (var item in gunBaseStat)
        {
            if(item.stat == stat)
            {
                return item.value;  
            }
        }

        return 0;
    }

    //each gundata will have passiveabilities linked to it, but onyl when you equip a perma are they used.

    public virtual void AddGunPassives()
    {
        //Debug.LogError("add gun_Perma passive failed " + itemName);
    }
    public virtual void RemoveGunPassives()
    {
        //Debug.LogError("remove gun_Perma passive failed " + itemName);
    }

    public override ItemGunData GetGun() { return this; }

    public virtual ItemGunDataCharge GetGunCharge() { return null; }
}

