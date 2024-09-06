using DG.Tweening;
using MyBox;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Windows;

public class EntityAnimation : MonoBehaviour
{
    
    [SerializeField] Animator _animator;
    [SerializeField] Transform playerModelTransform;

    //what we can do is that while holding 


    const string ANIMATIONFIRSTARGUMENT = "Animation_";
    string animationID;

    const string ANIMATIONCOMMAND_IDLE = "Idle";
    const string ANIMATIONCOMMAND_WALK = "Walk";
    const string ANIMATIONCOMMAND_BLENDRUN = "BlendTree_Run";
    const string ANIMATIONCOMMAND_RUN = "Run";
    const string ANIMATIONCOMMAND_RUNBACK = "RunBack";
    const string ANIMATIONCOMMAND_SIDELEFT = "Side_Left";
    const string ANIMATIONCOMMAND_SIDERIGHT = "Side_Right";
    const string ANIMATIONCOMMAND_HIT= "Hit";
    const string ANIMATIONCOMMAND_AIMPISTOL = "AimPistol";
    const string ANIMATIONCOMMAND_AIMRIFLE = "AimRifle";
    const string ANIMATIONCOMMAND_RELOAD = "Reload";
    const string ANIMATIONCOMMAND_ATTACK = "Attack";
    const string ANIMATIONCOMMAND_DEATH = "Death";



    string GetTotalAnimationId(string nameID, int numberID)
    {
        if(animationID == "")
        {
            Debug.Log("problem");
        }


        string numberString = "_" + numberID.ToString();

        if(numberID < 10)
        {
            numberString = "_" + "0" + numberID.ToString();
        }


        return ANIMATIONFIRSTARGUMENT + animationID + nameID + numberString;

    }

    private void Awake()
    {
        
    }

    public void SetAnimationID(string animationID)
    {
        this.animationID = animationID + "_";
    }

    //need to change the 
    //now lets do
    public void ResetPlayerAnimation()
    {
        ControlIfAnimatorApplyRootMotion(false);

        CallAnimation_Idle(0);

        ControlWeight(1, 1);
        ControlWeight(2, 1);
        ControlWeight(3, 0.5f);
        //ControlWeight(4, 0);

        playerModelTransform.DOKill();
        playerModelTransform.transform.localPosition = new Vector3(0,-1,0);
        
    }

    private void Update()
    {
        HandleDamageAnimation();
        HandleUpperBody();
    }

    //i will do everything here for testing
    //the problem is that certain keys will not a number.
    void CallAnimation(string nameID, int numberID, int layer)
    {
        _animator.Play(GetTotalAnimationId(nameID, numberID), layer);
    }

    public void CallAnimation_Walk(int layer = 0, int numberID = 0)
    {
        _animator.Play(GetTotalAnimationId(ANIMATIONCOMMAND_WALK, numberID), layer);
    }

    //ONLY THE PLAYER CAN CALL THIS
    public void CallAnimation_Reload(float reloadTimer)
    {
       
        _animator.SetFloat("Reload_SpeedModifier", GetReloadSpeed(reloadTimer));
        _animator.SetLayerWeight(4, 0.7f);
        CallAnimation(ANIMATIONCOMMAND_RELOAD, 0, 4);
    }
    float GetReloadSpeed(float reloadValue)
    {
        reloadValue = Mathf.Clamp(reloadValue, 0.01f, 3f);

        // Calculate the mapped value
        float output = 3f - ((reloadValue - 0.01f) / (3f - 0.01f)) * (3f - 0.2f);

        return output;
    }
    public void StopAnimation_Reload()
    {
        _animator.SetLayerWeight(4, 0);
    }

    //ONLY THE PLAYER CAN CALL THIS
    public void CallAnimation_BlendRun()
    {
        
        //so i will update it here
        
        float speed = GetSpeedForAnimator();

        _animator.SetFloat("Bleed_Run_SpeedModifier", speed);

        _animator.Play(ANIMATIONCOMMAND_BLENDRUN + "_00", 2);
    }

    float GetSpeedForAnimator()
    {
        //for now i will just do the basic.
        float speed = PlayerHandler.instance._playerMovement.currentSpeed;
        // Calculate the new value in the range 0 to 3
        float newValue = (speed / 10);

        // Round to the nearest integer to get a discrete value between 0 and 3
        return newValue;


    }

    public void CallAnimation_Run(int layer = 2, int numberID = 0)
    {

        CallAnimation(ANIMATIONCOMMAND_RUN, numberID, layer);
    }
    public void CallAnimation_Idle(int layer = 2, int numberID = 0)
    {
        CallAnimation(ANIMATIONCOMMAND_IDLE, numberID, layer);
    }

    //when we call death we remove the weight of all layers and then we call death.

    public void UpdateMovementBleed(float forward, float side)
    {
        _animator.SetFloat("Forward", forward);
        _animator.SetFloat("Side", side);
       
    }


    public void CallAnimation_Attack(int layer = 2)
    {       
        int attackID = enemyIDDictionary[ANIMATIONCOMMAND_ATTACK].numberID_Current;        
        CallAnimation(ANIMATIONCOMMAND_ATTACK, attackID, layer);
    }

    public bool IsAttacking( int layer)
    {
        return IsAnimationRunning(ANIMATIONCOMMAND_ATTACK, layer);
    }
      

    public bool IsAnimationRunning(string nameID, int layer)
    {
        int value = enemyIDDictionary[nameID].numberID_Current;
        string totalID = GetTotalAnimationId(nameID, value); 
       return _animator.GetCurrentAnimatorStateInfo(layer).IsName(totalID);
    }

    #region DEATH

    public void ControlIfAnimatorApplyRootMotion(bool doesApply) => _animator.applyRootMotion = doesApply;
    public void CallAnimation_Death()
    {

        int attackID = 0;

        if (enemyIDDictionary.ContainsKey(ANIMATIONCOMMAND_DEATH))
        {
           attackID = enemyIDDictionary[ANIMATIONCOMMAND_DEATH].numberID_Current;
        }
        

        CallAnimation(ANIMATIONCOMMAND_DEATH, attackID, 0); //DEATH IS ALWAYS CALLED FROM THE MAIN LAYER
    }

    public bool IsDeathPlaying()
    {
        return IsAnimationRunning(ANIMATIONCOMMAND_DEATH, 0);
    }

    public void RerollDeathAnimation()
    {
        Debug.Log("yo");
        enemyIDDictionary[ANIMATIONCOMMAND_DEATH].RandomizeNumberID();
    }

    public float GetDurationForDeath()
    {
        return enemyIDDictionary[ANIMATIONCOMMAND_DEATH].numberID_Current;
    }
    #endregion

    #region ID FOR ENEMIES
    //enemies can have different animations that do the same thing, like idle_01, idle_02
    //i will create a dictionary here that must be update by the thing.


    Dictionary<string, EnemyInfoAnimationClass> enemyIDDictionary = new();



    public void AddEnemyID(string nameID, int min, int max)
    {
        enemyIDDictionary.Add(nameID, new EnemyInfoAnimationClass(nameID, min, max));
    }   

    public void UpdateAttackAnimationSpeed(float speed)
    {
        _animator.SetFloat("AttackSpeed", speed);
    }
    public void RerollAttackAnimation()
    {
        enemyIDDictionary[ANIMATIONCOMMAND_ATTACK].RandomizeNumberID();
    }

    public float GetDurationForAttack()
    {
        return enemyIDDictionary[ANIMATIONCOMMAND_ATTACK].numberID_Current;
    }

   

    #endregion

    #region CONTROL UPPERBODY
    AnimationState_UpperBody _stateUpperBody;

    public void SetStateUpperBody(AnimationState_UpperBody _stateUpperBody)
    {
        this._stateUpperBody = _stateUpperBody;
    }

    void HandleUpperBody()
    {
        if (_stateUpperBody == AnimationState_UpperBody.Nothing) return;
 
        switch (_stateUpperBody)
        {
            case AnimationState_UpperBody.Idle:
                _animator.SetLayerWeight(1, 0);
                CallAnimation(ANIMATIONCOMMAND_IDLE, 0, 1);
                playerModelTransform.localRotation = Quaternion.Euler(new Vector3(0, 10, 0));
                break;
            case AnimationState_UpperBody.Pistol:
                _animator.SetLayerWeight(1, 1);
                CallAnimation(ANIMATIONCOMMAND_AIMPISTOL, 0, 1);
                playerModelTransform.localRotation = Quaternion.Euler(new Vector3(0, 25, 0));
                break;
            case AnimationState_UpperBody.Rifle:
                _animator.SetLayerWeight(1, 1);
                CallAnimation(ANIMATIONCOMMAND_AIMRIFLE, 0, 1);
                playerModelTransform.localRotation = Quaternion.Euler(new Vector3(0, 25, 0));
                break;
        }
    }

    public void ControlWeight(int layer, float weight) => _animator.SetLayerWeight(layer, weight);


    #endregion

    #region TAKE DAMAGE

    //
    float hit_Current;
    float hit_Total;

    //instead i will just raise the weight and then reduce it after the time has passed

    public void CallAnimation_Hit(float timer)
    {
        Debug.Log("yo");
        CallAnimation(ANIMATIONCOMMAND_HIT, 1, 1);
        hit_Total = timer;
        hit_Current = 0;
        _animator.SetLayerWeight(1, 1);
    }

    void HandleDamageAnimation()
    {
        if (hit_Total == 0) return;
        if(hit_Current > hit_Total)
        {
            Debug.Log("here");
            _animator.SetLayerWeight(1, 0);
            hit_Total = 0;
        }
        else
        {
            hit_Current += Time.deltaTime;
        }
    }


    #endregion

    [Separator("PORTAL - ONLY FOR PLAYER")]
    [SerializeField] ParticleSystem _portal;
    public void ControlPortal(bool isVisible)
    {
        _portal.gameObject.SetActive(isVisible);


        if(isVisible)
        {
           
            _portal.gameObject.transform.localPosition = new Vector3(playerModelTransform.transform.localPosition.x, 0, playerModelTransform.transform.localPosition.z);
            _portal.Play();
        }

    }

    public void MovePlayerModelDown()
    {
        playerModelTransform.DOLocalMove(playerModelTransform.localPosition + new Vector3(0, -3, 0), 10);
    }
}

//this systems is not being universal.
//
//we need a min and a max.

public class EnemyInfoAnimationClass
{
    //we are going to get a id
    //int 

    public EnemyInfoAnimationClass(string nameID, int min, int max)
    {
        this.nameID = nameID;
        numberID_Min = min;
        numberID_Max = max;

        RandomizeNumberID();
    }

    public string nameID { get; private set; }

    int numberID_Max;
    int numberID_Min;

    public int numberID_Current { get; private set; }


    public void RandomizeNumberID()
    {
        numberID_Current = Random.Range(numberID_Min, numberID_Max + 1);
    }


}

public enum AnimationState_UpperBody
{
    Idle,
    Pistol,
    Rifle,
    Nothing
}