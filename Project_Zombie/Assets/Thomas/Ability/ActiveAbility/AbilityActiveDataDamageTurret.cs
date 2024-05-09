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

        //we needd to ask the mouse if we can place it in the place.
        //onoyl in the ground. in the place it is created it will push enemies away.
        //also cannot be placed in top of other enemies.
        //it has a healthbar and a durationbar.

        Vector3 mousePosition = Input.mousePosition;

        Ray ray = Camera.main.ScreenPointToRay(mousePosition);

        LayerMask layer = 10;
        layer |= (1 << 9);
        //we only check 


        // Perform the raycast
        if (Physics.Raycast(ray, out RaycastHit hit, 150, layer))
        {
            if(hit.collider != null)
            {
                //if we have found ground we will spawn it. however
                if(hit.collider.gameObject.layer == 10)
                {
                    //then we are going to spawn it. if its too much by the side then it should reange itself

                    Turret newObject = Instantiate(damageTurretTemplate);


                }


            }


        }



            return false;
    }


    public override string GetDamageDescription(int level)
    {
        return "Turret deals 15 damage base + 50% of player´s damage";
    }

}
