using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sonar : MonoBehaviour
{
    public bool sonarEnabled = false;

    public GameObject sonarScanObj;


    private void Start()
    {
        //circleCollider2D = GetComponent<CircleCollider2D>();
        //parent = this.gameObject.transform;
        //localPos = scanObj.transform.localPosition;
    }

    public void SonarUpdate(bool buttonPressed) {
        if (buttonPressed && !sonarEnabled) {

            //Create new SonarScan
            GameObject go = Instantiate(sonarScanObj);
            go.transform.position = this.transform.position;
            go.transform.parent = null;
            go.SetActive(true);
            go.GetComponent<SonarScan>().StartScan(this);
            sonarEnabled = true;
        }
    }

    public void SonarComplete() {
        sonarEnabled = false;
    }

}
