using DG.Tweening;
using MyBox;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameHandler : MonoBehaviour
{
    public static GameHandler instance {  get; private set; }

    public SoundHandler _soundHandler {  get; private set; }
    public SceneLoaderHandler _sceneLoader {  get; private set; }
    public PoolHandler _pool { get; private set; }
    public SaveHandler _saveHandler { get; private set; }


    [field:SerializeField] public SettingsData _settingsData { get; private set; }
    [field: SerializeField] public CityDataHandler cityDataHandler { get; private set; }



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


        DontDestroyOnLoad(gameObject);

        _soundHandler = GetComponent<SoundHandler>();
        _sceneLoader = GetComponent<SceneLoaderHandler>();
        _pool = GetComponent<PoolHandler>();    
        _saveHandler = GetComponent<SaveHandler>(); 

        _settingsData.Initialize();


        //cityDataHandler.cityArmory.Initialize();
        //cityDataHandler.cityLab.Initialize();
        cityDataHandler.cityMain.Initialize();
        //cityDataHandler.cityDropLauncher.Initalize();

        ResumeGame();
    }



    private void Start()
    {

        EntityStat stat = PlayerHandler.instance._entityStat;

        if(stat == null)
        {
            Debug.LogError("no stat here");
            return;
        }


        cityDataHandler.cityBodyEnhancer.Initalize();
        cityDataHandler.cityDropLauncher.GenerateListForEquipContainer();
    }

    private void Update()
    {
        if (rotateImageHolder.activeInHierarchy)
        {
            rotateImageHolder.transform.Rotate(new Vector3(0, 0, 1));
        }
    }

    //i need to set this thing. by taking the stuff 

    #region PAUSE
    public float timeModifier { get; private set; }


    public void PauseGame()
    {
        Time.timeScale = 0f;
        timeModifier = 10000;
    }
    public void ResumeGame()
    {
        Time.timeScale = 1;
        timeModifier = 1;
    }


    #endregion

    #region FADE SCREEN
    [Separator("UI")]
    [SerializeField] Image loadScreen;
    [SerializeField] TextMeshProUGUI tipText;
    [SerializeField] TextMeshProUGUI titleText;
    [SerializeField] Image rotateImageBackground;
    [SerializeField] Image rotateImage;
    [SerializeField] GameObject rotateImageHolder;


    public IEnumerator LowerCurtainProcess()
    {
        //i need to load everyone and decided before that the text.

        float duration = 0.3f;


        loadScreen.gameObject.SetActive(true);
        loadScreen.DOFade(1, duration).SetUpdate(true);
        tipText.DOFade(1, duration).SetUpdate(true);
        titleText.DOFade(1, duration).SetUpdate(true);
        rotateImageBackground.DOFade(1, duration).SetUpdate(true);
        rotateImage.DOFade(1, duration).SetUpdate(true);


        tipText.text = "Tip: " + GetRandomTip();

        yield return new WaitUntil(() => loadScreen.color.a >= 1);

    }
    public IEnumerator RaiseCurtainProcess()
    {
        float duration = 0.3f;

        loadScreen.DOFade(0, duration).SetUpdate(true);
        tipText.DOFade(0, duration).SetUpdate(true);
        titleText.DOFade(0, duration).SetUpdate(true);
        rotateImageBackground.DOFade(0, duration).SetUpdate(true);
        rotateImage.DOFade(0, duration).SetUpdate(true);

        tipText.text = "Tip: " + GetRandomTip();

        yield return new WaitUntil(() => loadScreen.color.a == 0);
        loadScreen.gameObject.SetActive(false);
    }

    public void UpdateText(string text)
    {
        titleText.text = text;
    }

    string GetRandomTip()
    {
        string[] tips =
        {
            "Tip 1",
            "Tip 2",
            "Voce sabia q se o seu nome e rodrigo vc e gay?",
            "Tip 3"
        };

        int random = Random.Range(0, tips.Length);


        return tips[random];
    }

    #endregion

    #region TELEPORT SCREEN
    [Separator("TELEPORTER SCREEN")]
    [SerializeField] Image teleportScreen;

    //

    public IEnumerator LowerCurtainProcess_Teleport(float timer)
    {
        teleportScreen.gameObject.SetActive(true);
        teleportScreen.DOFade(0, 0); //we set as fade here
        teleportScreen.DOFade(1, timer).SetEase(Ease.Linear);

        yield return new WaitForSeconds(timer);

    }

    public IEnumerator RaiseCurtainProcess_Teleport(float timer)
    {
        
        teleportScreen.DOFade(0, timer).SetEase(Ease.Linear);

        yield return new WaitForSeconds(timer);

        teleportScreen.gameObject.SetActive(false);
    }

    #endregion

    #region COLOR


    #endregion

    public void RestoreGameState(SaveClass saveClass)
    {

        cityDataHandler.RestoreState(saveClass);

    }

    public void CaptureGameState(SaveClass saveClass)
    {
        //we will get this stuff and passs what we should.

        cityDataHandler.CaptureState(saveClass);


    }

}
