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
        pointSpeed = 300;
        speed = 100;
        bless_Speed = 10;

        holder = transform.GetChild(0).gameObject;

        originalPos = roundHolder.transform.position;
        originalPos_New = roundHolder.transform.position;


        originalRoundSprite = roundIcon_New.sprite;
    }
    public void ControlUI(bool isVisible)
    {
        holder.SetActive(isVisible);
    }

    private void Update()
    {
        mouseIcon.transform.position = Input.mousePosition;

        if (!isFlashing)
        {
            var alpha = roundText_New.color.a;
            alpha = 1;
            roundText_New.color = new Color(roundText_New.color.r, roundText_New.color.g, roundText_New.color.b, alpha);          
        }

        HealthHandle();
        PointHandle();
        BlessHandle();
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

    void HealthHandle()
    {
        if (healthCurrent != healthTemporary)
        {
            if (healthCurrent > healthTemporary)
            {
                healthTemporary += Time.deltaTime * speed;
            }
            if (healthCurrent < healthTemporary)
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

    void PointHandle()
    {
        if (pointCurrent != pointTemporary)
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
        if (change != 0)
        {
            //spawn stuff to show
            CreateFadeUI_Point(change);

        }

        pointCurrent = current;
    }

    void CreateFadeUI_Point(int value)
    {

        FadeUI newObject = Instantiate(fadeTemplate);
        newObject.transform.SetParent(pointText.transform);

        float amount = 20f;
        float x = Random.Range(-amount * 3, amount * 3);
        float z = Random.Range(-amount, amount);

        newObject.transform.localPosition = Vector3.zero + new Vector3(0, 35, 0) + new Vector3(x, z, 0);

        Color color = Color.white;

        if (value > 0)
        {
            color = Color.green;
        }
        if (value < 0)
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

    public void ControlRoundProgressBarUI(bool isVisible)
    {

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

    #region ROUND NEW
    [Separator("ROUND")]
    [SerializeField] GameObject roundHolder_New;
    [SerializeField] Image roundIcon_New;
    [SerializeField] TextMeshProUGUI roundText_New;
    [SerializeField] Image roundBackground;
    Vector3 originalPos_New;

    Sprite originalRoundSprite;

    public void CloseRound_New()
    {
        roundHolder_New.transform.position = originalPos_New + new Vector3(0, Screen.height * 0.1f, 0);

        roundHolder_New.SetActive(false);
    }

    public void OpenRound_New()
    {
        roundHolder.SetActive(false);
        roundHolder_New.SetActive(true);
        roundHolder_New.transform.DOMove(originalPos_New, 1.5f);

        roundIcon_New.sprite = originalRoundSprite;
    }

    bool isFlashing;

    public void UpdateRoundText_New(int newValue, bool isForce, Sprite newImage)
    {
        StopAllCoroutines();
        isFlashing = false;
        if (isForce)
        {
            roundText_New.text = newValue.ToString();
            return;
        }
        
        roundText_New.DOFade(1, 0);
        StartCoroutine(UpdateRoundTextProcess(newValue, newImage));
    }

    IEnumerator UpdateRoundTextProcess(int newValue, Sprite newImage)
    {
        isFlashing = true;

        roundText_New.DOKill();

        float timer = 0.15f;
        roundText_New.DOFade(0, timer);

        yield return new WaitForSeconds(timer);

        roundText_New.text = newValue.ToString();
        roundText_New.DOFade(1, timer);

        yield return new WaitForSeconds(timer);

        roundText_New.DOFade(0, timer);

        if(newImage == null)
        {
            roundIcon_New.sprite = originalRoundSprite;
            roundBackground.DOColor(Color.black, 0.3f);
        }
        else
        {
            roundIcon_New.sprite = newImage;
            roundBackground.DOColor(Color.red, 0.3f);
        }

        


        yield return new WaitForSeconds(timer);

        roundText_New.DOFade(1, timer);

        yield return new WaitForSeconds(timer);
        roundText_New.DOFade(0, timer);

        yield return new WaitForSeconds(timer);

        roundText_New.DOFade(1, timer);

        isFlashing = false;



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


    #region BLESS
    [Separator("BLESS")]
    [SerializeField] GameObject blessHolder; //it should only be visible if there is more than just one bless.
    [SerializeField] TextMeshProUGUI blessText;

    float bless_Current;
    float bless_Temporary;
    float bless_Speed;

    public void UpdateBless(int current, int change = 0)
    {
        if (change != 0)
        {
            //spawn stuff to show
            CreateFadeUI_Bless(change);

        }

        bless_Current = current;
    }

    void BlessHandle()
    {
        if (bless_Current != bless_Temporary)
        {
            if (bless_Current > bless_Temporary)
            {
                bless_Temporary += Time.unscaledDeltaTime * bless_Speed;
            }
            if (bless_Current < bless_Temporary)
            {
                bless_Temporary -= Time.unscaledDeltaTime * bless_Speed;
            }
        }

        blessText.text = "Bless: " + bless_Temporary.ToString("f0");
    }

    public void Bless_ForceUpdate(int current)
    {
        bless_Current = current;
        bless_Temporary = current;
    }

    void CreateFadeUI_Bless(int value)
    {

        FadeUI newObject = Instantiate(fadeTemplate);
        newObject.transform.SetParent(blessText.transform);

        float amount = 20f;
        float x = Random.Range(-amount * 3, amount * 3);
        float z = Random.Range(-amount, amount);

        newObject.transform.localPosition = Vector3.zero + new Vector3(0, 35, 0) + new Vector3(x, z, 0);

        Color color = Color.white;

        if (value > 0)
        {
            color = Color.green;
        }
        if (value < 0)
        {
            color = Color.red;
        }

        newObject.SetUp(value.ToString(), color);
    }

    #endregion


}