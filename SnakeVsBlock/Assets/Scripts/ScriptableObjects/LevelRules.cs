using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level Rule", menuName = "SnakeVsBlock/Level Rule", order = 0)]
public class LevelRules : ScriptableObject
{
	public Color backgroundColor = Color.black;
	public Color finishLineColor = Color.white;

	public int snakeStartLength = 4;
	public float snakeSpeed = 1f;

	[Header("Level Creation")]
	public float levelLength = 50f;
	public int nbColumn = 5;

	public Vector2Int distanceBetweenWallBlocks = new Vector2Int(15, 30);
	public Vector2Int distanceBetweenRandomBlocks = new Vector2Int(8, 14);
}
