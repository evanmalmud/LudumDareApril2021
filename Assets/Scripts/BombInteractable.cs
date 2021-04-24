using PowerTools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombInteractable : Interactable
{
    //37 x 36 sprite
    public float timeUntilExplosion = 3f;

    public float bombExpRadius = 10f;

    public LayerMask collisionMask;

    public AnimationClip m_blinking = null;
    public AnimationClip m_explosion = null;

    private IEnumerator coroutine;
    SpriteAnim m_anim = null;

    public void Start()
    {
        base.Start();
        m_anim = m_anim = GetComponent<SpriteAnim>();
        if (m_anim.Clip != m_blinking) { // (check we're not already in the animation first though)
            m_anim.Play(m_blinking);
        }
    }
    public override void ScanHit()
    {
        Debug.Log("bomb scan hit");
        base.ScanHit();
        coroutine = WaitAndExplode(timeUntilExplosion);
        StartCoroutine(coroutine);
    }

    private IEnumerator WaitAndExplode(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        Collider2D[] hitCollider = Physics2D.OverlapCircleAll(this.transform.position, bombExpRadius, collisionMask);
        foreach (Collider2D hit in hitCollider) {
            TilePrefab interact;
            if (hit.TryGetComponent<TilePrefab>(out interact)) {
                Destroy(interact.gameObject);
            }
            Player player;
            if (hit.TryGetComponent<Player>(out player)) {
                player.takeDamage();
            }
        }
        light2d.enabled = false;
        if (m_anim.Clip != m_explosion) { // (check we're not already in the animation first though)
            m_anim.Play(m_explosion);
        }
        print("Coroutine ended: " + Time.time + " seconds");
    }

}
