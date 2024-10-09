using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveHandler : MonoBehaviour
{



    [field:SerializeField]public SaveSlotData[] _saveSlotArray { get; private set; }


    //at the start we get this and pass the information to them.
    //


    public static SaveHandler instance;


    private void Awake()
    {
        RestoreSaveSlots();
    }
    private void Start()
    {
        RestoreStateUsingCurrentSaveSlot(); //in case we have one selecteed for debuging, otherwise it will be just ignored.
    }


    #region BASE FUNCTIONS
    void SaveFile(object state, string savePath)///HERE WE SERIALIZE TEH DATA, TURNING INTO SOMETHING ONLY THE COMPUTER CAN READ AND CHANGE.
    {

        using (var stream = File.Open(savePath, FileMode.Create))
        {

            var formatter = new BinaryFormatter();
            formatter.Serialize(stream, state);
        }

    }

    public Dictionary<string, object> LoadFile(string savePath) ///HERE WE READ THE FILE AND TAKE AN OBJECT, WHICH WILL BE READ AS A TYPE OF SAVEDATA LATER.
    {

        if (!File.Exists(savePath))
        {
            return new Dictionary<string, object>();
        }
        using (FileStream stream = File.Open(savePath, FileMode.Open))
        {
            //the problem is here.
            var formatter = new BinaryFormatter();


            return (Dictionary<string, object>)formatter.Deserialize(stream);
        }

    }

    public SaveClass LoadFile_SaveClass(string savePath)
    {
        if (!File.Exists(savePath))
        {
            return new SaveClass();
        }
        using (FileStream stream = File.Open(savePath, FileMode.Open))
        {
            //the problem is here.
            var formatter = new BinaryFormatter();


            return formatter.Deserialize(stream) as SaveClass;
        }
    }


    public bool FileExists(string savePath) ///WE CHECK IF THE FILE EXISTS IN THE SAVE FOLDER.
    {
        return File.Exists(savePath);
    }

    public void DeleteFile(string savePath) ///WE DELETE THE DATA. I KNOW. PRETTY CRAZY EXPLANATION.
    {
        if (FileExists(savePath))
        {
            File.Delete(savePath);
        }
    }


    #endregion

    #region OLD SYSTEM

    [ContextMenu("Delete all files")]
    public void DeleteFiles()
    {
        DeleteFile("first");
        DeleteFile("second");
        DeleteFile("third");

    }

    public void Save(string savePath)
    {

        var state = LoadFile(savePath);
        CaptureState(state);
        SaveFile(state, savePath);
    }

    public void Load(string savePath)
    {

        var state = LoadFile(savePath);
        RestoreState(state);
    }



    private void CaptureState(Dictionary<string, object> state)
    {
        foreach (var saveable in FindObjectsOfType<SaveableEntity>())///WE LOOK FOR OBJECTS IN THE SCENE OF TYPE SAVEABLE ENTITY.
        {

            state[saveable.Id] = saveable.CaptureState();///THEN WE ASK THE SAVEABLE ENTITY TO LOOK FOR ANY OTHER SCRIPT THAT HAS DATA TO SAVE.
                                                         ///WE SAVE THAT DATA WITH A STRING THAT IS A GUID.
        }

    }

    void RestoreState(Dictionary<string, object> state)
    {
        foreach (var saveable in FindObjectsOfType<SaveableEntity>())///WE ALSO LOOK FOR SAVEABLE ENTITY.
        {

            if (state.TryGetValue(saveable.Id, out object value))
            {
                saveable.RestoreState(value);
            }
            else
            {
                Debug.LogError("me-Save: save data failed to be restored: " + saveable.name);
            }
        }
    }

    #endregion

    //so here i look for everyone. can i simply get the value and pass to the data instead?

    #region NEW SYSTEM

    //we use the base function to do stuff here
    //

    [field:SerializeField]public SaveSlotData currentSaveSlotData { get; private set; }

    void RestoreSaveSlots()
    {
        //we do this at the start
        //when we start the game the gamehandler will get this data and pass through the same way the savehandler does it
        //we cant ask each.

        for (int i = 0; i < _saveSlotArray.Length; i++)
        {
            var item = _saveSlotArray[i];

            SaveClass saveClass = LoadFile_SaveClass(item.GetSaveID);

            item.SetData(saveClass);
        }

        //
         //in case we have. and thats the only time we will be doing that, but savcing i will be doing everytime you do anything.
        
    }

    [ContextMenu("DEBUG SAVE ALL SAVE SLOTS")]
    public void CaptureSaveSlots()
    {
        for (int i = 0; i < _saveSlotArray.Length; i++)
        {
            var item = _saveSlotArray[i];

            SaveFile(item._saveClass, item.GetSaveID);
        }

        //but instead of setting data.
        //we will send this data to save
    }

    public void SelectSaveSlot(SaveSlotData saveSlotData)
    {
        currentSaveSlotData = saveSlotData;

        RestoreStateUsingCurrentSaveSlot();
    }

    public void RestoreStateUsingCurrentSaveSlot()
    {
        if(currentSaveSlotData == null)
        {
            Debug.Log("weird");
            return;
        }

        currentSaveSlotData.RestoreGameState();
    }
    public void CaptureStateUsingCurrentSaveSlot()
    {
        if (currentSaveSlotData == null)
        {
            Debug.Log("weird");
            return;
        }

        currentSaveSlotData.CaptureGameState();
    }


    [ContextMenu("DELETE ALL SLOTS")]
    public void DeleteAllSaveSlots()
    {
        for (int i = 0; i < _saveSlotArray.Length; i++)
        {
            var item = _saveSlotArray[i];

            item.DeleteSaveData();
        }
    }

    //when to restore state. everytime the city is loaded.
    //when to save, when you do any action worth saving.

    #endregion

    void LoadDataToAllSlots()
    {
        //then we are going to get  the right information to the fellas.

        //i need all the information.


    }



}



//thats all i imagine.
//characters probably also need to be saved.
//so lets resume this work when i go to the city get things right.


//what i want to save
//Inventory - just city 
//city buildings. city buildings can save things gained by quests or other ways through its own index.
//