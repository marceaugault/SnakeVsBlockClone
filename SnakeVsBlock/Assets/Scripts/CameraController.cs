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

    void Update()
    {
		transform.position = Vector3.Lerp(transform.position, target.GetHeadPosition() + offset, speed * Time.deltaTime);
    }
}
