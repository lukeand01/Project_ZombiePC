using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyBase : MonoBehaviour, IDamageable
{
    [Separator("ALLY BASE - ")]
    [SerializeField] EnemyCanvas _canvas;
    [SerializeField] float initialHealth;
    [SerializeField] float initialDuration;
    private void FixedUpdate()
    {
        FixedUpdateFunction();
    }

    protected virtual void FixedUpdateFunction()
    {
        HandleDuration();
    }

    public void SetUp_Ally_WithBaseValues()
    {
        SetUp_Ally(initialHealth, initialDuration);
    }

    public void SetUp_Ally(float health, float duration)
    {
        health_Total = health;
        health_Current = health_Total;

        duration_Total = duration;
        duration_Current = duration_Total ;


        _canvas.UpdateDuration(duration_Current, duration_Total);
    }


    #region DAMAGEABLE

    float health_Current;
    float health_Total;

    public void ApplyBD(BDClass bd)
    {
        
    }

    public string GetID()
    {
        return "Ally";
    }

    public GameObject GetObjectRef()
    {
        return gameObject;
    }

    public float GetTargetCurrentHealth()
    {
        return -1;
    }

    public float GetTargetMaxHealth()
    {
        return -1;
    }

    public bool IsDead()
    {
        return false;
    }

    public void RestoreHealth(float value)
    {
        
    }

    public void TakeDamage(DamageClass damage)
    {
        Debug.Log("take damage");

        float damageValue = damage.GetDamage(0, health_Total, false);


        health_Current -= damageValue;
        _canvas.UpdateHealth(duration_Current, health_Total);

        if(health_Current <= 0)
        {
            Destroy(gameObject);
        }

    }
    #endregion

    #region DURATION
    float duration_Current;
    float duration_Total;

    void HandleDuration()
    {
        if (duration_Total <= 0) return;

        if(duration_Current > 0)
        {
            duration_Current -= Time.fixedDeltaTime;
            _canvas.UpdateDuration(duration_Current, duration_Total);
        }
        else
        {
            CallEndDuration();
        }

    }

    protected virtual void CallEndDuration()
    {
        Destroy(gameObject);
    }

    #endregion



}


