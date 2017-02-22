using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigCoinView : RexJumpElement
{
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.name == "Player")
        {
            app.controller.AddCoins(5);
            gameObject.SetActive(false);
        }
    }
}
