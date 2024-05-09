using MyBox;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AbilityUnit : ButtonBase
{
    AbilityClass _abilityClass;
    public AbilityPassiveData _abilityPassiveData {  get; private set; }

    [Separator("ABILITY UNIT")]
    [SerializeField] Image cooldownImage;
    [SerializeField] TextMeshProUGUI cooldownText;
    [SerializeField] GameObject levelHolder;
    [SerializeField] TextMeshProUGUI levelText;
    [SerializeField] GameObject selected;
    [SerializeField] Image icon;
    [SerializeField] TextMeshProUGUI keycodeText;
    [SerializeField] GameObject empty;

    private void Update()
    {
        if(Time.timeScale > 0)
        {
            selected.SetActive(false);
        }
    }


    public void SetUpActive(AbilityClass ability, int index)
    {
        if (ability.IsEmpty())
        {
            empty.SetActive(true);
            return;
        }

        ability.SetUI(this);
        empty.SetActive(false);
        SetUpBase(ability.dataActive);      
        _abilityClass = ability;

        keycodeText.text = (index + 1).ToString();


    }
    public void SetUpPassive(AbilityClass ability)
    {
        _abilityClass = ability;
        _abilityPassiveData = ability.dataPassive;

        ability.SetUI(this);
        empty.SetActive(false);

        icon.sprite = _abilityPassiveData.abilityIcon;

        levelHolder.SetActive(true);
        UpdatePassiveLevel();

        cooldownImage.gameObject.SetActive(false);
        keycodeText.gameObject.SetActive(false);
    }

    public void UpdatePassiveLevel()
    {
        levelText.text = _abilityClass.level.ToString();
    }

    void SetUpBase(AbilityBaseData data)
    {
        icon.sprite = data.abilityIcon;
        cooldownImage.gameObject.SetActive(false);
        levelHolder.SetActive(false);
    }

    public void UpdateCooldown(float current, float total)
    {
        cooldownImage.gameObject.SetActive(current > 0);
        cooldownImage.fillAmount = current / total;
        cooldownText.text = current.ToString("f1"); 

    }


    public override void OnPointerEnter(PointerEventData eventData)
    {
        if(_abilityClass == null)
        {
            Debug.Log("same");
            return;
        }

        if (_abilityClass.IsEmpty())
        {
            Debug.Log("is empty");
            return;
        }

        if(Time.timeScale == 0)
        {
            selected.SetActive(true);
            UIHandler.instance._pauseUI.DescribeAbiliy(_abilityClass, transform);
        }
    }
    public override void OnPointerExit(PointerEventData eventData)
    {
        if (Time.timeScale == 0)
        {
            selected.SetActive(false);
            UIHandler.instance._pauseUI.StopDescription();
        }
    }

    public void DestroyItself()
    {
        Destroy(gameObject);
    }

}
