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


    [FMODUnity.EventRef]
    public string commoncollectSfx = "";
    FMOD.Studio.EventInstance commoncollectSfxInstance;

    [FMODUnity.EventRef]
    public string rarecollectSfx = "";
    FMOD.Studio.EventInstance rarecollectSfxInstance;

    [FMODUnity.EventRef]
    public string legendarycollectSfx = "";
    FMOD.Studio.EventInstance legendarycollectSfxInstance;


    public override void Start()
    {
        GameState.DepthType depthType = GameState.depthCheck(this.transform.position.y + Random.Range(-5f, 5f));

        if (depthType == GameState.DepthType.DEEP && deepArtifacts != null && deepArtifacts.Count > 0) {
            chosenArtifact = deepArtifacts[Random.Range(0, deepArtifacts.Count)];
        } else if (depthType == GameState.DepthType.MEDIUM && mediumArtifacts != null && mediumArtifacts.Count > 0) {
            chosenArtifact = mediumArtifacts[Random.Range(0, mediumArtifacts.Count)];
        } else {
            chosenArtifact = shallowArtifacts[Random.Range(0, shallowArtifacts.Count)];
        }

        //Debug.Log(this.transform.position.y + " " + depthType + " " + chosenArtifact.name);
        GetComponent<SpriteRenderer>().sprite = chosenArtifact.dirtySprite;
    }
    
    public void CollectSfx(ArtifactScriptableObject.ArtifactType type) {
        if (type == ArtifactScriptableObject.ArtifactType.COMMON) {
            CommonCollectSfx();
        } else if (type == ArtifactScriptableObject.ArtifactType.RARE) {
            RareCollectSfx();
        } else {
            LegendaryCollectSfx();
        }
    }

    public void CommonCollectSfx()
    {
        if (!commoncollectSfx.Equals(null) && !commoncollectSfx.Equals("") && !commoncollectSfxInstance.isValid()) {
            commoncollectSfxInstance = FMODUnity.RuntimeManager.CreateInstance(commoncollectSfx);
            commoncollectSfxInstance.start();
        } else if (commoncollectSfxInstance.isValid()) {
            commoncollectSfxInstance.start();
        }
    }

    public void RareCollectSfx()
    {
        if (!rarecollectSfx.Equals(null) && !rarecollectSfx.Equals("") && !rarecollectSfxInstance.isValid()) {
            rarecollectSfxInstance = FMODUnity.RuntimeManager.CreateInstance(rarecollectSfx);
            rarecollectSfxInstance.start();
        } else if (rarecollectSfxInstance.isValid()) {
            rarecollectSfxInstance.start();
        }
    }

    public void LegendaryCollectSfx()
    {
        if (!legendarycollectSfx.Equals(null) && !legendarycollectSfx.Equals("") && !legendarycollectSfxInstance.isValid()) {
            legendarycollectSfxInstance = FMODUnity.RuntimeManager.CreateInstance(legendarycollectSfx);
            legendarycollectSfxInstance.start();
        } else if (legendarycollectSfxInstance.isValid()) {
            legendarycollectSfxInstance.start();
        }
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        commoncollectSfxInstance.release();
        rarecollectSfxInstance.release();
        legendarycollectSfxInstance.release();
    }

    public override void takeDamage(float damage)
    {
        damageUntilDestroyed -= damage;
        if (damageUntilDestroyed <= 0) {
            CollectSfx(chosenArtifact.artifactType);
            Player player = FindObjectOfType<Player>();
            player.collectedArtifacts.Add(chosenArtifact);
            player.collectAnim();
            Destroy(this.gameObject);
        }
    }

}
