using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BossUI : MonoBehaviour
{

    [SerializeField] GameObject _bossHolder;
    [SerializeField] Image _bossFill;
    [SerializeField] TextMeshProUGUI _bossNameText;
    [SerializeField] Transform _refPosIn;
    [SerializeField] Transform _refPosOut;
    [SerializeField] Image[] _defeatedBossComponents;
    [SerializeField] TextMeshProUGUI _defeatedBossText;

    private void Start()
    {
        _bossHolder.transform.localPosition = _refPosOut.localPosition;
    }
    

    public void OpenBossHealth(string bossName)
    {
        _bossHolder.SetActive(true);
        _bossHolder.transform.DOKill();
        _bossHolder.transform.DOLocalMoveY(_refPosIn.localPosition.y, 1.5f);

        _bossNameText.text = bossName;
    }
    public void CloseBossHealth()
    {
        _bossHolder.transform.DOKill();
        _bossHolder.transform.DOLocalMoveY(_refPosOut.localPosition.y, 1.5f);

    }

    public void CallBossDefeatedWarn()
    {
        StopAllCoroutines();
        StartCoroutine(CallBossDefeatedProcess());
    }

    IEnumerator CallBossDefeatedProcess()
    {

        _defeatedBossComponents[0].gameObject.SetActive(true);

        for (int i = 0; i < _defeatedBossComponents.Length; i++)
        {
            var item = _defeatedBossComponents[i];
            item.DOKill();
            item.DOFade(0, 0);
        }
        _defeatedBossText.DOKill();
        _defeatedBossText.DOFade(0, 0);


        for (int i = 0; i < _defeatedBossComponents.Length; i++)
        {
            var item = _defeatedBossComponents[i];
            item.DOFade(0.8f, 0.8f).SetEase(Ease.Linear);
        }
        _defeatedBossText.DOFade(0.8f, 0.8f).SetEase(Ease.Linear);

        yield return new WaitForSeconds(0.8f);

        for (int i = 0; i < _defeatedBossComponents.Length; i++)
        {
            var item = _defeatedBossComponents[i];
            item.DOFade(0f, 0.8f).SetEase(Ease.Linear);
        }
        _defeatedBossText.DOFade(0f, 0.8f).SetEase(Ease.Linear);
    }


    public void UpdateHealth(float current, float total)
    {
        _bossFill.fillAmount = current / total;  
    }


}
