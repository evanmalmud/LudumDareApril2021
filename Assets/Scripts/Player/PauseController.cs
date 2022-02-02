using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseController : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject pauseCanvas;

    public PlayerConfig playerConfig;

    public bool pauseEnabled = false;

    private void Start()
    {
        pauseCanvas.SetActive(false);
    }

    public void PauseInteractionToggle()
    {
        if (pauseEnabled) {
            pauseEnabled = false;
            pauseCanvas.SetActive(pauseEnabled);
            playerConfig.PausedState(pauseEnabled);
        } else {
            pauseEnabled = true;
            pauseCanvas.SetActive(pauseEnabled);
            playerConfig.PausedState(pauseEnabled);
        }
    }
}
