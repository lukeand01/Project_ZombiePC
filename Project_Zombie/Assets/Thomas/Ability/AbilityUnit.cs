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

    bool isPause = false;
    bool isEnd = false;

    private void Start()
    {
       
    }

    private void Update()
    {
         isPause = UIHandler.instance._pauseUI.IsPauseOn();
         isEnd = UIHandler.instance._EndUI.IsEnd();


        if(!isPause && !isEnd)
        {
            selected.SetActive(false);
        }
    }



    public void SetUpActive(AbilityClass ability, int index)
    {
        gameObject.name = "ActiveUnit " + Random.Range(0, 100).ToString();
        ability.SetUI(this);
        _abilityClass = ability;
        keycodeText.text = (index + 1).ToString();
        



        if (ability.IsEmpty())
        {
            empty.SetActive(true);
            return;
        }


        empty.SetActive(false);
        SetUpBase(ability.dataActive);      


      


    }

    public void SetUpPassive(AbilityClass ability)
    {
        _abilityClass = ability;
        _abilityPassiveData = ability.dataPassive;

        gameObject.name = "PassiveUnit " + Random.Range(0, 1000).ToString();
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

    public void UpdateCooldown(float current, float total, bool displayAsInt = false)
    {
        cooldownImage.gameObject.SetActive(current > 0);
        cooldownImage.fillAmount = current / total;

        if (displayAsInt)
        {
            cooldownText.text = current.ToString("f0");
        }
        else
        {
            cooldownText.text = current.ToString("f1");
        }

       

    }

    [Separator("CHARGE UI")]
    [SerializeField] GameObject chargeHolder;
    [SerializeField] Image chargeFill;

    public void UpdateChargeFill(float current, float total)
    {
        chargeHolder.SetActive(current > 0);
        chargeFill.fillAmount = current / total;
    }


    public void UpdateActiveUI()
    {


        if (_abilityClass == null) return;


        if (_abilityClass.dataActive != null)
        {

            empty.SetActive(false);
            icon.sprite = _abilityClass.dataActive.abilityIcon;
        }
        else
        {

            empty.SetActive(true);
        }
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        if(_abilityClass == null)
        {

            return;
        }

        if (_abilityClass.IsEmpty())
        {

            return;
        }


        if (isPause || isEnd)
        {
            selected.SetActive(true);
            UIHandler.instance._pauseUI.DescribeAbiliy(_abilityClass, transform);
        }
    }
    public override void OnPointerExit(PointerEventData eventData)
    {

        if (isPause || isEnd)
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
