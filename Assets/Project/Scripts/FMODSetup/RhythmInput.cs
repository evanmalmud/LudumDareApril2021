using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.InputSystem;

public class RhythmInput : MonoBehaviour
{

    public InputMaster controls;

    public Camera mainCamera;

    public GameObject spotLight;
    public bool spotlightEnabled = false;
    Light2D spotLight2d;

    public List<string> expectedButtons = new List<string>();

    void Awake()
    {
        controls = new InputMaster();
        controls.Rhythm.Aim.performed += ctx => mouse(ctx.ReadValue<Vector2>());
        controls.Rhythm.Mousebutton.started += mousebutton;
        InputAction myAction = new InputAction(binding: "/<Keyboard>/<button>");
        myAction.performed += keyboardPress;
        myAction.Enable();

        spotLight2d = spotLight.GetComponent<Light2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("test");
    }

    private void mouse(Vector2 aim)
    {
        Vector2 newPos = mainCamera.ScreenToWorldPoint(aim);
        spotLight.transform.position = newPos;
    }

    private void keyboardPress(InputAction.CallbackContext context)
    {
        //Debug.Log("Keyboard " + context.control.name);
        if(expectedButtons.Contains(context.control.name.ToUpper())) {
            Debug.Log("WE MATCHED A NOTE!");
            expectedButtons.Remove(context.control.name.ToUpper());
        }
    }

    private void mousebutton(InputAction.CallbackContext context)
    {
        spotlightEnabled = !spotlightEnabled;
        spotLight2d.enabled = spotlightEnabled;
    }

    void OnEnable()
    {
        controls.Rhythm.Enable();
    }

    /// <summary>
    /// This function is called when the behaviour becomes disabled or inactive.
    /// </summary>
    void OnDisable()
    {
        controls.Rhythm.Disable();
    }
}
