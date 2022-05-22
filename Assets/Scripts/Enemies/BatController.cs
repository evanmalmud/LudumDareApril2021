using PowerTools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatController : MonoBehaviour
{

    public SpriteAnim spriteAnim;
    public SpriteRenderer spriteRend;

    public AnimationClip m_death;
    public AnimationClip m_dive;
    public AnimationClip m_hangIdle;
    public AnimationClip m_hover;
    public AnimationClip m_shriek;
    public AnimationClip m_takeOff;


    // Start is called before the first frame update
    void Start()
    {
        spriteAnim = GetComponent<SpriteAnim>();
        spriteRend = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
