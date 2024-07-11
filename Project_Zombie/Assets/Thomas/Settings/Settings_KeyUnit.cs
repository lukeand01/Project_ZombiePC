using DG.Tweening;
using MyBox;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class Settings_KeyUnit : ButtonBase
{


    //when we fclick we remove it. if we click away it supposed to regen. 

    [Separator("KEY")]
    [SerializeField] TextMeshProUGUI keyNameText;
    [SerializeField] TextMeshProUGUI keyCodeNameText;

    KeyType _keyType;

    KeyCode keyCode_Initial;
    KeyCode keycode_new;

    Settings handler;

    bool isSelected;

    public void SetUp(KeyClass_Individual_ForShow keyClass, Settings handler)
    {
        _keyType = keyClass.key_Id;

        this.handler = handler;

        keyNameText.text = _keyType.ToString();
        keyCodeNameText.text = keyClass.key_Code.ToString();

        keyCode_Initial = keyClass.key_Code;
        keycode_new = keyClass.key_Code;
        //then we will be waiting for input or esc, whihc cancel the thing, or disabling the fella.
    }


    public void ConfirmChange()
    {
        //we will check if its teh same.
        //if not then we 

        //and we unselect this fella.

        if(keyCode_Initial == keycode_new)
        {
            return;
        }

        //then we need to pass this new info first to the setting and then to the player.
        keyCode_Initial = keycode_new;

        GameHandler.instance._settingsData.ChangeKey(_keyType, keyCode_Initial);

        handler.UnselectKeyUnit();
    }

    public void CheckForNewKeyInput()
    {
        //this is called when the thing is selected.

        if (Input.anyKeyDown)
        {
            keycode_new = GetFirstKeyWritten();
            keyCodeNameText.text = keycode_new.ToString();
        }

    }


    private void OnDisable()
    {
        ControlSelected(false);
        transform.DOScale(1f, 0).SetUpdate(true).SetEase(Ease.Linear);
        keyCodeNameText.DOFade(1, 0).SetUpdate(true).SetEase(Ease.Linear);
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);
        //we zoom it a bit. and we 

        handler.SelectKeyUnit(this);
    }

    IEnumerator SelectProcess()
    {
        float timer = 0.3f;
        transform.DOKill();
        transform.DOScale(1f, timer).SetUpdate(true).SetEase(Ease.Linear);
        yield return new WaitForSeconds(timer);
    }
    IEnumerator UnSelectProcess()
    {
        float timer = 0.3f;
        transform.DOKill();
        transform.DOScale(new Vector3(0.95f, 1, 0), timer).SetUpdate(true).SetEase(Ease.Linear);
        yield return new WaitForSeconds(timer);

        StopAllCoroutines();
    }
    IEnumerator FadeProcess()
    {
        float timer = 0.3f;
        keyCodeNameText.DOKill();
        keyCodeNameText.DOFade(0, timer).SetUpdate(true).SetEase(Ease.Linear);

        yield return new WaitForSecondsRealtime(timer);

        keyCodeNameText.DOFade(1, timer).SetUpdate(true).SetEase(Ease.Linear);

        yield return new WaitForSecondsRealtime(timer);

        StartCoroutine(FadeProcess());
    }

    public void Select()
    {
        isSelected = true;
        StopAllCoroutines();
        StartCoroutine(SelectProcess());
        StartCoroutine(FadeProcess());
    }
    public void UnSelect()
    {
        isSelected = false;
        StartCoroutine(UnSelectProcess());
        keyCodeNameText.DOKill();
        keyCodeNameText.DOFade(1, 0).SetUpdate(true).SetEase(Ease.Linear);

        var a = keyCodeNameText.color.a;
        a = 1;
        keyCodeNameText.color = new Color(keyCodeNameText.color.r, keyCodeNameText.color.g, keyCodeNameText.color.b, a);


        keyCodeNameText.text = keyCode_Initial.ToString();
    }

    

    //we will check every single possibility
    KeyCode GetFirstKeyWritten()
    {
        foreach (KeyCode kcode in Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKey(kcode))
            {
                Debug.Log("KeyCode down: " + kcode);
                return kcode;
            }
               
        }

        return KeyCode.None;
    }


}
