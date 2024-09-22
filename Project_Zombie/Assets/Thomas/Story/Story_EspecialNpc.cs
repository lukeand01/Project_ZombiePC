using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Story_EspecialNpc : MonoBehaviour, IInteractable
{
    [SerializeField] Story_NpcData npcData;
    [SerializeField] GameObject graphicHolder;
    [SerializeField] Animator _animator;

    Vector3 originalRotation;

    public void SetUp()
    {
        //i need to set up. what should i set up here?


    }

    public Story_EspecialNpc GetNpcModel()
    {
        return npcData.npcModel;
    }

    //how to determine the stage?
    //we can use an astring value_Level for adding new stuff. and once we use it we put in a list that we can no longer use it.

    private void Awake()
    {
        id = Guid.NewGuid().ToString();

        originalRotation = graphicHolder.transform.eulerAngles ;
    }

    //for now it will do interactions

    #region INTERACT
    [SerializeField] InteractCanvas _interactCanvas;
    string id;

    public string GetInteractableID()
    {
        return id;
    }

    public void Interact()
    {
        _animator.SetBool("IsTalking", true);
        RotateToPlayer();
        UIHandler.instance._DialogueUI.StartDialogue(npcData, this);
        isTalking = true;
    }

    void RotateToPlayer()
    {
        Vector3 direction = PlayerHandler.instance.transform.position - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(direction.normalized);

        graphicHolder.transform.DOKill();
        graphicHolder.transform.DORotate(targetRotation.eulerAngles, 0.8f).SetEase(Ease.Linear);
    }

    public void EndDialogue()
    {
        _animator.SetBool("IsTalking", false);
        ReturnToOriginalRotation();
        isTalking = false;

    }

    void ReturnToOriginalRotation()
    {
        graphicHolder.transform.DOKill();
        graphicHolder.transform.DORotate(originalRotation, 0.8f).SetEase(Ease.Linear);
    }


    public void InteractUI(bool isVisible)
    {
        _interactCanvas.gameObject.SetActive(isVisible);
        _interactCanvas.ControlInteractButton(isVisible);

    }

    bool isTalking;
    public bool IsInteractable()
    {
        return !isTalking;
    }

    #endregion


    public virtual void CallFunctionUnique(int triggerIndex)
    {

    }
}
