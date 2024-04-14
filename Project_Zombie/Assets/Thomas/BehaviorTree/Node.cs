using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node 
{
    protected NodeState state;




    public Node parent;
    public List<Node> children = new List<Node>();

    Dictionary<string, object> dataContext = new Dictionary<string, object>();

    public Node()
    {
        parent = null;
    }
    public Node(List<Node> children)
    {
        this.children = children;
    }

    public void Attach(Node node)
    {
        node.parent = this;
        children.Add(node);
    }

    public void AttachOrder()
    {
        //this gets the children which is assigned, and tells them that this is its parent.
        for (int i = 0; i < children.Count; i++)
        {
            children[i].parent = this;
            children[i].AttachOrder();
        }
    }

    public virtual NodeState Evaluate() => NodeState.Failure;

    public void SetData(string key, object value)
    {
        dataContext[key] = value;
    }

    public object GetData(string key)
    {
        object value = null;

        if (dataContext.TryGetValue(key, out value)) return value;

        Node node = parent;
        while(node != null)
        {
            value = node.GetData(key);
            if (value != null) return value;

            node = node.parent;

        }

        return null;
    }

    public bool ClearData(string key)
    {

        if (dataContext.ContainsKey(key))
        {
            dataContext.Remove(key);
            return true;
        }


        Node node = parent;
        while (node != null)
        {
            bool cleared = node.ClearData(key);

            if (cleared) return true;

            node = node.parent;
        }

        return false;
    }


    #region CHANCE
    protected int chance;

    protected bool IsRandom()
    {
        int random = Random.Range(0,100);
        return chance >= random;
    }
    #endregion
}

public enum NodeState
{
    Success,
    Failure,
    Running
}