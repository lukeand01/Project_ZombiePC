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
    #endregion

    
    

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
        //stage ui is gun, playerui, abilityu
        gunUI.ControlUI(isVisible);
        _playerUI.ControlUI(isVisible);
        _AbilityUI.ControlUI(isVisible);

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
