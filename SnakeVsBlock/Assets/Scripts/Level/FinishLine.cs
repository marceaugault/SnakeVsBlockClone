using UnityEngine;

public class FinishLine : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
	{
		if (((1 << other.gameObject.layer) & LayerMask.GetMask("Snake")) != 0)
		{
			GameManager gm = FindObjectOfType<GameManager>();
			gm?.LevelCompleted();
		}
	}
}
