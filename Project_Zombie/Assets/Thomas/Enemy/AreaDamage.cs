using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaDamage : MonoBehaviour
{
    //this will receive cordinates. go to the cordinate and prepare the ui
    //then deal damage in time.

    [SerializeField] AbilityIndicatorCanvas _indicatorCanvas;

    DamageClass _damage;

    float current;
    float total;

    float radius;

    LayerMask targetLayer;

    public void SetUp(Vector3 pos, float radius, float timer, DamageClass damage, int layer)
    {
        pos.y = 0;
        transform.position = pos;

        this.radius = radius;
        _indicatorCanvas.StartCircleIndicator(radius);

       _damage = damage;

        total = timer;

        targetLayer |= (1 << layer);
    }

    private void FixedUpdate()
    {
        if(current > total)
        {
            DealDamage();
            Destroy(gameObject);
        }
        else
        {
            _indicatorCanvas.ControlCircleFill(current, total);

            current += Time.fixedDeltaTime;
        }
    }

    void DealDamage()
    {
        //we check in this area and deal damage to those you cannot.


        RaycastHit[] targets = Physics.SphereCastAll(transform.position, radius, Vector3.up, 0, targetLayer);

        foreach(var item in targets) 
        {
            IDamageable targetDamageable = item.collider.GetComponent<IDamageable>();
            Debug.Log("found fella");
            if (targetDamageable == null) continue;
            targetDamageable.TakeDamage(_damage);

        }

    }

}
