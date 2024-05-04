using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    [SerializeField] Settings _settings;

    public void OpenSettings()
    {
        _settings.OpenSetting();
    }

}
