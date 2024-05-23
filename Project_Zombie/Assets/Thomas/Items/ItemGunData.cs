using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.UIElements;

[CreateAssetMenu(menuName = "Item / Gun / TempGun")]
public class ItemGunData : ItemData
{
    //basically every basic gun that behaves normally will take from this.

    [Range(1, 20)] public int bulletPerShot = 1;
    public float bulletOffset;
    [SerializeField] StatClass[] gunBaseStat;
    public GameObject gunModel;
    public BulletScript bulletTemplate;
    [field:SerializeField] public List<BulletBehavior> bulletBehaviorList { get; private set; } = new(); //the amount of stuff that a single bullet from this will do. like setting people on fire and dealing damage.
    public bool canHoldDownButtonToKeepShooting;


    private void OnEnable()
    {
        FormStatList();
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


    //butllet per shot is modifier
    //also we should be able to get damage from it
    //i can just put those variables in the gun? but then i would need to double check everytime.

    public void Shoot(GunClass gun, string ownerId, BulletScript bulletTemplate, Vector3 gunDir, List<BulletBehavior> newBulletBehaviorList)
    {
        //the shoot behavior.
        //we can change and replcae the behavior so its better for the class to keep it.

        Transform gunPointPosition = gun.gunPoint;

        PlayerCombat combat = PlayerHandler.instance._playerCombat;




        

        for (int i = 0; i < gun.bulletPerShot; i++)
        {


            float spread = Random.Range(-bulletOffset, bulletOffset);
            Vector3 direction = Quaternion.Euler(0f, spread, 0f) * gunDir;



            BulletScript newBullet = Instantiate(bulletTemplate, gunPointPosition.position, Quaternion.identity);
            newBullet.SetUp(ownerId, direction);

            

            newBullet.MakeDamage(gun._DamageClass, 0, 0);
            newBullet.MakeSpeed(25, 0, 0);


            

            newBullet.MakeBulletBehavior(newBulletBehaviorList);
        }



        //i need to receive
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
        //Debug.LogError("add gun passive failed " + itemName);
    }
    public virtual void RemoveGunPassives()
    {
        //Debug.LogError("remove gun passive failed " + itemName);
    }

    public override ItemGunData GetGun() { return this; }
}

