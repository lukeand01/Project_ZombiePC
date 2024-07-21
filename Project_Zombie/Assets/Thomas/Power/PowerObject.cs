using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerObject : MonoBehaviour
{
    [SerializeField] PowerData powerData;
    [SerializeField] GameObject objectToRotate;
    [SerializeField] float rotationSpeed;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player") return;


        powerData.ActivatePower();
        PlayerHandler.instance._entityStat.CallPowerFadeUI(powerData.powerName, Color.green);
        Destroy(gameObject);

    }


    private void Update()
    {
        objectToRotate.transform.Rotate(Time.deltaTime * rotationSpeed, 0, Time.deltaTime * rotationSpeed);
    }

}
