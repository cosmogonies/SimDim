using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using SimDim;

public class BHV_FSMBrain_Square : MonoBehaviour 
{
	public SimDim.LivingCreature theLivingCreature;
	
	//private List<MonoBehaviour> StatusList;
	public BHV_See Sight;  // this Eye pattern is evil
	
	public BHV_Social Empathy;
	
	BHV_Eat Hunger;
	
	BHV_Walk Locomotion;
	
	
	public void Start () 
	{
		//this.StatusList = new List<MonoBehaviour>();
	
		this.Hunger  = this.gameObject.AddComponent<BHV_Eat>() as BHV_Eat;
		this.Sight  = this.gameObject.AddComponent<BHV_See>() as BHV_See;

		this.Sight.Belonging = theLivingCreature.belonging;
		
		
		this.Locomotion  = this.gameObject.AddComponent<BHV_Walk>() as BHV_Walk;
		
		this.Empathy = this.gameObject.AddComponent<BHV_Social>() as BHV_Social;
	}
	
	
	void Update () 
	{
		//Idea: speed is hunger
		this.Locomotion.StepLength = this.Hunger.Hunger;
		
		
		if(this.Hunger.isHungry())
		{
			//When we are hungry we look for food.
			
			if( this.Sight.FoodList.Count==0 )
			{
				//Debug.Log (this.gameObject.name+"No food at sight, let's random walk");
				this.Locomotion.walkAtRandom();
			}
			else
			{
				//Searching nearest Food...
				SimDim.Food nearestFood = this.Hunger.lookForFood(this.Sight.FoodList);
				
				//Going forward Nearest food:
				float distance = this.Locomotion.goTo(nearestFood.root.transform.position);
				//Debug.Log (this.gameObject.name+" is at "+DistanceToGoal+"m from its goal");
				if(distance < this.Locomotion.StepLength)
				{	//Start Eating
					Debug.Log (this.gameObject.name+" has reached its goal");
					
					this.Hunger.eatFood(nearestFood);
					//this.Sight.FoodList.Remove(nearestFood); // not accurate. Must be removed from sight of anybodies.
					//this.Sight.currentVision.Remove(nearestFood);
					theLivingCreature.belonging.destroyMatter(nearestFood);
				}
				
			}
		}
		else
		{	//not hungry 
			
			//going for as BHV_Walk ?	
			this.Locomotion.walkAtRandom();
		}
		

		
	
	}
}
