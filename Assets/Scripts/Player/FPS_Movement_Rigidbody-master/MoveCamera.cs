using UnityEngine;

public class MoveCamera : MonoBehaviour {

    public Transform player;
    public bool active;

    void Update() {
        if (active)
            transform.position = player.position;
    }
}
