using MyBox;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InteractCanvas_BossPortal : InteractCanvas
{

    private void Start()
    {
        CallBlessHolder(10);
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

    void UpdateSigilList(List<BossSigilType> bossSigilType)
    {
        //i just need an image and a sprite for each type of boss.
    }
}
