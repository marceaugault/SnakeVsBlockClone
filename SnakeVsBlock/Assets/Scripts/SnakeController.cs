using System.Collections.Generic;
using UnityEngine;

public class SnakeController : MonoBehaviour
{
	public GameObject spherePrefab;
	
	[SerializeField] SnakeColors snakeColors;

	List<GameObject> bodyParts = null;
	int currentLength;
	[SerializeField] int poolSize = 100;

	Vector2 xBoundaries;

	float snakeSpeed = 5f;
	float bodyPartSize;

	void Start()
	{
		LevelController level = FindObjectOfType<LevelController>();

		if (level)
		{
			snakeSpeed = level.GetSnakeScrollSpeed();
			bodyPartSize = level.GetSnakeSphereSize();

			xBoundaries = level.GetLevelBoundariesX();

			SpawnBody();

			ToggleParts(0, level.GetSnakeLengthAtStart(), true);
		}
	}

	void Update()
	{
		MoveSnake();
	}

	void MoveSnake()
	{
		if (currentLength == 0)
		{
			return;
		}

		for (int i = currentLength - 1; i >= 1; i--)
		{
			Vector3 dir = (bodyParts[i].transform.position - bodyParts[i - 1].transform.position).normalized;
			bodyParts[i].transform.position = bodyParts[i - 1].transform.position + dir * bodyPartSize;
		}

		Vector3 inputs = new Vector3(Input.GetAxis("Horizontal"), 0f, 0f);

		bodyParts[0].transform.Translate((Vector3.forward + inputs) * snakeSpeed * Time.fixedDeltaTime);

		Vector3 crtPos = bodyParts[0].transform.position;
		crtPos.x = Mathf.Clamp(crtPos.x, xBoundaries.x, xBoundaries.y);

		bodyParts[0].transform.position = crtPos;
	}

	private void SpawnBody()
	{
		if (bodyParts != null)
		{
			bodyParts.Clear();
		}
		else
		{
			bodyParts = new List<GameObject>();
		}
		
		for (int i = 0; i < poolSize; i++)
		{
			GameObject go = Instantiate(spherePrefab, new Vector3(0f, 0f, -i * bodyPartSize), 
				Quaternion.identity, transform);
			go.SetActive(false);
			go.transform.localScale = new Vector3(bodyPartSize, bodyPartSize, bodyPartSize);

			bodyParts.Add(go);
		}
	}

	public void AddSphere(int nb)
	{
		ToggleParts(currentLength, currentLength + nb, true);
	}

	public void RemoveFirstPart()
	{
		TogglePart(0, false);

		GameObject go = bodyParts[0];
		bodyParts.RemoveAt(0);
		bodyParts.Add(go);

		if (currentLength <= 0)
		{

		}
	}

	private void TogglePart(int i, bool active)
	{
		if (i >= 0 && i < bodyParts.Count)
		{
			bodyParts[i].SetActive(active);
			currentLength = active ? currentLength + 1 : currentLength - 1;

			if (active)
			{
				MeshRenderer ren = bodyParts[i].GetComponent<MeshRenderer>();
				ren.material.color = GetColor(i);
			}
		}
	}

	private void ToggleParts(int start, int end, bool active)
	{
		for (int i = start; i < end; i++)
		{
			TogglePart(i, active);
		}
	}

	private Color GetColor(int i)
	{
		i %= snakeColors.colors.Count;

		return snakeColors.colors[i];
	}

	public Vector3 GetHeadPosition()
	{
		return (bodyParts != null && bodyParts.Count >= 0) ? bodyParts[0].transform.position : Vector3.zero;
	}
}
