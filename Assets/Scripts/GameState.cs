using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour
{
    public Transform player;

    public CameraFollow cameraFollow;

    public bool loadReady = false;

    public MusicLoop musicLoop;

    public GameObject readyText;
    public GameObject loadingCanvas;
    public GameObject mainMenuCanvas;

    public LevelTimer leveltimer;
    public enum GAMESTATE {
        LOADING,
        MAIN_MENU,
        INTRO,
        GAME,
        GAMEOVER
    }
    public GAMESTATE lastState = GAMESTATE.LOADING;
    public GAMESTATE currentState = GAMESTATE.LOADING;

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
        loadingCanvas.SetActive(false);
        mainMenuCanvas.SetActive(false);
        readyText.SetActive(false);
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
        cameraFollow.target = player;
        player.gameObject.GetComponent<Player>().canMove = true;
        player.gameObject.GetComponentInChildren<Drill>().canDrill = true;
        player.gameObject.GetComponentInChildren<Sonar>().canSonar = true;
        currentState = GAMESTATE.GAME;
        leveltimer.counting = true;
    }


}
