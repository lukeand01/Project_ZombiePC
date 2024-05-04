using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyClass 
{


    public KeyClass()
    {
        SetUpKeys();
    }

    

    Dictionary<KeyType, KeyCode> keyDictionary = new Dictionary<KeyType, KeyCode>();



    public KeyCode GetKey(KeyType inputType)
    {
        return keyDictionary[inputType];
    }

    void ChangeKey(KeyType keyType, KeyCode key)
    {

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
    Dash

}