using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


namespace SimDim
{
	public class Dimension
	{
		public float SCALING=100.0f; //to be in readonly
		public int Rank;//to be in readonly: get design pattern
		
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
		{	//as we can NOT destroy objects, lets remove them for sights:
			
			foreach( Matter currentItem in this.Population)
			{
				BHV_Vision currentSight = currentItem.root.GetComponent<BHV_Vision>() as BHV_Vision;
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
		public Food(Dimension _Dim):base(_Dim)
		{
			GameObject shape = UnityEngine.GameObject.CreatePrimitive(PrimitiveType.Cube) ;
			this.Body.Add( shape );
			shape.transform.parent = this.root.transform;
			
			shape.transform.localScale *= 0.1f;
			
			//MonoBehaviour.Destroy(shape.GetComponent<BoxCollider>());
			
			this.root.name="food_"+_Dim.Population.Count;
			
			//Debug.Log ( System.Guid.NewGuid() );
			//this.root.transform.localScale *= 0.1f;
			this.root.transform.position = _Dim.giveAvailableLocation();
			
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
		public List<Vector3> Angles;
		//Dictionary<int,float> Ugliness;
		public float Uglyness=0.0f;
		
		public string Name;
		public Dimension belonging;
		
		public float Age=10.0f; //Age is scale
		//private float DEFAULT_SCALE = 10.0f;
		
		GameObject Eye;
		
		public LivingCreature(Dimension _Dim, int _NbSide):base(_Dim)
		{
			this.belonging = _Dim;
			this.NbSide = _NbSide;
			Angles = new List<Vector3>();
			
			this.Body = new List<GameObject>();
			
			//Age
			this.Age = UnityEngine.Random.Range(10.0f,30.0f);
			SphereCollider sc = this.root.AddComponent<SphereCollider>();
			sc.radius=0.1f*this.Age;
			
			//Uglyness
			if(UnityEngine.Random.value<0.3f) // 30% chance not to be regular
				this.Uglyness = UnityEngine.Random.Range(0.1f,0.3f);
			
			this.createShape();
			
			BHV_FSMBrain newBehaviour  = this.root.AddComponent<BHV_FSMBrain>() as BHV_FSMBrain;
			newBehaviour.theLivingCreature = this;
				
			//find a place.
		}
		
		
		
		
		private void createShape()
		{   // SHAPE CREATION
			float radius =0.1f*this.Age;
			Vector3[] AngleCoord = new Vector3[this.NbSide];
			float Angle = Mathf.PI*2 / this.NbSide;
			List<float> Angles = new List<float>(this.NbSide);
			
			for(int i=0;i<AngleCoord.Length;i++)
				Angles.Add(Angle*i);

			if(this.Uglyness>0.0f)
			{ //Uglyness Management, offseting regular angles..
				Angles[0] = this.Uglyness;
				Angles[1] = Angles[1] - this.Uglyness;
			}
			
			for(int i=1;i<=AngleCoord.Length;i++)
			{
				float currentAngle = Angles[i-1];
				AngleCoord[i-1] = new Vector3( radius * Mathf.Cos(currentAngle) , 0, radius * Mathf.Sin(currentAngle) );
				
				if(this.NbSide==2) //Patch to have lines forward vector in front of their point.
					AngleCoord[i-1] = new Vector3( radius * Mathf.Sin(currentAngle) , 0, radius * Mathf.Cos(currentAngle) );
			}
			for(int i=0;i<AngleCoord.Length;i++)
			{
				for(int j=0;j<this.Age;j++)
				{	
					int nextIndex = i+1;
					if(i>=AngleCoord.Length-1)
						nextIndex=0;
					
					Vector3 offset = (AngleCoord[nextIndex] - AngleCoord[i] ) / this.Age;
					
					GameObject SubShape = UnityEngine.GameObject.CreatePrimitive(PrimitiveType.Cube);
					SubShape.name = this.root.name+"_Sub_"+i+"x"+j;
					SubShape.transform.position = AngleCoord[i] + j*offset;
					SubShape.transform.parent = this.root.transform;	
					SubShape.transform.localScale *= 0.1f;
					
					MonoBehaviour.Destroy(SubShape.GetComponent<BoxCollider>());
					
					//Temporarily, shading them in Green to see them better.
					Material greenMat = new Material(Shader.Find("Diffuse"));
					greenMat.color = Color.green;
					SubShape.renderer.material = greenMat;						
					
					this.Body.Add( SubShape );
				}	
			}
			this.root.transform.position = this.belonging.giveAvailableLocation();
			
		}
		
		private void giveSightBehaviour(GameObject _Eye)
		{//Every living create can see others.
			BHV_Vision newBehaviour  = this.root.AddComponent<BHV_Vision>() as BHV_Vision;
			newBehaviour.TheEye = _Eye;
			newBehaviour.Belonging = this.belonging;
		}
				
		
	}

	public class LineCreature : LivingCreature
	{
		public LineCreature(Dimension _Dim): base(_Dim,2)
		{
			this.Name = "Line_"; //+uuid		
			this.root.name="Line"+this.belonging.Population.Count;
			//Get a Job
		}
	}
	
	public class TriangleCreature : LivingCreature
	{
		public TriangleCreature(Dimension _Dim): base(_Dim,3)
		{
			this.Name = "Triangle_"; //+uuid
			this.root.name="Triangle"+this.belonging.Population.Count;
			//Get a Job
		}
	}	
	
	//Create Square
	public class SquareCreature : LivingCreature
	{
		public SquareCreature(Dimension _Dim): base(_Dim,4)
		{
			this.Name = "Square_"; //+uuid
			this.root.name="Square"+this.belonging.Population.Count;
			//Get a Job
		}
	}
	
	public class PentagonCreature : LivingCreature
	{
		public PentagonCreature(Dimension _Dim): base(_Dim, UnityEngine.Random.Range(5,10) )
		{
			this.Name = "Pentagon_"; //+uuid
			this.root.name="Pentagon"+this.belonging.Population.Count;
			//this.NbSide = UnityEngine.Random.Range(5,10);
			//Get a Job
		}
	}	
}









