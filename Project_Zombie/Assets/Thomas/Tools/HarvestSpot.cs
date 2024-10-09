using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarvestSpot : MonoBehaviour, IInteractable
{
    //
    [SerializeField] InteractCanvas _interactCanvas;

    [SerializeField] ToolType _toolType;
    [SerializeField] int _cooldown_Max;
    [SerializeField] int _cooldown_Min;

    int _cooldown_Current;
    string id;
    //cooldown works by turn


    private void Start()
    {
        id = MyUtils.GetRandomID();
        PlayerHandler.instance._entityEvents.eventPassedRound += PassedRound;
    }

    void PassedRound()
    {
        if(_cooldown_Current > 0)
        {
            _cooldown_Current --;
        }
    }

    public bool IsInteractable()
    {
        return true;
    }

    public void InteractUI(bool isVisible)
    {

        if(_cooldown_Current > 0)
        {
            _interactCanvas.gameObject.SetActive(false);
            return;
        }

        bool hasTool = PlayerHandler.instance._playerInventory.HasTool(_toolType);

        if(hasTool)
        {
            _interactCanvas.ControlNameHolder("Use " + _toolType.ToString());
        }
        else
        {
            _interactCanvas.ControlNameHolder("No " + _toolType.ToString());
        }

        _interactCanvas.gameObject.SetActive(isVisible);
        _interactCanvas.ControlInteractButton(isVisible);
        _interactCanvas.ControlCannotHolder(!hasTool);
    }

    public void Interact()
    {
        //play the sound of the harvest.
        if (_cooldown_Current > 0) return;
        bool hasTool = PlayerHandler.instance._playerInventory.HasTool(_toolType);

        if (!hasTool) return;

        ToolData data = PlayerHandler.instance._playerInventory.GetRandomTool(_toolType);

        if (data == null) return;

        //then we simply need to roll for them.
        //there should be a modifier to harvestluck.
        

        IngredientData ingredient = data.GetRandomIngredient();

        ingredient.OnHarvested(data);
        PlayerHandler.instance._playerInventory.AddIngredient(data, ingredient, 1);

        _cooldown_Current = Random.Range(_cooldown_Min, _cooldown_Max);
    }

    public string GetInteractableID()
    {
        return id;
    }
}

