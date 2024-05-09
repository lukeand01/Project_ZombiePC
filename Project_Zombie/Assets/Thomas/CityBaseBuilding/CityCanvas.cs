using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityCanvas : MonoBehaviour
{
    //TO NOTE: City ui is not handled here because it was easier to handle in the playercanvas.



    public static CityCanvas instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        UIHandler.instance.ControlUI(true);
    }




}
