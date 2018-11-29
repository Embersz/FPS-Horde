using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumableSoundPlayer : MonoBehaviour {
    public AudioSource sound;

	void Update () {
        if (!sound.isPlaying)
            Destroy(gameObject);
	}
}
