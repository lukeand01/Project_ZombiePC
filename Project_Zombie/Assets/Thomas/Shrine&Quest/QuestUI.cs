using DG.Tweening;
using MyBox;
using System.Collections;
using System.Collections.Generic;
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


    }

    #region WINDOW
    [Separator("WINDOW")]
    [SerializeField] QuestUnit unitTemplate;
    [SerializeField] Transform container;
    [SerializeField] GameObject windowHolder;
    

    public void OpenUI()
    {
        windowHolder.transform.DOKill();
        windowHolder.transform.DOMove(originalPos, 0.5f);
    }
    public void CloseUI()
    {
        windowHolder.transform.DOKill();
        windowHolder.transform.DOMove(originalPos + new Vector3(Screen.width * -0.1f, 0, 0), 0.5f);
    }

    public void AddQuestUnit(QuestClass _questClass)
    {
        QuestUnit newObject = Instantiate(unitTemplate);
        newObject.SetUp(_questClass);   
        newObject.transform.SetParent(container);
    }
    public void RemoveQuestUnit(QuestUnit unit)
    {
        Destroy(unit.gameObject);
    }

    #endregion


    #region SHRINE

    [Separator("SHRINE")]
    [SerializeField] GameObject shrineHolder;
    [SerializeField] BlessUnit[] blessUnitArray;
    [SerializeField] GameObject shrineRefuseButton;
    [SerializeField] TextMeshProUGUI shrineTitleText;

    public void Shrine_OpenUI()
    {
        //at first we do a little bit of suspense.
        //then we reveal the cards.
        //where do i get the cards and how to decide what i can do?
        shrineHolder.SetActive(true);


    }

    public void Shrine_CloseUI()
    {

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
