using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityStatCanvas : MonoBehaviour
{
    Camera mainCam;

    [SerializeField] GameObject stunnedImage;

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


        if (stunnedImage.activeInHierarchy)
        {
            stunnedImage.transform.Rotate(new Vector3(0, 0, 0.8f));
        }
    }

    public void ControlStunned(bool isVisible)
    {
        stunnedImage.SetActive(isVisible);
    }


    [SerializeField] BDUnit bdUnitTemplate; //get it from here and give it to the bd
    [SerializeField] Transform bdContainer;

    public BDUnit CreateBDUnit(BDClass bd)
    {
        if(bdUnitTemplate == null || bdContainer == null) 
        {
            return null;
        }


        //need to put this into container;
        BDUnit newObject = Instantiate(bdUnitTemplate);
        newObject.transform.SetParent(bdContainer);
        newObject.SetUp(bd);
        return newObject;
    }


    #region DODGE
    [Separator("DODGE")]
    [SerializeField] FadeUI fadeTemplate;
    [SerializeField] Transform dodgePos;
    public void CreateFadeUIForDodge()
    {
        FadeUI newObject = Instantiate(fadeTemplate);
        newObject.transform.SetParent(dodgePos);

        

        float amount = 20f;
        float x = Random.Range(-amount * 3, amount * 3);
        float z = Random.Range(-amount, amount);

        newObject.transform.localPosition = Vector3.zero + new Vector3(0, 35, 0) + new Vector3(x, z, 0);
        newObject.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
        newObject.SetUp("Dodge!", Color.black);
    }

    #endregion

    #region RECOVER HEALTH
    [Separator("RECOVER HEALTH")]
    [SerializeField] Transform recoverHealthPos;

    public void CreateFadeUIForRecoverHealth(float value)
    {
        FadeUI newObject = Instantiate(fadeTemplate);
        newObject.transform.SetParent(recoverHealthPos);


        float amount = 20f;
        float x = Random.Range(-amount * 3, amount * 3);
        float z = Random.Range(-amount, amount);

        newObject.transform.localPosition = Vector3.zero + new Vector3(0, 35, 0) + new Vector3(x, z, 0);
        newObject.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
        newObject.SetUp(value.ToString(), Color.green);
    }

    #endregion

    #region POWER
    public void CreateFadeUIForPower(string powerName, Color fadeColor, float duration = 1)
    {
        FadeUI newObject = Instantiate(fadeTemplate);
        newObject.transform.SetParent(recoverHealthPos);

        float amount = 20;
        float x = Random.Range(-amount * 3, amount * 3);
        float z = Random.Range(-amount, amount);

        newObject.transform.localPosition = Vector3.zero + new Vector3(0, 35, 0) + new Vector3(x, z, 0);
        newObject.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
        newObject.SetUp(powerName.ToString(), fadeColor);
        newObject.ChangeColorModifier(duration);
        newObject.ChangeScaleModifier(1.5f);
    }


    public void CreateFadeUIForDrop(string dropName)
    {
        FadeUI newObject = Instantiate(fadeTemplate);
        newObject.transform.SetParent(recoverHealthPos);

        newObject.ChangeScaleModifier(2);

        float amount = 20;
        float x = Random.Range(-amount * 3, amount * 3);
        float z = Random.Range(-amount, amount);

        newObject.transform.localPosition = Vector3.zero + new Vector3(0, 35, 0) + new Vector3(x, z, 0);
        newObject.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
        newObject.SetUp(dropName, Color.yellow);
    }

    #endregion


    #region NEW FADE UI


    //

    #endregion
}
