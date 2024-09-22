using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MainMenuScene : MonoBehaviour
{
    //this will handle triggering the scene animation. randomly choosing them.

    //we reference them through index. and there is an animation in the mainmenuscene that takes care of it.

    [SerializeField] Animator _mainAnimator;
    [SerializeField] MainMenuAnimationClass[] _mainMenuAnimationClassArray;

    List<int> animationList_Cooldown = new(); //
    List<int> animationList_Forbidden = new(); //no longer can be used

    [SerializeField] Animator animator_Dog;


    float cooldown_Total;
    float cooldown_Current;

    bool isAnimationRunning { get { return isAnimationRunning_Regular || isAnimationRunning_Especial; } }
    bool isAnimationRunning_Especial;
    bool isAnimationRunning_Regular;

    private void Start()
    {
        cooldown_Current = 0;
        cooldown_Total = Random.Range(25, 35);
    }

    private void FixedUpdate()
    {
        if (isAnimationRunning) return;

        if(cooldown_Current > cooldown_Total)
        {
            CallAnimation();
            cooldown_Current = 0;
            cooldown_Total = Random.Range(25, 35);
        }
        else
        {
            cooldown_Current += Time.fixedDeltaTime;
        }
    }

    void CallAnimation()
    {
        //we need to get a random one
        //we need a list for those that were already used, and so they wont come back at least for the next three 
        //and another for those that cannot be used again.

        int safeBreak = 0;

        bool isDone = false;


        while (!isDone)
        {
            safeBreak++;

            if(safeBreak > 1000)
            {
                Debug.Log("safe returned here");
                break;
            }

            int randomAnimation = Random.Range(0, _mainMenuAnimationClassArray.Length);

            if (animationList_Cooldown.Contains(randomAnimation))
            {
                continue;
            }
            if(animationList_Forbidden.Contains(randomAnimation))
            {
                continue;
            }

            //if we get here we roll for it.

            var item = _mainMenuAnimationClassArray[randomAnimation];

            int roll = Random.Range(0, 101);

            if(item.chanceToActive > roll)
            {
                continue;
            }


            if (item.canActiveOnlyOnce)
            {
                animationList_Forbidden.Add(randomAnimation);

              
            }
            else
            {
                animationList_Cooldown.Add(randomAnimation);

                if (animationList_Cooldown.Count >= 3)
                {
                    animationList_Cooldown.RemoveAt(2);
                }

            }

            if(item._controlType == MainMenuAnimationKeyForEspecialControlType.None)
            {
                //play animation

                string name = "Animation_MainMenu_";
                _mainAnimator.Play(name + item._id);
            }
            else
            {
                TriggerRightEspecialAnimation(item._controlType);
            }


        }

    }

    void TriggerRightEspecialAnimation(MainMenuAnimationKeyForEspecialControlType _controlType)
    {

        if (_controlType == MainMenuAnimationKeyForEspecialControlType.Dog_01)
        {
            StartCoroutine(EspecificControl_Dog_01());
        }

    }


    IEnumerator EspecificControl_Dog_01()
    {
        //we spawn the dog.
        //make the dog walk
        //after some time the dog stop
        //he looks to the forest for a moment
        //then he starts running out of screen.




        yield return null;
    }

}
[System.Serializable]
public class MainMenuAnimationClass
{
    public string _id;
    public GameObject animationHolder;
    [Range(1,100)]public int chanceToActive;
    public bool canActiveOnlyOnce;
    public MainMenuAnimationKeyForEspecialControlType _controlType;
}

public enum MainMenuAnimationKeyForEspecialControlType
{
    None,
    Dog_01 
}

//for the dog animation i need to control teh dog itself. i will call a ienu


//
//a hell hound playing
//someone climbing the mountain
//open a portal somewhere and a head popouts
//a ghost appear somewhere and he waves.
//