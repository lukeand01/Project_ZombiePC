using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    //there will be the saveslotscreen
    //the continue is just direct
    //then settings.
    //


    [SerializeField] GameObject[] screenHolderArray;
    private void Start()
    {
        //if this ever loads then we turnn off all ui and block the player from doing anything.

        UIHandler.instance.ControlUiForMainMenu(true);
        PlayerHandler.instance._playerController.block.AddBlock("MainMenu", BlockClass.BlockType.Complete);
        UIHandler.instance._MouseUI.ControlAppear(false);
        Cursor.visible = true;
    }

    public void CallButton_NewGame()
    {
        GameHandler.instance._sceneLoader.LoadMainCity();
    }
    public void CallButton_LoadGame()
    {

    }
    public void CallButton_Settings()
    {

    }
    public void CallButton_Quit()
    {
        Application.Quit();
    }


}
