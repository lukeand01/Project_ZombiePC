using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CityStoreLevelButton : ButtonBase
{
    //if you hold this long enough you will call the upgrade place.
    //you can press esc to leave this place.

    bool isHovering;
    bool wasCalled;

    float current;
    float total = 1.5f;
    [SerializeField] Image fillImage;

    [SerializeField] CityCanvas _canvasHandler;

    private void Awake()
    {
        total = 0.5f;
    }

    private void FixedUpdate()
    {
        if (wasCalled)
        {
            //we instantly turn it on for a moment.
            return;
        }


        if (isHovering && Input.GetMouseButton(0) && !wasCalled)
        {
            current += Time.fixedDeltaTime;

            if(current > total) 
            {
                wasCalled = true;
                _canvasHandler.Upgrade_Open();
                StartCoroutine(UsedButtonProcess());
                //and we call the 
            }

            fillImage.fillAmount = current / total;
        }
        else if(current > 0)
        {
          
            current -= Time.fixedDeltaTime;

            if (current <= 0)
            {
                wasCalled = false;
                current = 0;
            }

           
        }

        if(fillImage == null)
        {
            Debug.Log("fill image is not found " + gameObject.name);
        }
        fillImage.fillAmount = current / total;
    }


    IEnumerator UsedButtonProcess()
    {
        float timer = 0.35f;
        current = 0;
        fillImage.fillAmount = current / total;
        transform.DOScale(1.25f, timer).SetEase(Ease.Linear).SetUpdate(true);
        yield return new WaitForSecondsRealtime(timer);
        transform.DOScale(1.1f, timer).SetEase(Ease.Linear).SetUpdate(true);
        yield return new WaitForSecondsRealtime(timer);

        wasCalled = false;
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
        isHovering = false;
    }


}
