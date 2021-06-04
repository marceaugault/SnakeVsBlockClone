using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeController : MonoBehaviour
{
	public GameObject spherePrefab;

	List<GameObject> spheres = null;
	int currentLength;

	[SerializeField] int poolSize = 100;

    // Start is called before the first frame update
    void Start()
    {
		LevelController level = FindObjectOfType<LevelController>();
		currentLength = level.SnakeLengthAtStart();

		SpawnSpheres();

		ToggleRangeSpheres(0, currentLength - 1, true);
	}

    // Update is called once per frame
    void Update()
    {
        
    }

	private void SpawnSpheres()
	{
		if (spheres != null)
		{
			spheres.Clear();
		}
		else
		{
			spheres = new List<GameObject>();
		}

		float sphereSize = spherePrefab.transform.localScale.z;
		
		for (int i = 0; i < poolSize; i++)
		{
			GameObject go = Instantiate(spherePrefab, new Vector3(0f, 0f, -i * sphereSize), Quaternion.identity, transform);
			spheres.Add(go);
			go.SetActive(false);
		}
	}

	private void ToggleSphere(int i, bool active)
	{
		if (i >= 0 && i < spheres.Count)
		{
			spheres[i].SetActive(active);
		}
	}

	private void ToggleRangeSpheres(int start, int end, bool active)
	{
		for (int i = start; i <= end; i++)
		{
			ToggleSphere(i, active);
		}
	}
}
