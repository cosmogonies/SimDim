using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using SimDim;

public class BHV_See : MonoBehaviour 
{
	//The see behaviour manage how a living creature know its environment
	
	public GameObject TheEye;
	
	public List<SimDim.Matter> currentVision;
	public List<SimDim.Food> FoodList;
	
	private float _AngleOfView = UnityEngine.Mathf.PI * 0.5f; //90 degree of vision.  no use right now, harcoded:Pi
	private float SightRange = 50.0f;  // purely arbitrary. Half the scaling of Dim.
	
	
	public SimDim.Dimension Belonging;
	
	void Start () 
	{
		this.currentVision = new List<Matter>();
		this.FoodList = new List<Food>();
		
		
		//Eye shape creation
		//GameObject EyeShape = UnityEngine.GameObject.CreatePrimitive(PrimitiveType.Cube);
		GameObject EyeShape = new UnityEngine.GameObject();
		EyeShape.transform.parent = this.gameObject.transform;
		EyeShape.name="Square_Eye";
		//EyeShape.transform.localScale *= 0.1f;
		EyeShape.transform.Translate(1.0f,0.0f,1.0f); //At creation, Eyes are NorthEast located
		//this.Body.Add( EyeShape );
		//this.Eye = EyeShape;
		
		this.TheEye = EyeShape;
		
	}
	
	//Todo a unique loop to return if I see food or danger or social.
	
	//Each update, I refresh my vision:
	public void Update ()
	{
		//this.currentVision = new List<Matter>(); //empty list for solving big issue, no time to fix (sight is not updated when food is eaten)
		//this.currentVision.Clear();
			
		Vector3 position = this.transform.position; //The position of the creature
		Vector3 EyePosition = TheEye.transform.position;
		
		
		//Debug.Log ("this.Belonging.Population"+this.Belonging.Population.Count);
		foreach( Matter currentItem in this.Belonging.Population)
		{
			//Debug.Log (this.Belonging.Population.IndexOf(currentItem) +" = "+ currentItem.root.name);
			
			//remove itself from loop !
			if(currentItem.root != this.gameObject)
			{
				
				float d = Vector3.Distance(currentItem.root.transform.position, EyePosition);
				
				//Collision detection (TODO: not the right place)
				if(currentItem.GetType().ToString() != "SimDim.Food")
				{
					SimDim.LivingCreature currentSeenGuy = currentItem as SimDim.LivingCreature;
					
					//behaviour from Social, but call here to optimize loop:
					if(d<2.0f)
					{	//Contact Management.
						BHV_Social comp = this.gameObject.GetComponent<BHV_Social>() as BHV_Social; //mayve, LATER, we can manage food like any kills.
						comp.kill(currentSeenGuy);
						return ;
					}
					
					/*
					if(d<10.0f)
					{	//Social Management.

						BHV_See comp = currentSeenGuy.root.GetComponent<BHV_See>() as BHV_See;
						foreach(SimDim.Matter cur in this.currentVision)
						{	
							if(cur.root == currentItem.root)							
							{	//I see a guy who is very near (maybe too much?)
								
								//BHV_Social comp2 = this.gameObject.GetComponent<BHV_Social>() as BHV_Social;
								//comp2.salute(currentItem);
								//return ;
							}
						}
					
					}
					*/
				}
				
				
				
				if( d < SightRange)
				{  // here we are at range
					
					//TODO: test if we are at angle
					Vector3 vSightDirection = EyePosition - position;
					Vector3 vObjDirection = currentItem.root.transform.position - EyePosition;
					
					
					float DotProduct = Vector3.Dot(vSightDirection, vObjDirection);
					
					//Debug.Log ( currentItem.root.name +" ==> "+ DotProduct );
					//Debug.Log (currentItem.GetType());
					
					if(DotProduct > 0)
					{//We are in front of the creature 
						
						if(!this.currentVision.Contains(currentItem))
						{
							this.currentVision.Add(currentItem);
							
							//FOOD MANAGEMENT
							if(currentItem.GetType().ToString() == "SimDim.Food")  //There must be a better way but... NO NET
							{
								//Debug.Log (this.gameObject.name+" see food :"+currentItem.root.name);
								this.FoodList.Add(currentItem as Food);
							}
							else
							{
							//SOCIAL MANAGEMENT
							//Debug.Log (currentItem.GetType().ToString());
								
								
								//Debug.Log (this.gameObject.name+" see :"+currentItem.root.name);
								
								
								
							}
							
						}
					}
					else
					{
						if(this.currentVision.Contains(currentItem))
							this.currentVision.Remove(currentItem);
					}
					
				}
				else
				{
					if(this.currentVision.Contains(currentItem))
						this.currentVision.Remove(currentItem);				
				}
			}
		}
		
	
	}
	
	
}
