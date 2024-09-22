using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPSOnTriggerScript : MonoBehaviour
{

    List<PSType> _psTypeList = new();

    public void SetUp(List<PSType> psTypeList)
    {
        _psTypeList = psTypeList;
    }

    private void OnDisable()
    {
        for (int i = 0; i < _psTypeList.Count; i++)
        {
            var item = _psTypeList[i];

            GameHandler.instance._pool.GetPS(item, transform);
        }
    }


    //before disabling it will spaw  the list of pstypes that were requested.
}
