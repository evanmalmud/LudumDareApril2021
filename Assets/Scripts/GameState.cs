using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour
{

    public enum DepthType {
        SHALLOW=0,
        MEDIUM=75,
        DEEP=150
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

    public GameObject midGameDialogue;
    public DialogueController midGameDialogueCont;

    public GameObject gameOverText;
    public GameObject playerCanvas;
    public GameObject howToCanvas;
    public GameObject missionCanvas;
    public GameObject dialogeCanvas;
    public DialogueController dialogueCont;

    public float timeToStayOnPlayerBeforeGameover = 5f;
    public float timeToStayOnPlayerBeforeGameoverRecalled = 5f;

    public LevelTimer leveltimer;


    public enum GAMESTATE {
        LOADING,
        TITLE,
        DIALOGUE,
        HOWTO,
        MISSION,
        GAME,
        REALL,
        GAMEOVER,
        MIDGAMELOAD,
    }
    public GAMESTATE lastState = GAMESTATE.LOADING;
    public GAMESTATE currentState = GAMESTATE.LOADING;

    public TilemapPrefabLoader loader;

    private IEnumerator coroutine;

    public TitleAnimPlayer titleAnimPlayer;

    [FMODUnity.EventRef]
    public string negative = "";
    [FMODUnity.EventRef]
    public string positive = "";
    [FMODUnity.EventRef]
    public string neutral = "";
    [FMODUnity.EventRef]
    public string timesheetIn = "";
    [FMODUnity.EventRef]
    public string timesheetOut = "";
    FMOD.Studio.EventInstance negInstance;
    FMOD.Studio.EventInstance posInstance;
    FMOD.Studio.EventInstance neutralInstance;
    FMOD.Studio.EventInstance timesheetInInstance;
    FMOD.Studio.EventInstance timesheetOutInstance;

    public void OnDestroy()
    {
        negInstance.release();
        posInstance.release();
        neutralInstance.release();
        timesheetInInstance.release();
        timesheetOutInstance.release();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (!negative.Equals(null) && !negative.Equals("")) {
            negInstance = FMODUnity.RuntimeManager.CreateInstance(negative);
        }
        if (!positive.Equals(null) && !positive.Equals("")) {
            posInstance = FMODUnity.RuntimeManager.CreateInstance(positive);
        }
        if (!neutral.Equals(null) && !neutral.Equals("")) {
            neutralInstance = FMODUnity.RuntimeManager.CreateInstance(neutral);
        }
        if (!timesheetIn.Equals(null) && !timesheetIn.Equals("")) {
            timesheetInInstance = FMODUnity.RuntimeManager.CreateInstance(timesheetIn);
        }
        if (!timesheetOut.Equals(null) && !timesheetOut.Equals("")) {
            timesheetOutInstance = FMODUnity.RuntimeManager.CreateInstance(timesheetOut);
        }
        dialogueCont = dialogeCanvas.GetComponent<DialogueController>();
        midGameDialogueCont = midGameDialogue.GetComponent<DialogueController>();
        lastState = GAMESTATE.LOADING;
        currentState = GAMESTATE.LOADING;
        cameraFollow.target = loadingCanvas.transform;
        playerCanvas.SetActive(false);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)) {
            Application.Quit();
        }
        if (!loadReady && FMODUnity.RuntimeManager.HasBankLoaded("Master")) {
            loadReady = true;
            updateText();
            //updateMainMenuObj(true, true);
        }
    }

    public void timesUp() {
        musicLoop.stopHelmet();
        leveltimer.counting = false;
        playerCanvas.SetActive(false);
        player.playRecall();
        player.canMove = false;
        player.canDrill = false;
        player.drillSfxInstance.setPaused(true);
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
        musicLoop.stopHelmet();
        leveltimer.counting = false;
        playerCanvas.SetActive(false);
        player.canMove = false;
        player.canDrill = false;
        player.drillSfxInstance.setPaused(true);
        player.sonar.canSonar = false;
        player.drillEnabled = false;
        player.drillL.SetActive(false);
        player.drillR.SetActive(false);
        currentState = GAMESTATE.GAMEOVER;
        disableAll();
        coroutine = LoadGameOver();
        StartCoroutine(coroutine);
    }

    void updateText()
    {
        posInstance.start();
        readyText.GetComponent<TMPro.TextMeshProUGUI>().text = "Click to Start";
    }

    void updateMainMenuObj(bool isEnabled) {
        loadingCanvas.SetActive(false);
        mainMenuCanvas.SetActive(isEnabled);
        titleAnimPlayer.playAnim();
        currentState = GAMESTATE.LOADING;
        musicLoop.startMusic();
        //cameraFollow.target = mainMenuCanvas.transform;

    }


    void disableAll() {
        loadingCanvas.SetActive(false);
        //mainMenuCanvas.SetActive(false);
       // readyText.SetActive(false);
        //gameOverCanvas.SetActive(false);
    }

    public void loadReadyClicked() {
        posInstance.start();
        if (loadReady) {
            disableAll();
            updateMainMenuObj(true);
            currentState = GAMESTATE.TITLE;
        }
    }

    public void mainMenuClicked()
    {
        disableAll();
        cameraFollow.target = dialogeCanvas.transform;
        currentState = GAMESTATE.DIALOGUE;
        dialogueCont.playIntroDialogue();
    }

    public void dialogueSkipped()
    {
        cameraFollow.target = howToCanvas.transform;
        currentState = GAMESTATE.HOWTO;
        timesheetInInstance.start();
    }

    public void dialogueComplete() 
    {
        cameraFollow.target = howToCanvas.transform;
        currentState = GAMESTATE.HOWTO;
        timesheetInInstance.start();
    }

    public void howToClicked() {
        timesheetInInstance.start();
        cameraFollow.target = missionCanvas.transform;
        currentState = GAMESTATE.MISSION;
    }

    public void missionClicked()
    {
        timesheetOutInstance.start();
        disableAll();
        cameraFollow.target = player.transform;
        player.ResetPlayer();
        currentState = GAMESTATE.GAME;
        musicLoop.startHelmet();
        playerCanvas.SetActive(true);
        leveltimer.startCount();
    }


    public void replayClicked()
    {
        disableAll();
        cameraFollow.target = midGameDialogue.transform;
        player.isDead = false;
        player.isRecalled = false;
        player.transform.position = new Vector3(0f, 0f, 0f);
        currentState = GAMESTATE.MIDGAMELOAD;

        musicLoop.startMusic();
        midGameDialogueCont.playIntroDialogue();
    }

    public void midGameDialogueSkipp() {
        cameraFollow.target = player.transform;
        currentState = GAMESTATE.GAME;
        musicLoop.startHelmet();
        player.ResetPlayer();
        playerCanvas.SetActive(true);
        leveltimer.startCount();
    }

    public bool checkIfTitle() {
        if (currentState.Equals(GAMESTATE.TITLE) || currentState.Equals(GAMESTATE.LOADING)) {
            return true;
        }
        return false;
    }

    public bool checkIfNotGameorTitle() {
        if(currentState.Equals(GAMESTATE.LOADING) || currentState.Equals(GAMESTATE.DIALOGUE)
            || currentState.Equals(GAMESTATE.HOWTO) || currentState.Equals(GAMESTATE.MISSION)
            || currentState.Equals(GAMESTATE.MIDGAMELOAD)) {
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
        if (player.isDead) {
            yield return new WaitForSeconds(timeToStayOnPlayerBeforeGameover);
        } else {
            yield return new WaitForSeconds(timeToStayOnPlayerBeforeGameoverRecalled);
        }
        cameraFollow.target = gameOverCanvas.transform;
        gameOverText.SetActive(false);
        gameOverController.onDisplay(player.isDead);
        yield return new WaitForSeconds(3.5f);
        IEnumerator coroutine2 = LoadNewLevel();
        StartCoroutine(coroutine2);
    }


}
