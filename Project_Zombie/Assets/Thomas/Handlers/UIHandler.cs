using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHandler : MonoBehaviour
{
    public static UIHandler instance;

    [Separator("REFERENCES")]
    [SerializeField] PlayerUI playerUIRef;
    [SerializeField] GunUI gunUIRef;
    [SerializeField] BDUI bdUIRef;
    [SerializeField] InventoryUI inventoryUIRef;
    [SerializeField] ChestUI chestUIRef;
    [SerializeField] PauseUI pauseUIRef;
    [SerializeField] AbilityUI abilityUIRef;
    [SerializeField] CityUI cityUIRef;
    [SerializeField] EquipWindowUI equipUIRef;
    [SerializeField] DescriptionWindow descriptionWindowRef;
    [SerializeField] EndUI endUIRef;
    [SerializeField] QuestUI questUIRef;
    [SerializeField] MouseUI mouseUIRef;
    [SerializeField] DialogueUI dialogueUIRef;
    [SerializeField] Settings settingUIRef;
    public DebugUI debugui;
    #region GETTERS 
    public PlayerUI _playerUI { get {  return playerUIRef; } }

    public GunUI gunUI { get { return gunUIRef; } }

    public BDUI bdUI { get { return bdUIRef; } }

    public InventoryUI InventoryUI { get { return inventoryUIRef; } }

    public ChestUI ChestUI { get { return chestUIRef; } }

    public PauseUI _pauseUI { get { return pauseUIRef; } }
    public AbilityUI _AbilityUI { get { return abilityUIRef; } }

    public CityUI _CityUI { get { return cityUIRef; } }

    public EquipWindowUI _EquipWindowUI { get { return equipUIRef; } }

    public DescriptionWindow _DescriptionWindow { get { return descriptionWindowRef; } }

    public EndUI _EndUI { get { return endUIRef; } }

    public QuestUI _QuestUI { get { return questUIRef; } }

    public MouseUI _MouseUI { get { return mouseUIRef; } }

    public DialogueUI _DialogueUI { get { return dialogueUIRef; } }

    public Settings _settingsUI { get { return settingUIRef; } }
    #endregion


    [Separator("OTHER CANVAS")]
    [SerializeField] Transform genericWorldCanvas; //we are going to put fade ui here.
    public Transform GetGenericWorldCanvas { get { return genericWorldCanvas; } }

    public void ControlUI(bool isCityUI)
    {
        ControlCityUI(isCityUI);
        ControlStageUI(!isCityUI);
    }

    public void ControlUiForMainMenu(bool isMainMenu)
    {
        ControlCityUI(!isMainMenu);
        ControlStageUI(!isMainMenu);
    }

    void ControlCityUI(bool isVisible)
    {
        _CityUI.ControlUI(isVisible);
    }
    void ControlStageUI(bool isVisible)
    {
        //stage ui is gun_Perma, playerui, abilityu

        gunUI.ControlUI(isVisible);
        _playerUI.ControlUI(isVisible);
        _AbilityUI.ControlUI(isVisible);
        _MouseUI.ControlVisibility(isVisible);
    }


    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }



        DontDestroyOnLoad(gameObject);
        
    }
    private void Start()
    {
       
    }
}
