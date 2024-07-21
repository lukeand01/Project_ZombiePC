using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    public static GameHandler instance {  get; private set; }

    public SoundHandler _soundHandler {  get; private set; }
    public SceneLoaderHandler _sceneLoader {  get; private set; }
    public PoolHandler _pool { get; private set; }

    [field:SerializeField] public SettingsData _settingsData { get; private set; }
    [field: SerializeField] public CityDataHandler cityDataHandler { get; private set; }


    //
    //we have the cityhandler here. always.


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

        _settingsData.Initialize();


        cityDataHandler.cityArmory.Initialize();
        cityDataHandler.cityLab.Initialize();
        cityDataHandler.cityMain.Initialize();
        cityDataHandler.cityDropLauncher.Initalize();

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

}
