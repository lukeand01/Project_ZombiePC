using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tree : MonoBehaviour
{

    protected Node root = null;

    
    private void Update()
    {
        UpdateFunction();     
    }

    protected virtual void UpdateFunction()
    {
        if (root != null)
        {
            root.Evaluate();
        }
    } 

    

    protected void UpdateTree(Node newNode)
    {
        root = newNode;
    }
    protected void EndTree()
    {
        root = null;
    }
}
