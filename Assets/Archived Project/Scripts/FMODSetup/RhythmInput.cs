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

    [FMODUnity.EventRef]
    public string spotlightSFX = "";
    FMOD.Studio.EventInstance instance;

    public float absPositionChange = 5f;
    //public float posChangeOverSeconds = .5f;
    public float squakTimerDefault = 3f;
    public float squakTimerCurrent = 0f;

    void Awake()
    {
        controls = new InputMaster();
        controls.Rhythm.Aim.performed += ctx => mouse(ctx.ReadValue<Vector2>());
        controls.Rhythm.Mousebutton.started += mousebutton;
        controls.Rhythm.Enable();
        //InputAction myAction = new InputAction(binding: "/<Keyboard>/<button>");
        //myAction.performed += keyboardPress;
        //myAction.Enable();

        spotLight2d = spotLight.GetComponent<Light2D>();
    }

    private void Start()
    {
        if (!spotlightSFX.Equals(null) && !spotlightSFX.Equals("")) {
            instance = FMODUnity.RuntimeManager.CreateInstance(spotlightSFX);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(squakTimerCurrent > 0f) {
            squakTimerCurrent -= Time.deltaTime;
        }
        if(Keyboard.current.anyKey.wasPressedThisFrame) {
            checkForKeyPressesThisFrame();
        }
    }

    private void mouse(Vector2 aim)
    {
        Vector2 newPos = mainCamera.ScreenToWorldPoint(aim);
        Vector2 diff = (Vector2)spotLight.transform.position - newPos;
        Debug.Log("MAG - " + diff.magnitude + " Vec2 " + diff);
        if (diff.magnitude >= absPositionChange) {
            instance.start();
            Debug.Log("CHANGE OVER MAG");
        }
        spotLight.transform.position = newPos;

    }

    private void mousebutton(InputAction.CallbackContext context)
    {
        spotlightEnabled = !spotlightEnabled;
        spotLight2d.enabled = spotlightEnabled;
    }

    void OnDisable()
    {
        controls.Rhythm.Disable();
    }

    void checkForKeyPressesThisFrame() {
        checkKey(Key.A);
        checkKey(Key.B);
        checkKey(Key.C);
    }

    void checkKey(Key key) {
        if (Keyboard.current[key].wasPressedThisFrame) {
            Debug.Log("Pressed " + key.ToString().ToUpper() + " " + Time.time);
            if (expectedButtons.Contains(key.ToString().ToUpper())) {
                Debug.Log("WE MATCHED A NOTE!");
                expectedButtons.Remove(key.ToString().ToUpper());
            }
        }
    }
}
