using UnityEngine;
using System.Collections;

public class MathUtil
{
	//将x限制在min和max之间
	public static int CMID(int x, int min, int max) 
    { 
        return (x < min) ? min : ((x > max) ? max : x); 
    }
}
