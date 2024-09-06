using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class FadeUI : MonoBehaviour
{
    //this is any ui that i want to fade overtime.
    [SerializeField]TextMeshProUGUI text;
    [SerializeField] GameObject critHolder;
    [SerializeField] AnimationCurve alphaColorCurve;
    [SerializeField] AnimationCurve scaleCurve;
    [SerializeField] AnimationCurve heightCurve;



    float timeForColor;
    float timeForColorModifier = 1;

    float timeForScale;
    float timeForScaleModifier = 1;

    float timeForHeight;
    float timeForHeightModifier = 1;

    [SerializeField] Color textColor;

    Vector3 origin;
    Vector3 originalScale;
    Camera cam;

    [SerializeField] bool debugDoesNotFade;

    bool isReusable;
    bool isReady;
    private void Start()
    {
        transform.localScale = new Vector3(1,1,1);
        //origin = transform.position;
        origin = transform.localPosition;
        originalScale = transform.localScale;
        cam = Camera.main;

    }

    public void ResetForPool()
    {

    }


    private void Update()
    {
        if (text == null) return;
        if (debugDoesNotFade) return;
        if (!isReady) return;

        text.color = new Color(textColor.r, textColor.g, textColor.b, alphaColorCurve.Evaluate(timeForColor));
        transform.localScale = originalScale * scaleCurve.Evaluate(timeForScale);
        //transform.position = origin + new Vector3(0, 0 + heightCurve.Evaluate(timeForHeight), 0);
        transform.localPosition = origin + new Vector3(0, 0 + heightCurve.Evaluate(timeForHeight), 0);


        timeForColor += Time.deltaTime * timeForColorModifier;
        timeForScale += Time.deltaTime * timeForScaleModifier;
        timeForHeight += Time.deltaTime * timeForHeightModifier;




        if (text.color.a <= 0)
        {
            if (isReusable) gameObject.SetActive(false);
            else Destroy(gameObject);

            
        }

    }

    public void Reuse()
    {
        isReusable = true;
    }

    public void SetUp(string text, Color color, bool isCrit = false)
    {
        //then we change a bit the vector.
        //the problem is that i dont want the fellas to overshadow each other.       

        this.text.text = text;
        this.text.color = color;

        textColor = color;

        timeForColor = 0;
        timeForScale = 0;
        timeForHeight = 0;

        critHolder.SetActive(isCrit);

        isReady = true;
    }

    
    public void ChangeScaleModifier(float newValue)
    {
        timeForScaleModifier = newValue;
    }
    public void ChangeHeightModifier(float newValue)
    {
        timeForHeight = newValue;
    }
    public void ChangeColorModifier(float newValue)
    {
        timeForColorModifier = newValue;
    }
}
