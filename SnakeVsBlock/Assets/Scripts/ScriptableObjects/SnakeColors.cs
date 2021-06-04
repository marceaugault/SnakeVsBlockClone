using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Sphere Color", menuName = "SnakeVsBlock/Sphere Color", order = 0)]
public class SnakeColors : ScriptableObject
{
	public List<Color> colors;
}
