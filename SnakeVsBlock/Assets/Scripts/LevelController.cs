using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
	[SerializeField] LevelRules rules;

	public GameObject blockPrefab;

	Vector3 blockScale;
	Vector2 xBoundaries;
	float xRange;
	float xColumnRange;
	private void Start()
	{
		Camera.main.backgroundColor = rules.backgroundColor;

		SetupFinishLine();

		float vFOVInRads = Camera.main.fieldOfView * Mathf.Deg2Rad;
		float hFOV = 2f * Mathf.Atan(Mathf.Tan(vFOVInRads / 2f) * Camera.main.aspect) * Mathf.Rad2Deg;

		xBoundaries.y = Mathf.Tan(hFOV / 2f * Mathf.Deg2Rad) * Camera.main.transform.position.y;
		xBoundaries.x = -xBoundaries.y;
		xRange = Mathf.Abs(xBoundaries.x) + Mathf.Abs(xBoundaries.y);
		xColumnRange = xRange / (float)rules.nbColumn;
		blockScale = new Vector3(xColumnRange, 1f, xColumnRange);

		CreateLevel();
	}

	private void SetupFinishLine()
	{
		GameObject finishLine = GameObject.Find("FinishLine");
		finishLine.transform.position = new Vector3(0f, 0f, rules.levelLength);

		MeshRenderer ren = finishLine.GetComponent<MeshRenderer>();
		ren.material.color = rules.finishLineColor;
	}

	private void CreateLevel()
	{
		float nextWallBlocks = blockScale.z * Random.Range(rules.distanceBetweenWallBlocks.x, rules.distanceBetweenWallBlocks.y + 1);
		float nextRandomBlocks = Random.Range(rules.distanceBetweenRandomBlocks.x, rules.distanceBetweenRandomBlocks.y + 1);

		int lastSpawn = 0;
		bool firstWallSpawned = false;

		for (float z = 0; z < rules.levelLength; z += blockScale.z)
		{
			if (Mathf.Abs(z - nextWallBlocks) <= 0.01f)
			{
				SpawnWallBlocks(z);
				nextWallBlocks += blockScale.z * Random.Range(rules.distanceBetweenWallBlocks.x, rules.distanceBetweenWallBlocks.y);

				lastSpawn = 0;
				firstWallSpawned = true;
			}
			else if (lastSpawn >= nextRandomBlocks && firstWallSpawned)
			{
				SpawnBlocks(z, Random.Range(1, rules.nbColumn - 2));
				nextRandomBlocks = Random.Range(rules.distanceBetweenRandomBlocks.x, rules.distanceBetweenRandomBlocks.y);
				
				lastSpawn = 0;
			}
			else
			{
				lastSpawn++;
			}
		}
	}

	private void SpawnWallBlocks(float z)
	{
		z -= z % blockScale.z;

		float halfColumn = xColumnRange / 2f;

		for (float x = xBoundaries.x + halfColumn; x < xBoundaries.y; x += xColumnRange)
		{
			Vector3 position = new Vector3(x, 0f, z);

			GameObject go = Instantiate(blockPrefab, position, Quaternion.identity);
			go.transform.localScale = blockScale;
		}
	}

	private void SpawnBlocks(float z, int nb)
	{
		if (nb == rules.nbColumn)
		{
			SpawnWallBlocks(z);
			return;
		}

		z -= z % blockScale.z;

		float xStart = xBoundaries.x + xColumnRange / 2f;
		int randomColumn;

		List<int> prevRandoms = new List<int>();
		
		for (int i = 0; i < nb; i++)
		{
			randomColumn = Random.Range(0, rules.nbColumn);

			while (prevRandoms.Contains(randomColumn))
			{
				randomColumn = Random.Range(0, rules.nbColumn);
			}

			Vector3 position = new Vector3(xStart + xColumnRange * randomColumn, 0f, z);
			prevRandoms.Add(randomColumn);

			GameObject go = Instantiate(blockPrefab, position, Quaternion.identity);
			go.transform.localScale = blockScale;
		}
	}

	public float GetSnakeSpeed()
	{
		return rules.snakeSpeed;
	}

	public int SnakeLengthAtStart()
	{
		return rules.snakeStartLength;
	}
}
