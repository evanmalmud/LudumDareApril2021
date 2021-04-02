using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetHealthText : MonoBehaviour
{

    public PlayerController playerController;
    public TMPro.TextMeshProUGUI text;
    // Update is called once per frame
    void Update()
    {
        text.text = playerController.getHealth().ToString("0");
    }
}
