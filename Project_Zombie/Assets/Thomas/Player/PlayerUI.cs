using DG.Tweening;
using MyBox;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    GameObject holder;

    [SerializeField] Image mouseIcon;

    private void Awake()
    {
        pointSpeed = 150;
        speed = 100;
        holder = transform.GetChild(0).gameObject;

        originalPos = roundHolder.transform.position;
        roundHolder.transform.position = originalPos;
    }
    public void ControlUI(bool isVisible)
    {
        holder.SetActive(isVisible);
    }

    private void Update()
    {
        mouseIcon.transform.position = Input.mousePosition;

        HandleHealth();
        HandlePoint();
    }


    #region MOUSE ICON ANIMATION
    public void CallMouseIconAnimation()
    {

        StopAllCoroutines();
        StartCoroutine(MouseIconAnimationProcess());
    }
    
    IEnumerator MouseIconAnimationProcess()
    {
        float timer = 0.03f;
        mouseIcon.transform.DOScale(0.6f, 0);
        mouseIcon.transform.DOScale(0.8f, timer);

        yield return new WaitForSecondsRealtime(timer);

        mouseIcon.transform.DOScale(0.6f, timer);
        yield return null;
    }
    #endregion

    #region HEALTH

    [Separator("HEALTH")]
    [SerializeField] Image healthBar;
    [SerializeField] TextMeshProUGUI healthText;

    float healthTotal;
    float healthCurrent;
    float healthTemporary;

    float speed;

    public void UpdateHealth(float current, float total)
    {
        healthTotal = total;
        healthCurrent = current;

    }

    public void ForceUpdateHealth(float current, float total)
    {
        UpdateHealth(current, total);
        healthTemporary = current;
    }

    void HandleHealth()
    {
        if(healthCurrent != healthTemporary)
        {
            if(healthCurrent > healthTemporary)
            {
                healthTemporary += Time.deltaTime * speed;
            }
            if(healthCurrent < healthTemporary)
            {
                healthTemporary -= Time.deltaTime * speed;
            }

           
        }

        healthBar.fillAmount = healthTemporary / healthTotal;
        healthText.text = healthTemporary.ToString("f0") + "/" + healthTotal.ToString();

    }

    #endregion

    

    #region POINTS
    [Separator("POINTS")]
    [SerializeField] TextMeshProUGUI pointText;
    [SerializeField] FadeUI fadeTemplate;

    float pointCurrent;
    float pointTemporary;
    float pointSpeed;

    void HandlePoint()
    {
        if(pointCurrent != pointTemporary)
        {
            if (pointCurrent > pointTemporary)
            {
                pointTemporary += Time.deltaTime * pointSpeed;
            }
            if (pointCurrent < pointTemporary)
            {
                pointTemporary -= Time.deltaTime * pointSpeed;
            }
        }

        pointText.text = "Points: " + pointTemporary.ToString("f0");

    }


    public void UpdatePoint(int current, int change = 0)
    {
        if(change != 0)
        {
            //spawn stuff to show
            CreateFadeUI(change);

        }

        pointCurrent = current;
    }

    void CreateFadeUI(int value)
    {

        FadeUI newObject = Instantiate(fadeTemplate);
        newObject.transform.SetParent(pointText.transform);

        float amount = 20f;
        float x = Random.Range(-amount * 3, amount * 3);
        float z = Random.Range(-amount, amount);

        newObject.transform.localPosition = Vector3.zero + new Vector3(0, 35, 0) + new Vector3(x, z, 0);

        Color color = Color.white;

        if(value > 0)
        {
            color = Color.green;
        }
        if(value < 0)
        {
            color = Color.red;
        }

        newObject.SetUp(value.ToString(), color);
    }

    public void ForceUpdatePoint(int current)
    {
        pointCurrent = current;
        pointTemporary = current;
    }

    #endregion
   

    #region ROUND
    [Separator("ROUND")]
    [SerializeField] GameObject roundHolder;
    [SerializeField] Image roundBar;
    [SerializeField] TextMeshProUGUI roundText;
    Vector3 originalPos;

    public void CloseRound()
    {
        roundHolder.transform.position = originalPos + new Vector3(0, Screen.height * 0.1f, 0);

        roundHolder.SetActive(false);
    }

    public void OpenRound()
    {
        roundHolder.SetActive(true);
        roundHolder.transform.DOMove(originalPos, 1.5f);
    }


    public void UpdateRoundText(string roundString)
    {
        roundText.text = roundString;
    }
    public void UpdateRoundBar(float current, float total)
    {
        roundBar.fillAmount = current / total;
    }


    #endregion

    #region SHIELD
    [Separator("SHIELD")]
    [SerializeField] GameObject shieldHolder;
    [SerializeField] Image shieldBar;
    [SerializeField] TextMeshProUGUI shieldText;
    [SerializeField] Image shieldRegenBar;

    public void UpdateShield(float current, float total)
    {
        shieldHolder.gameObject.SetActive(total > 0);

        shieldBar.fillAmount = current / total;
        shieldText.text = current.ToString() + " / " + total.ToString();

    }

    public void UpdateShieldRegen(float current, float total)
    {
        shieldRegenBar.fillAmount = current / total;   
    }

    #endregion

}
