using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	[SerializeField] float speed = 1f;
	[SerializeField] Vector3 offset = new Vector3(0f, 10f, 0f);

	Transform target;
    void Awake()
    {
		target = GameObject.Find("Snake").transform;

		transform.position = target.transform.position + offset;
	}

    void Update()
    {
		transform.position = Vector3.Lerp(transform.position, target.position + offset, speed * Time.deltaTime);
    }
}
