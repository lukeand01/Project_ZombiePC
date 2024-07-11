using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyClass 
{
   

    public KeyClass()
    {
        SetUpKeys();

        keyDictionary_initial = keyDictionary;

        keyRefList = new()
        {
            KeyType.MoveLeft,
            KeyType.MoveRight,
            KeyType.MoveDown, 
            KeyType.MoveUp,
            KeyType.Reload,
            KeyType.SwapWeapon,
            KeyType.Interact,
            KeyType.Ability1,
            KeyType.Ability2,
            KeyType.Ability3,
            KeyType.Shoot,
            KeyType.Pause,
            KeyType.Dash,
            KeyType.EquipWindow,
        };
    }



    Dictionary<KeyType, KeyCode> keyDictionary = new Dictionary<KeyType, KeyCode>();
    Dictionary<KeyType, KeyCode> keyDictionary_initial = new Dictionary<KeyType, KeyCode>();
    List<KeyType> keyRefList = new();


    public KeyCode GetKey(KeyType inputType)
    {
        return keyDictionary[inputType];
    }

    public void ChangeKey(KeyType keyType, KeyCode key)
    {
        if(keyDictionary.ContainsKey(keyType))
        {
            keyDictionary[keyType] = key;
        }
        else
        {
            Debug.LogError("found no key belonging to " + keyType);
        }
    }

    public void CreateNewDictionaryFromSettings(List<KeyClass_Individual_ForShow> keyList)
    {
        if (keyDictionary.Count == 0)
        {
            Debug.Log("this was no created");
            return;
        }

        foreach (var item in keyList)
        {
            keyDictionary[item.key_Id] = item.key_Code;
        }
    }
    
    public bool HasReplicatedKey(KeyCode _keyCode, KeyType _keyType)
    {
        //
        return false;
    }

    void SetUpKeys()
    {
        keyDictionary.Add(KeyType.MoveLeft, KeyCode.A);
        keyDictionary.Add(KeyType.MoveUp, KeyCode.W);
        keyDictionary.Add(KeyType.MoveDown, KeyCode.S);
        keyDictionary.Add(KeyType.MoveRight, KeyCode.D);

        keyDictionary.Add(KeyType.Reload, KeyCode.R);
        keyDictionary.Add(KeyType.SwapWeapon, KeyCode.Q);
        keyDictionary.Add(KeyType.Interact, KeyCode.F);

        keyDictionary.Add(KeyType.Ability1, KeyCode.Alpha1);
        keyDictionary.Add(KeyType.Ability2, KeyCode.Alpha2);
        keyDictionary.Add(KeyType.Ability3, KeyCode.Alpha3);

        keyDictionary.Add(KeyType.Shoot, KeyCode.Mouse0);

        keyDictionary.Add(KeyType.Pause, KeyCode.Escape);

        keyDictionary.Add(KeyType.Dash, KeyCode.LeftShift);

        keyDictionary.Add(KeyType.EquipWindow, KeyCode.Tab);
    }

    public List<KeyClass_Individual_ForShow> GetListForSettings()
    {
        //we will get all the values and put in the list.
        List<KeyClass_Individual_ForShow> keyList = new();

        foreach (var item in keyRefList)
        {
            KeyClass_Individual_ForShow key = new KeyClass_Individual_ForShow(item, keyDictionary[item]);
            keyList.Add(key);
        }

        return keyList;
    }

}

[System.Serializable]
public class KeyClass_Individual_ForShow
{
    [field:SerializeField] public KeyType key_Id { get; private set;}
    [field: SerializeField] public KeyCode key_Code { get; private set; }

    public KeyClass_Individual_ForShow(KeyType key_Id, KeyCode key_Code)
    {
        this.key_Id = key_Id;
        this.key_Code = key_Code;
    }

    public void ChangeKey(KeyCode _keycode)
    {
        key_Code = _keycode;
    }

}

public enum KeyType
{   
    
    MoveLeft,
    MoveRight,
    MoveUp,
    MoveDown,
    Reload,
    SwapWeapon,
    Interact,
    Ability1,
    Ability2,
    Ability3,
    Shoot,
    Pause,
    Dash,
    EquipWindow

}