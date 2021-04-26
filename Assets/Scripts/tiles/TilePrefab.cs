using PowerTools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameState;

public class TilePrefab : MonoBehaviour
{

    public float damageUntilDestroyed = 100f;

    public List<Sprite> possibleSprites;

    public List<Sprite> shallowSprites;

    public List<Sprite> mediumSprites;

    public List<Sprite> deepSprites;


    public GameObject shatterAnim;
    public SpriteAnim spriteAnim;
    public SpriteRenderer spriteRend;
    public AnimationClip m_shatterShallow = null;
    public AnimationClip m_breakShallow = null;
    public AnimationClip m_shatterMedium = null;
    public AnimationClip m_breakMedium =  null;
    public AnimationClip m_shatterDeep = null;
    public AnimationClip m_breakDeep = null;

    public GameState.DepthType depthType;

    [FMODUnity.EventRef]
    public string tileDestroySfx = "";
    FMOD.Studio.EventInstance tileDestroySfxInstance;

    // Start is called before the first frame update
    public virtual void Start()
    {
        depthType = GameState.depthCheck(this.transform.position.y + Random.Range(-5f, 5f));
        //Debug.Log(this.transform.position.y + " " + depthType);

        if (depthType == GameState.DepthType.DEEP && deepSprites != null && deepSprites.Count > 0) {
            GetComponent<SpriteRenderer>().sprite = deepSprites[Random.Range(0, deepSprites.Count)];
        } else if (depthType == GameState.DepthType.MEDIUM && mediumSprites != null && mediumSprites.Count > 0) {
            GetComponent<SpriteRenderer>().sprite = mediumSprites[Random.Range(0, mediumSprites.Count)];
        } else if (depthType == GameState.DepthType.SHALLOW && shallowSprites != null && shallowSprites.Count > 0) {
            GetComponent<SpriteRenderer>().sprite = shallowSprites[Random.Range(0, shallowSprites.Count)];
        } else if (possibleSprites != null && possibleSprites.Count > 0){
            GetComponent<SpriteRenderer>().sprite = possibleSprites[Random.Range(0, possibleSprites.Count)];
        }
    }


    public virtual void takeDamage(float damage) {
        if (shatterAnim != null) {
            if (depthType == GameState.DepthType.DEEP) {
                if (spriteAnim.Clip != m_shatterDeep) {
                    spriteAnim.Play(m_shatterDeep);
                }
            } else if (depthType == GameState.DepthType.MEDIUM) {
                if (spriteAnim.Clip != m_shatterMedium) {
                    spriteAnim.Play(m_shatterMedium);
                }
            } else {
                if (spriteAnim.Clip != m_shatterShallow) {
                    spriteAnim.Play(m_shatterShallow);
                }
            }
        }
        damageUntilDestroyed -= damage;
        if(damageUntilDestroyed <= 0) {

            destroy();
        }
    }

    public void destroy()
    {
        StartCoroutine("playBreakAndDestroy");
    }

    IEnumerator playBreakAndDestroy() {
        if (!tileDestroySfx.Equals(null) && !tileDestroySfx.Equals("")) {
            tileDestroySfxInstance = FMODUnity.RuntimeManager.CreateInstance(tileDestroySfx);
            tileDestroySfxInstance.start();
        }
        GetComponent<BoxCollider2D>().enabled = false;
        if (shatterAnim != null) {
            GetComponent<SpriteRenderer>().sprite = null;
            if (depthType == GameState.DepthType.DEEP) {
                if (spriteAnim.Clip != m_breakDeep) {
                    spriteAnim.Play(m_breakDeep);
                }
            } else if (depthType == GameState.DepthType.MEDIUM) {
                if (spriteAnim.Clip != m_breakMedium) {
                    spriteAnim.Play(m_breakMedium);
                }
            } else {
                if (spriteAnim.Clip != m_breakShallow) {
                    spriteAnim.Play(m_breakShallow);
                }
            }
        }
        yield return new WaitForSeconds(1f);
        Destroy(this.gameObject);
    }
}