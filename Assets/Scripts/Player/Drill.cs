using PowerTools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drill : MonoBehaviour
{
    public AnimationClip m_drillstarting = null;
    public AnimationClip m_drillloop = null;

    public GameObject drillL;
    private SpriteAnim drillLSpriteAnim;
    private SpriteRenderer drillLSpriteRend;
    public GameObject drillR;
    private SpriteAnim drillRSpriteAnim;
    private SpriteRenderer drillRSpriteRend;

    public bool drillEnabled;
	public bool drillEnabledThisFrame;

	[FMODUnity.EventRef]
    public string drillSfx = "";
    public FMOD.Studio.EventInstance drillSfxInstance;

    [FMODUnity.EventRef]
    public string drillEndSfx = "";
    public FMOD.Studio.EventInstance drillEndSfxInstance;

    private void Start()
    {
        drillLSpriteAnim = drillL.GetComponentInChildren<SpriteAnim>();
        drillLSpriteRend = drillL.GetComponentInChildren<SpriteRenderer>();
        drillRSpriteAnim = drillR.GetComponentInChildren<SpriteAnim>();
        drillRSpriteRend = drillR.GetComponentInChildren<SpriteRenderer>();
        drillEnabled = false;
        drillL.SetActive(drillEnabled);
        drillR.SetActive(drillEnabled);
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


    public void drillUpdate(bool mousePressedDown, bool mousePressedHeld, bool playerDirection) {
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
			drillL.SetActive(drillEnabled);
			drillR.SetActive(drillEnabled);
		}
        if (drillEnabled) {

            //rotation
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = 0f;

            Vector3 objectPos = Camera.main.WorldToScreenPoint(transform.position);
            mousePos.x = mousePos.x - objectPos.x;
            mousePos.y = mousePos.y - objectPos.y;

            float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
            
            if (playerDirection) {
                //Facing Left
                drillL.SetActive(drillEnabled);
                drillR.SetActive(!drillEnabled);
                if (angle < 90 && angle >= 0) {
                    angle = 90f;
                } else if (angle > -90 && angle <= 0) {
                    angle = -90f;
                }

                drillL.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle + 180));
            } else {
                drillL.SetActive(!drillEnabled);
                drillR.SetActive(drillEnabled);
                if (angle > 90) {
                    angle = 90f;
                } else if (angle < -90) {
                    angle = -90f;
                }
                drillR.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
            }
        }

        // Animations
        if (drillEnabled) {
            if (drillEnabledThisFrame) {

                if (drillRSpriteAnim.Clip != m_drillstarting && drillR.activeSelf) {
                    drillRSpriteAnim.Play(m_drillstarting);
                }
                if (drillLSpriteAnim.Clip != m_drillstarting && drillL.activeSelf) {
                    drillLSpriteAnim.Play(m_drillstarting);
                }
            } else {
                if (drillRSpriteAnim.Clip != m_drillloop && drillR.activeSelf) {
                    drillRSpriteAnim.Play(m_drillloop);
                }
                if (drillLSpriteAnim.Clip != m_drillloop && drillL.activeSelf) {
                    drillLSpriteAnim.Play(m_drillloop);
                }
            }
        }
    }
}
