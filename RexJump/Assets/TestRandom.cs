using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRandom : MonoBehaviour 
{
	[System.Serializable]
	public class TestObject
	{
		public GameObject objects;
		public int weights;
	}
	public TestObject[] myTestObject;

    public int weightTotal;
    public int myFinalInt;
 
    struct things 
    { //this is just for code-read niceness
        public const int aThing = 0;
        public const int anotherThing = 1;
        public const int something = 2;
        public const int somethingElse = 3;
    }
 
    void Awake ()
	{
//		weights = new int[4]; //number of things
//
// 
//		//weighting of each thing, high number means more occurrance
//		weights [things.aThing] = 2;
//		weights [things.anotherThing] = 3;
//		weights [things.something] = 9;
//		weights [things.somethingElse] = 20;
//
		weightTotal = 0;
//		foreach (int w in weights)
//		{
//			weightTotal += w;
//		}
//
		for (int i = 0; i < myTestObject.Length; i++)
		{
			weightTotal += myTestObject[i].weights;
		}

		myFinalInt = RandomWeighted();
    }
 
    int RandomWeighted () 
    {
        int result = 0, total = 0;
        int randVal = Random.Range( 0, weightTotal );
		for ( result = 0; result < myTestObject.Length; result++ ) 
        {
			total += myTestObject[result].weights;
            if ( total > randVal ) break;
        }
        return result;
    }
}
