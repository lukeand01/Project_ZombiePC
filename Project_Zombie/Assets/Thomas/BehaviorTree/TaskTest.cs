using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskTest : Node
{
    //this is to see if the thing works.
    //its going to count to 5 every 1 second for every number.
    //when its done it goes back o 0
    int maxNumber;
    int currentNumber;
    float timeBetweenNumber;
    float currentCounting = 0 ;
    public TaskTest(int maxNumber, float timeBetweenNumber)
    {
        Debug.Log("init tasktest");
        currentCounting = 0;
        currentNumber = 0;
        this.maxNumber = maxNumber;
        this.timeBetweenNumber = timeBetweenNumber;
    }


    public override NodeState Evaluate()
    {
        if (currentCounting < timeBetweenNumber)
        {
            Debug.Log("counting up");
            currentCounting += Time.deltaTime;
        }
        else
        {
            currentCounting = 0;
            currentNumber += 1;
            Debug.Log("i am counting up ");
        }

        state = NodeState.Running;
        return state;
    }
}
