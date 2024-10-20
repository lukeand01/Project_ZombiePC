using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TreeLog : MonoBehaviour
{

    bool isFalling;

    [SerializeField] GameObject _attackUIHolder;
    [SerializeField] Image _attackUIBar;
    [SerializeField] GameObject _logGraphic;
    [SerializeField] BoxCollider _boxCollider;
    public void StartLog()
    {
        _logGraphic.transform.localPosition = new Vector3(0, 0, -1);
        _attackUIHolder.SetActive(true);
        _attackUIBar.fillAmount = 0;

        _logGraphic.transform.DOLocalMoveY(0, 1.2f).SetEase(Ease.Linear).OnComplete(DealDamage);
        _attackUIBar.DOFillAmount(1, 1).SetEase(Ease.Linear);
    }
    public void CancelLog()
    {
        _logGraphic.transform.DOKill();
        _attackUIBar.DOKill();

        gameObject.SetActive(false);
    }

    void DealDamage()
    {
        //we turn on the boxcollider.
        //we set a delay and we 
        _boxCollider.enabled = true;
        PlayerHandler.instance.TryToCallExplosionCameraEffect(transform, 0.3f);
        Invoke(nameof(TurnOffDamageCollider), 0.05f);
    }

    void TurnOffDamageCollider()
    {
        _boxCollider.enabled = false;
    }


    private void OnTriggerEnter(Collider other)
    {
        //then we push the player away. and deal a bunch of damage. or do we just insta kill?

        if (!isFalling) return;
        if (other.gameObject.layer != 3) return;


        PlayerHandler.instance._playerResources.TakeDamage(new DamageClass(999, DamageType.Physical, 50));

    }
}
