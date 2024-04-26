using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

[CreateAssetMenu(menuName = "Item / Gun / GunBase")]
public class ItemGunData : ItemData
{
    //basically every basic gun that behaves normally will take from this.

    public float damagePerBullet;
    [Range(1, 20)] public int bulletPerShot = 1;
    public float bulletOffset;
    [SerializeField] StatClass[] gunBaseStat;
    public GameObject gunModel;
    public BulletScript bulletTemplate;
    public List<BulletBehavior> bulletBehaviorList = new(); //the amount of stuff that a single bullet from this will do. like setting people on fire and dealing damage.

    private void OnEnable()
    {


        if (gunBaseStat.Length <= 0)
        {
            gunBaseStat = new StatClass[] {new StatClass(StatType.Damage, 0),
            new StatClass(StatType.Pen, 0),
            new StatClass(StatType.CritChance, 0),
            new StatClass(StatType.CritDamage, 0),
            new StatClass(StatType.ReloadSpeed, 0),
            new StatClass(StatType.Magazine, 0),
            new StatClass(StatType.FireRate, 0)};
        }
    }



    public void Shoot(GunClass gun, string ownerId, BulletScript bulletTemplate, Vector3 gunDir, List<BulletBehavior> newBulletBehaviorList)
    {
        //the shoot behavior.
        //we can change and replcae the behavior so its better for the class to keep it.

        Transform gunPointPosition = gun.gunPoint;

       // BDHandler bdHandler = PlayerHandler.instance.playerResource.GetBDHandler();

        float increaseAngle = 5;

        for (int i = 0; i < bulletPerShot; i++)
        {
            //we create a new one for each fella.
            float offset = Random.Range(-bulletOffset, bulletOffset);
            Vector3 direction = Quaternion.Euler(offset, 0, offset) * gunDir;
            increaseAngle += 10;

            BulletScript newBullet = Instantiate(bulletTemplate, gunPointPosition.position, Quaternion.identity);
            newBullet.SetUp(ownerId, direction);

            //need toi tell the basedamage.
            //need to tell pen
            //

            //the damage should come from the gunclass.


            newBullet.MakeDamage(gun._DamageClass, 0, 0);
            newBullet.MakeSpeed(25f, 0, 0);

            newBullet.MakeBulletBehavior(bulletBehaviorList);
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

    public override ItemGunData GetGun() { return this; }
}

