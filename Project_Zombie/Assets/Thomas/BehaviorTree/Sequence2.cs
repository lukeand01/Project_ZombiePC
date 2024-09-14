using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Sequence2 : Node
{
    //the goal of this fella is that we count each one and stop till we either see sucess or failure
    //if its running then we keep only that one running.

    int currentIndex; //this is the fella currently running.
    public Sequence2() : base() { }
    public Sequence2(List<Node> children) : base(children) { }


    public override NodeState Evaluate()
    {
        //i dont care for each node just the current one.

        Node node = children[currentIndex];
        switch (node.Evaluate())
        {
            case NodeState.Failure:
                state = NodeState.Failure;
                //then we reset it.
                currentIndex = 0;
                return state;
            case NodeState.Success:
                //then we go to the next or close it.
                currentIndex++;
                if(currentIndex > children.Count - 1)
                {
                    currentIndex = 0;
                    return NodeState.Failure;
                }
                return NodeState.Success;
            case NodeState.Running:
                return NodeState.Running;
            default:
                state = NodeState.Success;
                return state;
        }


        return NodeState.Success;
    }
}
