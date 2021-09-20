using PowerTools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectEffect : MonoBehaviour
{

    public GameObject itemCollect;
	public SpriteAnim itemAnim;
	public SpriteRenderer itemRend;
    public AnimationClip m_itemCollect = null;


    public void collectAnim()
	{
		StartCoroutine("playCollectAnim");
		if (itemAnim.Clip != m_itemCollect) {
			itemAnim.Play(m_itemCollect);
		}
	}

	IEnumerator playCollectAnim()
	{
		itemCollect.SetActive(true);
		if (itemAnim.Clip != m_itemCollect) {
			itemAnim.Play(m_itemCollect);
		}
		yield return new WaitForSeconds(.33f);
		itemCollect.SetActive(false);
	}
}
