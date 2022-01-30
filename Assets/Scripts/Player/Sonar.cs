using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sonar : MonoBehaviour
{
    public bool sonarActive = false;
    public bool sonarAnimActive = false;

    public GameObject sonarScanObj;

    public float timeForScanAnimation = 1f;


    private void Start()
    {
        //circleCollider2D = GetComponent<CircleCollider2D>();
        //parent = this.gameObject.transform;
        //localPos = scanObj.transform.localPosition;
    }

    public void SonarUpdate(bool buttonPressed) {
        if (buttonPressed && !sonarActive && !sonarAnimActive) {

            //Create new SonarScan
            GameObject go = Instantiate(sonarScanObj);
            go.transform.position = this.transform.position;
            go.transform.parent = null;
            go.SetActive(true);
            go.GetComponent<SonarScan>().StartScan(this);
            sonarActive = true;
            sonarAnimActive = true;
            StartCoroutine(scanAnimationReturn());
        }
    }

    IEnumerator scanAnimationReturn() {
        yield return new WaitForSeconds(timeForScanAnimation);
        sonarAnimActive = false;
    }

    public void SonarComplete() {
        sonarActive = false;
    }

}
