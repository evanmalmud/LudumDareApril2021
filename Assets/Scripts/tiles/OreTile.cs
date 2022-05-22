using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OreTile : TilePrefab {

    public List<OreScriptableObject> shallowOres;

    public List<OreScriptableObject> mediumOres;

    public List<OreScriptableObject> deepOres;

    public OreScriptableObject chosenOre;

    public EventReference commoncollectSfx;
    FMOD.Studio.EventInstance commoncollectSfxInstance;

    public EventReference rarecollectSfx;
    FMOD.Studio.EventInstance rarecollectSfxInstance;

    public EventReference legendarycollectSfx;
    FMOD.Studio.EventInstance legendarycollectSfxInstance;

    public override void OnEnable()
    {
        currentDamageUntilDestroyed = defaultDamageUntilDestroyed;

        GameState.DepthType depthType = GameState.depthCheck(this.transform.position.y + Random.Range(-5f, 5f));

        if (depthType == GameState.DepthType.DEEP && deepOres != null && deepOres.Count > 0) {
            chosenOre = deepOres[Random.Range(0, deepOres.Count)];
        } else if (depthType == GameState.DepthType.MEDIUM && mediumOres != null && mediumOres.Count > 0) {
            chosenOre = mediumOres[Random.Range(0, mediumOres.Count)];
        } else {
            chosenOre = shallowOres[Random.Range(0, shallowOres.Count)];
        }

        //Debug.Log(this.transform.position.y + " " + depthType + " " + chosenArtifact.name);
        //Chose random picture
        GetComponent<SpriteRenderer>().sprite = chosenOre.spriteOptions[Random.Range(0, chosenOre.spriteOptions.Count)];
    }

    public void CollectSfx(OreScriptableObject.OreType type)
    {
        if (type == OreScriptableObject.OreType.COMMON) {
            CommonCollectSfx();
        } else if (type == OreScriptableObject.OreType.RARE) {
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

    public override void takeDamageVirtual(float damage)
    {
        currentDamageUntilDestroyed -= damage;
        if (currentDamageUntilDestroyed <= 0) {
            CollectSfx(chosenOre.oreType);
            PlayerConfig playerConfig = FindObjectOfType<PlayerConfig>();
            playerConfig.collectOre(chosenOre);
            Destroy(this.gameObject);
        }
    }

    public override void destroyVirtual()
    {
        Destroy(this.gameObject);
    }
}
