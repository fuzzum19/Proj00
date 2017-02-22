using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class CoroutineTest : MonoBehaviour 
{
    // abcdef

    public Text myText;

    public string s = ""; 
    public StringBuilder sb = new StringBuilder ();

    public bool isReplace = false;

    public string test()
    {
        sb.Length = 0;
        sb.Append("GAME MODE: ");
        sb.Append("CLASSIC");

        if (isReplace)
        {
            sb.Replace("CLASSIC", "ZEN");
        }

        return sb.ToString();
    }

//    public string myGameModeString()
//    {
//        my_strBuilder.Length = 0;
//        my_strBuilder.Append("GAME MODE: ");
//        my_strBuilder.Append("CLASSIC");
//        return my_strBuilder.ToString();
//    }

	// Use this for initialization
	void Start () 
    {
        
	}
	
	// Update is called once per frame
	void Update()
    {
        Debug.Log(sb.Length + " " + sb.Capacity);
        myText.text = test();
	}
}
