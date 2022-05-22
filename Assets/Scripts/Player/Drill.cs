using FMODUnity;
using PowerTools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drill : MonoBehaviour
{
    public AnimationClip m_drillstarting = null;
    public AnimationClip m_drillloop = null;

    public GameObject drill;
    private SpriteAnim drillSpriteAnim;
    private SpriteRenderer drillSpriteRend;

    public bool drillEnabled;
	public bool drillEnabledThisFrame;

    public EventReference drillSfx;
    FMOD.Studio.EventInstance drillSfxInstance;

    public EventReference drillEndSfx;
    FMOD.Studio.EventInstance drillEndSfxInstance;

    private void Start()
    {
        drillSfxInstance = FMODUnity.RuntimeManager.CreateInstance(drillSfx);
        drillEndSfxInstance = FMODUnity.RuntimeManager.CreateInstance(drillEndSfx);
        drillSpriteAnim = drill.GetComponentInChildren<SpriteAnim>();
        drillSpriteRend = drill.GetComponentInChildren<SpriteRenderer>();
        drillEnabled = false;
        drill.SetActive(drillEnabled);
    }

	public void DrillEndSfx()
	{
		drillEndSfxInstance.start();
	}

    public void DrillSfx()
    {
        drillSfxInstance.setPaused(false);
        drillSfxInstance.start();
    }


    public void drillUpdate(bool mousePressedDown, bool mousePressedHeld, bool playerDirectionLeft) {
		drillEnabledThisFrame = false;
		if (mousePressedDown) {
			DrillSfx();
			drillEnabled = true;
			drillEnabledThisFrame = true;
		} else if (!mousePressedHeld) {
			if (drillEnabled) {
				drillSfxInstance.setPaused(true);
				DrillEndSfx();
			}
			drillEnabled = false;
			drill.SetActive(drillEnabled);
		} else if (mousePressedHeld && !drillEnabled) {
            //Happens when drill is disabled due to scan or tele
            DrillSfx();
            drillEnabled = true;
            drillEnabledThisFrame = true;
        }
        if (drillEnabled) {
            drill.SetActive(drillEnabled);
            //rotation
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = 0f;

            Vector3 objectPos = Camera.main.WorldToScreenPoint(transform.position);
            mousePos.x = mousePos.x - objectPos.x;
            mousePos.y = mousePos.y - objectPos.y;

            float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
            
            if (playerDirectionLeft) {

                if (angle < 90 && angle >= 0) {
                    angle = 90f;
                } else if (angle > -90 && angle <= 0) {
                    angle = -90f;
                }

                drill.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle + 180));
            } else {
                if (angle > 90) {
                    angle = 90f;
                } else if (angle < -90) {
                    angle = -90f;
                }
                drill.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
            }
        }

        // Animations
        if (drillEnabled) {
            if (drillEnabledThisFrame) {

                if (drillSpriteAnim.Clip != m_drillstarting && drill.activeSelf) {
                    drillSpriteAnim.Play(m_drillstarting);
                }
            } else {
                if (drillSpriteAnim.Clip != m_drillloop && drill.activeSelf) {
                    drillSpriteAnim.Play(m_drillloop);
                }
            }
        }
    }

    private void OnDestroy()
    {
        drillSfxInstance.release();
        drillEndSfxInstance.release();
    }
}
