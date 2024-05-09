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
    //the player total stats.
    //it also needs settings.

    GameObject holder;

    private void Awake()
    {
        holder = transform.GetChild(0).gameObject;
    }

    private void Start()
    {
        CreateStatList();
    }

    private void Update()
    {
        if (!IsPauseOn())
        {
            descriptionHolder.SetActive(false);
        }
    }

    public void CallPause()
    {
        if (holder.activeInHierarchy)
        {
            GameHandler.instance.ResumeGame();
            PlayerHandler.instance._playerController.block.RemoveBlock("Pause");
            holder.SetActive(false);
        }
        else
        {
            GameHandler.instance.PauseGame();
            PlayerHandler.instance._playerController.block.AddBlock("Pause", BlockClass.BlockType.Partial);
            holder.SetActive(true);
        }
    }

    #region SETTINGS
    [SerializeField] Settings _settings;

    public void OpenSettings()
    {
        _settings.OpenSetting();
    }
    #endregion

    public bool IsPauseOn()
    {
        return holder.activeInHierarchy;
    }

    #region DESCRIPTION
    //description for bd, for ability, for stat and for gun.
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
    [SerializeField] GameObject descriptionHolder;
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI typeText;
    [SerializeField] TextMeshProUGUI tierText;
    [SerializeField] TextMeshProUGUI descriptionText;
    [SerializeField] TextMeshProUGUI damageText;
    [SerializeField] TextMeshProUGUI cooldownText;
    
    //i can just make everyone appear right at first. then we check.
    

    //the gun will be the only different.

    public void StopDescription()
    {
        descriptionHolder.SetActive(false);
    }

    public void DescribeBD(BDClass bd, Transform posRef)
    {
        //we do the same thing but we also 

        descriptionHolder.SetActive(true);
        descriptionHolder.transform.position = posRef.position + GetScreenOffset(posRef.position);


        nameText.text = bd.bdType.ToString();
        typeText.text = bd.GetTypeForDescription();

        tierText.text = "";


        string bdDescription = MyUtils.GetDescriptionForBD(bd);
        descriptionText.text = bdDescription;


        damageText.text = bd.GetDamageDescription();


        cooldownText.text = "Total Cooldown: " +  bd.GetTempDurationForDescription();
    }

    public void DescribeAbiliy(AbilityClass ability, Transform posRef)
    {
        descriptionHolder.SetActive(true);
        descriptionHolder.transform.position = posRef.position + GetScreenOffset(posRef.position);

        nameText.text = ability.GetNameForDescription();
        typeText.text = ability.GetTypeForDescription();
        tierText.text = ability.GetTierForDescription();
        descriptionText.text = ability.GetDescriptionForDescription();
        damageText.text = ability.GetDamageForDescription();
        cooldownText.text = ability.GetCooldownForDescription();    
    }

    public void DescribeGun(GunClass gun)
    {
        descriptionHolder.SetActive(true);
    }

    public void DescribeStat(StatClass stat, Transform posRef)
    {
        descriptionHolder.SetActive(true);
        descriptionHolder.transform.position = posRef.position + GetScreenOffset(posRef.position);

        nameText.text = stat.stat.ToString();
        typeText.text = "Stat";
        tierText.text = "";
        descriptionText.text = MyUtils.GetStatDescription(stat.stat);
        damageText.text = "Value is: " + stat.value.ToString();
        cooldownText.text = "";
    }

    public void DescribeDash(Transform posRef)
    {
        descriptionHolder.SetActive(true);
        descriptionHolder.transform.position = posRef.position + GetScreenOffset(posRef.position);

        nameText.text = "Dash";
        typeText.text = "Unique Ability";
        tierText.text = "";
        descriptionText.text = "Once you dahs you become immune to all forms of damage for a short amount of time";
        damageText.text = "";
        cooldownText.text = "";
    }

    

    [Separator(("Set Stat"))]
    [SerializeField] StatDescriptionUnit statUnitTemplate;
    [SerializeField] Transform statContainer;
    Dictionary<StatType, StatDescriptionUnit> statUnitDictionary = new();
    public void CreateStatList()
    {
        List<StatType> statListRef = MyUtils.GetStatListRef();

        //we will use this to get the value.

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
    //then we create a list so we can update stuff without changing everything.
    List<AbilityUnit> abilityUnitList = new();   
    public void AddPassive(AbilityClass ability)
    {
        AbilityUnit newObject = Instantiate(abilityUnitTemplate);
        newObject.transform.SetParent(passiveAbilityContainer);
        newObject.SetUpPassive(ability);


    }
    public void RemovePassive(AbilityClass ability)
    {

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
        StopDescription();
    }


}
