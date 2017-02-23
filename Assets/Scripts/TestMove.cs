using UnityEngine;
using System.Collections;

public class TestMove : MonoBehaviour {
	
	// Update is called once per frame
	private void FixedUpdate () {
        var pos = transform.position;
        transform.position = Vector3.Lerp(pos, pos + Vector3.left, 0.4f);
	}
}
