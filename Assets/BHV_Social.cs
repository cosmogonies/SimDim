using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using SimDim;

public class BHV_Social : MonoBehaviour 
{

	private bool isSaluting=false;
	
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		
		if(this.isSaluting)
		{
			this.gameObject.transform.RotateAroundLocal( Vector3.up, Mathf.PI * 0.01f);
		}
		
	}
	
	
	//typically a method of SimDim.LivingCreature. but not BHV-friendly
	public void kill( SimDim.Matter _Victim)
	{		
		//Debug.Log (this.gameObject.name+" killed "+_Victim.root.name +"===============================================");
		Debug.Log (this.gameObject.name+" killed "+_Victim.root.name);
		_Victim.belonging.Population.Remove(_Victim);
		
		
		//Spread Food: (later for full decomposition):
//		foreach(GameObject shrapnel in _Victim.Body)
//		{
//			SimDim.Food toto = new SimDim.Food(_Victim.belonging);
//			toto.root.transform.position = shrapnel.transform.position;
//			_Victim.belonging.Population.Add(toto);
//		}
		SimDim.Food toto = new SimDim.Food(_Victim.belonging);
		toto.root.transform.position = this.gameObject.transform.position;
		_Victim.belonging.Population.Add(toto);
		
		_Victim.root.SetActive(false);
	}
	
	
	public void salute( SimDim.Matter _Victim )
	{	//in 2D dimension world to recognize, we need to rotate around myself:
		Debug.Log (this.gameObject.name+" salute "+_Victim.root.name);
		this.gameObject.transform.RotateAroundLocal( Vector3.up, Mathf.PI * 0.01f);
		this.isSaluting = true;
	}
	
	
}
