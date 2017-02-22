using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPooler : RexJumpElement
{
	// Update is called once per frame
	void Update()
    {
       CoinPoolerPrefab();
	}

    public void CoinPoolerPrefab()
    {
        for (int i = 0; i < this.gameObject.transform.childCount; i++)
        {
            if (transform.position.x < app.view.coinPoolerRange.transform.position.x)
            {
                Transform child = this.gameObject.transform.GetChild(i);
                child.gameObject.SetActive(true);
            }
        }
    }
}
