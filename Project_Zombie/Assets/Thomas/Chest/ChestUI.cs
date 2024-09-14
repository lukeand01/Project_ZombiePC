using DG.Tweening;
using MyBox;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChestUI : MonoBehaviour
{
    //i use this for guns and abilities.



    private void Update()
    {
        if (Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(0))
        {
            if (canSkipGun)
            {
                //then we force the gun_Perma reveal.
                canSkipGun = false;
                StopAllCoroutines();
                GunReveal();

            }


        }


    }

    private void Awake()
    {
        holder = transform.GetChild(0).gameObject;

    }

    [Separator("BASE")]
    [SerializeField] TextMeshProUGUI titleText;
    GameObject holder;

    public void Leave()
    {
        //just close ti and start the game.
        holder.SetActive(false);
        GameHandler.instance.ResumeGame();


        PlayerHandler.instance._playerController.block.RemoveBlock("GunChest");
        PlayerHandler.instance._playerController.block.RemoveBlock("AbilityChest");
    }

    ChestBase currentChest;
    public void SetChest(ChestBase currentChest)
    {
        this.currentChest = currentChest;
    }



    //everytime you call a roll, it doubles. everytime you open a new 
    //for now it will the same value_Level. always.


    #region GUN
    [Separator("GUN")]
    [SerializeField] GameObject gunHolder;
    [SerializeField] GameObject gunSpinningHolder;
    [SerializeField] Image gunSpinningImage;
    [SerializeField] GameObject gunButtonsHolder;
    [SerializeField] GameObject gunStatDescriptionHolder;
    [SerializeField] TextMeshProUGUI chosenGunStatText;
    [SerializeField] TextMeshProUGUI selectedGunStatTitle;
    [SerializeField] GameObject gunSwapHolder;
    [SerializeField] StatDescriptionGroupHolder[] statDescriptionHolder;
    [SerializeField] GunSwapUnit[] gunSwapUnits;
    [SerializeField] ButtonBase gun_Reroll_Button;

    ItemGunData currentChosenGun;



    bool canSkipGun;


  

    public void CallChestGun(List<ItemGunData> gunListForSpinning, ItemGunData chosenGun)
    {
        titleText.text = "SPINNING";


        UpdateGunRollButton();

        holder.SetActive(true);
        gunHolder.SetActive(true); 
        abilityHolder.SetActive(false);

        gunSwapHolder.SetActive(false);
        gunStatDescriptionHolder.SetActive(false);


        PlayerHandler.instance._playerController.block.AddBlock("GunChest", BlockClass.BlockType.Partial);
        GameHandler.instance.PauseGame();

        currentChosenGun = chosenGun.GetGun();

        if(currentChosenGun == null)
        {
            Debug.LogError("the chosen gun has no gun");
        }


        StartCoroutine(GetGunSpinningProcess(gunListForSpinning, chosenGun));

    }

    void UpdateGunRollButton()
    {
       
    }

    //it need to be something different.

    IEnumerator GetGunSpinningProcess(List<ItemGunData> spinningProcess, ItemGunData chosenGun)
    {
        yield return null;
    }

    void GunReveal()
    {
        if(currentChosenGun == null)
        {
            Debug.LogError("no chosen gun");
            return;
        }

        gunSwapHolder.gameObject.SetActive(true);
        gunStatDescriptionHolder.gameObject.SetActive(true);

        titleText.text = "You got a " + currentChosenGun.itemName;
        gunSpinningImage.DOKill();
        canSkipGun = false;
        gunSpinningHolder.transform.DOScale(4.3f, 0.1f).SetUpdate(true);
        gunSpinningImage.sprite = currentChosenGun.itemIcon;

        foreach (var item in statDescriptionHolder)
        {
            item.ChoseGun(currentChosenGun);
        }

        chosenGunStatText.text = currentChosenGun.itemName;
        selectedGunStatTitle.text = "";
        gunButtonsHolder.SetActive(true);

        ShowGunSwap();


    }

    public void GunReroll()
    {



        UpdateGunRollButton();

        List<ItemGunData> spinningGunList = GameHandler.instance.cityDataHandler.cityArmory.GetGunSpinningList();
        ItemGunData chosenGun = GameHandler.instance.cityDataHandler.cityArmory.GetGunChosen();
        CallChestGun(spinningGunList, chosenGun);
    }

    void ShowGunSwap()
    {
        gunSwapHolder.SetActive(true);

        //we get the two guns.
        GunClass[] tempGuns = PlayerHandler.instance._playerCombat.GetTempGuns();

        if(tempGuns.Length != 2 || gunSwapUnits.Length != 2)
        {
            Debug.Log("soimeone is not matching");
            return;
        }


        gunSwapUnits[0].SetUp(tempGuns[0], this, 0);
        gunSwapUnits[1].SetUp(tempGuns[1], this, 1);




    }

    public void GunEquip(ButtonChestEquip buttonEquip)
    {
        //why is this not updating?


        PlayerCombat combat = PlayerHandler.instance._playerCombat;

        int index = combat.GetGunEmptySlot();

        if(index == -1)
        {
            if(currentGunSwapUnit != null)
            {
                combat.ReceiveTempGunToReplace(currentChosenGun, currentGunSwapUnit.index);
            }
            else
            {
                //we warn the button equip to signal the player
                return;
            }
        }
        else
        {
            //there is space so we give the gun_Perma right away.                   
            combat.ReceiveTempGunToReplace(currentChosenGun, index);
        }

        Leave();

        if (currentChest != null)
        {
            currentChest.ProgressChest();
        }
    }


    GunSwapUnit currentGunSwapUnit;
    public void SelectGunOwned(GunSwapUnit gun)
    {
        //show the thing.
        if(currentGunSwapUnit != null)
        {
            currentGunSwapUnit.Unselect();
        
        }


        currentGunSwapUnit = gun;
        currentGunSwapUnit.Select();

    }
    public void UnselectGunOwned()
    {
        if(currentGunSwapUnit != null)
        {
            currentGunSwapUnit.Unselect();
            currentGunSwapUnit = null;
        }
    }

    public void HoverGunSwapUnit(GunSwapUnit gun)
    {
        SelectInfoGroup(gun);
    }
    public void StopHover()
    {
        //we call it out if you dont have current gun_Perma swap unit.
        if(currentGunSwapUnit != null)
        {
            SelectInfoGroup(currentGunSwapUnit);
        }
        else
        {
            StopInfoGroup();
        }

    }

    void SelectInfoGroup(GunSwapUnit swapUnit)
    {
        if(swapUnit == null)
        {
            Debug.Log("no swapn unit");
            return;
        }
        selectedGunStatTitle.text = swapUnit.gun.data.itemName;
        foreach (var item in statDescriptionHolder)
        {
            item.SelectGun(swapUnit.gun, swapUnit.index);
        }
    }
    void StopInfoGroup()
    {
        selectedGunStatTitle.text = "";
        foreach (var item in statDescriptionHolder)
        {
            item.UnselectGun();
        }
    }

    #endregion

    #region ABILITY
    [Separator("ABILITY")]
    [SerializeField] GameObject abilityHolder;
    [SerializeField] GameObject abilityButtonHolder;
    [SerializeField] ChestAbilityUnit[] chestAbilityUnitArray;
    [SerializeField] ButtonBase ability_Reroll_Button;
    [SerializeField] ButtonBase ability_OpenAbilityInventory_Button;
    [SerializeField] ButtonBase ability_Scrap_Button;
    [SerializeField] AudioClip audio_Scrap;
    [SerializeField] AudioClip audio_Bless;
    [SerializeField] AudioClip audio_BlessFailure;
    int currentScrapValue;

    public void CallChestAbility(List<AbilityPassiveData> passiveAbilities)
    {
        //we dont need to create the images as we will always have them.

        UpdateAbilityRollButton();

        holder.SetActive(true);
        abilityHolder.SetActive(true);
        gunHolder.SetActive(false);

        GameHandler.instance.PauseGame();

        titleText.text = "Choose an Ability!";


        PlayerHandler.instance._playerController.block.AddBlock("AbilityChest", BlockClass.BlockType.Partial);

        int scrapCost = 500; //it scales with the rarity of the ability.



        for (int i = 0; i < passiveAbilities.Count; i++)
        {
            var item = passiveAbilities[i];
            chestAbilityUnitArray[i].SetUp(item, this, i);

            scrapCost += 150 * (int)item.abilityTier;
        }

        currentScrapValue = scrapCost;

        UpdateAbilityScrapButton();
    }

    bool abilityProcess = false;

    public void ChooseAbility(AbilityPassiveData data, int orderIndex)
    {
        //we pass tot he player siomple as that.
        if (abilityProcess) return;

        StopAllCoroutines();
        StartCoroutine(ChoseAbilityProcess(data, orderIndex));
        
    }


    IEnumerator ChoseAbilityProcess(AbilityPassiveData data, int orderIndex)
    {
        abilityProcess = true;
        Debug.Log("start this process");

        Vector3 originalScale = chestAbilityUnitArray[0].transform.localScale; //THEY ALL SHOULD ALWAYS HAVE THE SAME SCALE.

        for (int i = 0; i < chestAbilityUnitArray.Length; i++)
        {
            var item = chestAbilityUnitArray[i];

            
            if(i == orderIndex)
            {
                item.transform.DOScale(originalScale * 1.2f, 0.3f).SetUpdate(true);
            }
            else
            {
                item.transform.DOScale(0, 0.3f).SetUpdate(true);
            }
        }

        yield return new WaitForSecondsRealtime(0.3f);

        chestAbilityUnitArray[orderIndex].transform.DOScale(originalScale, 0.3f).SetUpdate(true);

        yield return new WaitForSecondsRealtime(0.3f);

        abilityProcess = false;
        Leave();
        PlayerHandler.instance._playerController.block.RemoveBlock("AbilityChest");
        PlayerHandler.instance._playerAbility.AddAbility(data);
    }

    //

    public void AbilityReroll()
    {
        Debug.Log("called");
        int blessQuantity = PlayerHandler.instance.abilityRoll_Cost;

        if (!PlayerHandler.instance._playerResources.Bless_HasEnough(blessQuantity))
        {
            GameHandler.instance._soundHandler.CreateSfx(SoundType.AudioClip_BlessFailure);
            return;
        }

        GameHandler.instance._soundHandler.CreateSfx(SoundType.AudioClip_BlessSuccess);


        PlayerHandler.instance._playerResources.Bless_Lose(blessQuantity);

        List<AbilityPassiveData> passiveListForReroll = GameHandler.instance.cityDataHandler.cityLab.GetPassiveAbilityList();
        CallChestAbility(passiveListForReroll);

        PlayerHandler.instance.AbilityRoll_Add();
        UpdateAbilityRollButton();


    }

    void UpdateAbilityRollButton()
    {
        int blessQuantity = PlayerHandler.instance.abilityRoll_Cost;
        ability_Reroll_Button.SetText($"Reroll for {blessQuantity} Bless");
       
    }

    void UpdateAbilityScrapButton()
    {
        ability_Scrap_Button.SetText($"Scrap for {currentScrapValue} points");
    }

    public void CallButton_OpenAbilityInventory()
    {
        //we create it once everytime. perfomance? lets worry only if there are problems.
        //what about pausing the game? wont that mess with the timescale?

    }

    public void CallButton_Scrap()
    {
        GameHandler.instance._soundHandler.CreateSfx(SoundType.AudioClip_Scrap);
        Debug.Log("scrap");
        PlayerHandler.instance._playerResources.GainPoints(currentScrapValue);
        Leave();
    }

    //i also gain points.
    //the points gained should be based

    //everytime i check this i will also clean and create a new ability thing to show the player what he already has.


    #endregion



}
