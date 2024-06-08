using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PowerData : ScriptableObject
{
    [field:SerializeField] public string powerName {  get; private set; }
    public abstract void ActivatePower();
    
}
