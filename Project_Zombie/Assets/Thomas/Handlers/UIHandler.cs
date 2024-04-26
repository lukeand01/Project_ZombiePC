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
    #region GETTERS 
    public PlayerUI _playerUI { get {  return playerUIRef; } }

    public GunUI gunUI { get { return gunUIRef; } }

    public BDUI bdUI { get { return bdUIRef; } }

    public InventoryUI InventoryUI { get { return inventoryUIRef; } }

    public ChestUI ChestUI { get { return chestUIRef; } }

    public PauseUI PauseUI { get { return pauseUIRef; } }
    public AbilityUI AbilityUI { get { return abilityUIRef; } }
    #endregion

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
}
