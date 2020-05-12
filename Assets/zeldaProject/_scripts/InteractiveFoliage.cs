using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InteractiveFoliage : MonoBehaviour 
{

	public Material[] materials;
	public Transform thePlayer;
	Vector3 thePosition;

	// Use this for initialization
	void Start () 
	{
	
		StartCoroutine("writeToMaterial");
	}
	
	// ---------------------
	IEnumerator writeToMaterial () 
	{
	
		while(true)
		{
			thePosition = thePlayer.transform.position;
			for(int i=0; i< materials.Length;i++)
			{
				materials[i].SetVector("_position", thePosition);
			}

			yield return null;
		}
		
	}
}
