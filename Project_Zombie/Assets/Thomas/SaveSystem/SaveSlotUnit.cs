using MyBox;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class SaveSlotUnit : ButtonBase
{
    SaveSlotData _saveSlot;
    [Separator("SAVE SLOT")]
    [SerializeField] GameObject _saveInfoHolder;
    [SerializeField] TextMeshProUGUI _saveInfo_lastTimePlayerText;
    [SerializeField] TextMeshProUGUI _saveInfo_HqLevelText;
    [SerializeField] GameObject _emptySaveHolder;
    [SerializeField] GameObject _selectedHolder;
    [SerializeField] GameObject _confirmDeleteHolder;
    [SerializeField] ButtonBase[] _confirmationButtonArray;
    public void SetUp(SaveSlotData saveSlot)
    {
        _saveSlot = saveSlot;

        UpdateUI();
        
        
    }

    void UpdateUI()
    {
        bool hasSave = _saveSlot._saveClass._hasSaveData;


        _emptySaveHolder.SetActive(!hasSave);
        _saveInfoHolder.SetActive(hasSave);

        if (hasSave)
        {
            //we load data into the things.
        }
    }

    //
    private void Update()
    {
        if (!IsHovering)
        {
            if (_confirmationButtonArray[0].IsHovering) return;
            if (_confirmationButtonArray[1].IsHovering) return;
            CloseConfirmHolder();
        }
    }


    private void OnDisable()
    {
        _selectedHolder.SetActive(false);
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Left)
        {
            //we load the game
            GameHandler.instance._saveHandler.SelectSaveSlot(_saveSlot);
            GameHandler.instance._sceneLoader.LoadMainCity();
            Debug.Log("load the game.");
        }
        if (eventData.button == PointerEventData.InputButton.Right && _saveSlot._saveClass._hasSaveData)
        {
            //we trigger this thing.
            _confirmDeleteHolder.SetActive(true);

        }
    }

    public void DeleteSaveSlot()
    {
        _saveSlot.DeleteSaveData();
        CloseConfirmHolder();
        UpdateUI();
    }
    public void CloseConfirmHolder()
    {
        _confirmDeleteHolder.SetActive(false);
    }


    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        _selectedHolder.SetActive(true);


    }
    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
        _selectedHolder.SetActive(false);

    }

}
