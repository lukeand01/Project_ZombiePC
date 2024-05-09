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



    #region GETTERS 
    public PlayerUI _playerUI { get {  return playerUIRef; } }

    public GunUI gunUI { get { return gunUIRef; } }

    public BDUI bdUI { get { return bdUIRef; } }

    public InventoryUI InventoryUI { get { return inventoryUIRef; } }

    public ChestUI ChestUI { get { return chestUIRef; } }

    public PauseUI _pauseUI { get { return pauseUIRef; } }
    public AbilityUI AbilityUI { get { return abilityUIRef; } }

    public CityUI CityUI { get { return cityUIRef; } }
    #endregion

    
    

    public void ControlUI(bool isCityUI)
    {
        ControlCityUI(isCityUI);
        ControlStageUI(!isCityUI);
    }


    void ControlCityUI(bool isVisible)
    {
        CityUI.ControlUI(isVisible);
    }
    void ControlStageUI(bool isVisible)
    {
        //stage ui is gun, playerui, abilityu
        gunUI.ControlUI(isVisible);
        _playerUI.ControlUI(isVisible);
        AbilityUI.ControlUI(isVisible);

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

        
    }
    private void Start()
    {
       
    }
}
