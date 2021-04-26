using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class ArtifactTile : TilePrefab
{

    public List<ArtifactScriptableObject> possibleArtifacts;

    public List<ArtifactScriptableObject> shallowArtifacts;

    public List<ArtifactScriptableObject> mediumArtifacts;

    public List<ArtifactScriptableObject> deepArtifacts;

    public ArtifactScriptableObject chosenArtifact;
    public override void Start()
    {
        GameState.DepthType depthType = GameState.depthCheck(this.transform.position.y + Random.Range(-5f, 5f));
        //Debug.Log(this.transform.position.y + " " + depthType);

        if (depthType == GameState.DepthType.DEEP && deepArtifacts != null && deepArtifacts.Count > 0) {
            chosenArtifact = deepArtifacts[Random.Range(0, deepArtifacts.Count)];
        } else if (depthType == GameState.DepthType.MEDIUM && mediumArtifacts != null && mediumArtifacts.Count > 0) {
            chosenArtifact = mediumArtifacts[Random.Range(0, mediumArtifacts.Count)];
        } else if (depthType == GameState.DepthType.SHALLOW && shallowSprites != null && shallowSprites.Count > 0) {
            chosenArtifact = shallowArtifacts[Random.Range(0, shallowArtifacts.Count)];
        } else if (possibleArtifacts != null && possibleArtifacts.Count > 0) {
            chosenArtifact = possibleArtifacts[Random.Range(0, possibleArtifacts.Count)];
        }

        GetComponent<SpriteRenderer>().sprite = chosenArtifact.dirtySprite;
    }

    public override void takeDamage(float damage)
    {
        damageUntilDestroyed -= damage;
        if (damageUntilDestroyed <= 0) {
            FindObjectOfType<Player>().collectedArtifacts.Add(chosenArtifact);
            Destroy(this.gameObject);
        }
    }

}
