using DG.Tweening;
using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonEvent : ButtonBase
{
    public UnityEvent unityEvent;

    //the weay ti

    [Separator("FILL BEHAVIOR")]
    [SerializeField] Image fillImage; //

    [Separator("END ANIMATION")]
    [SerializeField] bool callEndAnimation; //everytime we execute the event we will increse and decrease the button. quickly.
    [SerializeField] bool waitForEndAnimation; //if we have to wait till the animation is over till we can call it again.


    bool isHovering;
    bool isProcess;
    float current;
    float total;


    private void Start()
    {
        
        total = 5f;
    }

    private void Update()
    {

        if(isHovering && fillImage != null && !isProcess)
        {
            if (Input.GetMouseButton(0))
            {
                current += Time.fixedDeltaTime;

                if(current > total)
                {
                    unityEvent.Invoke();
                    StartCoroutine(EndAnimationProcess());
                    return;
                }

            }
            else if(current > 0)
            {
                current -= Time.fixedDeltaTime;
            }

            fillImage.fillAmount = current / total;
        }
    }

    IEnumerator EndAnimationProcess()
    {
        isProcess = true;
        fillImage.fillAmount = 0;
        current = 0;

        float timer = 0.15f;

        transform.DOKill();
        transform.DOScale(1.2f, timer).SetUpdate(true).SetEase(Ease.Linear);

        yield return new WaitForSecondsRealtime(timer);

        transform.DOScale(1f, timer).SetUpdate(true).SetEase(Ease.Linear);

        yield return new WaitForSecondsRealtime(timer);

        isProcess = false;
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {

        base.OnPointerEnter(eventData);
        isHovering = true;
    }
    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
        isHovering = false;
    }

    private void OnDisable()
    {
        
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        if (fillImage != null) return;
        unityEvent.Invoke();
    }
}
