using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour
{
    public Player player;

    public CameraFollow cameraFollow;

    public bool loadReady = false;

    public MusicLoop musicLoop;

    public GameObject readyText;
    public GameObject loadingCanvas;
    public GameObject mainMenuCanvas;
    public GameObject gameOverCanvas;

    public GameObject gameOverText;

    public LevelTimer leveltimer;
    public enum GAMESTATE {
        LOADING,
        MAIN_MENU,
        GAME,
        REALL,
        GAMEOVER
    }
    public GAMESTATE lastState = GAMESTATE.LOADING;
    public GAMESTATE currentState = GAMESTATE.LOADING;

    public TilemapPrefabLoader loader;

    private IEnumerator coroutine;

    // Start is called before the first frame update
    void Start()
    {
        lastState = GAMESTATE.LOADING;
        currentState = GAMESTATE.LOADING;
        cameraFollow.target = loadingCanvas.transform;
    }

    void Update()
    {
        if (!loadReady && FMODUnity.RuntimeManager.HasBankLoaded("Master")) {
            loadReady = true;
            updateText();
            //updateMainMenuObj(true, true);
        }
    }

    public void timesUp() {
        //leveltimer.counting = true;
        currentState = GAMESTATE.REALL;
        player.playRecall();
    }

    public void playerRecalled()
    {
        Debug.Log("playerRecalled");
        currentState = GAMESTATE.GAMEOVER;
        disableAll();
        cameraFollow.target = gameOverCanvas.transform;
        gameOverText.SetActive(false);
        coroutine = LoadNewLevel();
        StartCoroutine(coroutine);
    }

    public void playerDied() {
        currentState = GAMESTATE.GAMEOVER;
        disableAll();
        cameraFollow.target = gameOverCanvas.transform;
        gameOverText.SetActive(false);
        coroutine = LoadNewLevel();
        StartCoroutine(coroutine);
    }

    void updateText()
    {
        readyText.GetComponent<TMPro.TextMeshProUGUI>().text = "Ready?...";
    }

    void updateMainMenuObj(bool isEnabled) {
        mainMenuCanvas.SetActive(isEnabled);
        musicLoop.startMusic();
        cameraFollow.target = mainMenuCanvas.transform;

    }


    void disableAll() {
       // loadingCanvas.SetActive(false);
       // mainMenuCanvas.SetActive(false);
       // readyText.SetActive(false);
        //gameOverCanvas.SetActive(false);
    }

    public void loadReadyClicked() {
        if(loadReady) {
            disableAll();
            updateMainMenuObj(true);
            currentState = GAMESTATE.MAIN_MENU;
        }
    }

    public void mainMenuClicked()
    {
        disableAll();
        cameraFollow.target = player.transform;
        player.ResetPlayer();
        currentState = GAMESTATE.GAME;
        leveltimer.startCount();
    }

    public void replayClicked()
    {
        disableAll();
        cameraFollow.target = player.transform;
        player.ResetPlayer();
        currentState = GAMESTATE.GAME;
        leveltimer.startCount();
    }


    public bool checkIfLoadingOrMainMenu() {
        if(currentState.Equals(GAMESTATE.LOADING) || currentState.Equals(GAMESTATE.MAIN_MENU)) {
            return true;
        }
        return false;
    }

    IEnumerator LoadNewLevel() {
        Debug.Log("load new level");
        yield return new WaitForSeconds(1f);
        loader.deleteOldLevel();
        loader.reloadLevel();
        gameOverText.SetActive(true);
    }


}
