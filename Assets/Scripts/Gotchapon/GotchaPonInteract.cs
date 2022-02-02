using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PowerTools;

public class GotchaPonInteract : PlayerInteractable {

    public SpriteAnim _spriteAnim;

    public AnimationClip gotchapon_anim;
    public AnimationClip gotchapon_idle;

    void Awake()
    {
        _spriteAnim = GetComponent<SpriteAnim>();
    }

    override public void InteractWith()
    {
        //Play animation
        if (_spriteAnim.Clip != gotchapon_anim) {// (check we're not already in the animation first though)
            _spriteAnim.Play(gotchapon_anim);
            StartCoroutine(animationReturn());
        }
    }

    public override void UnInteract()
    {
        
    }

    override public void UnInteractWith() {
        

    }


    IEnumerator animationReturn()
    {
        yield return new WaitForSeconds(gotchapon_anim.length);
        //Play animation
        if (_spriteAnim.Clip != gotchapon_idle) {// (check we're not already in the animation first though)
            _spriteAnim.Play(gotchapon_idle);
            interactActive = false;
        }
    }
}
