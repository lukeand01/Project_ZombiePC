using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuAnimationEventCall : MonoBehaviour
{
    //here we can call especifc things to make them active.

    [SerializeField] ParticleSystem _ps_Meteor_Smoke;
    public void Meteor_MakeSmoke()
    {
        _ps_Meteor_Smoke.gameObject.SetActive(true);
        _ps_Meteor_Smoke.Play();
    }




}
