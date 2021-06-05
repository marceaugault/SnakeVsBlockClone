using UnityEngine;
using UnityEngine.UI;
public class Block : MonoBehaviour
{
	int life = 10;

	Text lifeTxt;

	public int Life 
	{
		get { return life; }
		set 
		{
			life = value;
			lifeTxt.text = life.ToString();

			if (life <= 0)
			{
				gameObject.SetActive(false);
			}
		}
	}

	private void Awake()
	{
		lifeTxt = GetComponentInChildren<Text>();
	}

	public void SetColor(Color color)
	{
		MeshRenderer ren = GetComponentInChildren<MeshRenderer>();
		ren.material.color = color;
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (((1 << collision.gameObject.layer) & LayerMask.GetMask("Snake")) != 0)
		{
			Life--;

			SnakeController snake = collision.gameObject.GetComponentInParent<SnakeController>();
			snake?.RemoveFirstPart();
		}
	}
}
