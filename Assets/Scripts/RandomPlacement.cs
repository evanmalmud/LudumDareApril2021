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
    void Start()
    {
        parent = this.transform.parent.gameObject;
        float bombrandomFloat = Random.Range(0, rateOf);
        float artifactrandomFloat = Random.Range(0, rateOf);
        if (bombspawnrate >= bombrandomFloat) {
            GameObject obj = Instantiate(bombPrefab, parent.transform, false);
            obj.transform.position = this.transform.position;
        }
        //Debug.Log(rateOf + " " + artifactspawnrate + " " + randomFloat + gameObject.name);
        else if (artifactspawnrate >= artifactrandomFloat) {
            //Debug.Log("SUC" + rateOf + " " + artifactspawnrate + " " + randomFloat);
            GameObject obj = Instantiate(artifactPrefab, parent.transform, false);
            obj.transform.position = this.transform.position;
        }

    }
}
