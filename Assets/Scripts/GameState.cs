using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour
{
    
    public enum DepthType {
        SHALLOW=0,
        MEDIUM=75,
        DEEP=120
    };


    public static DepthType depthCheck(float depth)
    {
        if (Mathf.Abs(depth) > (int)DepthType.DEEP) {
            return DepthType.DEEP;
        }
        else if (Mathf.Abs(depth) > (int)DepthType.MEDIUM) {
            return DepthType.MEDIUM;
        } else {
            return DepthType.SHALLOW;
        }
        
    }

    public Player player;

    public CameraFollow cameraFollow;

    public bool loadReady = false;

    public MusicLoop musicLoop;

    public GameObject readyText;
    public GameObject loadingCanvas;
    public GameObject mainMenuCanvas;
    public GameObject gameOverCanvas;
    public GameOverController gameOverController;

    public GameObject gameOverText;
    public GameObject playerCanvas;
    public GameObject howToCanvas;
    public GameObject missionCanvas;
    public GameObject dialogeCanvas;
    public DialogueController dialogueCont;

    public float timeToStayOnPlayerBeforeGameover = 5f;

    public LevelTimer leveltimer;


    public enum GAMESTATE {
        LOADING,
        MAIN_MENU,
        DIALOGUE,
        HOWTO,
        MISSION,
        GAME,
        REALL,
        GAMEOVER,
        LOAD_REPLAY,
    }
    public GAMESTATE lastState = GAMESTATE.LOADING;
    public GAMESTATE currentState = GAMESTATE.LOADING;

    public TilemapPrefabLoader loader;

    private IEnumerator coroutine;

    public TitleAnimPlayer titleAnimPlayer;

    // Start is called before the first frame update
    void Start()
    {
        dialogueCont = dialogeCanvas.GetComponent<DialogueController>();
        lastState = GAMESTATE.LOADING;
        currentState = GAMESTATE.LOADING;
        cameraFollow.target = loadingCanvas.transform;
        playerCanvas.SetActive(false);
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
        player.playRecall();
        player.canMove = false;
        player.canDrill = false;
        player.sonar.canSonar = false;
        player.drillEnabled = false;
        player.drillL.SetActive(false);
        player.drillR.SetActive(false);
        Debug.Log("playerRecalled");
        currentState = GAMESTATE.GAMEOVER;
        disableAll();
        coroutine = LoadGameOver();
        StartCoroutine(coroutine);
    }

    public void playerDied() {
        player.canMove = false;
        player.canDrill = false;
        player.sonar.canSonar = false;
        player.drillEnabled = false;
        currentState = GAMESTATE.GAMEOVER;
        disableAll();
        coroutine = LoadGameOver();
        StartCoroutine(coroutine);

        //coroutine = LoadNewLevel();
        //StartCoroutine(coroutine);
    }

    void updateText()
    {
        readyText.GetComponent<TMPro.TextMeshProUGUI>().text = "Click to Start";
    }

    void updateMainMenuObj(bool isEnabled) {
        loadingCanvas.SetActive(false);
        mainMenuCanvas.SetActive(isEnabled);
        titleAnimPlayer.playAnim();
        musicLoop.startMusic();
        //cameraFollow.target = mainMenuCanvas.transform;

    }


    void disableAll() {
        loadingCanvas.SetActive(false);
        mainMenuCanvas.SetActive(false);
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
        cameraFollow.target = dialogeCanvas.transform;
        currentState = GAMESTATE.DIALOGUE;
        dialogueCont.playIntroDialogue();
    }

    public void dialogueComplete() 
    {
        cameraFollow.target = howToCanvas.transform;
        currentState = GAMESTATE.HOWTO;
    }

    public void howToClicked() {
        cameraFollow.target = missionCanvas.transform;
        currentState = GAMESTATE.MISSION;
    }

    public void missionClicked()
    {
        disableAll();
        cameraFollow.target = player.transform;
        player.ResetPlayer();
        currentState = GAMESTATE.GAME;
        playerCanvas.SetActive(true);
        leveltimer.startCount();
    }

    public void replayClicked()
    {
        disableAll();
        cameraFollow.target = player.transform;
        player.ResetPlayer();
        currentState = GAMESTATE.GAME;
        playerCanvas.SetActive(true);
        leveltimer.startCount();
    }


    public bool checkIfNotGame() {
        if(currentState.Equals(GAMESTATE.LOADING) || currentState.Equals(GAMESTATE.MAIN_MENU) || currentState.Equals(GAMESTATE.DIALOGUE)
            || currentState.Equals(GAMESTATE.HOWTO) || currentState.Equals(GAMESTATE.MISSION)) {
            return true;
        }
        return false;
    }

    IEnumerator LoadNewLevel() {
        Debug.Log("load new level");
        loader.deleteOldLevel();
        loader.reloadLevel();
        gameOverText.SetActive(true);
        yield return null;
    }

    IEnumerator LoadGameOver()
    {
        Debug.Log("load new level");
        yield return new WaitForSeconds(timeToStayOnPlayerBeforeGameover);
        cameraFollow.target = gameOverCanvas.transform;
        gameOverText.SetActive(false);
        gameOverController.onDisplay(player.isDead);
        yield return new WaitForSeconds(3f);
        IEnumerator coroutine2 = LoadNewLevel();
        StartCoroutine(coroutine2);
    }


}
