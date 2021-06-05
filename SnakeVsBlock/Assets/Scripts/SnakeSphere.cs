using UnityEngine;

public class SnakeSphere : MonoBehaviour
{
	MeshRenderer meshRenderer = null;

	SnakeSphere targetSphere = null;

	float speed;
	void Awake()
    {
		meshRenderer = GetComponent<MeshRenderer>();
	}

	private void Update()
	{
		if (targetSphere)
		{
			//Vector3 dir = (transform.position - targetSphere.transform.position).normalized;
			//transform.position = targetSphere.transform.position + dir * targetSphere.transform.localScale.x;
			//Vector3 p = transform.position;
			//p.z = targetSphere.transform.position.z - targetSphere.transform.localScale.x;
			//transform.position = p;

			Vector3 targetPos = new Vector3(targetSphere.transform.position.x, 0f, transform.position.z);
			transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * speed);
		}
		else
		{
			Vector3 inputs = new Vector3(Input.GetAxis("Horizontal"), 0f, 0f);
			transform.position += inputs * Time.deltaTime * speed;
		}
	}

	public void Init(float speed)
	{
		this.speed = speed;
	}
	public void Activate(Color color)
	{
		gameObject.SetActive(true);

		meshRenderer.material.color = color;
	}

	public void Deactivate()
	{
		gameObject.SetActive(false);
		targetSphere = null;
	}

	public void ChangeTargetSphere(SnakeSphere target)
	{
		targetSphere = target;

		if (target)
		{
			transform.position = target.transform.position - Vector3.forward * transform.localScale.x;
		}
	}
}
