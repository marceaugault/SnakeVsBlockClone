using UnityEngine;
using UnityEngine.UI;

public class LootableSphere : MonoBehaviour
{
	int sphereNb = 2;

	Text nbTxt;
    void Awake()
    {
		nbTxt = GetComponentInChildren<Text>();
	}

	public void SetSphereNb(int nb)
	{
		sphereNb = nb;
		nbTxt.text = sphereNb.ToString();
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (((1 << collision.gameObject.layer) & LayerMask.GetMask("Snake")) != 0)
		{
			SnakeController snake = collision.gameObject.GetComponentInParent<SnakeController>();
			if (snake)
			{
				snake.AddSphere(sphereNb);
				gameObject.SetActive(false);
			}
		}
	}
}
