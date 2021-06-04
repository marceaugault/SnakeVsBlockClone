using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
	[SerializeField] LevelRules rules;

	public GameObject blockPrefab;
	public GameObject spherePrefab;

	Vector3 blockScale;
	Vector3 sphereScale;

	Vector2 xBoundaries;
	float levelWidth;
	float columnSize;
	private void Awake()
	{
		Camera.main.backgroundColor = rules.backgroundColor;

		SetupFinishLine();

		float vFOVInRads = Camera.main.fieldOfView * Mathf.Deg2Rad;
		float hFOV = 2f * Mathf.Atan(Mathf.Tan(vFOVInRads / 2f) * Camera.main.aspect) * Mathf.Rad2Deg;

		xBoundaries.y = Mathf.Tan(hFOV / 2f * Mathf.Deg2Rad) * Camera.main.transform.position.y;
		xBoundaries.x = -xBoundaries.y;
		levelWidth = Mathf.Abs(xBoundaries.x) + Mathf.Abs(xBoundaries.y);
		columnSize = levelWidth / (float)rules.nbColumn;

		blockScale = new Vector3(columnSize, 1f, columnSize);
		sphereScale = new Vector3(columnSize * rules.sphereSizeRelativeToColumn, columnSize * rules.sphereSizeRelativeToColumn,
			columnSize * rules.sphereSizeRelativeToColumn);

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
		float nextSphere = Random.Range(rules.distanceBetweenSpheres.x, rules.distanceBetweenSpheres.y + 1);

		int lastBlockSpawn = 0;
		int lastSphereSpawn = 0;
		bool firstWallSpawned = false;

		for (float z = 0; z < rules.levelLength; z += blockScale.z)
		{
			if (Mathf.Abs(z - nextWallBlocks) <= 0.01f)
			{
				SpawnWallBlocks(z);
				nextWallBlocks += blockScale.z * Random.Range(rules.distanceBetweenWallBlocks.x, rules.distanceBetweenWallBlocks.y);

				lastBlockSpawn = 0;
				firstWallSpawned = true;
			}
			else if (lastBlockSpawn >= nextRandomBlocks && firstWallSpawned)
			{
				SpawnBlocks(z, Random.Range(1, rules.nbColumn - 2));
				nextRandomBlocks = Random.Range(rules.distanceBetweenRandomBlocks.x, rules.distanceBetweenRandomBlocks.y);

				lastBlockSpawn = 0;
				lastSphereSpawn++;
			}
			else if (lastSphereSpawn >= nextSphere)
			{
				SpawnSpheres(z, Random.Range(1, rules.nbColumn - 2));
				nextSphere = Random.Range(rules.distanceBetweenSpheres.x, rules.distanceBetweenSpheres.y + 1);

				lastSphereSpawn = 0;
				lastBlockSpawn++;
			}
			else
			{
				lastSphereSpawn++;
				lastBlockSpawn++;
			}
		}
	}

	private void SpawnWallBlocks(float z)
	{
		float halfColumn = columnSize / 2f;

		for (float x = xBoundaries.x + halfColumn; x < xBoundaries.y; x += columnSize)
		{
			Vector3 position = new Vector3(x, 0f, z);

			SpawnBlock(position, Random.Range(rules.blocksLifeRange.x, rules.blocksLifeRange.y));
		}
	}

	private void SpawnBlocks(float z, int nb)
	{
		if (nb == rules.nbColumn)
		{
			SpawnWallBlocks(z);
			return;
		}

		float xStart = xBoundaries.x + columnSize / 2f;
		int randomColumn;

		List<int> prevRandoms = new List<int>();
		
		for (int i = 0; i < nb; i++)
		{
			randomColumn = Random.Range(0, rules.nbColumn);

			while (prevRandoms.Contains(randomColumn))
			{
				randomColumn = Random.Range(0, rules.nbColumn);
			}

			Vector3 position = new Vector3(xStart + columnSize * randomColumn, 0f, z);
			prevRandoms.Add(randomColumn);

			SpawnBlock(position, Random.Range(rules.blocksLifeRange.x, rules.blocksLifeRange.y));
		}
	}

	private void SpawnBlock(Vector3 position, int life)
	{
		GameObject go = Instantiate(blockPrefab, position, Quaternion.identity);
		go.transform.localScale = blockScale;

		Block block = go.GetComponent<Block>();
		if (block)
		{
			block.Life = life;

			Vector2Int lifeRange = rules.blocksLifeRange;
			Color c = Color.Lerp(rules.lowLifeBlockColor, rules.highLifeBlockColor, (float)life / (lifeRange.y - lifeRange.x));

			block.SetColor(c);
		}
	}

	private void SpawnSpheres(float z, int nb)
	{
		float xStart = xBoundaries.x + columnSize / 2f;
		int randomColumn;

		List<int> prevRandoms = new List<int>();

		for (int i = 0; i < nb; i++)
		{
			randomColumn = Random.Range(0, rules.nbColumn);

			while (prevRandoms.Contains(randomColumn))
			{
				randomColumn = Random.Range(0, rules.nbColumn);
			}

			Vector3 position = new Vector3(xStart + columnSize * randomColumn, 0f, z);
			prevRandoms.Add(randomColumn);

			SpawnSphere(position, Random.Range(rules.sphereLifeNb.x, rules.sphereLifeNb.y));
		}
	}
	private void SpawnSphere(Vector3 position, int nb)
	{
		GameObject go = Instantiate(spherePrefab, position, Quaternion.identity);
		go.transform.localScale = new Vector3(rules.sphereSizeRelativeToColumn, rules.sphereSizeRelativeToColumn, rules.sphereSizeRelativeToColumn);
		
		LootableSphere sphere = go.GetComponent<LootableSphere>();
		if (sphere)
		{
			sphere.SetSphereNb(nb);
		}
	}

	public float GetSnakeScrollSpeed()
	{
		return rules.snakeScrollSpeed;
	}

	public float GetSnakeTurnSpeed()
	{
		return rules.snakeTurnSpeed;
	}

	public int GetSnakeLengthAtStart()
	{
		return rules.snakeStartLength;
	}

	public float GetSnakeSphereSize()
	{
		return columnSize * rules.snakeSizeRelativeToColumn;
	}
}
