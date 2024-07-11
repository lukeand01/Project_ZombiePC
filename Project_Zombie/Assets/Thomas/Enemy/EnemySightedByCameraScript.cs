using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySightedByCameraScript : MonoBehaviour
{
    public bool IsVisibleByCamera { get; private set; }

    void OnBecameInvisible()
    {

        IsVisibleByCamera = false;
    }
    void OnBecameVisible()
    {

        IsVisibleByCamera = true;
    }
}
