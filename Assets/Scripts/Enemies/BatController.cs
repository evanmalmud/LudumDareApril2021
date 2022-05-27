using DG.Tweening;
using PowerTools;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatController : MonoBehaviour, EnemyController
{

    public SpriteAnim spriteAnim;
    public SpriteRenderer spriteRend;
    public Rigidbody2D rb;
    public Collider2D collider2d;

    public AnimationClip m_death;
    public AnimationClip m_dive;
    public AnimationClip m_hangIdle;
    public AnimationClip m_hover;
    public AnimationClip m_shriek;
    public AnimationClip m_takeOff;

    public enum BAT_STATE
    {
        IDLE, //Perched
        IDLE_TO_ATTACK,
        ATTACK_DIVE,
        ATTACK_HOVER,
        ATTACK_SHRIEK,
        DEATH
    }

    public BAT_STATE state = BAT_STATE.IDLE;

    public bool newState = false;

    public Transform target;

    public Vector2 timeToMoveAfterPassingPlayer;

    public Vector2 attackSpeed;

    public float hoverMoveSpeed = 2f;

    public Ease _attackYEase = Ease.InExpo;

    public bool followplayer = false;

    public float heightToDoanotherAttack = 2f;

    public Vector2 timeBetweenAttacks;

    public bool spriteXFlip = false;
    public float spriteFlipLeeway = .5f;

    public LayerMask collisionMask;


    // Start is called before the first frame update
    void Start()
    {
        spriteAnim = GetComponent<SpriteAnim>();
        spriteRend = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        collider2d = GetComponent<CircleCollider2D>();

        spriteXFlip = UnityEngine.Random.value > 0.5f;
        spriteRend.flipX = spriteXFlip;
    }

    // Update is called once per frame
    void Update()
    {
        if(newState)
        {
            //Swap state
            swapState();
            newState = false;
        }

        if(followplayer)
        {
            Vector2 distance = target.transform.position - transform.position;
            Vector2 newPos = transform.position;
            newPos += distance.normalized * Time.deltaTime * hoverMoveSpeed;
            transform.position = newPos;
        }

        if(target != null)
        {
            // Only flip if greater than leeway distance away from target
            if(Math.Abs(target.transform.position.x - transform.position.x) > spriteFlipLeeway)
            {
                if (target.transform.position.x >= transform.position.x)
                {
                    spriteXFlip = true;
                }
                else
                {
                    spriteXFlip = false;
                }
                spriteRend.flipX = spriteXFlip;
            }
        }
    }

    public void setTarget(Transform transform)
    {
        target = transform;
    }

    public void enableAttackState()
    {
        state = BAT_STATE.IDLE_TO_ATTACK;
    }

    void swapState()
    {
        switch (state)
        {
            case BAT_STATE.IDLE_TO_ATTACK:
                StartCoroutine(PlayAnimationThen(m_takeOff,
                    () => setState(BAT_STATE.ATTACK_DIVE)));
                //Add a fall during this time then right into attack.
                break;

            case BAT_STATE.ATTACK_DIVE:
                //Debug.Log("currentPos - " + transform.position);
                //Debug.Log("currentTARGETPos - " + target.transform.position);
                float finalXPos;
                if (transform.position.x < target.transform.position.x)
                {
                    finalXPos = target.transform.position.x + (attackSpeed.x * timeToMoveAfterPassingPlayer.x);
                } else
                {
                    finalXPos = target.transform.position.x - (attackSpeed.x * timeToMoveAfterPassingPlayer.x);
                }
                float finalYPos = target.transform.position.y - (attackSpeed.y * timeToMoveAfterPassingPlayer.y);

                float timeToMoveFor = Mathf.Max(Mathf.Abs((finalXPos - transform.position.x) / attackSpeed.x),
                    Mathf.Abs((finalYPos - transform.position.y) / attackSpeed.y));
                Vector2 endLocation = new Vector2(finalXPos, finalYPos);
                transform.DOMoveY(endLocation.y, timeToMoveFor).SetEase(_attackYEase);
                transform.DOMoveX(endLocation.x, timeToMoveFor);
                StartCoroutine(PlayLoopAnimationThen(m_dive, timeToMoveFor / 2,
                   () =>
                   StartCoroutine(PlayLoopAnimationThen(m_hover, timeToMoveFor / 2,
                   () => setState(BAT_STATE.ATTACK_HOVER)))
                ));
                break;

            case BAT_STATE.ATTACK_HOVER:
                followplayer = true;
                float count = UnityEngine.Random.Range(timeBetweenAttacks.x, timeBetweenAttacks.y);
                //Debug.Log("Hover check - " + transform.position.y + " " +  target.transform.position.y);
                StartCoroutine(PlayLoopAnimationThen(m_hover, count,
                  () => {
                      if (transform.position.y - target.transform.position.y >= heightToDoanotherAttack)
                      {
                          setState(BAT_STATE.ATTACK_DIVE);
                          followplayer = false;
                      }
                      else
                      {
                          setState(BAT_STATE.ATTACK_HOVER);
                      }
                  }));
                break;

            case BAT_STATE.ATTACK_SHRIEK:
                break;

            case BAT_STATE.DEATH:
                collider2d.enabled = false;
                StartCoroutine(PlayAnimationThen(m_death, () =>
                {
                    this.gameObject.SetActive(false);
                }));
                break;
            default:
                break;
        }
    }



    IEnumerator PlayLoopAnimationThen(AnimationClip clip, float time, System.Action onCompleteMethod)
    {
        if (spriteAnim.Clip != clip || !spriteAnim.IsPlaying())
        {
            spriteAnim.Play(clip);
        }
        yield return new WaitForSeconds(time);
        if (onCompleteMethod != null)
        {
            onCompleteMethod();
        }
    }


    IEnumerator PlayAnimationThen(AnimationClip clip, System.Action onCompleteMethod)
    {

        if (spriteAnim.Clip != clip || !spriteAnim.IsPlaying())
        {
            spriteAnim.Play(clip);
        }
        yield return new WaitForSeconds(clip.length);
        if(onCompleteMethod != null)
        {
            onCompleteMethod();
        }
    }

    
    private void setState(BAT_STATE state)
    {
        newState = true;
        this.state = state;
    }

    public void ActivateEnemy(Transform target)
    {
        setTarget(target);
        setState(BAT_STATE.IDLE_TO_ATTACK);
    }

    public void TakeDamage(float amount)
    {
        StopAllCoroutines();
        transform.DOKill();
        setState(BAT_STATE.DEATH);
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log("collision BAT " + collision.gameObject);
        if (collisionMask == (collisionMask | (1 << collision.gameObject.layer)))
        {
            var player = collision.gameObject.GetComponent<PlayerConfig>();
            if (player != null)
            {
                player.takeDamage();
            }
        }
    }
}
