using UnityEngine;

public class DamageObjectChallenge : MonoBehaviour, IDamageable
{

    string id;
    private void Awake()
    {
        id = MyUtils.GetRandomID();
    }

    public void ApplyBD(BDClass bd)
    {
        //does nothing
    }

    public string GetID()
    {
        return id;
    }

    public GameObject GetObjectRef()
    {
        return gameObject;
    }

    public float GetTargetCurrentHealth()
    {
        return -2;
    }

    public float GetTargetMaxHealth()
    {
        return -2;
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
        //once dead it warns that which triggered it.

    }
}
