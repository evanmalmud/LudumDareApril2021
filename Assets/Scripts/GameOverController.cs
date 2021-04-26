using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverController : MonoBehaviour
{
    public Sprite gameOverSprite;
    public Sprite recalledSprite;

    public GameObject commonItems, rareItems, legendaryItems;

    public GameObject accidentCard;

    public SpriteRenderer spriteRend;

    public Player player;

    public List<GameObject> commonItemPos;
    public List<GameObject> rareItemsPos;
    public List<GameObject> legendaryItemsPos;
    public int itemLimit = 10;

    public int commonIndex = 0;
    public int rareIndex = 0;
    public int legendaryIndex = 0;

    public TMPro.TextMeshProUGUI scoreText;
    public int score = 0;

    public void onDisplay(bool isGameOver) {

        resetDisplay();
        if (isGameOver) {
            spriteRend.sprite = gameOverSprite;
            accidentCard.SetActive(true);
        } else {
            spriteRend.sprite = recalledSprite;
        }
        
        foreach(ArtifactScriptableObject obj in player.collectedArtifacts) {
            //Debug.Log(obj);
            if(obj.artifactType == ArtifactScriptableObject.ArtifactType.LEGENDARY ||
                obj.artifactType == ArtifactScriptableObject.ArtifactType.PERSONAL) {
                //Debug.Log("legendary " + obj.cleanSprite.name);
                score += 3000;
                if (legendaryIndex > 9) {
                    Debug.Log("too many items to show...");
                } else {
                    legendaryItemsPos[legendaryIndex].SetActive(true);
                    legendaryItemsPos[legendaryIndex].GetComponent<SpriteRenderer>().sprite = obj.cleanSprite;
                    legendaryIndex++;

                }
            } else if (obj.artifactType == ArtifactScriptableObject.ArtifactType.RARE) {
                //Debug.Log("rare " + obj.cleanSprite.name);
                score += 1000;
                if (rareIndex > 9) {
                    Debug.Log("too many items to show...");
                } else {
                    rareItemsPos[rareIndex].SetActive(true);
                    rareItemsPos[rareIndex].GetComponent<SpriteRenderer>().sprite = obj.cleanSprite;
                    rareIndex++;
                }
            } else {
                score += 100;
                //Debug.Log("common " + obj.cleanSprite.name);
                if (commonIndex > 9) {
                    Debug.Log("too many items to show...");
                } else {
                    commonItemPos[commonIndex].SetActive(true);
                    commonItemPos[commonIndex].GetComponent<SpriteRenderer>().sprite = obj.cleanSprite;
                    commonIndex++;
                }
            }
        }
        score += (int)(10 * Mathf.Abs(player.transform.position.y));
        scoreText.text = "$" + score.ToString();
    }

    private void resetDisplay() {
        accidentCard.SetActive(false);
        score = 0;
        foreach (GameObject go in commonItemPos) {
            go.SetActive(false);
        }
        foreach (GameObject go in rareItemsPos) {
            go.SetActive(false);
        }
        foreach (GameObject go in legendaryItemsPos) {
            go.SetActive(false);
        }
    }
}
