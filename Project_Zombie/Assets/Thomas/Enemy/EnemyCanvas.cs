using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCanvas : MonoBehaviour
{
    // Start is called before the first frame update

    Camera mainCam;
    [SerializeField] FadeUI fadeTemplate;
    [SerializeField] Transform damageContainer;

    private void Awake()
    {
        mainCam = Camera.main;
    }

    private void Update()
    {
        transform.LookAt(mainCam.transform.position);
    }


    public void CreateDamagePopUp(float damage, DamageType damageType, bool isCrit)
    {
        FadeUI newObject = Instantiate(fadeTemplate);
        newObject.transform.SetParent(damageContainer);
        // newObject.transform.rotation = Quaternion.Euler(0, -180, 0);
        float modifier = 15f;
        float x = Random.Range(-modifier, modifier);
        float y = Random.Range(-modifier, modifier);

        Vector3 offset = new Vector3(x, y, 0);


        newObject.transform.localPosition = Vector3.zero + offset;

        string additional = "";

        if (isCrit)
        {
            additional = "*";
        }


        newObject.SetUp(damage.ToString() + additional, Color.red);

    }
}
