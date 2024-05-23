using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    public static GameHandler instance {  get; private set; }

    public SoundHandler _soundHandler {  get; private set; }
    public SceneLoaderHandler _sceneLoader {  get; private set; }


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


        ResumeGame();
    }


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
