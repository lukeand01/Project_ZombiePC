using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ChestAbilityUnit : ButtonBase
{
    //to show info about abilities.
    //should also show if the player already have 

    //tier, name, the stats.

    [SerializeField] Image icon;
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI tierText;
    [SerializeField] GameObject selected;
    [SerializeField] TextMeshProUGUI stackText;
    [SerializeField] TextMeshProUGUI descriptionText;




    ChestUI _chestUI;

    AbilityPassiveData data;

    public void SetUp(AbilityPassiveData data, ChestUI _chestUI)
    {

        transform.localScale = Vector3.zero;
        transform.DOKill();
        transform.DOScale(new Vector3(5, 6.2f, 0), 0.3f).SetUpdate(true);
        selected.transform.localScale = new Vector3(0.95f, 0.95f, 0);

        this._chestUI = _chestUI;
        this.data = data;

        icon.sprite = data.abilityIcon;
        nameText.text = data.abilityName;
        tierText.text = data.abilityTier.ToString();
        descriptionText.text = data.abilityDescription.ToString();

        //now i need to ask if i have already have this fella and how much 
        AbilityClass ability = PlayerHandler.instance._playerAbility.GetTargetAbilityClass(data);

        if (!ability.IsEmpty())
        {
            stackText.text = ability.level.ToString();
        }
        else
        {
            stackText.text = "New!";
        }


    }


    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);

        if(data == null)
        {
            //there was nothing here.
            Debug.Log("no data?");
        }

        _chestUI.ChooseAbility(data);

    }
    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);

        selected.transform.DOKill();
        selected.transform.DOScale(1.05f, 0.15f).SetUpdate(true);
        selected.gameObject.SetActive(true);


    }
    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);

        selected.transform.DOKill();
        selected.transform.DOScale(0.95f, 0.15f).SetUpdate(true);

        selected.SetActive(false);
    }

    


}
