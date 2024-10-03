using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGraphicHandler : MonoBehaviour
{
    //we will put all the grpahic we want here, and every time we want we can a turn on a random one.

    [SerializeField] EnemyGraphicUnit[] graphicArray;

    [Separator("MATERIALS")]
    [SerializeField] Material material_Damaged;
    [SerializeField] Material material_Ally;
    [SerializeField] Material material_Original;

    public void ResetEnemyGraphic()
    {
        

    }

    int currentArrayIndex;
    public void SelectRandomGraphic()
    {
        foreach (var item in graphicArray)
        {
            item.gameObject.SetActive(false);
        }

        int random = Random.Range(0, graphicArray.Length);
        currentArrayIndex = random;        
        graphicArray[random].gameObject.SetActive(true);
        graphicArray[random].ResetGraphic_WithNewMaterial(material_Original);


    }

    public void MakeAlly()
    {
        //now 
        graphicArray[currentArrayIndex].MakeLowPrioMaterial(material_Ally);
    }
    public void MakeDamaged()
    {
        graphicArray[currentArrayIndex].MakeHighPrioMaterial(material_Damaged, 0.25f);
       
    }

}
