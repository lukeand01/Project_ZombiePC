using DG.Tweening;
using MyBox;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MouseUI : MonoBehaviour
{
    //i want to improve this fella by a lot
    //the aim needs to be more precise, it needs to show the area of error.

    //still need to rotate the damage thing.


    //we need to keep checking with

    GameObject holder;

    Camera _cam;

    [Separator("COLOR")]
    [SerializeField] Color color_Enemy;
    [SerializeField] Color color_Interactable;

    float targetScale;

    bool shouldNotUseMouseUI = false;

    LayerMask layer_Enemy;
    LayerMask layer_Interactable;

    MouseStatType state;
    [Separator("WEAPON MOUSE UI")]
    [SerializeField] List<MouseUIClass> _mouseUIClassList = new();
    int mouseUICurrentIndex;
    MouseUIClass GetCurrentMouse { get { return _mouseUIClassList[mouseUICurrentIndex]; } }


    [Separator("UI MOUSE")]
    [SerializeField] bool _isMouseUI;
    [SerializeField] Image _mouseUI;
    


    enum MouseStatType
    {
        Free,
        Enemy,
        Interactable,
        Menu
    }

    private void Awake()
    {
        _cam = PlayerHandler.instance._cam;

        holder = transform.GetChild(0).gameObject;

        layer_Enemy |= (1 << 6);
        layer_Interactable |= (1 << 7);

    }

    public void UpdateMouseUI(MouseUIType _type)
    {
        for (int i = 0; i < _mouseUIClassList.Count; i++)
        {
            var item = _mouseUIClassList[i];

            item.Reset();
        }
        mouseUICurrentIndex = (int)_type;
        GetCurrentMouse.mouseUIHolder.SetActive(true);
        targetScale = GetCurrentMouse.mouseUIHolder.transform.localScale.x;
    }

    public void Shoot()
    {
        if(!isReloading) GetCurrentMouse.Shoot();
    }

    public void ControlAppear(bool shouldAppear)
    {
        shouldNotUseMouseUI = !shouldAppear;
    }

    float mousePosZCorrecttor;
    bool isReloading;


    private void Update()
    {
        if (_isMouseUI)
        {
            MoveWithUIMouse();

            return;
        }

        isReloading = PlayerHandler.instance._playerCombat.isReloading;
       
        if(Time.timeScale == 0 || shouldNotUseMouseUI)
        {
            //if thats teh case then we show the cursos.
            Cursor.visible = true;
            GetCurrentMouse.mouseUIHolder.SetActive(false);
            return;
        }
        else
        {
            Cursor.visible = false;
            GetCurrentMouse.mouseUIHolder.SetActive(true);
        }

        MoveWithMouseInput();
        CheckScale();
        if (isReloading)
        {
            KeepRotatingWhileReloading();
            targetScale = 0.7f;
            //mouse_Game.transform.Rotate(0, 0, 30 * Time.deltaTime);
            _mouseUIClassList[mouseUICurrentIndex].ChangeColor(Color.white);
            return;
        }
        else
        {
            RotateToAlwaysFacePlayer();

        }
        //we need to get information frmo where it is in the canvas.

        switch (state)
        {
            case MouseStatType.Free:
                targetScale = 0.7f;
                GetCurrentMouse.mouseUIHolder.transform.Rotate(0, 0, 30 * Time.deltaTime);
                _mouseUIClassList[mouseUICurrentIndex].ChangeColor(Color.white);
                break;
            case MouseStatType.Enemy:
           
                targetScale = 1.5f;
                _mouseUIClassList[mouseUICurrentIndex].ChangeColor(color_Enemy);
                break;
            case MouseStatType.Interactable:

                targetScale = 1.5f;
                _mouseUIClassList[mouseUICurrentIndex].ChangeColor(color_Interactable);
                break;
        }




 
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


    public void ControlMouseUI(bool isMouseUI)
    {
        //there is a different ui for stage and city.

        _isMouseUI = isMouseUI;
        GetCurrentMouse.mouseUIHolder.SetActive(!isMouseUI);
        _mouseUI.gameObject.SetActive(isMouseUI);
    }
    public void ControlMouseHolderVisibility(bool isVisible)
    {
        holder.SetActive(isVisible);
    }
    void RotateToAlwaysFacePlayer()
    {

        mousePosZCorrecttor = 9;

        Vector3 mousePosition = _cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 50));
        mousePosition.z += mousePosZCorrecttor;

        Vector3 direction = new Vector3(
        mousePosition.x - PlayerHandler.instance.transform.position.x,
        0,
        mousePosition.z - PlayerHandler.instance.transform.position.z
        );



        Vector3 screenPos = _cam.WorldToScreenPoint(direction);

        var angle = Mathf.Atan2(direction.z, direction.x) * Mathf.Rad2Deg;
        angle += 90;
        GetCurrentMouse.mouseUIHolder.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        //i want to perphaps rotate it a bit

    }

    void MoveWithMouseInput()
    {
        GetCurrentMouse.mouseUIHolder.transform.position = Input.mousePosition + new Vector3(0, 25, 0);
        
    }
    void MoveWithUIMouse()
    {
        _mouseUI.transform.position = Input.mousePosition + new Vector3(0,-10,0);

     
    }

    void KeepRotatingWhileReloading()
    {
        GetCurrentMouse.mouseUIHolder.transform.Rotate(new Vector3(0, 0, 1));
    }



    void CheckScale()
    {
        float scale_Current = GetCurrentMouse.mouseUIHolder.transform.localScale.x;

        bool shouldScale = Mathf.Abs(scale_Current - targetScale) > 1;
        if (!shouldScale)
        {

            return;
        }

        if(scale_Current > targetScale)
        {
            _mouseUIClassList[mouseUICurrentIndex].ChangeSize(- new Vector3(1, 1, 1) * Time.deltaTime * 100);
        }
        if(scale_Current < targetScale)
        {
            _mouseUIClassList[mouseUICurrentIndex].ChangeSize(new Vector3(1, 1, 1) * Time.deltaTime * 100); 
        }
    }

    bool CheckForEnemy()
    {
        Vector3 mousePosition = Input.mousePosition;

        // Convert the mouse position to a ray
        Ray ray = _cam.ScreenPointToRay(mousePosition);

        RaycastHit hit;

        return Physics.Raycast(ray, out hit, 999, layer_Enemy);
       
    }
    bool CheckForInteractable()
    {
        Vector3 mousePosition = Input.mousePosition;

        // Convert the mouse position to a ray
        Ray ray = _cam.ScreenPointToRay(mousePosition);

        RaycastHit hit;

        return Physics.Raycast(ray, out hit, 999, layer_Interactable);

    }

}

public enum MouseUIType
{
    Simple = 0,
    Shotgun = 1,
    Laser = 2,
    Sniper = 3,
}

[System.Serializable]
public class MouseUIClass
{
    [field:SerializeField] public GameObject mouseUIHolder { get; private set; }

    [SerializeField] Image[] piecesArray;
    [SerializeField] MouseUIType mouseUIType;

    float originalSpreadSize; //we update this everytime we change the gun_Perma
    Vector3 originalPosition { get { return new Vector3(originalSpreadSize, 0, 0); } }
    public void SetUp(float originalSpreadSize)
    {
        this.originalSpreadSize = originalSpreadSize;   
    }

    public void Reset()
    {

    }

    public void Shoot()
    {
        originalSpreadSize = 50;

        float timer = 0.05f;
        piecesArray[0].transform.DOKill();
        piecesArray[1].transform.DOKill();

        piecesArray[0].transform.DOLocalMove(-originalPosition - new Vector3(20, 0, 0), timer).SetEase(Ease.Linear).OnComplete(() => ReturnToNormal());       
        piecesArray[1].transform.DOLocalMove(originalPosition + new Vector3(20, 0, 0), timer).SetEase(Ease.Linear);
    }

    void ReturnToNormal()
    {


        float timer = 0.05f;
        piecesArray[0].transform.DOKill();
        piecesArray[1].transform.DOKill();
        piecesArray[0].transform.DOLocalMove(-originalPosition, timer).SetEase(Ease.Linear);

        
        piecesArray[1].transform.DOLocalMove(originalPosition, timer).SetEase(Ease.Linear);
    }


    public void ChangeColor(Color color)
    {
        foreach (var item in piecesArray)
        {
            item.color = color;
        }
    }
    public void ChangeSize(Vector3 vectorValue)
    {
        foreach (var item in piecesArray)
        {
            item.transform.localScale += vectorValue;
        }
    }


}