using FMODUnity;
using PowerTools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimAndSFX : MonoBehaviour
{
	public SpriteRenderer _spriteRend;
	public SpriteAnim _spriteAnim;

    public AnimationClip player_idle;
    public AnimationClip player_walk;
    public AnimationClip player_land;
    public AnimationClip player_fall;
    public AnimationClip player_jump;
    public AnimationClip player_death;
    public AnimationClip player_teleport;
    public AnimationClip player_scan;

	public EventReference footstepL;
	FMOD.Studio.EventInstance footstepLInstance;

	public EventReference footstepR;
	FMOD.Studio.EventInstance footstepRInstance;

	public EventReference deathSfx;
	FMOD.Studio.EventInstance deathSfxInstance;

	public EventReference scanSfx;
	FMOD.Studio.EventInstance scanSfxInstance;

	public EventReference drillSfx;
	FMOD.Studio.EventInstance drillSfxInstance;

	public EventReference drillEndSfx;
	FMOD.Studio.EventInstance drillEndSfxInstance;

	public EventReference teleportSfx;
	FMOD.Studio.EventInstance teleportSfxInstance;

	public void Start()
    {
        _spriteAnim = GetComponent<SpriteAnim>();
		_spriteRend = GetComponent<SpriteRenderer>();

		if (!teleportSfx.Equals(null) && !teleportSfx.Equals("")) {
			teleportSfxInstance = FMODUnity.RuntimeManager.CreateInstance(teleportSfx);
		}
		if (!footstepL.Equals(null) && !footstepL.Equals("")) {
			footstepLInstance = FMODUnity.RuntimeManager.CreateInstance(footstepL);
		}
		if (!footstepR.Equals(null) && !footstepR.Equals("")) {
			footstepRInstance = FMODUnity.RuntimeManager.CreateInstance(footstepR);
		}
		if (!deathSfx.Equals(null) && !deathSfx.Equals("")) {
			deathSfxInstance = FMODUnity.RuntimeManager.CreateInstance(deathSfx);
		}
		if (!scanSfx.Equals(null) && !scanSfx.Equals("")) {
			scanSfxInstance = FMODUnity.RuntimeManager.CreateInstance(scanSfx);
		}
		if (!drillSfx.Equals(null) && !drillSfx.Equals("")) {
			drillSfxInstance = FMODUnity.RuntimeManager.CreateInstance(drillSfx);
		}
		if (!drillEndSfx.Equals(null) && !drillEndSfx.Equals("")) {
			drillEndSfxInstance = FMODUnity.RuntimeManager.CreateInstance(drillEndSfx);
		}
	}

	public void OnDestroy()
	{
		footstepLInstance.release();
		footstepRInstance.release();
		deathSfxInstance.release();
		scanSfxInstance.release();
		drillSfxInstance.release();
		drillEndSfxInstance.release();
		teleportSfxInstance.release();
	}

	public void playAnimation(AnimationClip animationClip) {
        if (_spriteAnim.Clip != animationClip) {// (check we're not already in the animation first though)
            _spriteAnim.Play(animationClip);
        }

    }

	public void flipSprite(bool faceRight) {
		//Flip character if we need to
		if (faceRight == _spriteRend.flipX) {
			//Looking left moving right OR THE OPPOSITE
			_spriteRend.flipX = !_spriteRend.flipX;
		}
	}

	public void footStepL()
	{
		footstepLInstance.start();
	}

	public void footStepR()
	{
		footstepRInstance.start();
	}

	public void DeathSfx()
	{
		deathSfxInstance.start();
	}

	public void ScanSfx()
	{
		scanSfxInstance.start();
	}

	public void DrillSfx()
	{
		drillSfxInstance.setPaused(false);
		drillSfxInstance.start();
	}

	public void DrillEndSfx()
	{
		drillEndSfxInstance.start();
	}


	public void TeleportSfx()
	{
		teleportSfxInstance.start();
	}
}
