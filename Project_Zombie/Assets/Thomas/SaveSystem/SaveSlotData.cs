using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SaveSlotData : ScriptableObject
{
    //we will put all the information here



    [SerializeField] string _saveID;

    public string GetSaveID { get { return _saveID; } }


    [field: SerializeField] public SaveClass _saveClass {  get; private set; }

    public void DeleteSaveData()
    {
        _saveClass = new SaveClass();
        GameHandler.instance._saveHandler.DeleteFile(_saveID);
    }

    public void SetData(SaveClass saveClass)
    {
        _saveClass = saveClass;

    }


    public void RestoreGameState()
    {
        //we will get information here and send to the right places.
        //we send to cityhandler and another to player. the player goes first.
        PlayerHandler.instance.RestoreState(_saveClass);
        GameHandler.instance.RestoreGameState(_saveClass);

    }



    [ContextMenu("MAKE SAVE FILE")]
    public void CaptureGameState()
    {
        //
        _saveClass.MakeHasSaveData(true);

        PlayerHandler.instance.CaptureState(_saveClass);
        GameHandler.instance.CaptureGameState(_saveClass);


        GameHandler.instance._saveHandler.CaptureSaveSlots();
    }

    //
    
}
