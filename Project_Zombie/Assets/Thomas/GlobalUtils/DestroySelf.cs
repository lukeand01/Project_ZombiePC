using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroySelf : MonoBehaviour
{
    float current;
    float total;
    BulletScript bullet;
    public void SetUpDestroy(float total, BulletScript bullet)
    {
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
                GameHandler.instance._pool.Bullet_Release(bullet);
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
