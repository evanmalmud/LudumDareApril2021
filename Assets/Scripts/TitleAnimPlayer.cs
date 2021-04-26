using PowerTools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleAnimPlayer : MonoBehaviour
{

    SpriteRenderer spriteRend;
    SpriteAnim m_anim = null;
    public AnimationClip m_loadTitle = null;

    // Start is called before the first frame update
    void Awake()
    {
        m_anim = GetComponent<SpriteAnim>();
        spriteRend = GetComponent<SpriteRenderer>();
    }

    public void playAnim() {
        if (m_anim.Clip != m_loadTitle) {// (check we're not already in the animation first though)
            m_anim.Play(m_loadTitle);
        }
    }
}
