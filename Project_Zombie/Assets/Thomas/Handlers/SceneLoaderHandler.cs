using DG.Tweening;
using MyBox;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoaderHandler : MonoBehaviour
{

    GameHandler handler;

    private void Awake()
    {
        handler = GetComponent<GameHandler>();
    }


    [Separator("Scene")]
    [SerializeField] int currentSceneIndex;
    StageData currentStageData;


    const int MAINMENU_INDEX = 0;
    const int LOADINGSCREEN_INDEX = 1;
    const int CITY_INDEX = 2;

    
    public void LoadMainMenu()
    {

    }
    public void LoadMainCity()
    {
        //load the city.
        StopAllCoroutines();
        StartCoroutine(LoadSceneProcess(CITY_INDEX));
    }

    public void ReloadCurrentScene()
    {
        if(LocalHandler.instance == null)
        {
            return;
        }

        currentSceneIndex = LocalHandler.instance._stageData.stageIndex;
        StopAllCoroutines();
        StartCoroutine(LoadSceneProcess(LocalHandler.instance._stageData.stageIndex, LocalHandler.instance._stageData));
    }

    public void LoadStage(StageData data)
    {
        StopAllCoroutines();
        StartCoroutine(LoadSceneProcess(data.stageIndex, data));
    }


    IEnumerator LoadSceneProcess(int index, StageData stage = null)
    {
        PlayerHandler.instance._playerController.block.AddBlock("ChangeScene", BlockClass.BlockType.Complete);

        if(index == 0)
        {
            handler.UpdateText("Loading City"); 
        }
        else if(stage != null)
        {
            handler.UpdateText("Loading " + stage.stageName); 
        }
        else
        {
            handler.UpdateText("Nothing");

        }


        yield return StartCoroutine(handler.LowerCurtainProcess());


        yield return StartCoroutine(LoadProcess(index));

        yield return new WaitForSecondsRealtime(1);

        GameHandler.instance.ResumeGame();
        UIHandler.instance._pauseUI.ForceClosePause();

        currentStageData = stage;
        if (CityHandler.instance != null)
        {
            //we tell the cityhandler to recalculate everything regarding the citystores and equip window
            CityHandler.instance.StartCity();
        }

        if (PlayerHandler.instance != null)
        {
            //reset the abilities and guns.

            PlayerHandler.instance.ResetPlayer();
        }

        yield return StartCoroutine(handler.RaiseCurtainProcess());

        PlayerHandler.instance._playerController.block.ClearBlock();

        if (CityHandler.instance != null)
        {
            //we tell the cityhandler to recalculate everything regarding the citystores and equip window
            PlayerHandler.instance._playerController.block.AddBlock("City", BlockClass.BlockType.Combat);
        }


    }


    IEnumerator LoadProcess(int index)
    {
        AsyncOperation emptyAsync = SceneManager.LoadSceneAsync(LOADINGSCREEN_INDEX, LoadSceneMode.Additive); //this is just empty.

        yield return new WaitUntil(() => emptyAsync.isDone);


        AsyncOperation unloadAsync = SceneManager.UnloadSceneAsync(currentSceneIndex, UnloadSceneOptions.None);

        yield return new WaitUntil(() => unloadAsync.isDone);

        //yield break;

        AsyncOperation loadAsync = SceneManager.LoadSceneAsync(index, LoadSceneMode.Additive);

        yield return new WaitUntil(() => loadAsync.isDone);

        AsyncOperation unloadEmptyAsync = SceneManager.UnloadSceneAsync(LOADINGSCREEN_INDEX); //this is just empty.

        yield return new WaitUntil(() => unloadEmptyAsync.isDone);


        yield return new WaitUntil(() => GameHandler.instance != null && UIHandler.instance != null);


        yield return new WaitUntil(() => CityHandler.instance != null || LocalHandler.instance != null);

        if(CityHandler.instance != null)
        {
            //UIHandler.instance.debugui.UpdateDEBUGUI("FIXED THIS");
        }


        currentSceneIndex = index;
    }

    


    
}

