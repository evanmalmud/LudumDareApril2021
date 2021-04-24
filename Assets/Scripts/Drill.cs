using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drill : MonoBehaviour
{
    Vector3 currentPos;
    Camera mainCam;

    public GameObject drill;
    public BoxCollider2D boxCollider;

    public bool drillEnabled = false;

    private void Start()
    {
        mainCam = Camera.main;
        drill.SetActive(drillEnabled);

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0)) {
            drillEnabled = true;
            drill.SetActive(drillEnabled);
        } else if (Input.GetKeyUp(KeyCode.Mouse0)) {
            drillEnabled = false;
            drill.SetActive(drillEnabled);
        }
        if (drillEnabled) {
            //rotation
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = 0f;

            Vector3 objectPos = Camera.main.WorldToScreenPoint(transform.position);
            mousePos.x = mousePos.x - objectPos.x;
            mousePos.y = mousePos.y - objectPos.y;

            float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }
    }
}
