using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

/**
 * Props Controller
 * Responsible for spawning and moving a collection of (random) props.
 */
public class PropsController : MonoBehaviour 
{
	public GameObject[] props;
	public GameObject container;

	public float minSpacing;
	public float maxSpacing;

	private List<GameObject> instances;


	void Start() {
		instances = new List<GameObject>();

		// randomize props position in array
		props = props.OrderBy(item => Random.value).ToArray();

		// instantiate and grab their references
		Vector3 pos = new Vector3(0f, -6.4f, 0f);

		foreach (GameObject prop in props) {
			pos.x += Random.Range(minSpacing, maxSpacing);

			GameObject propObj = Instantiate(prop, Vector3.zero, Quaternion.identity) as GameObject;
			propObj.transform.localPosition = pos;
			propObj.transform.parent = container.transform;

			instances.Add(propObj);
		}
	}
}
