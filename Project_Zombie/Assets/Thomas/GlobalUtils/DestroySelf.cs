using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroySelf : MonoBehaviour
{
    float current;
    float total;
    public void SetUpDestroy(float total)
    {
        current = 0; 
        this.total = total;
    }

    private void Update()
    {
        if(current >= total)
        {
            Destroy(gameObject);
        }
        else
        {
            current += Time.deltaTime;
        }
    }
}
