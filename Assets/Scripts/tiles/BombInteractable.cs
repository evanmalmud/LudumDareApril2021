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

    public ScreenShake cameraShake;

    public bool bombTriggered = false;

    public override void Start()
    {
        cameraShake = Camera.main.GetComponent<ScreenShake>();
        base.Start();
        m_anim = m_anim = GetComponent<SpriteAnim>();
        if (m_anim.Clip != m_blinking) { // (check we're not already in the animation first though)
            m_anim.Play(m_blinking);

        }
    }
    public override void ScanHit()
    {
        if(bombTriggered) {
            return;
        }
        bombTriggered = true;
        base.ScanHit();
        coroutine = WaitAndExplode(timeUntilExplosion);
        StartCoroutine(coroutine);
    }

    public void OnDestroy()
    {
        Debug.Log("Bomb On Destroy");
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
            BombTilePrefab bombTilePrefab;
            if (hit.TryGetComponent<BombTilePrefab>(out bombTilePrefab)) {
                continue;
            }
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
            Player player;
            if (hit.TryGetComponent<Player>(out player)) {
                player.takeDamage();
                continue;
            }
        }
        if (m_anim.Clip != m_explosion) { // (check we're not already in the animation first though)
            m_anim.Play(m_explosion);
        }
        yield return new WaitForSeconds(.4f);
        light2d.enabled = false;
        bombBlinkingInstance.setPaused(true);
        bombBlinkingInstance.release();
        bombExplosionSfxInstance.setPaused(true);
        bombExplosionSfxInstance.release();
        Destroy(this.gameObject);
        //print("Coroutine ended: " + Time.time + " seconds");
    }

    public void BombBlinkingSfx()
    {
        if (!bombBlinkingSfx.Equals(null) && !bombBlinkingSfx.Equals("")) {
            bombBlinkingInstance.release();
            bombBlinkingInstance = FMODUnity.RuntimeManager.CreateInstance(bombBlinkingSfx);
            bombBlinkingInstance.start();
        } 
    }

    public void BombExplosionSfx()
    {   //Called from animation
        cameraShake.ShakeScreenDefault();
        if (!bombExplosionSfx.Equals(null) && !bombExplosionSfx.Equals("")) {
            bombExplosionSfxInstance.release();
            bombExplosionSfxInstance = FMODUnity.RuntimeManager.CreateInstance(bombExplosionSfx);
            bombExplosionSfxInstance.start();
        }
    }

}
