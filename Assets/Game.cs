using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    static public Game Instance;
    [HideInInspector] public Player Player;

	private void Awake() {
        if (Game.Instance == null) {
            Instance = this;
        } else {
            Destroy(this);
		}
	}
    public void ModifyHealth(int _modify) {
        Player.UpdateHealth(_modify);
	}    
    
    public void IncreaseHappiness() {
        Player.IncrementHappiness();
	}


}
