using MyBox;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FlyUnit : ButtonBase
{
    [Separator("FLY")]
    [SerializeField] Image fillImage_Cooldown;
    [SerializeField] Image fillImage_Use;
    [SerializeField] TextMeshProUGUI cooldownText;
    [SerializeField] GameObject selected;


    public void UpdateCooldown_Cooldown(float current, float total)
    {


        fillImage_Cooldown.fillAmount = current / total;

        cooldownText.gameObject.SetActive(current > 0);
        cooldownText.text = current.ToString("f1");

        

        fillImage_Cooldown.gameObject.SetActive(true);
        fillImage_Use.gameObject.SetActive(false);
    }
    public void UpdateCooldown_Use(float current, float total)
    {


        fillImage_Use .fillAmount = current / total;

        fillImage_Cooldown.gameObject.SetActive(false);
        fillImage_Use.gameObject.SetActive(true);
        //we change the color based in 
    }




    private void Update()
    {
        if (Time.timeScale > 0)
        {
            selected.SetActive(false);
        }


    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);

        if (Time.timeScale == 0)
        {
            selected.SetActive(true);
            UIHandler.instance._pauseUI.DescribeDash(transform);
        }

    }
    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);

        if (Time.timeScale == 0)
        {
            selected.SetActive(false);
            UIHandler.instance._pauseUI.StopDescription();
        }
    }



}
