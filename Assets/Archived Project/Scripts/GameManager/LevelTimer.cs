using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTimer : MonoBehaviour
{

    public float defaultLevelTimeLength;

    public float levelTimeLeft;

    public bool counting = false;
    
    // Start is called before the first frame update
    void Start()
    {
        levelTimeLeft = defaultLevelTimeLength;
    }

    // Update is called once per frame
    void Update()
    {
        if(levelTimeLeft > 0 && counting) {
            levelTimeLeft -= Time.deltaTime;
        }
    }

    public float getLevelTimeLeft() {
        return levelTimeLeft;
    }
}
