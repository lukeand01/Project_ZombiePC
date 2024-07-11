using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(menuName = "Ability / Active / Fireball")]
public class AbilityActiveDataFireball : AbilityActiveData
{
    //it flies to where mouse it 


    [SerializeField] FireballProjectil fireBallTemplate;
    [Range(0.01f, 1)][SerializeField] float damageModifier = 1;

    //instead i want to charge this ability and to zoomb the camerea a bit and to not move while holding
    //

    //we just ask if its chargeable.
    //then in that case

    public override bool Call(AbilityClass ability)
    {
        //get the mouise position and shoot a projectile.
        //the projectile explode in contact with wall or enemy.
        //it deals damage to the target and then a lower amount to all around. apply burning to all.

        Vector3 mousePos = getMouseDirection();

        FireballProjectil newObject = Instantiate(fireBallTemplate, PlayerHandler.instance.transform.position, Quaternion.identity);
        float damage = PlayerHandler.instance._entityStat.GetTotalValue(StatType.Damage);

        newObject.SetUp(mousePos.normalized, damage * damageModifier);


        return true;
    }


    public override void StartCharge(AbilityClass ability)
    {
        base.StartCharge(ability);
        Debug.Log("start charge");

        PlayerHandler.instance._playerController.block.AddBlock("Fireball", BlockClass.BlockType.OnlyCharge);
        PlayerHandler.instance._playerCamera.ControlFieldOfView(45);
        
        

    }
    public override void StopCharge(AbilityClass ability)
    {
        base.StopCharge(ability);
        Debug.Log("stop charge");

        PlayerHandler.instance._playerController.block.RemoveBlock("Fireball");
        PlayerHandler.instance._playerCamera.ReturnFieldOfViewToOriginal();
    }

    Vector3 getMouseDirection()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            // Calculate direction to rotate character
            Vector3 targetDirection = (hit.point - PlayerHandler.instance.transform.position).normalized;
            targetDirection.y = 0f; // Ignore vertical component (if needed)

            // Rotate character towards mouse position


            return targetDirection;
        }


        return Vector3.zero;
    }

}
