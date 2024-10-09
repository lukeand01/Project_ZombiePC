using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropData : ScriptableObject
{
    //what are the drops that i want?


    public string dropName;
    public Sprite dropIcon;

    public int rollRequired;

    public Chest_Drop dropModel;

    [TextArea]public string dropDescription;

    public int storeIndex {  get; private set; }

    public void SetIndex(int index)
    {
        storeIndex = index;
    }

    //we create the box.

    public void CreateDrop()
    {

    }

    public virtual void CallDrop()
    {

    }

    public virtual void RemoveDrop()
    {

    }

    public string GetChanceDescription()
    {
        if(rollRequired > 90)
        {
            return "Change to spawn: Low";
        }

        if(rollRequired > 50 && rollRequired <= 90)
        {
            return "Change to spawn: Medium";
        }

        if (rollRequired > 0 && rollRequired <= 50)
        {
            return "Change to spawn: High";
        }

        return "Error";
    }

}
public class DropClass 
{
    //this will be taking care of actually spawning and checking when it should spawn.
    DropData data;
    int turnProgress;

    public DropClass(DropData data, int turnProgress)
    {
        this.data = data;
        this.turnProgress = turnProgress;
    }

}



//drops do somethinge very x turns
//they are stored in teh player and checked every turn
//the drop point will always be the closest drop point if there is more than one
//more than one thing can spawn at the drop points. so i should set a system for it.