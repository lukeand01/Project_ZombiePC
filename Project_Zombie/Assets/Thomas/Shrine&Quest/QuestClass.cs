using MyBox;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Windows.WebCam;

[System.Serializable]
public class QuestClass 
{
    //the quest creates a questclass.
    //it will store the progress.
    //but how do i decide the reward?
    //i want to do a better dvision of those two to not wya to much information that i dont require.

    //is it worth?
    public string id { get; private set; }

    [Separator("BASE")]
    [SerializeField] string editorName;
    [field: SerializeField] public QuestData questData {  get; private set; }
    [field: SerializeField]public int amountTotal {  get; private set; }


    [SerializeField] List<Quest_RewardClass> rewardList = new(); //these are all the reawrds once you are done with this thing.
   

    [Separator("Only for Stage")]
    [SerializeField] List<Quest_RewardClass> punishList = new();
    [field: SerializeField] public QuestType questType { get; private set; }
    [field: SerializeField] public float timerTotal { get; private set; }

    [Separator("Only for Story")]
    [SerializeField] string questName;
    [SerializeField][TextArea] string questDescription;
    [SerializeField] string questGiverName;

    public string GetQuestName { get { return questName; } }
    public string GetDescription_Story { get { return questDescription; } }
    public string GetQuestGiverName { get { return questGiverName; } }

    public int amountCurrent { get; private set; }

    public float timerCurrent { get; private set; }


    PlayerResources _playerQuestHandler;

    public QuestClass(QuestData data)
    {



    }

    public QuestClass(QuestClass refClass)
    {
        //we will use this.
        questData = refClass.questData;
        amountTotal = refClass.amountTotal;
        timerTotal = refClass.timerTotal;
        questType = refClass.questType;
        
        foreach (var item in refClass.rewardList)
        {
            rewardList.Add(new Quest_RewardClass(item.data, item.value));
        }
        foreach (var item in refClass.punishList)
        {
            punishList.Add(new Quest_RewardClass(item.data, item.value));
        }

    }

    public void SetID(string id, PlayerResources _playerQuestHandler)
    {
        this.id = id;
        this._playerQuestHandler = _playerQuestHandler;
    }

    #region QUEST PROGRESS
    public void ProgressTimer()
    {
        if(timerTotal <= 0)
        {
            return;
        }

        timerCurrent += Time.fixedDeltaTime;
        UpdateBarQuestUnit(timerCurrent, timerTotal);

        if(timerCurrent > timerTotal)
        {
            PunishQuest();
        }
    }

    public void ProgressQuest(int value)
    {
        amountCurrent += value;
        UpdateQuestUnit();



        if(amountCurrent >= amountTotal)
        {
            FinishQuest();
        }
    }
    #endregion

    #region QUEST END
    void FinishQuest()
    {

        foreach (var item in rewardList)
        {
            item.data.ReceiveReward(item);
        }


        OrderQuestUnitToEnd();
        RemoveQuestFromPlayer();
    }
    void PunishQuest()
    {
        foreach (var item in punishList)
        {
            item.data.ReceiveReward(item);
        }

        OrderQuestUnitToEnd();
        RemoveQuestFromPlayer();
    }

    void RemoveQuestFromPlayer()
    {
        if (_playerQuestHandler != null)
        {
            _playerQuestHandler.RemoveQuest(id);
        }
        else
        {
            Debug.Log("there was no questhandler");
        }
    }
    #endregion
    //need to inform the player.


    #region DESCRIPTION
    public string GetDescription()
    {
        
        string originalString = questData.quest_Description;
        char letterToFind = '#';
        string valueToReplace = amountTotal.ToString();

        string description = ReplaceLetterCaseInsensitive(originalString, letterToFind, valueToReplace);

        return description;
    }

    public string GetDescription_Reward()
    {
        //reward i must pick every fella and replace the letter.
        //
        string result = "";

        foreach (var item in rewardList)
        {
            string originalString = item.data.rewardDescription;
            char letterToFind = '#';
            string valueToReplace = item.value.ToString();

            string description = ReplaceLetterCaseInsensitive(originalString, letterToFind, valueToReplace);

            result += "-";
            result += description + "\\";

        }

        return "";
    }

    string ReplaceLetterCaseInsensitive(string input, char letterToFind, string stringToReplace)
    {
        StringBuilder sb = new StringBuilder();
        char lowerLetterToFind = char.ToLower(letterToFind);
        char upperLetterToFind = char.ToUpper(letterToFind);

        foreach (char c in input)
        {
            if (c == lowerLetterToFind || c == upperLetterToFind)
            {
                sb.Append(stringToReplace);
            }
            else
            {
                sb.Append(c);
            }
        }

        return sb.ToString();
    }
    #endregion
    #region UI
    QuestUnit _questUnit;

    void UpdateQuestUnit()
    {
        if(_questUnit != null)
        {
            _questUnit.UpdateUI();
        }

    }
    void OrderQuestUnitToEnd()
    {
        if(_questUnit != null)
        {
            _questUnit.CompleteQuest();
        }
        else
        {
            Debug.Log("There was no quest unit here");
        }
    }
    public void SetUnit(QuestUnit _questUnit)
    {
        this._questUnit = _questUnit;
    }

    void UpdateBarQuestUnit(float current, float total)
    {
        if (_questUnit != null)
        {
            _questUnit.UpdateBar(current, total);
        }
    }
    #endregion

}



//