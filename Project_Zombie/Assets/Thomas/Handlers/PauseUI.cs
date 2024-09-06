using MyBox;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;

public class PauseUI : MonoBehaviour
{
    //so here i will show a bunch of info about the player
    //all the passives he has
    //all the actives.
    //the guns and their damage 
    //the player total_Damage stats.
    //it also needs settings.

    //is there another place where the ability unit is set? or just pause because that should be the think


    GameObject holder;

    private void Awake()
    {
        holder = transform.GetChild(0).gameObject;
    }

    private void Start()
    {
        CreateStatList();
        descriptionWindow = UIHandler.instance._DescriptionWindow;

    }

    private void Update()
    {
        
    }

    public void CallPause()
    {
        if (holder.activeInHierarchy)
        {
            GameHandler.instance.ResumeGame();
            PlayerHandler.instance._playerController.block.RemoveBlock("Pause");
            holder.SetActive(false);
            descriptionWindow.StopDescription();
        }
        else
        {
            CheckContainers();
            GameHandler.instance.PauseGame();
            PlayerHandler.instance._playerController.block.AddBlock("Pause", BlockClass.BlockType.Partial);
            descriptionWindow.StopDescription();
            holder.SetActive(true);
        }
    }

    public void ForceClosePause()
    {
        if (holder.activeInHierarchy)
        {
            GameHandler.instance.ResumeGame();
            PlayerHandler.instance._playerController.block.RemoveBlock("Pause");
            holder.SetActive(false);
        }
    }

    #region SETTINGS


    public void OpenSettings()
    {
        UIHandler.instance._settingsUI.OpenSetting();
    }

    public void CloseSettings()
    {
        UIHandler.instance._settingsUI.CloseSetting();
    }

    #endregion

    public bool IsPauseOn()
    {
        return holder.activeInHierarchy;
    }

    #region DESCRIPTION
    //description for bd, for ability, for stat and for gun_Perma.
    //i need show the passive abilities he has gathered.


    //BD
    //what the bd is changing. Bleeding: ""
    //how much is left for the duration.

    //ABILITY
    //what it does. "place a sentry where mouse is. and does that"
    //base damage and scaling.
    //the cooldown


    //STAT
    //how the stat work. protection reduces the damage by that percent. max is 90;
    //how much base you have and how much is being altered.

    //GUN
    //ammo
    //damage
    //bullet per shot

    [Separator("Description")]
    DescriptionWindow descriptionWindow;

    //i can just make everyone appear right at first. then we check.
    

    //the gun_Perma will be the only different.

    public void StopDescription()
    {
        descriptionWindow.StopDescription();
    }

    public void DescribeBD(BDClass bd, Transform posRef)
    {
        //we do the same thing but we also 

        descriptionWindow.DescribeBD(bd, posRef);

    }

    public void DescribeAbiliy(AbilityClass ability, Transform posRef)
    {
        descriptionWindow.DescribeAbiliy(ability, posRef);
 
    }

    public void DescribeGun(GunClass gun, Transform posRef)
    {
        //descriptionHolder.SetActive(true);
        descriptionWindow.DescribeGun(gun, posRef);
    }

    public void DescribeStat(StatClass stat, Transform posRef)
    {
        descriptionWindow.DescribeStat(stat, posRef);



    }

    public void DescribeDash(Transform posRef)
    {

        descriptionWindow.DescribeDash(posRef); 
        return;
        //descriptionHolder.SetActive(true);
        //descriptionHolder.transform.position = posRef.position + GetScreenOffset(posRef.position);


    }

    

    [Separator(("Set Stat"))]
    [SerializeField] StatDescriptionUnit statUnitTemplate;
    [SerializeField] Transform statContainer;
    Dictionary<StatType, StatDescriptionUnit> statUnitDictionary = new();
    public void CreateStatList()
    {
        List<StatType> statListRef = MyUtils.GetStatListRef();

        //we will use this to get the value_Level.

        EntityStat stat = PlayerHandler.instance._entityStat; 

        //for each we create
        foreach (var item in statListRef)
        {
            StatDescriptionUnit newObject = Instantiate(statUnitTemplate);
            newObject.SetUp(item, stat.GetTotalValue(item));
            newObject.transform.SetParent(statContainer);

            if (statUnitDictionary.ContainsKey(item))
            {
                Debug.Log("something wrong");
            }
            else
            {
                statUnitDictionary.Add(item, newObject);
            }
        }


    }
    public void UpdateStat(StatType stat, float value)
    {
        if(statUnitDictionary.ContainsKey(stat))
        {
            statUnitDictionary[stat].SetUp(stat, value);
        }
        else
        {
            Debug.Log("didnt find " + stat);
        }
    }


    #endregion


    #region PASSIVE 
    [Separator("Set Passive")]
    [SerializeField] AbilityUnit abilityUnitTemplate;
    [SerializeField] Transform passiveAbilityContainer;
    [SerializeField] Transform curseAbilityContainer;
    [SerializeField] GameObject buttonOptionHolder;

    //this only shows 


    public void OpenContainer(int choice)
    {

        if(choice == 0)
        {
            passiveAbilityContainer.gameObject.SetActive(true);
            curseAbilityContainer.gameObject.SetActive(false);
        }
        else
        {
            passiveAbilityContainer.gameObject.SetActive(false);
            curseAbilityContainer.gameObject.SetActive(true);
        }


    }

    void CheckContainers()
    {
        if(curseAbilityContainer.childCount > 0)
        {
            buttonOptionHolder.SetActive(true);
        }
        else
        {
            buttonOptionHolder.SetActive(false);
            OpenContainer(0);
        }
    }
    //then we create a list so we can update stuff without changing everything.
    List<AbilityUnit> abilityUnitList = new();   
    public void AddPassive(AbilityClass ability)
    {
        AbilityUnit newObject = Instantiate(abilityUnitTemplate);
        newObject.transform.SetParent(passiveAbilityContainer);
        newObject.SetUpPassive(ability);

        //Debug.Log("added an ability to the passive " + ability._abilityUnit.gameObject.name);
    }

    public void AddCurse(AbilityClass ability)
    {
        //we will put this in another
        AbilityUnit newObject = Instantiate(abilityUnitTemplate);
        newObject.transform.SetParent(curseAbilityContainer);
        newObject.SetUpPassive(ability);
    }

    public void RemovePassive(AbilityClass ability)
    {

    }


    #endregion

    #region BUTTONS
    public void ButtonCall_Resume()
    {
        CallPause();
    }
    public void ButtonCall_OpenSettings()
    {
        OpenSettings();
    }
    public void ButtonCall_DebugFinishStage()
    {
        if(CityHandler.instance != null)
        {
            Debug.Log("cannot call this here");
            return;
        }

        //we call the victoryUI.

        //then we load the first stage.

        GameHandler.instance._sceneLoader.LoadMainCity();
    }
    public void ButtonCall_QuitGame()
    {
        Application.Quit();
    }


    #endregion



    Vector3 GetScreenOffset(Vector3 posRef)
    {
        float screenHeight = Screen.height;
        float screenWidth = Screen.width;
        Vector3 newOffset = new Vector3(0, screenHeight * 0.15f, 0);



        if(posRef.y > screenHeight * 0.7f)
        {
            newOffset = new Vector3(0, -screenHeight * 0.25f, 0);
        }

        if(posRef.y < screenHeight * 0.05f)
        {
            //newOffset += new Vector3(0, 30, 0);
        }

        if(posRef.x > screenWidth * 0.9f)
        {
            newOffset += new Vector3(-screenWidth * 0.07f, 0, 0);
        }
        if (posRef.x < screenWidth * 0.3f)
        {
            newOffset += new Vector3(screenWidth * 0.07f, 0, 0);
        }

        return newOffset;
    }

    private void OnDisable()
    {
       if(descriptionWindow != null)
        {
            StopDescription();
        }
        
    }


}
