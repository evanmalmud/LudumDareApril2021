using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTimer : MonoBehaviour
{

    public float defaultLevelTimeLength;

    public float levelTimeLeft;

    public bool counting = false;

    public GameState gameState;
    
    // Start is called before the first frame update
    void Start()
    {
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
