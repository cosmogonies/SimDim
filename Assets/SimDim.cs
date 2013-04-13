using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


namespace SimDim
{
	public class Dimension
	{
		public float SCALING=100.0f; //to be in readonly
		public int Rank;//to be in readonly: get pattern
		
		public string Name;
		
		public List<Matter> Population;
			
		public Dimension(int _NbMesures)
		{
			this.Rank=_NbMesures;
			this.Population = new List<Matter>();
		}
		
		public Vector3 giveAvailableLocation() //We made here the choice to give coord in Unity3D ref space, not the local one (Dimension ref).
		{
			//No time, just HIT the RANDOM box
			float posX = UnityEngine.Random.Range(-1.0F*this.SCALING, this.SCALING); 
			float posY = 0;
			float posZ = UnityEngine.Random.Range(-1.0F*this.SCALING, this.SCALING); 
			
			return new Vector3( posX, posY, posZ );
		}
		
		public void destroyMatter(SimDim.Food _Target)  //later do it with Matter
		{	//as we can NOT destroy bojects, lets remove them for sights:
			
			foreach( Matter currentItem in this.Population)
			{
				BHV_See currentSight = currentItem.root.GetComponent<BHV_See>() as BHV_See;
				if(currentSight != null)
				{
					currentSight.currentVision.Remove(_Target);
					currentSight.FoodList.Remove(_Target);
					
				}
			}
		}
		
	}
	
	
	public class Plan : Dimension
	{
		public Plan(string _Name) : base(2)
		{		
			this.Name = _Name;
			
			//for now we simple put a plane to simulate it:
			GameObject PlanGizmo = UnityEngine.GameObject.CreatePrimitive(PrimitiveType.Plane);
			PlanGizmo.name = _Name+"Gizmo";
			PlanGizmo.transform.localScale*= this.SCALING*0.25f;
			
			Material blackMat = new Material(Shader.Find("Diffuse"));
			blackMat.color = Color.black;
			PlanGizmo.renderer.material = blackMat;
		
		}
		
	}
	public class Line : Dimension
	{
		public Line(string _Name) : base(1)
		{		
			this.Name = _Name;
		}
	}	
	
	
	
	public class Matter
	{
		public List<GameObject> Body;
		public GameObject root;
		public Dimension belonging;
		
		public Matter(Dimension _Dim)
		{
			_Dim.Population.Add(this);
			
			this.Body = new List<GameObject>();
			this.root = new GameObject();
			this.belonging = _Dim;
		}
	}	

	
	
	
	public class Food:Matter
	{
		//GameObject Shape;
		
		public Food(Dimension _Dim):base(_Dim)
		{
			GameObject shape = UnityEngine.GameObject.CreatePrimitive(PrimitiveType.Cube) ;
			this.Body.Add( shape );
			shape.transform.parent = this.root.transform;
			
			//this.Shape.transform.localScale *= 0.1f;
			this.root.name="food_"+_Dim.Population.Count;
			
			
			//Debug.Log ( System.Guid.NewGuid() );
			//this.root.transform.localScale *= 0.1f;
			this.root.transform.position = _Dim.giveAvailableLocation();
			// this.root.transform.localScale*= 0.1f;
			
			//Temporarily, shading them in red to see them better.
			Material redMat = new Material(Shader.Find("Diffuse"));
			redMat.color = Color.red;
			shape.renderer.material = redMat;
		}
		
		public void beEaten()
		{
			Debug.Log (this.root.name+" as eaten");
			this.belonging.Population.Remove(this);
			this.root.SetActive(false);
		}
	}
	
	
	
	
	
	
	
	//class LivingCreature : Transform
	public class LivingCreature:Matter
	{
		int NbSide;
		public List<Vector3> DangerAngles;
		//Dictionary<int,float> Ugliness;
		public string Name;
		public Dimension belonging;
		
		
		GameObject Eye;
		private float DEFAULT_SCALE = 10.0f;
		
//		public LivingCreature(Dimension _Dim):base(_Dim)
//		{
//			this.belonging = _Dim;
//			//Randomly Creation
//			//this.
//		}
		
		public LivingCreature(Dimension _Dim, int _NbSide):base(_Dim)
		{
			this.belonging = _Dim;
			this.NbSide = _NbSide;
			this.DangerAngles = new List<Vector3>();
			
			this.Body = new List<GameObject>();
			
			this.createShape();
			
			if(_NbSide == 4)
			{	
//				//Eye shape creation
//				GameObject EyeShape = UnityEngine.GameObject.CreatePrimitive(PrimitiveType.Cube);
//				EyeShape.transform.parent = this.root.transform;
//				EyeShape.name="Square_Eye";
//				EyeShape.transform.localScale *= 0.1f;
//				EyeShape.transform.Translate(1.0f,0.0f,1.0f);
//				this.Body.Add( EyeShape );
//				this.Eye = EyeShape;
	
//				BHV_FSMBrain_Square newBehaviour  = this.root.AddComponent<BHV_FSMBrain_Square>() as BHV_FSMBrain_Square;
//				newBehaviour.Start();
//				newBehaviour.Sight.TheEye = EyeShape;
				
				//brainify();
				//BHV_FSMBrain_Square newBehaviour  = this.root.AddComponent<BHV_FSMBrain_Square>() as BHV_FSMBrain_Square;
				//newBehaviour.theLivingCreature = this;
				
			}	
			BHV_FSMBrain_Square newBehaviour  = this.root.AddComponent<BHV_FSMBrain_Square>() as BHV_FSMBrain_Square;
			newBehaviour.theLivingCreature = this;
				
			//find a place.
		}
		
		
		private void giveSightBehaviour(GameObject _Eye)
		{//Every living create can see others.
			
			BHV_See newBehaviour  = this.root.AddComponent<BHV_See>() as BHV_See;
			newBehaviour.TheEye = _Eye;
			newBehaviour.Belonging = this.belonging;
		}
		
		private void createShape()
		{
			//GameObject shape = UnityEngine.GameObject.CreatePrimitive(PrimitiveType.Cube);
			//shape.transform.parent = this.root.transform;
			//shape.transform.localScale *= 10f; //see them better
			if(this.NbSide==2)
				this.root.name="Line"+this.belonging.Population.Count;
			if(this.NbSide==3)
				this.root.name="Triangle"+this.belonging.Population.Count;
			if(this.NbSide==4)
				this.root.name="Square"+this.belonging.Population.Count;
			if(this.NbSide==5)
			{
				this.root.name="Pentagon"+this.belonging.Population.Count;
				this.NbSide = UnityEngine.Random.Range(5,10);
			}
			
			//this.Body.Add( shape );
			
			// SHAPE CREATION
			float radius =1.0f;
			Vector3[] AngleCoord = new Vector3[this.NbSide];
			float Angle = Mathf.PI*2 / this.NbSide;
			for(int i=1;i<=AngleCoord.Length;i++)
			{
				float currentAngle = Angle*i;
				AngleCoord[i-1] = new Vector3( radius * Mathf.Cos(currentAngle) , 0, radius * Mathf.Sin(currentAngle) );
				
				if(this.NbSide==2) //Patch to have lines forward vector in front of their point.
					AngleCoord[i-1] = new Vector3( radius * Mathf.Sin(currentAngle) , 0, radius * Mathf.Cos(currentAngle) );
				
				DangerAngles.Add(AngleCoord[i-1]);
			}
			for(int i=0;i<AngleCoord.Length;i++)
			{
				for(int j=0;j<this.DEFAULT_SCALE;j++)
				{	
					int nextIndex = i+1;
					if(i>=AngleCoord.Length-1)
						nextIndex=0;
					
					//Debug.Log(i+" "+nextIndex);
					Vector3 offset = (AngleCoord[nextIndex] - AngleCoord[i] ) / this.DEFAULT_SCALE;
					
					GameObject toto = UnityEngine.GameObject.CreatePrimitive(PrimitiveType.Cube);
					toto.name = this.root.name+"_Sub_"+i+"x"+j;
					toto.transform.position = AngleCoord[i] + j*offset;
					toto.transform.parent = this.root.transform;	
					toto.transform.localScale *= 0.1f;
					
					//Temporarily, shading them in Green to see them better.
					Material greenMat = new Material(Shader.Find("Diffuse"));
					greenMat.color = Color.green;
					toto.renderer.material = greenMat;						
					
					this.Body.Add( toto );
				}	
			}
			this.root.transform.position = this.belonging.giveAvailableLocation();
			
		}
			
		
	}
	
	
	//Create Square
	
	public class SquareCreature : LivingCreature
	{
		public SquareCreature(Dimension _Dim): base(_Dim,4)
		{
			this.Name = "Square_"; //+uuid		
			//Get a Job
		}
	}
	
	
	public class LineCreature : LivingCreature
	{
		public LineCreature(Dimension _Dim): base(_Dim,2)
		{
			this.Name = "Line_"; //+uuid		
			//Get a Job
		}
	}
	
	
	public class TriangleCreature : LivingCreature
	{
		public TriangleCreature(Dimension _Dim): base(_Dim,3)
		{
			this.Name = "Triangle_"; //+uuid		
			//Get a Job
		}
	}	
		
	public class PentagonCreature : LivingCreature
	{
		public PentagonCreature(Dimension _Dim): base(_Dim,5)
		{
			this.Name = "Pentagon_"; //+uuid		
			//Get a Job
		}
	}	
	
	
	
}









