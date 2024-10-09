using MyBox;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EquipWindowUI : MonoBehaviour
{
    //we create the ability or gun_Perma everytime respecive is changed.

    GameObject holderMain;
    [SerializeField] GameObject holder;

    [SerializeField] Transform equipWindowPosRef_Open;
    [SerializeField] Transform equipWindowPosRef_Close;
    [SerializeField] PlayerEquipmentData _playerEquipmentData;

    [Separator("Player Part")]
    [SerializeField] EquipWindowEquipUnit equipUnit_Gun_Player;
    [SerializeField] List<EquipWindowEquipUnit> _equipUnit_Ability_List = new();
    [SerializeField] List<EquipWindowEquipUnit> _equipUnit_Drop_List = new(); 


    //i also need to update this fella.

    public DescriptionWindow descriptionWindow {  get; private set; }


    //

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

    public void UpdateAbilitySlot(int abilitySlot)
    {
        //if we are checking 

        for (int i = 0; i < _equipUnit_Ability_List.Count; i++)
        {
            var item = _equipUnit_Ability_List[i];



            item.gameObject.SetActive(abilitySlot > i);



        }

    }
    public void UpdateDropSlot(int dropSlot)
    {

        for (int i = 0; i < _equipUnit_Drop_List.Count; i++)
        {
            var item = _equipUnit_Drop_List[i];

            item.gameObject.SetActive(dropSlot > i);


        }
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

    //so we do this. when we create a new one we come here 

    public EquipWindowEquipUnit GetEquipForPermaGun(GunClass gun)
    {
        equipUnit_Gun_Player.SetGun(gun.data, this);
        //equipUnit_Gun_Player.SetGunClass(gun_Perma);
        gun.SetGunEquip(equipUnit_Gun_Player);
        return equipUnit_Gun_Player;
    }

    public EquipWindowEquipUnit GetEquipForAbility(AbilityClass ability)
    {
        //i need the currentBulletIndex for the thing which will not change
        EquipWindowEquipUnit equipUnit = _equipUnit_Ability_List[ability.slotIndex];

       
        equipUnit.SetAbility(ability.dataActive, this);
        equipUnit.SetAbilitySlotIndex(ability.slotIndex);
        
        return equipUnit;
    }

    public EquipWindowEquipUnit GetEquipForDrop(DropData dropData, int slotIndex)
    {
        //i need the currentBulletIndex for the thing which will not change

        EquipWindowEquipUnit equipUnit = _equipUnit_Drop_List[slotIndex];

        equipUnit.SetDrop(dropData, this);

        return equipUnit;
    }



    #endregion

    #region OPTION AND CONTAINERS
    [Separator("OPTIONS")]
    [SerializeField] EquipWindowOptionButton equipWindowOptionButtonTemplate;
    [SerializeField] EquipWindowContainer equipWindownContainerTemplate;
    [SerializeField] EquipWindowContainer equipWindownContainerTemplate_Quest; //we use this if the option is for quest because i need a different grid set up.
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
        CreateOptionButton(EquipWindowType.Trinket);
        CreateOptionButton(EquipWindowType.Drop);
        CreateOptionButton(EquipWindowType.Quest);

    }

    void CreateOptionButton(EquipWindowType equipType)
    {



        EquipWindowOptionButton newObject = Instantiate(equipWindowOptionButtonTemplate);

       


        newObject.transform.SetParent(equipWindownContainerForButtons.transform);
        newObject.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 90));
        newObject.SetUp(equipType, this);

        //and we create a container as well.
        EquipWindowContainer newContainer = null;

        if (equipType == EquipWindowType.Quest)
        {
            newContainer = Instantiate(equipWindownContainerTemplate_Quest, equipWindownContainerTemplate.transform.position, Quaternion.identity);
        }
        else
        {
            newContainer = Instantiate(equipWindownContainerTemplate, equipWindownContainerTemplate.transform.position, Quaternion.identity);
        }
        

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
    public void UpdateOptionForStoryQuest(List<QuestClass> questList)
    {
        optionContainerList[(int)EquipWindowType.Quest].UpdateContainerQuest(questList);
    }
    public void UpdateOptionForTrinkets()
    {
        //optionContainerList[(int)EquipWindowType.Ability].UpdateContainerAbility(abilityList);
    }
    public void UpdateOptionForDrops(List<DropData> dropList)
    {
        optionContainerList[(int)EquipWindowType.Drop].UpdateContainerDrop(dropList);
    }

    //i need the thing to be different

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
            
            if (currentDraggingUnit._abilityData != null)
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
        CaptureState();
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


    public void CreateListForDrop()
    {
        List<DropData> dropList = new();

        for (int i = 0; i < _equipUnit_Drop_List.Count; i++)
        {
            var item = _equipUnit_Drop_List[i];
            var data = item._dropData;

            if (data == null) continue;

            if (dropList.Contains(data))
            {
                //then we need to remove this thing because we cant have duplicates.
                item.SetDrop(null, this);
            }
            else
            {
                dropList.Add(data);
            }


        }


        GameHandler.instance.cityDataHandler.cityDropLauncher.SetEquippedDropList(dropList);
        GameHandler.instance.cityDataHandler.cityDropLauncher.GenerateListForEquipContainer();

    }


    void CaptureState()
    {
        //we get every fella and store that in the saveclass.

        int gunIndex = -1;

        if(equipUnit_Gun_Player._gunData != null)
        {
            gunIndex = equipUnit_Gun_Player._gunData.storeIndex;
        }
        
        _playerEquipmentData.MakeGunStored(gunIndex);


        List<int> abilityList = new();

        for (int i = 0; i < _equipUnit_Ability_List.Count; i++)
        {
            var item = _equipUnit_Ability_List[i];

            int abilityIndex = -1;

            if(item._abilityData != null)
            {
                abilityIndex = item.abilityIndex;
            }

            abilityList.Add(abilityIndex);

        }

        _playerEquipmentData.MakeAbilitiesStoredList(abilityList);


        List<int> dropList = new();
        for (int i = 0; i < _equipUnit_Drop_List.Count; i++)
        {
            var item = _equipUnit_Drop_List[i];

            int dropIndex = -1;

            if (item._dropData != null)
            {
                dropIndex = item._dropData.storeIndex;
            }

            dropList.Add(dropIndex);

        }

        _playerEquipmentData.MakeDropStoredList(dropList);

        //everytime we capture the state we have to inform to save this information.
        GameHandler.instance._saveHandler.CaptureStateUsingCurrentSaveSlot();
    }
    public void RestoreState()
    {

        int gunIndex = _playerEquipmentData._gunStored;



        ItemGunData gunData = GameHandler.instance.cityDataHandler.cityArmory.GetGunWithIndex(gunIndex);

        if(gunData != null)
        {
            PlayerHandler.instance._playerCombat.ReceivePermaGun(gunData);
            ItemGunData newPermaGun = PlayerHandler.instance._playerCombat.GetCurrentPermaGun();
            equipUnit_Gun_Player.SetGun(newPermaGun, this);
        }
        

        List<int> abilityList = _playerEquipmentData._abilitiesStoredList;

        if(abilityList.Count > 0)
        {
            for (int i = 0; i < abilityList.Count; i++)
            {
                var value = abilityList[i];
                var item = _equipUnit_Ability_List[i];

                if (value == -1) continue;

                AbilityActiveData abilityData = GameHandler.instance.cityDataHandler.cityLab.GetActiveAbilityWithIndex(value);

                if (abilityData == null)
                {
                    Debug.Log("this ability data was null");
                }

                PlayerHandler.instance._playerAbility.ReplaceActiveAbility(abilityData, i);
                item.SetAbility(abilityData, this);
                //draggingUnit.RemoveAbilityFromPlayer();

            }
        }

        List<int> dropList = _playerEquipmentData._dropStoredList;
        Debug.Log("droplist is the value stored " + dropList.Count);
        if (dropList.Count > 0)
        {
            for (int i = 0; i < dropList.Count; i++)
            {
                var value = dropList[i];
                var item = _equipUnit_Drop_List[i];

                if (value == -1) continue;

                DropData dropData = GameHandler.instance.cityDataHandler.cityDropLauncher.GetDropDataFromIndex(value);
                item.SetDrop(dropData, this);
                CreateListForDrop();
                GameHandler.instance._soundHandler.CreateSfx(SoundType.AudioClip_Equip_Drop);

            }
        }

    }

    private void OnDisable()
    {
        fakeUnitForDragging.gameObject.SetActive(false);
        ForceEndDrag();
    }


}

//how am i spawning?


public enum EquipWindowType
{
    Gun = 0,
    Ability = 1,
    Trinket = 2,
    Drop = 3,
    Quest = 4
}