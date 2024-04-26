using MyBox;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AbilityUnit : MonoBehaviour
{
    AbilityClass _abilityClass;


    [Separator("ABILITY UNIT")]
    [SerializeField] Image cooldownImage;
    [SerializeField] TextMeshProUGUI cooldownText;
    [SerializeField] GameObject levelHolder;
    [SerializeField] TextMeshProUGUI levelText;
    [SerializeField] GameObject selected;
    [SerializeField] Image icon;
    [SerializeField] TextMeshProUGUI keycodeText;
    [SerializeField] GameObject empty;

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
}
