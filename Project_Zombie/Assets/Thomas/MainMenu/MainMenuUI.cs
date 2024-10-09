using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    //there will be the saveslotscreen
    //the continue is just direct
    //then settings.
    //


    [SerializeField] GameObject _playHolder;
    [SerializeField] Settings _settings;
    private void Start()
    {
        //if this ever loads then we turnn off all ui and block the player from doing anything.

        UIHandler.instance.ControlUiForMainMenu(true);
        PlayerHandler.instance._playerController.block.AddBlock("MainMenu", BlockClass.BlockType.Complete);
        UIHandler.instance._MouseUI.ControlAppear(false);
        Cursor.visible = true;

        SetSlotUnits();
    }


    public void CallButton_Continue()
    {
        //same thing but its goes directly to load. we check every 
    }
    public void CallButton_Play()
    {
        //GameHandler.instance._sceneLoader.LoadMainCity();

        _playHolder.SetActive(true);
        _settings.CloseSetting();
        //_settings.gameObject.SetActive(false);
        
    }

    public void CallButton_Settings()
    {
        _playHolder.SetActive(false);
        _settings.OpenSetting();
        _settings.gameObject.SetActive(true);
    }
    public void CallButton_Quit()
    {
        Application.Quit();
    }



    [SerializeField] SaveSlotUnit saveSlotTemplate;
    [SerializeField] Transform container;

    void SetSlotUnits()
    {
        SaveSlotData[] slotDataArray = GameHandler.instance._saveHandler._saveSlotArray;


        for (int i = 0; i < slotDataArray.Length; i++)
        {
            var item = slotDataArray[i];

            SaveSlotUnit newObject = Instantiate(saveSlotTemplate);
            newObject.SetUp(item);
            newObject.transform.SetParent(container);

        }

    }

}
