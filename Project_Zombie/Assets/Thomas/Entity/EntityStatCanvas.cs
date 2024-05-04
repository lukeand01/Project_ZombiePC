using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityStatCanvas : MonoBehaviour
{
    Camera mainCam;

    [SerializeField] GameObject stunnedImage;

    private void Awake()
    {
        mainCam = Camera.main;
    }

    private void Update()
    {
        transform.LookAt(mainCam.transform.position);


        if (stunnedImage.activeInHierarchy)
        {
            stunnedImage.transform.Rotate(new Vector3(0, 0, 0.8f));
        }
    }

    public void ControlStunned(bool isVisible)
    {
        stunnedImage.SetActive(isVisible);
    }


    [SerializeField] BDUnit bdUnitTemplate; //get it from here and give it to the bd
    [SerializeField] Transform bdContainer;

    public BDUnit CreateBDUnit(BDClass bd)
    {
        if(bdUnitTemplate == null || bdContainer == null) 
        {
            return null;
        }

        //need to put this into container;
        BDUnit newObject = Instantiate(bdUnitTemplate);
        newObject.transform.SetParent(bdContainer);
        newObject.SetUp(bd);
        return newObject;
    }

}
