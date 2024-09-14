using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChallengeComponent_StayArea : ChallengeComponent
{

    //t the start we will spawn the area around itself. based on values given in teh editor
    //we shoot a raycast at this area and check for player.
    //if the player is not there we warn and start lowering the failure
    //if it gets to the limit it wins.
    //the number of enemies should be based on teh round. otherwise it might be too hard or too easy.

    //


    [Separator("STAY AREA")]
    //the timer for when youa re sitting in the right area.
    float success_Current;
    [SerializeField]float success_Total;

    //the timer for when you are otuside the area. reset when you are inside.
    float failure_Current;
    [SerializeField]float failure_Total;

    [SerializeField] AbilityIndicatorCanvas _abilityCanvas;
    [SerializeField] float radius;

    [SerializeField] LayerMask playerLayer;


    ChallengeHandler _handler;

    //i need to spawn every x time. at least send the data.
    //what about cap system? should i use it as well;
    //i need to base off the current round somehow, because the difficulty needs to adjust to the round
    //we will have a spawn system here. wewill use the stage data to get its stuff.
    //cannot have certain enemies?
    //i need to spawn basic enemies, and then maybe have one really strong enemy in the end.
    //


    private void Awake()
    {
        //playerLayer = 1 << 3;
        _handler = GetComponent<ChallengeHandler>();

        failure_Current = failure_Total;

       
    }

   

    public override void GiveReward()
    {
        Debug.Log("give reward");
    }

    public override void HandleChallengeComponent()
    {

        Ray ray = new Ray(transform.position - (Vector3.right), Vector3.right);

        //bool isPlayerInside = Physics.SphereCast(transform.position, radius, Vector3.right, out RaycastHit hit, Mathf.Infinity, playerLayer);
        bool isPlayerInside = Physics.OverlapSphere(transform.position, radius, playerLayer).Length > 0;       

        if(isPlayerInside)
        {
            PlayerInsideCircle();
        }
        else
        {
            PlayerOutsideCircle();
        }




    }

    void PlayerInsideCircle()
    {

        failure_Current = failure_Total;
        UIHandler.instance._playerUI.Challenge_FillBar(0, 0);
        if (success_Current > success_Total)
        {
            //slowly rotate the circle.
            //make it 

            _handler.WinChallenge();
        }
        else
        {
            success_Current += Time.deltaTime;

            float time = success_Current / success_Total;
            UIHandler.instance._playerUI.Challenge_UpdateQuantity("Progress: " + (time * 100).ToString("F0") + "%");
            _abilityCanvas.RotateCircle(new Vector3(0, 0, 20 * Time.deltaTime));
            _abilityCanvas.CircleChangeColor(Color.blue);

        }

    }
    void PlayerOutsideCircle()
    {

        if(failure_Current <= 0)
        {
            _handler.LoseChallenge();
        }
        else
        {

            failure_Current -= Time.deltaTime;
            UIHandler.instance._playerUI.Challenge_FillBar(failure_Current, failure_Total);
            _abilityCanvas.CircleChangeColor(Color.red);
        }
    }

    public override void StartChallengeComponent()
    {
        _abilityCanvas.gameObject.SetActive(true);
        _abilityCanvas.StartCircleIndicator(radius);
        UIHandler.instance._playerUI.Challenge_ControlTextHolder(true);
        UIHandler.instance._playerUI.Challenge_UpdateTitle("Hold your ground and survive!");
        _abilityCanvas.ControlCircleFill(1, 1);


        float time = success_Current / success_Total;
        UIHandler.instance._playerUI.Challenge_UpdateQuantity("Progress: " + (time * 100).ToString("F0") + "%");
    }

    public override void StopChallengeComponent()
    {
        _abilityCanvas.gameObject.SetActive(false);
        UIHandler.instance._playerUI.Challenge_ControlTextHolder(false);
        UIHandler.instance._playerUI.Challenge_ControlTextHolder(false);

        Debug.Log("this");
    }
}


//how do we deal with the portals enemy spawning
//
