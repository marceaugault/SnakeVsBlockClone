using UnityEngine;

[CreateAssetMenu(fileName = "Snake Variables", menuName = "SnakeVsBlock/Snake Variables", order = 0)]
public class SnakeVariables : ScriptableObject
{
	public int snakeStartLength = 4;
	public float snakeSpeed = 1f;
}
