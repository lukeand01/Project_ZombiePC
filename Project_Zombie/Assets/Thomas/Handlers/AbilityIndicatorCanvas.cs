using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityIndicatorCanvas : MonoBehaviour
{
    //this will work for both enemies and players. it always by the ground.
    [SerializeField] SpriteRenderer circleIndicator;
    [SerializeField] Image circleFill;
    public void StartCircleIndicator(float range)
    {
        //its a indicator 
        circleIndicator.transform.localScale = new Vector3(range, range, 0);
        circleIndicator.gameObject.SetActive(true);

    }

    public void CircleChangeColor(Color color)
    {
        circleIndicator.color = color;
        circleFill.color = color;   
    }

    public void StopCircleIndicator()
    {
        ControlCircleFill(0, 0);
        circleIndicator.gameObject.SetActive(false);
        
    }
    public void ControlCircleFill(float current, float total)
    {
        circleFill.gameObject.SetActive(total != 0);

        if (total == 0) return;


        float value = current / total;
        value = Mathf.Clamp(value, 0.1f, 0.85f);   
        circleFill.transform.localScale = new Vector3(value, value, 0); 

    }

}
