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

        timerForTeleport_OriginalPosition = timerForTeleportHolder.transform.position;
        timerForTeleportHolder.SetActive(false);
        timerForTeleportHolder.transform.position += new Vector3(0, Screen.height * 0.1f, 0);


        damageWarnImage.DOFade(0, 0);

    }
    public void ControlUI(bool isVisible)
    {
        holder.SetActive(isVisible);
    }

    bool isUpdatingShield;
    bool isUpdatingHealth;

    private void Update()
    {
        mouseIcon.transform.position = Input.mousePosition;


        if(shieldBar.fillAmount > 0 && shieldBar.isActiveAndEnabled )
        {
            if(shieldText.transform.localScale != Vector3.one && !isUpdatingShield)
            {
                isUpdatingShield = true;
                shieldText.transform.DOKill();
                shieldText.transform.DOScale(1, 0.25f).SetEase(Ease.Linear);

                healthText.transform.DOKill();
                healthText.transform.DOScale(0.75f, 0.25f).SetEase(Ease.Linear);
            }

            if (shieldText.transform.localScale == Vector3.one && isUpdatingShield)
            {
                isUpdatingShield = false;
            }


        }
        else
        {
            if (healthText.transform.localScale != Vector3.one && !isUpdatingHealth)
            {
                isUpdatingHealth = true;
                shieldText.transform.DOKill();
                shieldText.transform.DOScale(0.75f, 0.25f).SetEase(Ease.Linear);

                healthText.transform.DOKill();
                healthText.transform.DOScale(1, 0.25f).SetEase(Ease.Linear);
            }

            if (healthText.transform.localScale == Vector3.one && isUpdatingHealth)
            {
                isUpdatingHealth = false;
            }


        }


        HandleDamageFlash();
        HandleRoundText();

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
    [SerializeField] Image damageWarnImage;

    float healthTotal;
    float healthCurrent;
    float healthTemporary;

    float speed;

    public void UpdateHealth(float current, float total, float damage)
    {
        healthTotal = total;
        healthCurrent = current;

        if(damage > 0)
        {
            StartDamageFlash();
        }
    }

    public void ForceUpdateHealth(float current, float total)
    {
        UpdateHealth(current, total,0);
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

    bool isDamageFlashing;

    void StartDamageFlash()
    {
        float time = 0.1f;
        damageWarnImage.DOKill();
        damageWarnImage.DOFade(0.3f, time).SetEase(Ease.Linear);

        isDamageFlashing = true;
    }


    void HandleDamageFlash()
    {
        if (isDamageFlashing && damageWarnImage.color.a == 0.3f)
        {
            isDamageFlashing = false;
            damageWarnImage.DOFade(0, 0.1f);
        }

    }



    #endregion

    #region POINTS
    [Separator("POINTS")]
    [SerializeField] TextMeshProUGUI pointText;
    [SerializeField] FadeUI fadeTemplate;

    float pointCurrent;
    float pointTemporary;
    float pointSpeed;

    float pointTimer_Current;
    [SerializeField] float pointTimer_Total;

    void PointHandle()
    {

        //here is the problem. we need to count a timer and then call it, but call it only 1 at a time.

       

        

        if (pointCurrent != pointTemporary)
        {
            if (pointTimer_Current > 0)
            {
                pointTimer_Current -= Time.deltaTime * pointSpeed;
                return;
            }

            if (pointCurrent > pointTemporary)
            {
                pointTemporary += 1;
            }
            if (pointCurrent < pointTemporary)
            {
                pointTemporary -= 1;
            }

            pointTimer_Current = pointTimer_Total;
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

    Coroutine roundTextCoroutine;

    [ContextMenu("TEST ROUND")]
    public void Debug_RoundText()
    {
        UpdateRoundText_New(5, false, null);
    }
    [ContextMenu("TEST ROUND_1")]
    public void Debug_RoundText_1()
    {
        UpdateRoundText_New(10, false, null);
    }

    public void UpdateRoundText_New(int newValue, bool isForce, Sprite newImage)
    {


        //Debug.Log("functuin was called");
        if (isForce)
        {
            //Debug.Log("force was called");
            roundText_New.text = newValue.ToString();
            roundText_New.DOFade(1, 0);
            return;
        }

        //Debug.Log("started this");
        StartRoundText(newValue, 6, newImage, 0.18f);
       //roundTextCoroutine = StartCoroutine(UpdateRoundTextProcess(newValue, newImage));
    }

    int amountRotations_Total = 0;
    int amountRotations_Current = 0;

    int newRoundValue = -1;
    Sprite newImage;


    float timer;

    void StartRoundText(int newRoundValue, int amountRotations_Total, Sprite newImage, float timer)
    {
        isFlashing = true;
        this.newRoundValue = newRoundValue; 
        this.amountRotations_Total = amountRotations_Total;
        amountRotations_Current = 0;
        this.newImage = newImage;
        this.timer = timer;
    }

    void HandleRoundText()
    {
        if (!isFlashing)
        {
            return;
        }

        if(roundText_New.color.a == 1)
        {
            roundText_New.DOKill();
            roundText_New.DOFade(0, timer).SetEase(Ease.Linear);
                
            amountRotations_Current ++;
        }
        if (roundText_New.color.a == 0)
        {

            if (newRoundValue != -1)
            {
                roundText_New.text = newRoundValue.ToString();
                newRoundValue = -1;
            }

            if (newImage == null)
            {
                roundIcon_New.sprite = originalRoundSprite;
                roundBackground.DOColor(Color.black, 0.3f).SetEase(Ease.Linear);
            }
            else
            {
                roundIcon_New.sprite = newImage;
                roundBackground.DOColor(Color.red, 0.3f).SetEase(Ease.Linear);
            }

            roundText_New.DOKill();
            roundText_New.DOFade(1, timer).SetEase(Ease.Linear);

            amountRotations_Current++;
        }

        if (amountRotations_Current >= amountRotations_Total || amountRotations_Total == 0)
        {
            roundText_New.DOFade(1, timer).SetEase(Ease.Linear);
            isFlashing = false;
        }

    }



    IEnumerator UpdateRoundTextProcess(int newValue, Sprite newImage)
    {

        isFlashing = true;

        float timer = 0.15f;
        roundText_New.DOKill();
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


    #region TIMER FOR TELEPORT
    [Separator("TIMER FOR TELEPORT")]
    [SerializeField] GameObject timerForTeleportHolder;
    [SerializeField] TextMeshProUGUI timerForTeleportText;

    Vector3 timerForTeleport_OriginalPosition;

    

    public void ShowTimerForTeleport()
    {
        timerForTeleportHolder.SetActive(true);
        timerForTeleportHolder.transform.DOMoveY(timerForTeleport_OriginalPosition.y, 0.5f);

    }


    public void HideTimerForTeleport()
    {        
        timerForTeleportHolder.transform.DOMoveY(timerForTeleport_OriginalPosition.y + Screen.height * 0.1f, 0.25f);
    }

    Coroutine warnCoroutine;

    public void UpdateTimerForTeleport(int timer)
    {
        timerForTeleportText.text = timer.ToString("F0");   
        if(timer < 10)
        {
            if(warnCoroutine != null)
            {
                StopCoroutine(warnCoroutine);   
            }
            StartCoroutine(WarnTimerProcess());

        }
        else
        {
            timerForTeleportText.color = Color.white;
        }
    }

    IEnumerator WarnTimerProcess()
    {
        float duration = 0.4f;
        timerForTeleportText.DOKill();
        timerForTeleportText.transform.DOScale(1.2f, duration);
        timerForTeleportText.DOColor(Color.red, duration * 0.5f);
        yield return new WaitForSeconds(duration);
        timerForTeleportText.transform.DOScale(1f, duration);
        timerForTeleportText.DOColor(Color.white, duration * 0.5f);
    }

    #endregion

}