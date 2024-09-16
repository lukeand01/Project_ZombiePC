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

    //what we can try is a copy? but that is not that good.
    //i dont want to create that many canvas?

    private void Awake()
    {
        mainCam = Camera.main;

    }



    private void Start()
    {

        //the problem is with 


        return;
        Vector3 direction = mainCam.transform.position - transform.position;
        direction.y = 0f;

        Quaternion rotation = Quaternion.LookRotation(direction);
        rotation.eulerAngles = new Vector3(0, rotation.eulerAngles.y, 0);
        transform.rotation = rotation;


        sameTarget = direction;
    }

    private void Update()
    {

        return;
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

    public void ResetFadeList()
    {
        
        
    }


    //i need to put it in the container

    public void CreateDamagePopUp(DamageClass damage)
    {

        float modifierX = 30;
        float modifierY = 80;
        float modifierZ = 50;
        float x = Random.Range(-modifierX, modifierX);
        float y = Random.Range(0, modifierY) + 50;
        float z = Random.Range(-modifierZ, modifierZ);

        Vector3 offset = new Vector3(x, y, z);   



        int random = Random.Range(0, 2);

        if (random == 0)
        {
            random = -1;
        }

        for (int i = 0; i < damage.damageList.Count; i++)
        {
            var item = damage.damageList[i];

            FadeUI_New newObject = GameHandler.instance._pool.GetFadeUI(damageContainer.position);
            newObject.transform.SetParent(damageContainer);
            //newObject.transform.rotation = Quaternion.Euler(0, -180, 0);

            Color damageColor = PlayerHandler.instance.GetColorForDamageType(item._damageType);

            FadeClass _fadeClass = new FadeClass(item._value.ToString(), damageColor, 0.8f);

            //the crit is too ugly.

            if (item.isCrit)
            {
                _fadeClass.Make_Crit();
            }

            newObject.transform.localPosition = Vector3.zero + offset + new Vector3(0,-55 * i, 0);
            newObject.transform.localScale = Vector3.one * 1.5f;

            newObject.SetUp_Damage(_fadeClass, random);

            //newObject.SetUp_Regular(item._value.ToString(), damageColor, item.isCrit);


        }



        
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
        if (durationHolder == null) return;
        durationHolder.SetActive(total > 0);
        durationFill.fillAmount = current / total;
    }

}


