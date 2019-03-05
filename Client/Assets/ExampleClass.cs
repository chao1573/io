using UnityEngine;
using System.Collections;

public class ExampleClass : MonoBehaviour {
	public Transform target;
	void Update() {
		transform.rotation = Quaternion.Inverse(target.rotation);
	}
}