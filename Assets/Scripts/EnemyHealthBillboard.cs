using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthBillboard : MonoBehaviour {
	void Update () {
        transform.LookAt(Camera.main.transform);
	}
}
