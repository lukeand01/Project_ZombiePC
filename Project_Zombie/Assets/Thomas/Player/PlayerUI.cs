using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] Image mouseIcon;

    private void Awake()
    {
        Cursor.visible = false;
    }

    private void Update()
    {
        mouseIcon.transform.position = Input.mousePosition;
    }

    public void CallMouseIconAnimation()
    {

        StopAllCoroutines();
        StartCoroutine(MouseIconAnimationProcess());
    }
    
    IEnumerator MouseIconAnimationProcess()
    {
        float timer = 0.03f;
        mouseIcon.transform.DOScale(0.6f, 0);
        mouseIcon.transform.DOScale(0.8f, timer);

        yield return new WaitForSecondsRealtime(timer);

        mouseIcon.transform.DOScale(0.6f, timer);
        yield return null;
    }



}
