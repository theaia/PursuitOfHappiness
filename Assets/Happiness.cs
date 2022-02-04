using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Happiness : MonoBehaviour
{
	private SpriteRenderer renderer;
	private BoxCollider2D collider;
	private void OnTriggerEnter2D(Collider2D collision) {
		if (collision.CompareTag("Player")) {
			Game.Instance.IncreaseHappiness();
			gameObject.SetActive(false);
		}
	}
}
