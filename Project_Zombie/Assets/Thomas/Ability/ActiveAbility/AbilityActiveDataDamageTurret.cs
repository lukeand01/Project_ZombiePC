using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Ability / Active / Turret_Offensive")]
public class AbilityActiveDataDamageTurret : AbilityActiveData
{
    [SerializeField] Turret damageTurretTemplate;

    public override bool Call(AbilityClass ability)
    {
        base.Call(ability);

        Vector3 mousePosition = Input.mousePosition;

        Ray ray = Camera.main.ScreenPointToRay(mousePosition);

        LayerMask layer = 10;
        layer |= (1 << 9);
        layer |= (1 << 10);

        Transform playerTransform = PlayerHandler.instance.transform;
        // Perform the raycast
        if (Physics.Raycast(ray, out RaycastHit hit, 150, layer))
        {
            Debug.Log("yo " + hit.collider.name);
            float distance = Vector3.Distance(playerTransform.position, hit.point);
        
            if(hit.collider.gameObject.layer == 10 && distance < 15)
            {
                Turret newObject = Instantiate(damageTurretTemplate, hit.point, Quaternion.identity);
                newObject.SetUp();
                return true;
            }

        }



            return false;
    }


    public override string GetDamageDescription(AbilityClass ability)
    {
        return "Turret deals 15 damage base + 50% of player´s damage";
    }

}
