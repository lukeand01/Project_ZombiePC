using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroySelf : MonoBehaviour
{
    float current;
    float total;
    BulletScript bullet;
    bool isEnemy;
    public void SetUpDestroy(bool isEnemy, float total, BulletScript bullet)
    {
        this.isEnemy = isEnemy;
        current = 0; 
        this.total = total;
        this.bullet= bullet;
    }

    private void Update()
    {
        if(current >= total)
        {
            if (bullet != null)
            {
                int index = 0;

                if (isEnemy) index = 1;


                GameHandler.instance._pool.Bullet_Release(index,bullet);
            }
            else
            {
                Destroy(gameObject);
            }
            
        }
        else
        {
            current += Time.deltaTime;
        }
    }
}
