using DG.Tweening;
using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour
{
    //it will have different categories.
    //General
    //Key bindings
    //Graphics
    //Language
    //Audio
    //Credits


    private void Awake()
    {
        holder = transform.GetChild(0).gameObject;
    }
    private void Start()
    {
        //PlayerHandler.instance._playerController.block.AddBlock("TESTE", BlockClass.BlockType.Complete);

        originalPos = holder.transform.position;
        holder.transform.position += new Vector3(0, Screen.height, 0);

        foreach (var item in selectButtonArray)
        {
            item.SetUp(this);
        }


        InitializeUI();
    }

    private void Update()
    {
        if (!holder.activeInHierarchy) return;


        if (Input.GetKeyDown(KeyCode.Return))
        {

            if(currentUnit != null)
            {
                //we change the keycode instead. only this will truly change the input.
                currentUnit.ConfirmChange();
            }
            else
            {
                foreach (var item in audioUnitArray)
                {
                    item.UpdateInputText();
                }
            }

            return;
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(currentUnit != null)
            {
                //then we close this thing.
                UnselectKeyUnit();
                return;
            }

            CloseSetting();
        }

        if(currentUnit != null)
        {
            currentUnit.CheckForNewKeyInput();
        }
    }


    void InitializeUI()
    {
        SettingsData data = GameHandler.instance._settingsData;

        InitUI_Sound(data);
        InitiaUI_KeyBindings(data);
    }



    GameObject holder;
    [SerializeField] SettingsCategoryButton[] selectButtonArray;
    [SerializeField] GameObject[] optionHolderArray;

    Vector3 originalPos;

    int index;

    public void OpenSetting()
    {
        holder.SetActive(true);
        OpenCategory();
        holder.transform.DOMove(originalPos, 0.2f).SetUpdate(true).SetEase(Ease.Linear);
        PlayerHandler.instance._playerController.block.AddBlock("Settings", BlockClass.BlockType.Complete);
    }

    public void CloseSetting()
    {
        holder.transform.DOMove(transform.position + new Vector3(0, Screen.height, 0), 0.2f).SetUpdate(true).SetEase(Ease.Linear);
        PlayerHandler.instance._playerController.block.RemoveBlock("Settings");
    }


    public void SelectCategory(int index)
    {
        this.index = index;
        OpenCategory();
        
    }

    void OpenCategory()
    {
        //i wont move the thing. i will just make the right one appear.

        ResetCategoryAndHolders();

        selectButtonArray[index].ControlMouseClick(true);
        optionHolderArray[index].SetActive(true);

        
    }

    void ResetCategoryAndHolders()
    {
        foreach (var item in selectButtonArray)
        {
            item.ControlMouseClick(false);
        }

        foreach (var item in optionHolderArray)
        {
            item.SetActive(false);
        }
    }


    #region GENERAL

    #endregion

    #region KEY BINDINGS

    //i will just create them and just 

    [SerializeField] Settings_KeyUnit[] keyUnitList;

    void InitiaUI_KeyBindings(SettingsData data)
    {
        //now i will create this.
        //we can create directly with this because of playercontroller will be doijg the same with the same data.
        List<KeyClass_Individual_ForShow> keyClassList = data.keyList;

        if(keyClassList.Count != keyUnitList.Length)
        {
            Debug.Log("the two are not matching " + keyClassList.Count + " " + keyUnitList.Length);
            return;
        }

        for (int i = 0; i < keyClassList.Count; i++)
        {
            keyUnitList[i].SetUp(keyClassList[i], this);
        }

    }

    Settings_KeyUnit currentUnit;
    public void SelectKeyUnit(Settings_KeyUnit unit)
    {
        if(currentUnit != null)
        {
            currentUnit.UnSelect();
        }

        currentUnit = unit;
        currentUnit.Select();

    }

    public void UnselectKeyUnit()
    {
        currentUnit.UnSelect();
        currentUnit = null;
    }

    public void SetBackDefaultKeycode()
    {
        //first we inform the settings data to return to normal.
        //
        SettingsData data = GameHandler.instance._settingsData;

        data.SetKeycodeBackToDefault();
        PlayerHandler.instance._playerController.SetKeyBasedInSettings();


        InitiaUI_KeyBindings(data);

        //then we pass the new informatiuon back to the keys
    }

    #endregion

    #region SOUND
    [Separator("AUDIO")]
    [SerializeField] Settings_AudioUnit[] audioUnitArray;
    void InitUI_Sound(SettingsData data)
    {
        foreach (var item in audioUnitArray)
        {
            item.UpdateAudioValue(data);
        }
    }

    #endregion

}

public enum SettingsType 
{ 
    General = 0,
    KeyBindings = 1,
    Audio = 2,
    Language = 3,
    Graphic = 4,
    Credits = 5

}
