using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FadeUI_New : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] Image critHolder;

    //the problem is that 

    public void ResetForPool()
    {
        transform.DOKill();
    }

    //i want it to 


    FadeClass _fadeClass;

    bool runDuration;
    float duration_Current;
    float duration_Total;

    Vector3 sameTarget;
    Camera cam;
    private void Start()
    {
        cam = Camera.main;

        Vector3 direction = Camera.main.transform.position - transform.position;
        direction.y = 0f;

        Quaternion rotation = Quaternion.LookRotation(direction);
        rotation.eulerAngles = new Vector3(rotation.eulerAngles.x, rotation.eulerAngles.y, 0);
        transform.rotation = rotation;


        sameTarget = direction;
    }

    private void Update()
    {

        

        if (transform.parent != null)
        {
            float entityRotation = transform.parent.parent.parent.rotation.eulerAngles.y;


            Quaternion rotation = Quaternion.LookRotation(cam.transform.position);

            float cappedX = Mathf.Clamp(rotation.eulerAngles.x, 0, 30);

            rotation.eulerAngles = new Vector3(cappedX * -1, entityRotation * -1, 0);
            transform.localRotation = rotation;
        }

       




        //the problem is also the 

        if (!runDuration) return;

        if(duration_Current > duration_Total)
        {
            runDuration = false;
            DamageStep_1();
        }
        else
        {
            duration_Current += Time.fixedDeltaTime;
        }
    }


    public void SetUp(FadeClass _fadeClass)
    {
        //we will domoves for each

        duration_Current = 0;
        duration_Total = _fadeClass._duration_InBetween;

        this._fadeClass = _fadeClass;

        text.text = _fadeClass._name;
        text.color = _fadeClass._color;
        critHolder.gameObject. SetActive(_fadeClass._isCrit);

        if(_fadeClass._isCrit )
        {
            //i want to increase the size of text with this one.
            text.text += "!";
        }
        Vector3 initialScale = transform.localScale;

       //we make it small.
       //we are supposed



    }


    //it want to randomize to where it falls to, but all the damage should fall together.
    //it can go to either side.


    //

    int side;

    public void SetUp_Damage(FadeClass _fadeClass, int side)
    {
        //i will hard code it.

        duration_Current = 0;
        duration_Total = _fadeClass._duration_InBetween;

        text.text = _fadeClass._name;
        text.color = _fadeClass._color;
        //critHolder.gameObject.SetActive(_fadeClass._isCrit);

        this.side = side;

        Vector3 targetSize = Vector3.one ;
        Vector3 targetPos = transform.position;

        if (_fadeClass._isCrit)
        {
            //i want to increase the size of text with this one.
            text.text += "!";
            targetSize = Vector3.one * 1.4f;
        }


        transform.DOScale(0, 0); //instant make it small.
        text.DOFade(0, 0);
        //critHolder.DOFade(0, 0);

        float duration = 0.25f;
        transform.DOScale(targetSize, duration);
        text.DOFade(0.8f, duration).OnComplete(StartRunning);
       // critHolder.DOFade(0.8f, duration);

        


        //transform.DOMove(targetPos + new Vector3(0,5,0), 0.5f).SetEase(Ease.Linear);
        //it goes up as it fades in and scales to its target size.


        //i will hard code everything, i nust use the fadeclass for the name and if i want to replace it one day its easier.

    }

    void StartRunning()
    {
        runDuration = true;
    }

    void DamageStep_1()
    {
        //
        //hold it for a moment.
        //once this happen they choose a side to go to.



        transform.DOMove(transform.position + new Vector3(3 * side, -1f, 0), 0.5f).SetEase(Ease.Linear).OnComplete(DamageStep_3);
        transform.DOScale(0, 0.5f); //instant make it small.
        text.DOFade(0, 0.5f);
        //critHolder.DOFade(0, 0.5f);

    }
    void DamageStep_2()
    {
        //

        transform.DOScale(0, 0.25f); //instant make it small.
        text.DOFade(0, 0.25f);
        critHolder.DOFade(0, 0.25f);
        transform.DOMove(transform.position + new Vector3(5 * side, 2.5f, 0), 0.25f).SetEase(Ease.Linear).OnComplete(DamageStep_3);

    }
    void DamageStep_3()
    {
        //

        GameHandler.instance._pool.FadeUI_Release(this);


        //transform.DOMove(transform.position + new Vector3(1, -5,0), 0.5f).SetEase(Ease.Linear).OnComplete(DamageStep_4);

    }

    void DamageStep_4()
    {
        
    }

    //scale the damaged based in size?



    //the movement needs to be smooth.
    //it doesnt move up, it moves to the side and starts falling.

}

public class FadeClass
{
    //position and initial scale are set outside.

    public FadeClass(string _name, Color _color, float _duration_InBetween)
    {
        //this is the absolute necessary.
        this._name = _name; 
        this._color = _color;
        this._duration_InBetween = _duration_InBetween;

        //this is the standar time for fade in and fade out.
        _duration_Start = 0.1f;
        _duration_End = 0.1f;

    }


    //

    [field: SerializeField] public string _name { get; private set; }
    [field: SerializeField] public Color _color { get; private set; }


    [field: SerializeField] public float _duration_Start { get; private set; }
    [field: SerializeField] public float _duration_InBetween { get; private set; }
    [field: SerializeField] public float _duration_End { get; private set; }

    #region CRIT
    [field: SerializeField] public bool _isCrit { get; private set; }
    public void Make_Crit() => _isCrit = true;
    #endregion

    #region CONTROLLING THE SIZE
    [field: SerializeField] public Vector3 _targetScale_Text { get; private set; } = Vector3.one;
    [field: SerializeField] public Vector3 _targetScale_Holder { get; private set; } = Vector3.one;

    public void Make_Scale_Text(Vector3 size) => _targetScale_Text = size;
    public void Make_Scale_Holder(Vector3 size) => _targetScale_Holder = size;
    #endregion

    #region MOVEMENT

    [field: SerializeField] public Vector3 _targetDir { get; private set; }
    [field: SerializeField] public float _timeToReachDestination { get; private set; }
    public void Make_Movement(Vector3 _targetDir, float _timeToReachDestination)
    {
        this._targetDir = _targetDir;
        this._timeToReachDestination = _timeToReachDestination;
    }
    #endregion

    

}