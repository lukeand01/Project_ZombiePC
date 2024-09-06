using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Button_Event_End : ButtonBase
{
    [Separator("EVENT")]
    [SerializeField] UnityAction _unityActions; //

    [Separator("COMPONENTS")]
    [SerializeField] GameObject _icon;
    [SerializeField] GameObject _hover;

    [Separator("SELECTED LIGHTS")]
    [SerializeField] Image[] _selectedImageArray;

    private void OnDisable()
    {
        _hover.SetActive(false);
        Unselect();
    }

    IEnumerator SelectedProcess()
    {
        //just keep flashing the lights.


        foreach (var item in _selectedImageArray)
        {
            item.gameObject.SetActive(true);
        }

        yield return new WaitForSeconds(0.3f);

        foreach (var item in _selectedImageArray)
        {
            item.gameObject.SetActive(false);
        }

        yield return new WaitForSeconds(0.3f);

        StartCoroutine(SelectedProcess());
    }


    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
       _hover.SetActive(false);
    }
    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
        _hover.SetActive(true);
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        _unityActions.Invoke();
        Select();
    }


    void Select()
    {
        StartCoroutine(SelectedProcess());
    }
    public void Unselect()
    {
        foreach (var item in _selectedImageArray)
        {
            item.gameObject.SetActive(false);
        }
    }

}
