using MyBox;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EquipWindowUI : MonoBehaviour
{
    //we create the ability or gun everytime respecive is changed.

    GameObject holderMain;
    [SerializeField] GameObject holder;

    [SerializeField] Transform equipWindowPosRef_Open;
    [SerializeField] Transform equipWindowPosRef_Close;

    [Separator("Player Part")]
    [SerializeField] EquipWindowEquipUnit equipUnit_Gun_Player;
    [SerializeField] List<EquipWindowEquipUnit> equipUnit_Ability_List = new();


    public DescriptionWindow descriptionWindow {  get; private set; }


    private void Awake()
    {
        holderMain = transform.GetChild(0).gameObject;
        SetUIEquipWindow();
        currentOpenOptionIndex = -1;
    }



    private void Start()
    {
        descriptionWindow = UIHandler.instance._DescriptionWindow;
    }

    private void Update()
    {
        HandleDrag();
    }

    #region SET UP
    public void SetUIEquipWindow()
    {
        //transform.transform.position = equipWindowPosRef_Close.position;
        isOpen = false;
        holder.transform.DOMove(equipWindowPosRef_Close.position, 0);
        CreateOptions();

        //the player will call for these ui parts.

    }

    #endregion

    #region BASIC ORDERS

    public bool isOpen { get; private set; }    

    public void CallUI()
    {

        if (isOpen )
        {
            CloseUI();
        }
        else
        {
            OpenUI();
        }
    }
    void OpenUI()
    {

        isOpen = true;

        holder.transform.DOKill();
        holder.transform.DOMove(equipWindowPosRef_Open.position, 0.15f);

        holderMain.SetActive(true);
        //equipWindownContainerForButtons.Force();

    }
    void CloseUI()
    {

        isOpen = false;

        holder.transform.DOKill();
        holder.transform.DOMove(equipWindowPosRef_Close.position, 0.15f);

        //equipWindownContainerForButtons.gameObject.SetActive(false);

        ForceEndDrag();
        CloseOption();

        descriptionWindow.StopDescription();

    }

    public void ForceCloseUI()
    {
        if(isOpen )
        {
            CloseUI();
        }
    }


    #endregion

    #region PLAYER

    //

    public EquipWindowEquipUnit GetEquipForPermaGun(GunClass gun)
    {
        equipUnit_Gun_Player.SetGun(gun.data, this);
        //equipUnit_Gun_Player.SetGunClass(gun);
        gun.SetGunEquip(equipUnit_Gun_Player);
        return equipUnit_Gun_Player;
    }

    public EquipWindowEquipUnit GetEquipForAbility(AbilityClass ability)
    {
        //i need the index for the thing which will not change

        EquipWindowEquipUnit equipUnit = equipUnit_Ability_List[ability.slotIndex];

        equipUnit.SetAbility(ability.dataActive, this);
        equipUnit.SetAbilitySlotIndex(ability.slotIndex);
        
        return equipUnit;
    }

    #endregion

    #region OPTION
    [Separator("OPTIONS")]
    [SerializeField] EquipWindowOptionButton equipWindowOptionButtonTemplate;
    [SerializeField] EquipWindowContainer equipWindownContainerTemplate;
    [SerializeField] Transform equipWindownContainerForContainer;
    [SerializeField] MyGrid_New equipWindownContainerForButtons;
    [SerializeField] Transform equipWindowOptionPosRef_Open;

    List<EquipWindowContainer> optionContainerList = new();
    int currentOpenOptionIndex;

    public void OpenOption(EquipWindowType equipType)
    {
        //we order a certain holderMain to move.;

        if((int)equipType == currentOpenOptionIndex)
        {
            CloseOption();
            currentOpenOptionIndex = -1;
            return;
        }

        if(currentOpenOptionIndex != -1)
        {
            CloseOption();
        }


        currentOpenOptionIndex = (int)equipType;
        optionContainerList[currentOpenOptionIndex].transform.DOLocalMove(equipWindowOptionPosRef_Open.localPosition, 0.2f);
    }

   
    void CloseOption()
    {
        if (currentOpenOptionIndex == -1) return;
        optionContainerList[currentOpenOptionIndex].transform.DOLocalMove(equipWindownContainerTemplate.transform.localPosition, 0.2f);
        currentOpenOptionIndex = -1;
    }

    void CreateOptions()
    {
        CreateOptionButton(EquipWindowType.Gun);
        CreateOptionButton(EquipWindowType.Ability);




    }

    void CreateOptionButton(EquipWindowType equipType)
    {
        EquipWindowOptionButton newObject = Instantiate(equipWindowOptionButtonTemplate);
        newObject.transform.SetParent(equipWindownContainerForButtons.transform);
        newObject.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 90));
        newObject.SetUp(equipType, this);

        //and we create a container as well.
        EquipWindowContainer newContainer = Instantiate(equipWindownContainerTemplate, equipWindownContainerTemplate.transform.position, Quaternion.identity);
        newContainer.transform.SetParent(equipWindownContainerForContainer);
        optionContainerList.Add(newContainer);
        newContainer.SetUp(equipType.ToString(), this);
        newContainer.transform.DOScale(Vector3.one, 0);
    }


    public void UpdateOptionForGunContainer(List<ItemGunData> gunDataList)
    {
        optionContainerList[(int)EquipWindowType.Gun].UpdateContainerGun(gunDataList);   
    }
    public void UpdateOptionForAbilityContainer(List<AbilityActiveData> abilityList)
    {
        optionContainerList[(int)EquipWindowType.Ability].UpdateContainerAbility(abilityList);
    }


    #endregion


    #region DRAG SYSTEM

    [Separator("DRAGGING SYSTEM")]
    [SerializeField] EquipWindowEquipUnit fakeUnitForDragging; //we show all the info here. merely visual
    EquipWindowEquipUnit currentUnitHover;
    EquipWindowEquipUnit currentDraggingUnit;

    public bool isDragging { get; private set; } = false;

    void HandleDrag()
    {

        if (Input.GetMouseButtonDown(0))
        {
            if (!fakeUnitForDragging.gameObject.activeInHierarchy && currentUnitHover != null)
            {
                StartDragging();
                return;
            }


        }

        if (currentDraggingUnit == null) return;
       
        fakeUnitForDragging.transform.position = Input.mousePosition;       

        if(fakeUnitForDragging.transform.position == Input.mousePosition)
        {
            fakeUnitForDragging.gameObject.SetActive(true);
        }

        if (Input.GetMouseButtonUp(0))
        {
            EndDrag();
        }


    }


    public void StartDragging()
    {

        //while we are holding the mousebutton. if we let go then we try to give somewhere.


        if (currentUnitHover.cannotDrag) return;

        fakeUnitForDragging.gameObject.SetActive(true);
        isDragging = true;

        descriptionWindow.StopDescription();

        fakeUnitForDragging.UseRef(currentUnitHover);
        currentDraggingUnit = currentUnitHover;

    }

    public void EndDrag()
    {
        //we check 

        if (currentUnitHover == null)
        {
            
            if (currentDraggingUnit.abilityData != null)
            {
                //then we will use this remove it.
                currentDraggingUnit.RemoveAbilityFromPlayer();
            }
        }

        if (currentUnitHover != null)
        {
            //we need to know if we can change.
            HandleSwap();
        }

        currentDraggingUnit = null;
        fakeUnitForDragging.gameObject.SetActive(false);
        isDragging = false;
        //then we check if we can change them

    }

    void HandleSwap()
    {

        
        if(currentDraggingUnit == null)
        {
            return;
        }

        //we should be able to swap information between two abilities

        currentUnitHover.ReceiveInfo(currentDraggingUnit);

    }


    void ForceEndDrag()
    {
        fakeUnitForDragging.gameObject.SetActive(false);
        currentUnitHover = null;
        isDragging = false;
    }

    public void StartHover(EquipWindowEquipUnit newItem)
    {

        currentUnitHover = newItem;
    }
    public void EndHover()
    {

        currentUnitHover = null;
    }



    #endregion

    private void OnDisable()
    {
        fakeUnitForDragging.gameObject.SetActive(false);
        ForceEndDrag();
    }


}

//i need to hold somewhere the list of item the player has.


public enum EquipWindowType
{
    Gun = 0,
    Ability = 1
}