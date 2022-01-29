using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour {

    public float initialDelay = 0f;

    public String sceneToLoad;

    public Animator animator;


    //Get this from the animation possibly?
    public float transitionTime = 1f;

    public void LoadScene()
    {
        if (sceneToLoad != null) {

            StartCoroutine(LoadLevel(sceneToLoad));
        }
    }

    public void LoadScene(float initialDelay)
    {
        if (sceneToLoad != null) {

            StartCoroutine(LoadLevel(initialDelay, sceneToLoad));
        }
    }

    IEnumerator LoadLevel(String sceneToLoad) {

        //Wait
        yield return new WaitForSeconds(initialDelay);

        //Play Anim
        animator.SetTrigger("Start");

        //Wait
        yield return new WaitForSeconds(transitionTime);

        //Load scene
        SceneManager.LoadScene(sceneToLoad);
    }

    IEnumerator LoadLevel(float initialDelay, String sceneToLoad)
    {

        //Wait
        yield return new WaitForSeconds(initialDelay);

        //Play Anim
        animator.SetTrigger("Start");

        //Wait
        yield return new WaitForSeconds(transitionTime);

        //Load scene
        SceneManager.LoadScene(sceneToLoad);
    }
}
