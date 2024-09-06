using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGraphicUnit : MonoBehaviour
{
    [SerializeField] BodyRendererClass _mainBody; //probably wont be using this
    [SerializeField] BodyRendererClass _highlightBody; //this is the same body but around the main grpahic to serve as highlight.

    //red should flash when damaged is dealt
    //purple should stay while the person is stunned
    //blue should stay while the person is ally.

    //mainbody can only perma change it

    //thse are only for highlight
    //there is a high prio material
    //there is a low prio material

    //high prio has a duration and while it has the duration it will be calledd
    //low prio does not. and it will remain as such, if no high, 

    Material material_HighPrio;
    Material material_LowPrio;

    float highPrio_Current;
    float highPrio_Total;

    private void Awake()
    {
        _mainBody.SetUp();
        _highlightBody.SetUp();
    }

    private void Update()
    {
        if(material_HighPrio != null)
        {

            if(highPrio_Current > highPrio_Total)
            {

                material_HighPrio = null;


                if(material_LowPrio != null)
                {
                    _highlightBody.ChangeMaterial_New(material_LowPrio);
                }
                else
                {
                    _highlightBody.ChangeMaterial_Original();
                }
            }
            else
            {
                highPrio_Current += Time.fixedDeltaTime;
                _highlightBody.ChangeMaterial_New(material_HighPrio);
            }

        }
    }

    public void MakeHighPrioMaterial(Material _material, float total)
    {
        highPrio_Current = 0;
        highPrio_Total = total;
        material_HighPrio = _material;
    }
    public void MakeLowPrioMaterial(Material _material)
    {
        material_LowPrio = _material;
        _highlightBody.ChangeMaterial_New(material_LowPrio);
    }
   
    public void ResetGraphic()
    {
        material_HighPrio = null;
        material_LowPrio = null;
        _mainBody.ChangeMaterial_Original();
        _highlightBody.ChangeMaterial_Original();
    }
    
}
