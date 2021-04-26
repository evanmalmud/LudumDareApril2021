using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelTimer : MonoBehaviour
{

    public float defaultLevelTimeLength;

    public float levelTimeLeft;

    public bool counting = false;

    public GameState gameState;

    public TextMeshProUGUI text;

    public int timeWarning1;
    public int timeWarning2;
    public int timeWarning3;

    [FMODUnity.EventRef]
    public string timeWarning1Sfx = "";
    [FMODUnity.EventRef]
    public string timeWarning2Sfx = "";
    [FMODUnity.EventRef]
    public string timeWarning3Sfx = "";

    FMOD.Studio.EventInstance timeWarning1SfxInstance;
    FMOD.Studio.EventInstance timeWarning2SfxInstance;
    FMOD.Studio.EventInstance timeWarning3SfxInstance;


    // Start is called before the first frame update
    void Start()
    {
        if (!timeWarning1Sfx.Equals(null) && !timeWarning1Sfx.Equals("")) {
            timeWarning1SfxInstance = FMODUnity.RuntimeManager.CreateInstance(timeWarning1Sfx);
        }
        if (!timeWarning2Sfx.Equals(null) && !timeWarning2Sfx.Equals("")) {
            timeWarning2SfxInstance = FMODUnity.RuntimeManager.CreateInstance(timeWarning2Sfx);
        }
        if (!timeWarning3Sfx.Equals(null) && !timeWarning3Sfx.Equals("")) {
            timeWarning3SfxInstance = FMODUnity.RuntimeManager.CreateInstance(timeWarning3Sfx);
        }
        gameState = GetComponent<GameState>();
        levelTimeLeft = defaultLevelTimeLength;
    }

    // Update is called once per frame
    void Update()
    {
        if(levelTimeLeft > 0 && counting) {
            levelTimeLeft -= Time.deltaTime;
        }
        if(levelTimeLeft < 0 && counting) {
            counting = false;
            gameState.timesUp();
        }
        text.text = "T-" + (int)levelTimeLeft;
        if((int)levelTimeLeft == timeWarning1) {
            timeWarning1SfxInstance.start();
        }
        if ((int)levelTimeLeft == timeWarning2) {
            timeWarning2SfxInstance.start();
        }
        if ((int)levelTimeLeft == timeWarning3) {
            timeWarning3SfxInstance.start();
        }
    }

    public float getLevelTimeLeft() {
        return levelTimeLeft;
    }

    public float getLevelTimeLeftPercent()
    {
        return levelTimeLeft/defaultLevelTimeLength;
    }

    public void startCount(){
        levelTimeLeft = defaultLevelTimeLength;
        counting = true;
    }
}
