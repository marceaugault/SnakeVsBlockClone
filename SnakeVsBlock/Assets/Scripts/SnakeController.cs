using System.Collections.Generic;
using UnityEngine;

public class SnakeController : MonoBehaviour
{
	public GameObject spherePrefab;
	
	[SerializeField] SnakeColors snakeColors;

	List<SnakeSphere> spheres = null;
	int currentLength;
	[SerializeField] int poolSize = 100;

	float snakeSpeed = 5f;

	void Start()
	{
		LevelController level = FindObjectOfType<LevelController>();

		snakeSpeed = level.GetSnakeScrollSpeed();

		SpawnSpheres(level);

		int length = level.GetSnakeLengthAtStart();
		ToggleRangeSpheres(0, length, true);
	}

	void Update()
	{
		if (currentLength > 0)
		{
			transform.position += Vector3.forward * Time.deltaTime * snakeSpeed;
		}
	}

	private void SpawnSpheres(LevelController level)
	{
		if (spheres != null)
		{
			spheres.Clear();
		}
		else
		{
			spheres = new List<SnakeSphere>();
		}

		float sphereSize = level.GetSnakeSphereSize();
		
		for (int i = 0; i < poolSize; i++)
		{
			GameObject go = Instantiate(spherePrefab, new Vector3(0f, 0f, -i * sphereSize), Quaternion.identity, transform);
			go.SetActive(false);
			go.transform.localScale = new Vector3(sphereSize, sphereSize, sphereSize);

			SnakeSphere sphere = go.GetComponent<SnakeSphere>();
			sphere.Init(level.GetSnakeTurnSpeed());
			spheres.Add(sphere);

		}
	}

	public void AddSphere(int nb)
	{
		ToggleRangeSpheres(currentLength, currentLength + nb, true);
	}

	public void RemoveFirstSphere()
	{
		ToggleSphere(0, false);

		SnakeSphere go = spheres[0];
		spheres.RemoveAt(0);
		spheres.Add(go);

		spheres[0].ChangeTargetSphere(null);

		if (currentLength <= 0)
		{

		}
	}

	private void ToggleSphere(int i, bool active)
	{
		if (i >= 0 && i < spheres.Count)
		{
			if (active)
			{
				spheres[i].Activate(GetColor(i));
				spheres[i].ChangeTargetSphere(i - 1 >= 0 ? spheres[i - 1] : null);
			}
			else
			{
				spheres[i].Deactivate();
			}

			currentLength = active ? currentLength + 1 : currentLength - 1;
		}
	}

	private void ToggleRangeSpheres(int start, int end, bool active)
	{
		for (int i = start; i < end; i++)
		{
			ToggleSphere(i, active);
		}
	}

	private Color GetColor(int i)
	{
		i %= snakeColors.colors.Count;

		return snakeColors.colors[i];
	}

	public Vector3 GetHeadPosition()
	{
		return (spheres != null && spheres.Count >= 0) ? spheres[0].transform.position : Vector3.zero;
	}
}
