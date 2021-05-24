using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPlacement : MonoBehaviour
{

    public GameObject bombPrefab;
    public GameObject artifactPrefab;


    public float bombspawnrate;
    public float artifactspawnrate;
    public float rateOf;

    public GameObject parent;

    // Start is called before the first frame update
    void Awake()
    {
        parent = this.transform.parent.gameObject;
        float bombrandomFloat = Random.Range(0, rateOf);
        float artifactrandomFloat = Random.Range(0, rateOf);
        if (bombspawnrate >= bombrandomFloat) {
            Instantiate(bombPrefab, this.transform.position, this.transform.rotation, parent.transform);
        }
        //Debug.Log(rateOf + " " + artifactspawnrate + " " + randomFloat + gameObject.name);
        else if (artifactspawnrate >= artifactrandomFloat) {
            //Debug.Log("SUC" + rateOf + " " + artifactspawnrate + " " + randomFloat);
            Instantiate(artifactPrefab, this.transform.position, this.transform.rotation, parent.transform);
        }

    }
}
