using MyBox;
using UnityEngine;

public abstract class GunUpgradeData : ScriptableObject
{
    [Separator("BASE UPGRADE")]
    public string upgradeName;
    [SerializeField] Sprite upgradeIcon;
    [TextArea] public string upgradeDescription;

    public bool upgradeCanStack;

    public abstract void AddUpgrade(GunClass _gunClass);
    
    public abstract void RemoveUpgrade(GunClass _gunClass);
    

}
