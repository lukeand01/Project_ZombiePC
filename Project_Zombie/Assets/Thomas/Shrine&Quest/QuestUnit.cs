using DG.Tweening;
using MyBox;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestUnit : ButtonBase
{
    QuestClass _quest;

    [Separator("QUEST")]
    [SerializeField] TextMeshProUGUI quantityText;
    [SerializeField] TextMeshProUGUI descriptionText;
    [SerializeField] GameObject barHolder;
    [SerializeField] Image bar;

    public void SetUp(QuestClass _quest)
    {

        if(_quest == null)
        {
            Debug.Log("quest class");
            return;
        }
        if(_quest.questData == null)
        {
            Debug.Log("quest data");
            return;
        }

        barHolder.SetActive(false);

        this._quest = _quest;
        _quest.SetUnit(this);
        descriptionText.text = _quest.GetDescription();
        UpdateUI();
    }


    public void UpdateUI()
    {
        quantityText.text = _quest.amountCurrent + " / " + _quest.amountTotal;
    }

    public void UpdateBar(float current, float total)
    {
        barHolder.SetActive(true);

        bar.fillAmount = current / total;
    }

    public void SelectEffect(float timer)
    {
        StartCoroutine(SelectEffectProcess(timer));
    }
    IEnumerator SelectEffectProcess(float timer)
    {
        transform.DOScale(1.3f, timer).SetUpdate(true);

        yield return new WaitForSecondsRealtime(timer);

        transform.DOScale(1.2f, timer).SetUpdate(true);
    }

    public void CompleteQuest()
    {
        //do an effect then order its destruction.
        Destroy(gameObject);
    }

}


//we spawn shrine in a random place of the options
//we keep on spawning at long cooldowns but we can have at most 3.
//decided the quest and type in the moment of itneraction
//we call ui to make the reveal.
//