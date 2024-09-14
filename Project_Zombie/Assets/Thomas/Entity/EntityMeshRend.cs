using MyBox;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EntityMeshRend : MonoBehaviour
{
    [Separator("REND REF")]
    [SerializeField] BodyRendererClass[] bodyRendArray;




    private void Awake()
    {
        //this for the player.
        for (int i = 0; i < bodyRendArray.Length; i++)
        {
            var item = bodyRendArray[i];   
            item.SetUp();
        }
    }


    public void ApplyMaterial_EntireBody_New(Material material)
    {
        foreach (var item in bodyRendArray)
        {
            item.ChangeMaterial_New(material);
        }
    }
    public void ApplyMaterial_EntireBody_Original()
    {
        foreach (var item in bodyRendArray)
        {
            item.ChangeMaterial_Original();
        }
    }
}
[System.Serializable]
public class BodyRendererClass 
{
    //i will do this to save the original material individually.

    [ConditionalField(nameof(_meshRend), true)][SerializeField] SkinnedMeshRenderer _skinnedRend;
    [ConditionalField(nameof(_skinnedRend), true)][SerializeField] MeshRenderer _meshRend;

    [SerializeField] Material originalMaterial;



    public void ControlIfBodyVisible(bool isVisible)
    {
        if (_skinnedRend != null)
        {
            _skinnedRend.gameObject.SetActive(isVisible);
            return;
        }
        if (_meshRend != null)
        {
            _meshRend.gameObject.SetActive(isVisible);
            return;
        }
    }

    public void SetUp()
    {
        if(_skinnedRend != null)
        {
            originalMaterial = _skinnedRend.material;
            return;
        }
        if (_meshRend != null)
        {
            originalMaterial = _meshRend.material;
            return;
        }

    }

    public void ChangeMaterial_New(Material material)
    {
        if (_skinnedRend != null)
        {
            _skinnedRend.material = material;
            return;
        }
        if (_meshRend != null)
        {
            _meshRend.material = material;
            return;
        }
    }
    public void ChangeMaterial_Original()
    {
        if (_skinnedRend != null)
        {
            _skinnedRend.material = originalMaterial;
            return;
        }
        if (_meshRend != null)
        {
            _meshRend.material = originalMaterial;
            return;
        }
    }

    public void SetMaterial(Material originalMaterial)
    {        
        if (_skinnedRend != null)
        {
            _skinnedRend.material = originalMaterial;
            return;
        }
        if (_meshRend != null)
        {
            _meshRend.material = originalMaterial;
            return;
        }
    }


}

//we use this so we can cahnge body parts
public enum BodyPartType
{
    Head,
    Body,
}
