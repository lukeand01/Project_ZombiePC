using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Diagnostics;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    PlayerHandler handler;

    [SerializeField] GameObject graphic;



    private void Awake()
    {
        handler = GetComponent<PlayerHandler>();
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        HandleActualRotation();
    }

    public void SetSpeed(float newValue)
    {
        speed = newValue;
    }

    #region MOVEMENT
    [Separator("MOVEMENT")]
    float speed;  //for now we will be using this. but later it will e the stathandler.
    public void MovePlayer(Vector3 dirVector)
    {
        float moveModifier = 1;

        Vector3 movement = new Vector3(dirVector.x, 0, dirVector.y) * speed * moveModifier;

        handler._rb.velocity = movement;
    }

    #endregion

    #region ROTATION

    [Separator("ROTATION")]
    [SerializeField] Vector3 rotationVector;

    void HandleActualRotation()
    {
        //why do i care about rotationX
        /* THIS IS FOR ORTOGRPAHIC CAMERA
        
        Vector3 lookDirection = new Vector3(rotationDirX, 0, rotationDirZ);
        Quaternion targetRotation = Quaternion.LookRotation(lookDirection, Vector3.up);
        graphic.transform.rotation = Quaternion.RotateTowards(graphic.transform.rotation, targetRotation, Time.deltaTime * 1000);

        */

        Quaternion targetRotation = Quaternion.LookRotation(rotationVector, Vector3.up);
        graphic.transform.rotation = Quaternion.RotateTowards(graphic.transform.rotation, targetRotation, Time.deltaTime * 700);


    }

    public void RotatePlayer(Vector3 dir)
    {
        rotationVector = dir;


        //rotationDirX = dir.y;
        //rotationDirZ = -dir.x;

        //Quaternion targetRotation = Quaternion.LookRotation(dir);
        //graphic.transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 1000 * Time.deltaTime);
    }

    #endregion



}
