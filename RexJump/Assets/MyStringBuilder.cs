using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;
using System.Text;

public class MyStringBuilder : MonoBehaviour 
{
    ///<summary>
    /// Mutable String class, optimized for speed and memory allocations while retrieving the final result as a string.
    /// Similar use than StringBuilder, but avoid a lot of allocations done by StringBuilder (conversion of int and float to string, frequent capacity change, etc.)
    /// Author: Nicolas Gadenne contact@gaddygames.com
    ///</summary>

    #region Old StringTest
    private StringBuilder m_strBuilder = new StringBuilder( 64 );
    private delegate string Test();
 
    public string StringBuilder()
    {
        m_strBuilder.Length = 0;
        m_strBuilder.Append("1234567890").Append("abcdefghijklmnopqrstuvwxyz").Append("123hjjkkjh238sjs");
        return m_strBuilder.ToString();
    }


    private void RunTest( string testName, Test test )
    {
        
        Profiler.BeginSample( testName );
        string lastResult = null;
        for( int i=0; i<1000; i++ )
            lastResult = test();
        Profiler.EndSample();

        Debug.Log( "Check test result: test=" + testName + " result='" + lastResult + "' (" + lastResult.Length + ")" );
    }
 
    private void RunTests()
    {
        Debug.Log( "=================" );
        RunTest( "Test #3: StringBuilder    ", StringBuilder );
    }

    public void Update()
    {
        RunTests();
    }
    #endregion
}
