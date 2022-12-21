using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MoveCamera : MonoBehaviour {

    public Transform player;
    public bool active;
    public Image blkScreen;

    void Update() {
        if (active)
            transform.position = player.position;
    }

    public IEnumerator FadeOut()
    {
        yield return new WaitForSeconds(1.2f);
        Color c = blkScreen.color;
        for (float alpha = 0f; alpha <= 1f; alpha += 0.001f)
        {
            c.a = alpha;
            blkScreen.color = c;
            yield return null;
        }
    }
}
