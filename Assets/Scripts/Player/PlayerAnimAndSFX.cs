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

	[FMODUnity.EventRef]
	public string footstepL = "";
	[FMODUnity.EventRef]
	public string footstepR = "";
	FMOD.Studio.EventInstance footstepLInstance;
	FMOD.Studio.EventInstance footstepRInstance;

	[FMODUnity.EventRef]
	public string deathSfx = "";
	public FMOD.Studio.EventInstance deathSfxInstance;

	[FMODUnity.EventRef]
	public string scanSfx = "";
	public FMOD.Studio.EventInstance scanSfxInstance;

	[FMODUnity.EventRef]
	public string drillSfx = "";
	public FMOD.Studio.EventInstance drillSfxInstance;

	[FMODUnity.EventRef]
	public string drillEndSfx = "";
	public FMOD.Studio.EventInstance drillEndSfxInstance;

	[FMODUnity.EventRef]
	public string teleportSfx = "";
	public FMOD.Studio.EventInstance teleportSfxInstance;

	public void Start()
    {
        _spriteAnim = GetComponent<SpriteAnim>();
		_spriteRend = GetComponent<SpriteRenderer>();
	}
    public void playAnimation(AnimationClip animationClip) {
        if (_spriteAnim.Clip != animationClip) {// (check we're not already in the animation first though)
            _spriteAnim.Play(animationClip);
        }

    }

	public void flipSprite(Vector2 velocity) {
		//Flip character if we need to
		if ((_spriteRend.flipX && velocity.x > 0) || (!_spriteRend.flipX && velocity.x < 0)) {
			//Looking left moving right OR THE OPPOSITE
			_spriteRend.flipX = !_spriteRend.flipX;
		}
	}
}
