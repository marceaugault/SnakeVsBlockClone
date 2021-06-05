using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SnakeController : MonoBehaviour
{
	public GameObject spherePrefab;

	[SerializeField] SnakeVariables snakeVariables;
	[SerializeField] SnakeColors snakeColors;

	List<GameObject> bodyParts = null;
	int currentLength;
	[SerializeField] int poolSize = 100;

	float snakeSpeed = 5f;
	float bodyPartSize;

	float levelLength;

	GameManager manager = null;

	GameObject countCanvas = null;
	Text bodyPartsCount = null;

	delegate void OnHeadLost();
	OnHeadLost onHeadLost;
	public int CurrentLength 
	{
		get { return currentLength; }
		private set
		{
			currentLength = value;

			if (bodyPartsCount)
			{
				bodyPartsCount.text = currentLength.ToString();
			}
		}
	}

	void Start()
	{
		countCanvas = GameObject.Find("SnakeCountCanvas");

		if (countCanvas)
		{
			bodyPartsCount = countCanvas.GetComponentInChildren<Text>();
		}

		LevelGeneration level = FindObjectOfType<LevelGeneration>();

		if (level)
		{
			snakeSpeed = snakeVariables.snakeSpeed;
			bodyPartSize = level.GetSnakeSphereSize();

			levelLength = level.GetLevelLength();

			SpawnBody();

			ToggleParts(0, snakeVariables.snakeStartLength, true);
		}

		manager = FindObjectOfType<GameManager>();
		if (manager)
		{
			onHeadLost += manager.OnHeadLost;
		}
	}

	void Update()
	{
		MoveSnake();

		UpdateTextCount();
	}

	void MoveSnake()
	{
		if (CurrentLength == 0 || manager.State != GameState.Running)
		{
			return;
		}

		for (int i = CurrentLength - 1; i >= 1; i--)
		{
			Vector3 dir = (bodyParts[i].transform.position - bodyParts[i - 1].transform.position).normalized;
			bodyParts[i].GetComponent<Rigidbody>().MovePosition(bodyParts[i - 1].transform.position + dir * bodyPartSize * 0.9f);
		}

		Vector3 inputs = new Vector3(Input.GetAxis("Horizontal"), 0f, 0f);

		if (Input.touchCount > 0)
		{
			Vector3 touchDelta = Input.GetTouch(0).deltaPosition;
			inputs.x += touchDelta.x * 0.04f;
		}

		Vector3 mvt = Vector3.forward + inputs;
		if (mvt.z < 0f)
		{
			mvt.z = 0f;
		}

		bodyParts[0].GetComponent<Rigidbody>().MovePosition(bodyParts[0].transform.position + mvt * snakeSpeed * Time.fixedDeltaTime);

		manager.UpdateCompletionPercent(bodyParts[0].transform.position.z / levelLength);
	}

	void UpdateTextCount()
	{
		if (CurrentLength > 0)
		{
			countCanvas.transform.position = bodyParts[0].transform.position + Vector3.forward * 0.5f;
		}
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

			go.GetComponent<SphereCollider>().enabled = false;

			bodyParts.Add(go);
		}

		bodyParts[0].GetComponent<SphereCollider>().enabled = true;
	}

	public void AddSphere(int nb)
	{
		ToggleParts(CurrentLength, CurrentLength + nb, true);
	}

	public void RemoveFirstPart()
	{
		TogglePart(0, false);

		GameObject go = bodyParts[0];
		go.GetComponent<SphereCollider>().enabled = false;

		bodyParts.RemoveAt(0);
		bodyParts.Add(go);

		bodyParts[0].GetComponent<SphereCollider>().enabled = true;

		onHeadLost?.Invoke();

		if (CurrentLength <= 0 && manager)
		{
			manager.GameOver();
		}
	}

	private void TogglePart(int i, bool active)
	{
		if (i >= 0 && i < bodyParts.Count)
		{
			bodyParts[i].SetActive(active);

			if (i > 0)
			{
				bodyParts[i].transform.position = bodyParts[i - 1].transform.position - Vector3.forward * bodyPartSize;
			}

			CurrentLength = active ? CurrentLength + 1 : CurrentLength - 1;

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
