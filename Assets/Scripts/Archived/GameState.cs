using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameState : MonoBehaviour
{

    public enum DepthType {
        SHALLOW=0,
        MEDIUM=50,
        DEEP=100
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

    public GameObject loadingReadyText;
    public GameObject loadingCanvas;
    public GameObject logoCanvas;
    public GameObject gameOverCanvas;
    public GameOverController gameOverController;

    public GameObject pauseCanvas;
    public bool isPaused = false;

    public GameObject midGameDialogue;
    public DialogueController midGameDialogueCont;

    public GameObject gameOverText;
    public GameObject playerCanvas;
    public GameObject howToCanvas;
    public GameObject missionCanvas;
    public GameObject dialogeCanvas;
    public DialogueController dialogueCont;

    public GameObject sceneTransition;
    public Image sceneTranstionImage;
    public float cutoffClosed = -0.1f;
    public float cutoffOpen = 1.1f;
    public float currentCutoff = 1.1f;
    public float sceneTransitionTime = 1f;

    public float timeToStayOnPlayerBeforeGameover = 5f;
    public float timeToStayOnPlayerBeforeGameoverRecalled = 5f;

    public LevelTimer leveltimer;

    public bool loadingLockout = true;
    public bool titleLockout = true;
    public bool dialogueLockout = true;
    public bool howToLockout = true;
    public bool missionLockout = true;
    public bool gameoverLockout = true;
    public bool midgameLockout = true;

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
        loadingLockout = false;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)) {
            //Application.Quit();
            togglePause();
        }
        if (!loadReady && FMODUnity.RuntimeManager.HasBankLoaded("Master")) {
            loadReady = true;
            updateText();
            //updateMainMenuObj(true, true);
        }
    }

    public void togglePause() {
        if(isPaused || (!isPaused && currentState == GAMESTATE.GAME && !player.isDead && !player.isRecalled)) {
            isPaused = !isPaused;
            //Disable Player controls
            if (!player.isDead && !player.isRecalled) {
                leveltimer.counting = !isPaused;
                player.canMove = !isPaused;
                player.canDrill = !isPaused;
                //player.sonar.canSonar = !isPaused;
                if (isPaused) {
                    player.drillSfxInstance.setPaused(isPaused);
                    player.drillEnabled = !isPaused;
                }
            }

            pauseCanvas.SetActive(isPaused);
        }
    }

    public void timesUp() {
  
        levelEndItems();
        player.playRecall();

        Debug.Log("playerRecalled");
        currentState = GAMESTATE.GAMEOVER;
        disableAll();
        coroutine = LoadGameOver();
        StartCoroutine(coroutine);
    }

    public void levelEndItems() {
        musicLoop.stopHelmet();
        leveltimer.counting = false;
        leveltimer.levelTimeLeft = 0f;
        playerCanvas.SetActive(false);
        player.canMove = false;
        player.canDrill = false;
        player.drillSfxInstance.setPaused(true);
        //player.sonar.canSonar = false;
        player.drillEnabled = false;
        player.drillL.SetActive(false);
        player.drillR.SetActive(false);
    }

    public void playerDied() {
        levelEndItems();
        currentState = GAMESTATE.GAMEOVER;
        disableAll();
        coroutine = LoadGameOver();
        StartCoroutine(coroutine);
    }

    void updateText()
    {
        posInstance.start();
        loadingReadyText.GetComponent<TMPro.TextMeshProUGUI>().text = "Click to Start";
    }

    void updateMainMenuObj(bool isEnabled) {
        loadingCanvas.SetActive(false);
        logoCanvas.SetActive(isEnabled);
        titleAnimPlayer.playAnim();
        currentState = GAMESTATE.LOADING;
        //musicLoop.startMusic();
        //cameraFollow.target = mainMenuCanvas.transform;

    }


    void disableAll() {
        loadingCanvas.SetActive(false);
        //mainMenuCanvas.SetActive(false);
       // readyText.SetActive(false);
        //gameOverCanvas.SetActive(false);
    }

    public void loadReadyClicked() {
        if (!loadingLockout) {
            posInstance.start();
            if (loadReady) {
                disableAll();
                updateMainMenuObj(true);
                currentState = GAMESTATE.TITLE;
                titleLockout = false;
                loadingLockout = true;
            }
        }
    }

    public void titleClicked()
    {
        if (!titleLockout) {
            disableAll();
            cameraFollow.target = dialogeCanvas.transform;
            currentState = GAMESTATE.DIALOGUE;
            dialogueCont.playIntroDialogue();
            titleLockout = true;
            dialogueLockout = false;
        }
    }

    public void dialogueSkipped()
    {
        if (!dialogueLockout) {
            cameraFollow.target = howToCanvas.transform;
            currentState = GAMESTATE.HOWTO;
            timesheetInInstance.start();
            dialogueLockout = true;
            howToLockout = false;
        }
    }

    public void dialogueComplete() 
    {
        cameraFollow.target = howToCanvas.transform;
        currentState = GAMESTATE.HOWTO;
        timesheetInInstance.start();
        dialogueLockout = true;
        howToLockout = false;
    }

    public void howToClicked() {
        if (!howToLockout) {
            timesheetInInstance.start();
            cameraFollow.target = missionCanvas.transform;
            currentState = GAMESTATE.MISSION;
            howToLockout = true;
            missionLockout = false;
        }
    }

    public void missionClicked()
    {
        if (!missionLockout) {
            timesheetOutInstance.start();
            disableAll();
            cameraFollow.target = player.transform;
            player.ResetPlayer();
            currentState = GAMESTATE.GAME;
            musicLoop.startHelmet();
            playerCanvas.SetActive(true);
            leveltimer.startCount();
            missionLockout = true;
        }
    }


    public void replayClicked()
    {
        disableAll();
        cameraFollow.target = midGameDialogue.transform;
        player.isDead = false;
        player.isRecalled = false;
        player.transform.position = new Vector3(0f, 0f, 0f);
        currentState = GAMESTATE.MIDGAMELOAD;

        //musicLoop.startMusic();
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
        loader.ResetLevel();
        gameOverText.SetActive(true);
        yield return null;
    }

    IEnumerator LoadGameOver()
    {
        Debug.Log("load new level");
        if (player.isDead) {
            yield return new WaitForSeconds(timeToStayOnPlayerBeforeGameover - sceneTransitionTime);

        } else {
            yield return new WaitForSeconds(timeToStayOnPlayerBeforeGameoverRecalled - sceneTransitionTime);
        }
        //Scene Transtion
        DOTween.To(() => cutoffOpen, x => sceneTranstionImage.material.SetFloat("Cutoff", x), cutoffClosed, sceneTransitionTime);
        //sceneTranstionImage.material.SetFloat("Cutoff", 
            //Mathf.MoveTowards(sceneTranstionImage.material.GetFloat("Cutoff"), cutoffOpen, sceneTransitionTime));
        yield return new WaitForSeconds(sceneTransitionTime);
        float currentSmoothDamp = cameraFollow.smoothDampTime;
        cameraFollow.smoothDampTime = 0f;
        cameraFollow.target = gameOverCanvas.transform;
        gameOverText.SetActive(false);
        gameOverController.onDisplay(player.isDead);
        yield return new WaitForSeconds(1f);
        DOTween.To(() => cutoffClosed, x => sceneTranstionImage.material.SetFloat("Cutoff", x), cutoffOpen, sceneTransitionTime);
        yield return new WaitForSeconds(sceneTransitionTime);
        cameraFollow.smoothDampTime = currentSmoothDamp;
        yield return new WaitForSeconds(1f);
        IEnumerator coroutine2 = LoadNewLevel();
        StartCoroutine(coroutine2);
    }


}
