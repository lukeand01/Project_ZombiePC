using MyBox;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InteractCanvas_BossPortal : InteractCanvas
{

    private void Start()
    {
        CallBlessHolder(10);
        UpdateSigilList(new List<BossSigilType>() { BossSigilType.Nothing, BossSigilType.Nothing, BossSigilType.Nothing });
        PlayerHandler.instance._playerInventory.eventUpdateBossSigilUI += UpdateSigilList;
    }


    [Separator("BOSS UI - BlessHolder")]
    [SerializeField] GameObject _blessHolder;
    [SerializeField] TextMeshProUGUI _blessText;
    [SerializeField] GameObject _keyHolder;

    public void CallBlessHolder(int value)
    {
        _blessHolder.SetActive(true);
        _blessText.text = value.ToString();
    }

    public void CallKeyHolder()
    {
        //we need to get available keys to spend from the player
        //it should try its best to get varied.
        _keyHolder.SetActive(true);
    }


    [Separator("BOSS UI - SIGIL")]
    [SerializeField] Image[] _sigilImageArray;
    [SerializeField] Color _sigilColor_Base;
    [SerializeField] Color _sigilColor_Null;
    [SerializeField] Color _sigilColor_First;
    [SerializeField] Color _sigilColor_Second;
    [SerializeField] Color _sigilColor_Third;

    void UpdateSigilList(List<BossSigilType> bossSigilType)
    {
        //i just need an image and a sprite for each type of boss.

        //we only apply the color if there is at least three.

        bool cannotContinue = false;


        for (int i = 0; i < bossSigilType.Count; i++)
        {
            var item = bossSigilType[i];
            var image = _sigilImageArray[i];

            if(item == BossSigilType.Nothing)
            {
                image.color = _sigilColor_Null;
                cannotContinue = true;
            }
            else
            {
                image.color = _sigilColor_Base;
            }
        }


        if (cannotContinue) return;

        _sigilImageArray[0].color = _sigilColor_First;

        if (bossSigilType[0] != bossSigilType[1])
        {
            _sigilImageArray[1].color = _sigilColor_Second;
        }
        else
        {
            _sigilImageArray[1].color = _sigilColor_First;
        }

        if (bossSigilType[1] != bossSigilType[2])
        {
            _sigilImageArray[2].color = _sigilColor_Third;
        }
        else
        {
            _sigilImageArray[2].color = _sigilColor_First;
        }
    }
}
