using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Story / Dialogue")]
public class Story_DialogueData : ScriptableObject
{
    //i will use this to decided what the npc should say
    //every npc will have a default, where they have nothing to say
    //in default a npc might make commentaries based in achievements. so we are going to create reactionary dialogue



    //how should a dialogue data progress?
    //i want the npcs to comment about the player deeds.
    //i want the npcs to not keep talking about the same thing. so if you give them an answer you shouldnt give the same again. unless its something like "leave"
    //so i probly want a list and those lists have conditionals.
    //should have a normal conditional of progress.

    //every list has an id - if you achieved something to get that dialog
    //and also every dialog will have a default. what the player will say while it remains in that level.
    //


    [field:SerializeField]public List<DialogueClass> dialogueList { get; private set; } = new();


    [Separator("FOR ESPECIAL DIALOGUE")]
    [SerializeField] string especialID; //this id is used for especial events. like noticing the player
    [SerializeField] bool shouldConnectToNormalDialogue; //if we have this then this means that once this dialogue ends then we go to the next dialogue that should be its normal dialogue.


    //it needs to trigger with events
    //but also normal progress
    //it needs default for each
    //and a way to deal with more thanone progress.

    //certain actions create trigger taht will be held in a list of achievemtsn for the player
    //at the start we check those lists.
    //we need a trigger to tell if we should change or not.

    //


}
[System.Serializable]
public class DialogueClass
{
    public string dialogueKeyForResponse; //we are going to check the list
    [TextArea]public List<string> dialogueStringList = new(); //this is what the npc says. then when it ends it either ends or trigger response.
    public List<ResponseClass> responseList = new(); 


   //i want the dialogue to be simple. a bit liek darksouls.
   //

}
[System.Serializable]
public class ResponseClass
{
    //
    [TextArea]public string responseText;


    public string keyForResponse; //we use this to find the next fella.

    public ReponseTriggerType response_trigger;
    public int responseTrigger_Index;

    //we check dialogue
    //and we check quest. we can do both
    //and then we check the trigger. we can do all of them at teh same time
}

//this will be used 
public enum ReponseTriggerType 
{ 
    Nothing,
    Quest,
    Gun,
    TriggerUniqueToThisNPC, //so each npc will handle with that on its own way

}