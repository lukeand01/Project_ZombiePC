using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtilleryWheel : MonoBehaviour
{
    [SerializeField] Rigidbody _rb;
    [SerializeField] BoxCollider _boxCollider;

    Vector3 originalPosition;
    Vector3 originalRotation;

    private void Awake()
    {
        originalPosition = transform.localPosition ;
        originalRotation = transform.localRotation.eulerAngles ;
    }

    //
    public void KickWheel(Vector3 objectPosition)
    {

        _boxCollider.enabled = true;

        Vector3 dir = transform.position - objectPosition;
        dir.Normalize();

        _rb.constraints = RigidbodyConstraints.None;
        _rb.AddForce(dir * 15, ForceMode.Impulse);

    }

    public void LockWheel()
    {
        _boxCollider.enabled = false;
        _rb.constraints = RigidbodyConstraints.FreezeAll;
    }


    public void ResetWheel()
    {
        _boxCollider.enabled = false;
        _rb.constraints = RigidbodyConstraints.FreezeAll;

        transform.DOLocalMove(originalPosition, 0);
        transform.DOLocalRotate(originalRotation, 0);
    }

}
