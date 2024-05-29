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
        this._quest = _quest;
        descriptionText.text = _quest.questData.quest_Description;
        UpdateUI();
    }


    public void UpdateUI()
    {
        quantityText.text = _quest.amountCurrent + " / " + _quest.amountTotal;
    }

}


//we spawn shrine in a random place of the options
//we keep on spawning at long cooldowns but we can have at most 3.
//decided the quest and type in the moment of itneraction
//we call ui to make the reveal.
//