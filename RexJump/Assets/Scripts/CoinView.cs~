using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinView : RexJumpElement 
{
	void OnTriggerEnter2D (Collider2D col)
	{
		if (col.gameObject.name == "Player")
		{
			app.controller.AddCoins(app.model.coinsToGive);
            gameObject.SetActive(false);
			// gameObject.GetComponent<Renderer>().enabled = false;
            // gameObject.tag = "coinsOff";
		}
	}
}
