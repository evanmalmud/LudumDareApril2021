using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTargetTracker : MonoBehaviour
{
    public List<Transform> playerTargets;

    List<Transform> unusedTargets = new List<Transform>();

    List<Transform> currentTargets = new List<Transform>();

    private void Start()
    {
        unusedTargets.AddRange(playerTargets);
    }

    public Transform getTarget()
    {
        if(unusedTargets.Count <= 0)
        {
            return playerTargets[Random.Range(0, playerTargets.Count)];
        } 
        else
        {
            Transform result = unusedTargets[Random.Range(0, unusedTargets.Count)];
            unusedTargets.Remove(result);
            currentTargets.Add(result);
            return result;
        }
    }

    public bool returnTarget(Transform transform)
    {
        if (!playerTargets.Contains(transform))
        {
            //RETURNING A NON-EXISTENT-TARGET
            return false;
        }
        if(currentTargets.Contains(transform))
        {
            currentTargets.Remove(transform);
        }

        if(!unusedTargets.Contains(transform))
        {
            unusedTargets.Add(transform);
        }
        return true;
    }
}
