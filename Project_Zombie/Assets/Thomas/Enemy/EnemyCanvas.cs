using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class EnemyCanvas : MonoBehaviour
{
    // Start is called before the first frame update

    Camera mainCam;
    [SerializeField] FadeUI fadeTemplate;
    [SerializeField] Transform damageContainer;

    Vector3 sameTarget;


    private void Awake()
    {
        mainCam = Camera.main;
    }

    private void Start()
    {
        Vector3 direction = mainCam.transform.position - transform.position;
        direction.y = 0f;

        Quaternion rotation = Quaternion.LookRotation(direction);
        rotation.eulerAngles = new Vector3(0, rotation.eulerAngles.y, 0);
        transform.rotation = rotation;


        sameTarget = direction;
    }

    private void Update()
    {

       
        Quaternion rotation = Quaternion.LookRotation(sameTarget);
        rotation.eulerAngles = new Vector3(0, rotation.eulerAngles.y, 0);
        transform.rotation = rotation;
       
        //transform.LookAt(mainCam.transform.position);
    }

    public void MakeDestroyItself(Vector3 pos)
    {
        transform.position = pos;
        healthHolder.SetActive(false);
    }

    public void CreateDamagePopUp(float damage, DamageType damageType, bool isCrit)
    {
        FadeUI newObject = Instantiate(fadeTemplate);
        newObject.transform.SetParent(damageContainer);
         newObject.transform.rotation = Quaternion.Euler(0, -180, 0);
        float modifierX = 0;
        float modifierY = 35;
        float x = Random.Range(-modifierX, modifierX);
        float y = Random.Range(0, modifierY) + 50;

        Vector3 offset = new Vector3(x, y, 0);

        newObject.ChangeHeightModifier(1.8f);
        newObject.ChangeColorModifier(1);

        if (isCrit)
        {
            newObject.ChangeScaleModifier(1.6f);
        }
        
        //i need this to keep following the player.

        newObject.transform.localPosition = Vector3.zero + offset;

        string additional = "";

        if (isCrit)
        {
            additional = "*";
        }


        newObject.SetUp(damage.ToString("f1") + additional, Color.red);

    }

    public void CreateShieldPopUp()
    {
        FadeUI newObject = Instantiate(fadeTemplate);
        newObject.transform.SetParent(damageContainer);
        newObject.transform.rotation = Quaternion.Euler(0, -180, 0);
        float modifierX = 0;
        float modifierY = 25;
        float x = Random.Range(-modifierX, modifierX);
        float y = Random.Range(0, modifierY) + 100;

        Vector3 offset = new Vector3(x, y, 0);

        newObject.ChangeHeightModifier(1.8f);
        newObject.ChangeColorModifier(1);
        newObject.ChangeScaleModifier(2f);
       
        //i need this to keep following the player.

        newObject.transform.localPosition = Vector3.zero + offset;

        newObject.SetUp("SHIELD", Color.black);

    }

    [Separator("HEALTH")]
    [SerializeField] GameObject healthHolder;
    [SerializeField] Image healthFill;

    public void UpdateHealth(float current, float total)
    {
        healthFill.fillAmount = current / total;
    }

    [Separator("DURATION")]
    [SerializeField] GameObject durationHolder;
    [SerializeField] Image durationFill;

    public void UpdateDuration(float current, float total)
    {
        durationFill.fillAmount = current / total;
    }

}


