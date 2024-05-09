using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BDUI : MonoBehaviour
{
    [SerializeField] BDUnit bdUnitTemplate; //get it from here and give it to the bd
    [SerializeField] Transform bdContainer;

    public BDUnit CreateBDUnit(BDClass bd)
    {

        if (bdUnitTemplate == null || bdContainer == null)
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
