using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checker : MonoBehaviour
{

    public GameObject gameManagerPrefab;

    // Start is called before the first frame update
    void Start()
    {
        if(FindObjectOfType<MusicLoopHandler>() == null) {
            GameObject gm = Instantiate(gameManagerPrefab);
            gm.GetComponent<MusicLoopHandler>().startMusic();
        }

        Destroy(this.gameObject);
    }
}
