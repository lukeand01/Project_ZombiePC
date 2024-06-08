using MyBox;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MouseUI : MonoBehaviour
{
    //i will hold the ref here.
    //things that are interactle can have unique sprites for the 
    //we will hold ref for the basic ones here. all white and they will be changed with color.

    //the mouse ui will slowly move when hovering an enem,y
    //and scale up. a bit



    //it should a different ui when inside the city.
    //

    //either 


    //we need to keep checking with
    [SerializeField] Image mouse_Game;

    [Separator("REF TO THE BASIC SPRITES")]
    [SerializeField] Sprite mouseSprite_DefaultAim; //this is the aim
    [SerializeField] Sprite mouseSprite_HoverEnemy;
    [SerializeField] Sprite mouseSprite_Interact;
    Camera cam;

    [Separator("COLOR")]
    [SerializeField] Color color_Enemy;
    [SerializeField] Color color_Interactable;

    float targetScale;

    bool shouldNotUseMouseUI = false;

    LayerMask layer_Enemy;
    LayerMask layer_Interactable;

    MouseStatType state;

    enum MouseStatType
    {
        Free,
        Enemy,
        Interactable,
        Menu
    }

    private void Awake()
    {
        cam = Camera.main;
        targetScale = mouse_Game.transform.localScale.x;

        layer_Enemy |= (1 << 6);
        layer_Interactable |= (1 << 7);

    }

    public void ControlAppear(bool shouldAppear)
    {
        shouldNotUseMouseUI = !shouldAppear;
    }

    private void Update()
    {
        //it keeps following
        //mouse_Game.transform.position = cam.ScreenToWorldPoint(Input.mousePosition);

      
        if(Time.timeScale == 0 || shouldNotUseMouseUI)
        {
            //if thats teh case then we show the cursos.
            Cursor.visible = true;
            mouse_Game.gameObject.SetActive(false);
            return;
        }
        else
        {
            Cursor.visible = false;
            mouse_Game.gameObject.SetActive(true);
        }

        mouse_Game.transform.position = Input.mousePosition;

        //we need to get information frmo where it is in the canvas.

        switch (state)
        {
            case MouseStatType.Free:
                targetScale = 0.7f;
                mouse_Game.transform.Rotate(0, 0, 30 * Time.deltaTime);
                mouse_Game.sprite = mouseSprite_DefaultAim;
                mouse_Game.color = Color.white;
                break;
            case MouseStatType.Enemy:
           
                mouse_Game.transform.Rotate(0, 0, -100 * Time.deltaTime);
                targetScale = 1.5f;
                mouse_Game.color = color_Enemy;
                mouse_Game.sprite = mouseSprite_HoverEnemy;
                break;
            case MouseStatType.Interactable:

                mouse_Game.transform.Rotate(0, 0, -100 * Time.deltaTime);
                targetScale = 1.5f;
                mouse_Game.color = color_Interactable;
                mouse_Game.sprite = mouseSprite_Interact;
                break;
        }




        CheckScale();

        if (CheckForEnemy())
        {
            state = MouseStatType.Enemy;
            return;
        }

        if(CheckForInteractable())
        {

            state = MouseStatType.Interactable;
            return;
        }


        state = MouseStatType.Free;
    }

    public void ControlVisibility(bool isVisible)
    {
        //there is a different ui for stage and city.
    }


    void CheckScale()
    {
        float scale_Current = mouse_Game.transform.localScale.x;

        bool shouldScale = Mathf.Abs(scale_Current - targetScale) > 1;
        if (!shouldScale)
        {

            return;
        }

        if(scale_Current > targetScale)
        {
            mouse_Game.transform.localScale -= new Vector3(1, 1, 1) * Time.deltaTime * 100;
        }
        if(scale_Current < targetScale)
        {
            mouse_Game.transform.localScale += new Vector3(1, 1, 1) * Time.deltaTime * 100;
        }
    }

    bool CheckForEnemy()
    {
        Vector3 mousePosition = Input.mousePosition;

        // Convert the mouse position to a ray
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);

        RaycastHit hit;

        return Physics.Raycast(ray, out hit, 999, layer_Enemy);
       
    }
    bool CheckForInteractable()
    {
        Vector3 mousePosition = Input.mousePosition;

        // Convert the mouse position to a ray
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);

        RaycastHit hit;

        return Physics.Raycast(ray, out hit, 999, layer_Interactable);

    }

}
