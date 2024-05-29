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
                //then we force the gun reveal.
                canSkipGun = false;
                StopAllCoroutines();
                GunReveal();

            }


        }


    }

    private void Awake()
    {
        holder = transform.GetChild(0).gameObject;

        freeReroll_Gun = true;
        freeReroll_Ability = true;
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

    bool freeReroll_Gun;
    bool freeReroll_Ability;
    int rollsUsed = 0;
    int blessCost = 0;

    //everytime you call a roll, it doubles. everytime you open a new 
    //for now it will the same value. always.


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


  

    public void CallChestGun(List<ItemData> gunListForSpinning, ItemData chosenGun)
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


        StartCoroutine(SpinningGunProcess(gunListForSpinning));

    }

    void UpdateGunRollButton()
    {
        if (freeReroll_Gun)
        {
            gun_Reroll_Button.SetText("Free Roll");
        }
        else
        {
            gun_Reroll_Button.SetText("Use 1 Bless");
        }
    }

    //it need to be something different.

    IEnumerator SpinningGunProcess(List<ItemData> gunListForSpinning)
    {
        gunButtonsHolder.SetActive(false);
        int rotations = Random.Range(6, 10);
        int current = 0;

        gunSpinningHolder.transform.DOScale(0, 0).SetUpdate(true);

        float timer = 0.15f;
        float timerForScale = timer;

        canSkipGun = true;

        while(rotations > current)
        {
            int random = Random.Range(0, gunListForSpinning.Count);
            gunSpinningImage.sprite = gunListForSpinning[random].itemIcon;

            gunSpinningHolder.transform.DOScale(4.3f, timerForScale).SetUpdate(true);

            yield return new WaitForSecondsRealtime(timer);

            gunSpinningHolder.transform.DOScale(0, timerForScale).SetUpdate(true);

            yield return new WaitForSecondsRealtime(timer);

            current++;

        }



        GunReveal();

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
        if (!PlayerHandler.instance._playerResources.Bless_HasEnough(1) && !freeReroll_Gun)
        {
            return;
        }

       if(!freeReroll_Gun) PlayerHandler.instance._playerResources.Bless_Lose(50);
        freeReroll_Gun = false;
        UpdateGunRollButton();

        List<ItemData> spinningGunList = PlayerHandler.instance.GetGunSpinningList();
        ItemData chosenGun = PlayerHandler.instance.GetGunChosen();
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
        //we need to check if there is a least one free slot.
        //then we give this weapon to the player

        //we use the buttonequip to trigger effects on it in case we need.


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
            //there is space so we give the gun right away.                   
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
        //we call it out if you dont have current gun swap unit.
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


        for (int i = 0; i < passiveAbilities.Count; i++)
        {
            chestAbilityUnitArray[i].SetUp(passiveAbilities[i], this);
        }


    }

    public void ChooseAbility(AbilityPassiveData data)
    {
        //we pass tot he player siomple as that.

        Leave();
        PlayerHandler.instance._playerController.block.RemoveBlock("AbilityChest");
        PlayerHandler.instance._playerAbility.AddAbility(data);
    }

    public void AbilityReroll()
    {
        Debug.Log("called");
        if (!PlayerHandler.instance._playerResources.Bless_HasEnough(1) && !freeReroll_Ability)
        {
            return;
        }

        UpdateAbilityRollButton();

        if (!freeReroll_Ability) PlayerHandler.instance._playerResources.Bless_Lose(1);
        freeReroll_Ability = false;
        List<AbilityPassiveData> passiveListForReroll = PlayerHandler.instance.GetPassiveList();
        CallChestAbility(passiveListForReroll);
    }

    void UpdateAbilityRollButton()
    {
        if (freeReroll_Ability)
        {
            ability_Reroll_Button.SetText("Free Roll");
        }
        else
        {
            ability_Reroll_Button.SetText("Use 1 Bless");
        }
    }

    #endregion



}
