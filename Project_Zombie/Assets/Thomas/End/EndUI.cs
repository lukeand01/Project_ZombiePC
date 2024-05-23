using MyBox;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndUI : MonoBehaviour
{
    GameObject holder;
    [SerializeField] EndUI_Info _info;
    [Separator("END")]
    [SerializeField] TextMeshProUGUI end_TitleText;
    [SerializeField] Image end_BackgroundImage;

    private void Awake()
    {
        holder = transform.GetChild(0).gameObject;
    }

    public void CloseEnd()
    {
        holder.SetActive(false);
    }
    #region VICTORY
    [Separator("VICTORY")]
    [SerializeField] GameObject victory_Holder;
    

    public void StartVictoryUI()
    {
        holder.SetActive(true);
        end_TitleText.text = "Victory";

        ShowEverything();
        _info.Open();

        PlayerHandler.instance._playerController.block.AddBlock("End", BlockClass.BlockType.Complete);
    }

    #endregion

    #region DEFEAT
    [Separator("DEFEAT")]
    [SerializeField] GameObject defeat_Holder;

    public void StartDefeatUI()
    {
        holder.SetActive(true);
        end_TitleText.text = "Defeat";

        ShowEverything();
        _info.Open();

        PlayerHandler.instance._playerController.block.AddBlock("End", BlockClass.BlockType.Complete);
    }

    #endregion

    [ContextMenu("Debug_Victory")]
    public void Debug_Victory()
    {
        StartVictoryUI();
    }


    #region SHOW

    void ShowEverything()
    {
        ShowPlayerPassiveAbilities();
        ShowPlayerFoundResourceItems();
        ShowPlayerStatTracker();
    }
    void ShowPlayerPassiveAbilities()
    {
       List<AbilityClass> abilityList = PlayerHandler.instance._playerAbility.GetPassiveAbilityList();
        _info.SetAbility(abilityList);

        //we also need to show the active abilities.
    }
    void ShowPlayerFoundResourceItems()
    {
       List<ItemClass> itemList =  PlayerHandler.instance._playerInventory.GetStageInventoryList();
        _info.SetResource(itemList);
    }
    void ShowPlayerGuns()
    {

    }
    void ShowPlayerStatTracker()
    {
        Dictionary<StatTrackerType, float> statTrackerDictionary = PlayerHandler.instance._playerStatTracker.GetStatTrackDictionary();
        _info.SetStatTracker(statTrackerDictionary);
    }
    #endregion

    #region BUTTON CALL
    public void ButtonCall_Replay()
    {
        CloseEnd();
        GameHandler.instance._sceneLoader.ReloadCurrentScene();
    }
    public void ButtonCall_ReturnToCity()
    {
        CloseEnd();
        GameHandler.instance._sceneLoader.LoadMainCity();
    }
    #endregion


    public bool IsEnd() => holder.activeInHierarchy;
}



//ALL STATS THAT I NEED TO TRACK
//Time alive
//Kills
//Point gained
//Points spent
//passive abilities found
//
//Damage received
//Damage Taken