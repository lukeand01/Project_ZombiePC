using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroySelf : MonoBehaviour
{
    float current;
    float total;
    BulletScript bullet;
    int index = 0;
    bool canRun;
    public void SetUpDestroy(int index, float total, BulletScript bullet)
    {
        canRun = true;
        this.index = index;
        current = 0; 
        this.total = total;
        this.bullet= bullet;
    }

    private void Update()
    {
        if (!canRun) return;

        if(current >= total)
        {
            if (bullet != null)
            {
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
