using DG.Tweening;
using MyBox;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;

public class QuestUI : MonoBehaviour
{
    GameObject holder;

    Vector3 originalPos;


    private void Awake()
    {
        holder = transform.GetChild(0).gameObject;

        originalPos = windowHolder.transform.position;
        windowHolder.transform.position = originalPos + new Vector3(Screen.width * -0.1f, 0, 0);

        originalTitlePos = shrineTitleHolder.transform.position;
        originalButtonPos = ShrineRefuseButton.transform.position;

        

    }

    private void Start()
    {
        CloseUI();
    }

    private void Update()
    {
        //it keeps checking if it has one quest at least.
    }


    #region WINDOW
    [Separator("WINDOW")]
    [SerializeField] QuestUnit unitTemplate;
    [SerializeField] Transform container;
    [SerializeField] GameObject windowHolder;

    public bool IsOpen {  get; private set; }


    //it checks only through the add function, there is no reason to check anywhere else.
    //


    //i want to do the presentation.
    //the unit will only start onec the window is in the right place.
    //the process is that the text inside the quest is changed to correct type, it scale up and down
    //when it scales up, it start filling the image till its done, then once that done the text fades away as the titleholder is pulled to the left
    

    public void OpenUI()
    {

        windowHolder.transform.DOKill();
        windowHolder.transform.DOMove(originalPos, 0.5f).OnComplete(OnOpenedUI);
    }


    public Action eventOpenUI;


    void OnOpenedUI()
    {
        //we inform everyone that the ui has been succesful
        //and if they are waiting they can start right now.
        Debug.Log("opened completed");
        eventOpenUI?.Invoke();
        IsOpen = true;
    }

    public void CloseUI()
    {
        windowHolder.transform.DOKill();
        windowHolder.transform.DOMove(originalPos + new Vector3(Screen.width * -0.3f, 0, 0), 0.5f);
        IsOpen = false;
    }

    public void AddQuestUnit(QuestClass _questClass)
    {

        if (!IsOpen)
        {
            OpenUI();
        }

        QuestUnit newObject = Instantiate(unitTemplate);
        newObject.SetUp_Challenge(_questClass, this);   
        newObject.transform.SetParent(container);



    }

    #endregion

    //we are not going to be doing this.
    #region SHRINE

    [Separator("SHRINE")]
    [SerializeField] GameObject shrineHolder;
    [SerializeField] GameObject shrinePiecesHolder;
    [SerializeField] GameObject ShrineRotatingImage;
    [SerializeField] ButtonBase ShrineRefuseButton;
    [SerializeField] TextMeshProUGUI shrineTitleText;
    [SerializeField] GameObject shrineTitleHolder;
    [SerializeField] BlessUnit[] blessUnitArray;
    Vector3 originalTitlePos;
    Vector3 originalButtonPos;
    Shrine shrineCurrent;


    bool hasAlreadySelected = false;
    public void Shrine_OpenUI(List<QuestClass> questList, Shrine shrine)
    {
        //at first we do a little bit of suspense.
        //then we reveal the cards.
        //where do i get the cards and how to decide what i can do?
        shrineHolder.SetActive(true);
        shrinePiecesHolder.SetActive(false);
        GameHandler.instance.PauseGame();
        hasAlreadySelected = false;

        shrineCurrent = shrine;

        PlayerHandler.instance._playerController.block.AddBlock("Shrine", BlockClass.BlockType.Complete);

        StartCoroutine(Shrine_OpenUI_Process(questList));
    }

    public void SelectQuest(BlessUnit blessUnit, QuestClass _questClass)
    {
        //
        if (hasAlreadySelected) return;

        hasAlreadySelected = true;
        float timer = 0.3f;
        blessUnit.SelectEffect(0.3f);
        StartCoroutine(EndProcess((timer * 2) + 1));

        PlayerHandler.instance._playerResources.AddQuest(_questClass);


    }

    public void CallButton_Refuse()
    {
        if (hasAlreadySelected) return;

        hasAlreadySelected = true;

        foreach (var item in blessUnitArray)
        {
            item.transform.DOScale(0, 0.3f).SetUpdate(true);
        }

        StartCoroutine(EndProcess(0.3f + 0.15f));

    }

    IEnumerator EndProcess(float timer)
    {
        ShrineRefuseButton.transform.DOMove(originalButtonPos + new Vector3(0, Screen.height * -0.2f, 0), 0.15f).SetUpdate(true);
        shrineTitleHolder.transform.DOMove(originalTitlePos + new Vector3(0, Screen.height * 0.2f, 0), 0.15f).SetUpdate(true);

        yield return new WaitForSecondsRealtime(timer);
        Shrine_CloseUI();
    }
    

    IEnumerator Shrine_OpenUI_Process(List<QuestClass> questList)
    {


        if (questList.Count == 0)
        {
            Debug.Log("no querst list");
            yield break;
        }

        if (questList[0] == null)
        {
            Debug.Log("first is null");
            yield break;    
        }

        shrineTitleText.text = questList[0].questType.ToString();

        ShrineRefuseButton.ControlCannotClick(questList[0].questType == QuestType.Curse);
        

        //set button event is blocked.


        ShrineRotatingImage.gameObject.SetActive(true);
        

        ShrineRefuseButton.transform.DOKill();
        ShrineRefuseButton.transform.position = originalButtonPos + new Vector3(0, Screen.height * -0.2f, 0);

        shrineTitleHolder.transform.DOKill();
        shrineTitleHolder.transform.position = originalTitlePos + new Vector3(0, Screen.height * 0.2f, 0);

        foreach (var item in blessUnitArray)
        {
            item.transform.localScale = Vector3.zero;
        }

        float current = 0;
        float total = 0.25f;

        while (total > current)
        {
            current += Time.unscaledDeltaTime;
            ShrineRotatingImage.transform.Rotate(new Vector3(0, 0, 5));
            yield return new WaitForSecondsRealtime(0.01f);
        }




        ShrineRotatingImage.gameObject.SetActive(false);
        shrinePiecesHolder.SetActive(true);

        //bring down the title.
        //bring up the button
        //there is only button: refuse - and we must block it if the player is not allowed to refuse it
        //we choose each of the cards appear and those that appear scale up.

        float timer = 0.8f;

        ShrineRefuseButton.transform.DOMove(originalButtonPos, timer).SetUpdate(true);
        shrineTitleHolder.transform.DOMove(originalTitlePos, timer).SetUpdate(true);

        yield return new WaitForSecondsRealtime(timer);



        float scaleTimer = 0.15f;


        for (int i = 0; i < questList.Count; i++)
        {
            var item = questList[i];

            if(item == null)
            {
                blessUnitArray[i].gameObject.SetActive(false);
            }
            else
            {
                blessUnitArray[i].gameObject.SetActive(true);
                blessUnitArray[i].SetUp(item);

                blessUnitArray[i].transform.DOScale(1, scaleTimer).SetUpdate(true);
            }

            
        }

        //then we set the cards and show them.

    }

    public void Shrine_CloseUI()
    {
        GameHandler.instance.ResumeGame();
        shrineHolder.SetActive(false);
        shrinePiecesHolder.SetActive(false);
        StopAllCoroutines();

        if(shrineCurrent != null)
        {
            shrineCurrent.Remove();
            shrineCurrent = null;
        }

        PlayerHandler.instance._playerController.block.RemoveBlock("Shrine");
    }

    #endregion

}


//how to get the quests? because certain quests should not work.



//IDEAS FOR QUESTS
//Kill x enemies.
//kill x bosses.
//find x ability boxes.
//find x resource boxes
//open all doors in the stage.
//break x tokens - tokens appear in the map and you never to hover over them to break them
//break x statues - statues appear and they need to be shot at to destroy.
//survive till turn x


//for now all quests will be about slaying enemies or about finding ability boxes
//i will create colors for them -

//1 - Curse - Red
//2 - {} - blue
//3 - challenge - yellow (should shine)


//so there are three types of shrine
//1 - you click and you instantly get the task. it has no timer. but if you end the mission without completing it you get penalized.
//2 - you click and you open the screen with three different quests. it has timer. you get penalized for no completing in time.
//3 - you click and you get one task. you can either accept or not. it has no timer. it is the hardest but it gives unique stuff.

//

//quests that are from the base will achieve things only in teh base.


//what about space for ui?
//we could only show it when its required.
//for now it will stay in the left.
//there should be a differentation for the main goal and secondary.
