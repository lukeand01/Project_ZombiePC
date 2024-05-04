using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{

    [SerializeField] MeshRenderer[] rendArray;

    [SerializeField] Material defaultMaterial;
    [SerializeField] Material transparentMaterial;



    public string id {  get; private set; }
    private void Awake()
    {
        id = Guid.NewGuid().ToString();
    }

    public bool IsSame(string id)
    {
        return this.id == id;
    }


    public void ChangeMaterial(bool isTransparent)
    {
        if(isTransparent)
        {
            foreach (var item in rendArray)
            {
                item.material = transparentMaterial;
            }

        }
        else
        {
            foreach (var item in rendArray)
            {
                item.material = defaultMaterial;
            }
        }

    }


}
