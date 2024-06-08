using DG.Tweening;
using MyBox;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class BlessUnit : ButtonBase
{
    QuestClass _questClass;

    [Separator("BLESS")]
    [SerializeField] TextMeshProUGUI titleText;
    [SerializeField] TextMeshProUGUI descriptionText;
    [SerializeField] TextMeshProUGUI rewardText;

    public void SetUp(QuestClass _questClass)
    {
        this._questClass = _questClass; 

        descriptionText.text = _questClass.GetDescription();
        rewardText.text = _questClass.GetDescription_Reward();

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

    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);
        //we will pass this quest to the player to add and start the game.
        UIHandler.instance._QuestUI.SelectQuest(this, _questClass);
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        ControlSelected(true);
    }
    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
        ControlSelected(false);
    }

    private void OnDisable()
    {
        ControlSelected(false);
    }
}
