using FMODUnity;
using PowerTools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilePrefab : MonoBehaviour
{

    public float defaultDamageUntilDestroyed = 100f;
    public float currentDamageUntilDestroyed;
    public List<Sprite> possibleSprites;

    public List<Sprite> shallowSprites;

    public List<Sprite> mediumSprites;

    public List<Sprite> deepSprites;


    public GameObject shatterAnim;
    public SpriteAnim spriteAnim;
    public SpriteRenderer spriteRend;

    public GameObject shatterAnim2;
    public SpriteAnim spriteAnim2;
    public SpriteRenderer spriteRend2;


    public AnimationClip m_shatterShallow = null;
    public AnimationClip m_breakShallow = null;
    public AnimationClip m_shatterMedium = null;
    public AnimationClip m_breakMedium =  null;
    public AnimationClip m_shatterDeep = null;
    public AnimationClip m_breakDeep = null;

    public GameState.DepthType depthType;

    public EventReference tileDestroySfx;
    FMOD.Studio.EventInstance tileDestroySfxInstance;

    BoxCollider2D boxCollider;


    public enum TileTypes {
        SINGLE,
        TWOXONE,
        TWOXTWO,
        BOMB,
        ARTIFACT
    }

    public TileTypes tileType;

    public void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }

    public virtual void OnEnable()
    {

        if (shatterAnim != null) {
            shatterAnim.SetActive(false);
        }
        if (shatterAnim2 != null) {
            shatterAnim2.SetActive(false);
        }

        boxCollider.enabled = true;
        currentDamageUntilDestroyed = defaultDamageUntilDestroyed;

        depthType = GameState.depthCheck(this.transform.position.y + Random.Range(-5f, 5f));
        //Debug.Log(this.transform.position.y + " " + depthType);

        if (depthType == GameState.DepthType.DEEP && deepSprites != null && deepSprites.Count > 0) {
            GetComponent<SpriteRenderer>().sprite = deepSprites[Random.Range(0, deepSprites.Count)];
        } else if (depthType == GameState.DepthType.MEDIUM && mediumSprites != null && mediumSprites.Count > 0) {
            GetComponent<SpriteRenderer>().sprite = mediumSprites[Random.Range(0, mediumSprites.Count)];
        } else if (depthType == GameState.DepthType.SHALLOW && shallowSprites != null && shallowSprites.Count > 0) {
            GetComponent<SpriteRenderer>().sprite = shallowSprites[Random.Range(0, shallowSprites.Count)];
        } else if (possibleSprites != null && possibleSprites.Count > 0) {
            GetComponent<SpriteRenderer>().sprite = possibleSprites[Random.Range(0, possibleSprites.Count)];
        }
    }

    public virtual void OnDestroy()
    {
        tileDestroySfxInstance.release();
    }

    public void takeDamage(float damage)
    {
        takeDamageVirtual(damage);
    }

    public virtual void takeDamageVirtual(float damage) {
        if (shatterAnim != null && spriteAnim != null) {
            shatterAnim.SetActive(true);
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
        if (shatterAnim2 != null && spriteAnim != null) {
            shatterAnim2.SetActive(true);
            if (depthType == GameState.DepthType.DEEP) {
                if (spriteAnim2.Clip != m_shatterDeep) {
                    spriteAnim2.Play(m_shatterDeep);
                }
            } else if (depthType == GameState.DepthType.MEDIUM) {
                if (spriteAnim2.Clip != m_shatterMedium) {
                    spriteAnim2.Play(m_shatterMedium);
                }
            } else {
                if (spriteAnim2.Clip != m_shatterShallow) {
                    spriteAnim2.Play(m_shatterShallow);
                }
            }
        }

        currentDamageUntilDestroyed -= damage;
        if(currentDamageUntilDestroyed <= 0) {
            destroy();
        }
    }

    public virtual void destroyVirtual()
    {
        StartCoroutine("playBreakAndDestroy");
    }

    public void destroy() {
        destroyVirtual();
    }

    IEnumerator playBreakAndDestroy() {
        if (!tileDestroySfx.Equals(null) && !tileDestroySfx.Equals("") && !tileDestroySfxInstance.isValid()) {
            tileDestroySfxInstance = FMODUnity.RuntimeManager.CreateInstance(tileDestroySfx);
            tileDestroySfxInstance.start();
        } else if (tileDestroySfxInstance.isValid()) {
            tileDestroySfxInstance.start();
        }
        boxCollider.enabled = false;
        if (shatterAnim != null && spriteAnim != null) {
            shatterAnim.SetActive(true);
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
        if (shatterAnim2 != null && spriteAnim != null) {
            shatterAnim2.SetActive(true);
            GetComponent<SpriteRenderer>().sprite = null;
            if (depthType == GameState.DepthType.DEEP) {
                if (spriteAnim2.Clip != m_breakDeep) {
                    spriteAnim2.Play(m_breakDeep);
                }
            } else if (depthType == GameState.DepthType.MEDIUM) {
                if (spriteAnim2.Clip != m_breakMedium) {
                    spriteAnim2.Play(m_breakMedium);
                }
            } else {
                if (spriteAnim2.Clip != m_breakShallow) {
                    spriteAnim2.Play(m_breakShallow);
                }
            }
        }
        yield return new WaitForSeconds(1f);
        this.gameObject.SetActive(false);
    }
}
