using DG.Tweening;
using MyBox;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class QuestUnit : ButtonBase
{

    //i will use this to show in other place. but also i want for more information on hover so i need to especify the type of fella.

    QuestClass _quest;
    QuestUI _questHandler;

    [Separator("QUEST")]
    [SerializeField] TextMeshProUGUI quantityText;
    [SerializeField] TextMeshProUGUI descriptionText;
    [SerializeField] GameObject barHolder;
    [SerializeField] Image bar;
    [SerializeField] GameObject titleHolder;
    [SerializeField] TextMeshProUGUI titleText;
    [SerializeField] Image presentationImage;
    [SerializeField] Image questTypeImage;

    [Separator("COLOR")]
    [SerializeField] Color color_Curse;
    [SerializeField] Color color_Bless;


    bool isStory = false;




    #region CHALLENGES

    public void SetUp_Challenge(QuestClass _quest, QuestUI _questHandler)
    {
       
        if (_quest == null)
        {
            Debug.Log("quest class");
            return;
        }
        if (_quest.questData == null)
        {
            Debug.Log("quest data");
            return;
        }

        //we check if the ui is open, if not with set an event with it.
        if(_quest.questType == QuestType.Bless)
        {
            questTypeImage.color = color_Bless;
        }
        else if (_quest.questType == QuestType.Curse)
        {
            questTypeImage.color = color_Curse;
        }

        this._questHandler = _questHandler;
        this._quest = _quest;
        _quest.SetUnit(this);
        descriptionText.text = _quest.GetDescription();
        UpdateUI();

        titleText.text = _quest.questType.ToString();

        if (_questHandler.IsOpen)
        {
            StartCoroutine(StartPresentation());
        }
        else
        {
            //we assign to an event. we dont actually need to.
            _questHandler.eventOpenUI += CallPresentationWhenUIIsOpen;
        }

        
    }


    void CallPresentationWhenUIIsOpen()
    {
        StartCoroutine(StartPresentation());
        _questHandler.eventOpenUI -= CallPresentationWhenUIIsOpen;
    }

    IEnumerator StartPresentation()
    {
       //diff between curse and challenge.
       //

        titleHolder.gameObject.SetActive(true);
        

        float timer = 1.2f;

        titleText.transform.DOScale(1.4f, timer / 2).SetEase(Ease.Linear);
        presentationImage.DOFillAmount(1, timer).SetEase(Ease.Linear);

        yield return new WaitForSeconds(timer / 2);

        titleText.transform.DOScale(1f, timer / 2).SetEase(Ease.Linear);

        yield return new WaitForSeconds((timer / 2) + 0.5f);

        //th
        presentationImage.DOFade(0, 0.5f);
        titleText.DOFade(0, 0.5f);
        questTypeImage.DOFade(1, 0.5f);

        titleHolder.transform.DOLocalMove(titleHolder.transform.localPosition + (new Vector3(1, 0, 0) * Screen.width * -1), 5);


    }

    IEnumerator CompleteProcess()
    {
        titleHolder.transform.localPosition = Vector3.zero;
        titleText.DOFade(1, 0);

        titleText.text = "COMPLETED!";

        yield return new WaitForSeconds(1.5f);

        Destroy(gameObject);
    }

    #endregion



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

    public void SetUp_Story()
    {
        isStory = true;
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

    //we show descriptor if it has the boolean for 
    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);

        if (!isStory) return;

        UIHandler.instance._DescriptionWindow.DescribeQuest(_quest, transform);

    }
    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);


        if (!isStory) return;

        UIHandler.instance._DescriptionWindow.StopDescription();

    }

    public void CompleteQuest()
    {
        //do an effect then order its destruction.
        StartCoroutine(CompleteProcess());
    }

}


//we spawn shrine in a random place of the options
//we keep on spawning at long cooldowns but we can have at most 3.
//decided the quest and type in the moment of itneraction
//we call ui to make the reveal.
//