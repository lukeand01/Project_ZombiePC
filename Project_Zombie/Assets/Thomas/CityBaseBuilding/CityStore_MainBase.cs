using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityStore_MainBase : CityStore
{
    [SerializeField] CityData_Main _cityData;


    private void Start()
    {
        PlayerParty party = PlayerHandler.instance._playerParty;

        _cityData.Initialize();

        UIHandler.instance._EquipWindowUI.UpdateOptionForStoryQuest(_cityData.storyQuestList_Active);

        _cityCanvas.SetEspecialNpcs(party.npcList, party.especialNpcLimit);
        _cityCanvas.SetQuests(_cityData.storyQuestList_Active, _cityData.storyQuestList_Completed);
    }

    //we only truly updaet this fella at the start, but if i need to updaet it i will do so through the cityhandler.


    public override void IncreaseStoreLevel()
    {
        //we tell teh cityhandler to deal with this.
        //and we update the pop.
        base.IncreaseStoreLevel();
        CityHandler.instance._cityBuildingHandler.UpdatePopResource();
      
    }

    


    public override CityData GetCityData => _cityData;

}

//i should not have a different canvas for each because even thought its more its also more work.