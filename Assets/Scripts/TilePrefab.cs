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

    // Start is called before the first frame update
    void Start()
    {
        GameState.DepthType depthType = GameState.depthCheck(this.transform.position.y + Random.Range(-5f, 5f));
        Debug.Log(this.transform.position.y + " " + depthType);

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

    // Update is called once per frame
    void Update()
    {
        
    }

    public void takeDamage(float damage) {
        damageUntilDestroyed -= damage;
        if(damageUntilDestroyed <= 0) {
            Destroy(this.gameObject);
        }
    }
}
