using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemNotificationUnit : MonoBehaviour
{
    //

    GameObject holder;

    InventoryUI _inventoryUI;
    Transform fakeCopy;


    [SerializeField] Image background;
    [SerializeField] Image portrait;
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI quantityText;

    private void Awake()
    {
        holder = transform.GetChild(0).gameObject;
    }


    public void MakeFake()
    {
        holder.SetActive(false);
    }

    public void SetUp(InventoryUI _inventoryUI, ItemClass item, Transform fakeCopy)
    {
        nameText.text = item.data.itemName;
        quantityText.text = item.quantity.ToString();  
        
        this._inventoryUI = _inventoryUI;

        this.fakeCopy = fakeCopy;

        total = 3f;
    }


    float current;
    float total;
    bool isDone;

    private void Update()
    {

        if (fakeCopy == null) return;

        transform.position = Vector3.MoveTowards(transform.position, fakeCopy.position, Time.deltaTime * 1000);

        if (isDone)
        {
            return;
        }


        if(current > total)
        {
            isDone = true;
            StopAllCoroutines();
            StartCoroutine(EndProcess());
        }
        else
        {
            current += Time.unscaledDeltaTime;
        }
    }

    IEnumerator EndProcess()
    {

        float duration = 0.3f;

        background.DOFade(0, duration).SetUpdate(true);
        portrait.DOFade(0, duration).SetUpdate(true);
        nameText.DOFade(0, duration).SetUpdate(true);
        quantityText.DOFade(0, duration).SetUpdate(true);


        yield return new WaitForSecondsRealtime(duration);
        
        

        //yield return new WaitForSeconds(0.3f);

        _inventoryUI.RemoveFirstInList();

        Destroy(fakeCopy.gameObject);
        Destroy(gameObject);



    }

}
