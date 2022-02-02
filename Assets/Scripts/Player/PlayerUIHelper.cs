using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUIHelper : MonoBehaviour
{

    public PlayerConfig playerConfig;

    public TMPro.TextMeshProUGUI scoreTextGO;


    // Start is called before the first frame update
    void Start()
    {
        playerConfig = FindObjectOfType<PlayerConfig>();
    }

    // Update is called once per frame
    void Update()
    {
        updateScoreText();
    }


    public void updateScoreText() {
        string scoreText = "$" + playerConfig.playerState.moneyScore;
        scoreTextGO.text = scoreText;
    }
}
