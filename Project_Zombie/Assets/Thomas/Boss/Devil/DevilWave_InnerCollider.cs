using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevilWave_InnerCollider : MonoBehaviour
{
    [SerializeField] DevilWave_OuterCollider _outerCollider;


    private void OnTriggerEnter(Collider other)
    {

       
        _outerCollider.SetAlreadyDamaged();
    }

}
