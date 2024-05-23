using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    private void Start()
    {
        //if this ever loads then we turnn off all ui and block the player from doing anything.

        UIHandler.instance.ControlUiForMainMenu(true);
        PlayerHandler.instance._playerController.block.AddBlock("MainMenu", BlockClass.BlockType.Complete);
    }
}
