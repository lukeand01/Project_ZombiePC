using MyBox;
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

    [SerializeField]List<int> animationList_Cooldown = new(); //
    [SerializeField]List<int> animationList_Forbidden = new(); //no longer can be used

    [Separator("DOG")]
    [SerializeField] Animator animator_Dog;
    [SerializeField] GameObject dogHolderDebug;
    [SerializeField] AudioSource _audio_Howl;
    [SerializeField] AudioSource _audio_Walk;
    [SerializeField] AudioSource _audio_Run;
    //
    float cooldown_Total;
    float cooldown_Current;

    bool isAnimationRunning { get { return isAnimationRunning_Regular || isAnimationRunning_Especial; } }
    bool isAnimationRunning_Especial;
    bool isAnimationRunning_Regular;

    private void Start()
    {
        cooldown_Current = 0;
        cooldown_Total = 10;

    }

    //
    private void FixedUpdate()
    {
        if (isAnimationRunning) return;

        if (!_mainAnimator.GetCurrentAnimatorStateInfo(0).IsName("Animation_MainMenu_Idle"))
        {
            Debug.Log("not idle");
            return;
        }

        if(cooldown_Current > cooldown_Total)
        {
            CallAnimation();
            cooldown_Current = 0;
            cooldown_Total = Random.Range(30, 30);
            //cooldown_Total = 3;
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
                Debug.Log("on cooldown");
                continue;
            }
            if(animationList_Forbidden.Contains(randomAnimation))
            {
                Debug.Log("forbidden");
                continue;
            }

            //if we get here we roll for it.

            var item = _mainMenuAnimationClassArray[randomAnimation];

            int roll = Random.Range(0, 101);

            if(item.chanceToActive > roll)
            {
                Debug.Log("couldnt trigger");
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
                TriggerRightEspecialAnimation(item._controlType, item.animationHolder);
            }

            isDone = true;
        }


    }

    void TriggerRightEspecialAnimation(MainMenuAnimationKeyForEspecialControlType _controlType, GameObject holder)
    {
        holder.SetActive(true);
        if (_controlType == MainMenuAnimationKeyForEspecialControlType.Dog_01)
        {
            StartCoroutine(EspecificControl_Dog_01(holder));
        }

    }


    IEnumerator EspecificControl_Dog_01(GameObject holder)
    {
        //we spawn the dog.
        //make the dog walk
        //after some time the dog stop
        //he looks to the forest for a moment
        //then he starts running out of screen.

        animator_Dog.Play("_POLYGON_Dog_Locomotion_Walking");
        _audio_Walk.Play();

        yield return new WaitForSeconds(10); //then it stops
        
        animator_Dog.Play("_POLYGON_Dog_Action_Standing_Sniff");
        _audio_Walk.Stop();

        yield return new WaitForSeconds(5);

        animator_Dog.Play("_POLYGON_Dog_Action_Standing_Howl");
        _audio_Howl.Play();

        yield return new WaitForSeconds(7);

        _audio_Howl.Stop();
        _audio_Run.Play();
        animator_Dog.Play("_POLYGON_Dog_Locomotion_Running");

        yield return new WaitForSeconds(15);

        _audio_Run.Stop();
        holder.SetActive(false);

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