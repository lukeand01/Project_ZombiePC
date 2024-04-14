using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class FadeUI : MonoBehaviour
{
    //this is any ui that i want to fade overtime.
    [SerializeField]TextMeshProUGUI text;
    [SerializeField] AnimationCurve alphaColorCurve;
    [SerializeField] AnimationCurve scaleCurve;
    [SerializeField] AnimationCurve heightCurve;



    float timeForColor;
    float timeForColorModifier = 1;

    float timeForScale;
    float timeForScaleModifier = 1;

    float timeForHeight;
    float timeForHeightModifier = 1;



    Vector3 origin;
    Vector3 originalScale;
    Camera cam;

    [SerializeField] bool debugDoesNotFade;

    bool isReusable;

    private void Start()
    {
        transform.localScale = new Vector3(1,1,1);
        origin = transform.position;
        originalScale = transform.localScale;
        cam = Camera.main;

    }


    private void Update()
    {
        if (text == null) return;
        if (debugDoesNotFade) return;

        text.color = new Color(text.color.r, text.color.g, text.color.b, alphaColorCurve.Evaluate(timeForColor));
        transform.localScale = originalScale * scaleCurve.Evaluate(timeForScale);
        transform.position = origin + new Vector3(0, 0 + heightCurve.Evaluate(timeForHeight), 0);


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

    public void SetUp(string text, Color color, bool crit = false)
    {
        //then we change a bit the vector.
        //the problem is that i dont want the fellas to overshadow each other.       
        this.text.text = text;
        this.text.color = color;

        timeForColor = 0;
        timeForScale = 0;
        timeForHeight = 0;
       
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
