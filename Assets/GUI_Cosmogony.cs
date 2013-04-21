using UnityEngine;
using System.Collections;

using SimDim;

public class GUI_Cosmogony: MonoBehaviour 
{
	private SimDim.Plan theWorld;  // Later, it must be a list
	
	private float TimeSpeed = 1.0f;
	
	private float NbF = 25.0f;
	private float Nb2 = 5.0f;
	private float Nb3 = 10.0f;
	private float Nb4 = 15.0f;
	private float Nb5 = 5.0f;

	// Use this for initialization
	void Start () 
	{
		
		//Creation of the main FLATLAND world(2)
		
		SimDim.Plan theFlatlandWorld = new SimDim.Plan("FLATLAND");
		this.theWorld = theFlatlandWorld;
			
 	}
	
	// Update is called once per frame
	void Update () 
	{
			
	}
	
	
	void OnGUI()
	{
		
		this.TimeSpeed = GUI.HorizontalSlider( new Rect( 0,Screen.height-50,Screen.width*0.1f,50 ), this.TimeSpeed, 0.1f, 10.0f );
		GUI.Label( new Rect( 50,Screen.height-50,Screen.width*0.1f,50 ), "Speed="+this.TimeSpeed);
		Time.timeScale = this.TimeSpeed;
		
		
		//Manual Creation
		
		if(GUI.Button( new Rect( Screen.width*0.9f,0,Screen.width*0.1f,50 ), "+Food"))
		{
			this.theWorld.Population.Add(new SimDim.Food(this.theWorld));	
		}
		
		
	
		if(GUI.Button( new Rect( Screen.width*0.9f,50,Screen.width*0.1f,50 ), "+#2 guy"))
		{
			this.theWorld.Population.Add(new SimDim.LineCreature(this.theWorld));	
		}		
		
	
		if(GUI.Button( new Rect( Screen.width*0.9f,100,Screen.width*0.1f,50 ), "+#3 guy"))
		{
			this.theWorld.Population.Add(new SimDim.TriangleCreature(this.theWorld));	
		}		
	
		if(GUI.Button( new Rect( Screen.width*0.9f,150,Screen.width*0.1f,50 ), "+#4 guy"))
		{
			this.theWorld.Population.Add(new SimDim.SquareCreature(this.theWorld));	
		}		
	
		if(GUI.Button( new Rect( Screen.width*0.9f,200,Screen.width*0.1f,50 ), "+#5 guy"))
		{
			this.theWorld.Population.Add(new SimDim.PentagonCreature(this.theWorld));	
		}
		
		
		//World Setup
		this.NbF = GUI.HorizontalSlider( new Rect( 0,0,Screen.width*0.1f,50 ), this.NbF, 0.0f, 100.0f );
		this.Nb2 = GUI.HorizontalSlider( new Rect( 0,50,Screen.width*0.1f,50 ), this.Nb2, 0.0f, 25.0f );
		this.Nb3 = GUI.HorizontalSlider( new Rect( 0,100,Screen.width*0.1f,50 ), this.Nb3, 0.0f, 25.0f );
		this.Nb4 = GUI.HorizontalSlider( new Rect( 0,150,Screen.width*0.1f,50 ), this.Nb4, 0.0f, 25.0f );
		this.Nb5 = GUI.HorizontalSlider( new Rect( 0,200,Screen.width*0.1f,50 ), this.Nb5, 0.0f, 25.0f );
		if(GUI.Button( new Rect( 0,250,Screen.width*0.1f,50 ), "Create"))
		{
			for(int i=0; i<=Mathf.RoundToInt(this.NbF);i++)
				this.theWorld.Population.Add(new SimDim.Food(this.theWorld));
			for(int i=0; i<=Mathf.RoundToInt(this.Nb2);i++)
				this.theWorld.Population.Add(new SimDim.LineCreature(this.theWorld));
			for(int i=0; i<=Mathf.RoundToInt(this.Nb3);i++)
				this.theWorld.Population.Add(new SimDim.TriangleCreature(this.theWorld));
			for(int i=0; i<=Mathf.RoundToInt(this.Nb4);i++)
				this.theWorld.Population.Add(new SimDim.SquareCreature(this.theWorld));
			for(int i=0; i<=Mathf.RoundToInt(this.Nb5);i++)
				this.theWorld.Population.Add(new SimDim.PentagonCreature(this.theWorld));		
		}	
		
		
		
		
	}	
	
}
