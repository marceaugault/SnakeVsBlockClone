using UnityEngine;

public class CameraController : MonoBehaviour
{
	[SerializeField] float speed = 1f;
	[SerializeField] Vector3 offset = new Vector3(0f, 10f, 0f);

	SnakeController target;
    void Awake()
    {
		target = FindObjectOfType<SnakeController>();

		transform.position = target.GetHeadPosition() + offset;
	}

    void FixedUpdate()
    {
		float targetPos = (target.GetHeadPosition() + offset).z;

		if (targetPos < transform.position.z)
		{
			targetPos = transform.position.z;
		}

		Vector3 newPos = transform.position;
		newPos.z = Mathf.Lerp(transform.position.z, targetPos, speed * Time.fixedDeltaTime);

		transform.position = newPos;
    }
}
