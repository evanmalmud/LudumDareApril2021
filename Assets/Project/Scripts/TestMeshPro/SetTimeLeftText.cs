using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetTimeLeftText : MonoBehaviour
{

    public LevelTimer levelTimer;
    public TMPro.TextMeshProUGUI text;
    // Update is called once per frame

    private void Start()
    {
        levelTimer = FindObjectOfType<LevelTimer>();
    }
    void Update()
    {
        if (levelTimer == null) {
            levelTimer = FindObjectOfType<LevelTimer>();
        } else {
            text.text = levelTimer.getLevelTimeLeft().ToString("0.00");
        }
    }
}
