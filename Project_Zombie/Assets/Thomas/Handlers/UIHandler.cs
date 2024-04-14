using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHandler : MonoBehaviour
{
    public static UIHandler instance;

    [Separator("REFERENCES")]
    [SerializeField] PlayerUI playerUIRef;
    [SerializeField] GunUI gunUIRef;

    #region GETTERS 
    public PlayerUI _playerUI { get {  return playerUIRef; } }

    public GunUI gunUI { get { return gunUIRef; } }
    #endregion

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
