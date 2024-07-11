using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Drop / Nuke")]
public class DropData_Nuke : DropData
{
    public override void CallDrop()
    {
        base.CallDrop();
        Debug.Log("called nuke");
    }
}
