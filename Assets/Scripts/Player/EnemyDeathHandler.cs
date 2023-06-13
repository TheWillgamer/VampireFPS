using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyDeathHandler : MonoBehaviour
{
    [SerializeField] private Image deathMarker;
    
    public void TriggerDeath()
    {
        StartCoroutine(UIDeathMarker());
    }

    public IEnumerator UIDeathMarker()
    {
        Color c = deathMarker.color;
        for (float alpha = 1f; alpha >= 0; alpha -= 0.005f)
        {
            c.a = alpha;
            deathMarker.color = c;
            yield return null;
        }
    }
}
