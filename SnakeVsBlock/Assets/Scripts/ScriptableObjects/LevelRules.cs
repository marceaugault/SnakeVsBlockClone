using UnityEngine;

[CreateAssetMenu(fileName = "Level Rule", menuName = "SnakeVsBlock/Level Rule", order = 0)]
public class LevelRules : ScriptableObject
{
	public Color backgroundColor = Color.black;
	public Color finishLineColor = Color.white;

	[Header("Snake")]
	[Range(0.1f, 1f)]
	public float snakeBodyPartSizeRelativeToColumn = 0.5f;

	[Header("Level Generation")]
	public float levelLength = 50f;
	public int nbColumn = 5;

	public Vector2Int distanceBetweenWallBlocks = new Vector2Int(15, 30);
	public Vector2Int distanceBetweenRandomBlocks = new Vector2Int(4, 10);

	public Vector2Int blocksLifeRange = new Vector2Int(1, 50);

	public Color lowLifeBlockColor = Color.white;
	public Color highLifeBlockColor = Color.black;

	public Vector2Int distanceBetweenSpheres = new Vector2Int(1, 4);
	public Vector2Int sphereLifeNb = new Vector2Int(1, 6);
	[Range(0.1f, 1f)]
	public float sphereSizeRelativeToColumn = 0.3f;

	public Vector2 wallSize = new Vector2(1f, 5f);
	public float wallSizeRelativeToColumn = 0.03f;
	[Range(0, 100)]
	public int wallSpawnChance = 50;
}
