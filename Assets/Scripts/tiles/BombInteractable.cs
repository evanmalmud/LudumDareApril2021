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

    [FMODUnity.EventRef]
    public string bombBlinkingSfx = "";
    FMOD.Studio.EventInstance bombBlinkingInstance;

    [FMODUnity.EventRef]
    public string bombExplosionSfx = "";
    FMOD.Studio.EventInstance bombExplosionSfxInstance;

    public override void Start()
    {
        base.Start();
        m_anim = m_anim = GetComponent<SpriteAnim>();
        if (m_anim.Clip != m_blinking) { // (check we're not already in the animation first though)
            m_anim.Play(m_blinking);

        }
    }
    public override void ScanHit()
    {
        base.ScanHit();
        coroutine = WaitAndExplode(timeUntilExplosion);
        StartCoroutine(coroutine);
    }

    public void OnDestroy()
    {
        bombBlinkingInstance.setPaused(true);
        bombBlinkingInstance.release();
        bombExplosionSfxInstance.setPaused(true);
        bombExplosionSfxInstance.release();
    }

    private IEnumerator WaitAndExplode(float waitTime)
    {
        BombBlinkingSfx();
        yield return new WaitForSeconds(waitTime);
        Collider2D[] hitCollider = Physics2D.OverlapCircleAll(this.transform.position, bombExpRadius, collisionMask);
        foreach (Collider2D hit in hitCollider) {
            TilePrefab tile;
            if (hit.TryGetComponent<TilePrefab>(out tile)) {
                tile.destroy();
                continue;
            }
            ArtifactTile artifact;
            if (hit.TryGetComponent<ArtifactTile>(out artifact)) {
                artifact.destroy();
                continue;
            }
            Interactable interact;
            if (hit.TryGetComponent<Interactable>(out interact)) {
                Destroy(interact.gameObject);
                continue;
            }
            Player player;
            if (hit.TryGetComponent<Player>(out player)) {
                player.takeDamage();
                continue;
            }
        }
        light2d.enabled = false;
        if (m_anim.Clip != m_explosion) { // (check we're not already in the animation first though)
            m_anim.Play(m_explosion);
        }
        yield return new WaitForSeconds(1.5f);
        Destroy(this.gameObject);
        print("Coroutine ended: " + Time.time + " seconds");
    }

    public void BombBlinkingSfx()
    {
        if (!bombBlinkingSfx.Equals(null) && !bombBlinkingSfx.Equals("") && !bombBlinkingInstance.isValid()) {
            bombBlinkingInstance = FMODUnity.RuntimeManager.CreateInstance(bombBlinkingSfx);
            bombBlinkingInstance.start();
        } else if (bombBlinkingInstance.isValid()) {
            bombBlinkingInstance.start();
        }
    }

    public void BombExplosionSfx()
    {
        if (!bombExplosionSfx.Equals(null) && !bombExplosionSfx.Equals("") && !bombExplosionSfxInstance.isValid()) {
            bombExplosionSfxInstance = FMODUnity.RuntimeManager.CreateInstance(bombExplosionSfx);
            bombExplosionSfxInstance.start();
        } else if (bombExplosionSfxInstance.isValid()) {
            bombExplosionSfxInstance.start();
        }
    }

}
