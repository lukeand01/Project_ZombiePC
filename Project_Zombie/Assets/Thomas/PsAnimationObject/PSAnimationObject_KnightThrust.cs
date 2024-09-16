using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PSAnimationObject_KnightThrust : PSAnimationObject
{
    //this must move from original to the end
    //
    [SerializeField] AudioClip audioClip; //for each time it explodes,.
    [SerializeField] ParticleSystem[] _psArray;
    
    //we trigger each in a cooldown
    //each time we trigger one 

    public override void CallAnimation()
    {
        ForceReset();

        StartCoroutine(ActivateProcess());
        StartCoroutine(DesactivateProcess());
    }


    Transform _owner;

    public void SetOwner(Transform owner)
    {
        _owner = owner;
    }

    public void ReturnToOwner()
    {

        transform.SetParent(_owner);
        transform.localPosition = Vector3.zero;
        gameObject.SetActive(false);
    }

    void ForceReset()
    {
        for (int i = 0; i < _psArray.Length; i++)
        {
            var item = _psArray[i];

            item.gameObject.SetActive(false);
        }
    }

    IEnumerator ActivateProcess()
    {
        for (int i = 0; i < _psArray.Length; i++)
        {
            var item = _psArray[i];
            item.gameObject.SetActive(true);
            item.Play();

            GameHandler.instance._soundHandler.CreateSfx_WithAudioClip(audioClip, item.transform, 0.5f);

            yield return new WaitForSeconds(0.05f);
        }
    }
    IEnumerator DesactivateProcess()
    {
        yield return new WaitForSeconds(2);

        for (int i = 0; i < _psArray.Length; i++)
        {
            var item = _psArray[i];
            item.gameObject.SetActive(false);

            yield return new WaitForSeconds(0.2f);
        }

        gameObject.SetActive(false);

    }


    //we can only trigger the thing when 
    //i need a way for this to remain in position, but when this dies this has to return to the knight.
    //the death will take long so this is not a problem
}
