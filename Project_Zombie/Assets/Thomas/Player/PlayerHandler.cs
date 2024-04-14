using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHandler : MonoBehaviour
{
    public static PlayerHandler instance {  get; private set; }
    public PlayerMovement _playerMovement {  get; private set; }
    public PlayerController _playerController { get; private set; }
    public PlayerCombat _playerCombat { get; private set; }

    public PlayerInventory _playerInventory { get; private set; }

    public Rigidbody _rb {  get; private set; }
    
    public EntityEvents _entityEvents {  get; private set; }
    public EntityStat _entityStat { get; private set; }


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


        _playerController = GetComponent<PlayerController>();
        _playerMovement = GetComponent<PlayerMovement>();
        _playerInventory = GetComponent<PlayerInventory>();
        _playerCombat = GetComponent<PlayerCombat>();

        _entityEvents = GetComponent<EntityEvents>();
        _entityStat = GetComponent<EntityStat>();

        _rb = GetComponent<Rigidbody>();


        

    }

    private void Start()
    {
        _playerMovement.SetSpeed(_entityStat.GetTotalValue(StatType.Speed));
    }


}
