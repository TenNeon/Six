using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;

[ExecuteInEditMode]
public class ResearchSystem : MonoBehaviour {
	List<Vector2> connections;
	public List<Researcher> researchers = new List<Researcher>();
	List<string> firstNames = new List<string>{"Sem", "Ath", "Fendreg", "Melbat", "Hols", "Veek", "Crek", "Wenel"};
	List<string> lastNames = new List<string>{"Geespar", "Rontees", "Treeblet", "Kolreeg", "Jeending", "Eespuk", "Creedy", "Heeplen" };
	public List<Sprite> portraits;
	public int researchersCount = 5;
	public int tech = 0;
	public bool isResearching = false;
	public float researchPerSecond = 1;
	public float researchTaskSize = 10;
	public float progress = 0;
	// Use this for initialization
	void Start () {
		connections = new List<Vector2>();

		GenerateConnections();
		WriteConnections();

		if(researchers.Count == 0)
		{
			Generate();
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(isResearching)
		{
			progress += researchPerSecond*Time.deltaTime;
			if (Time.frameCount%60 == 0) {
				Debug.Log ("Research left:" + (researchTaskSize-progress).ToString() );
			}

			if(progress >= researchTaskSize)
			{
				isResearching = false;
				CompleteResearch();
				Debug.Log ("Research done.");
			}
		}
	}

	public void Generate()
	{
		GenerateResearchers();//Create some researchers
	}

	void GenerateResearchers ()
	{
		for (int i = 0; i < researchersCount; i++) {
			AddNewResearcher();
		}
		foreach (var item in researchers) {
			Debug.Log (item.name + " " + item.age);
		}
	}

	public Researcher AddNewResearcher()
	{
		Researcher r = new Researcher();
		//r.name = "Frank";
		r.name = firstNames[UnityEngine.Random.Range(0,firstNames.Count-1)] + " " + lastNames[UnityEngine.Random.Range(0,lastNames.Count-1)];
		r.age = 15 + UnityEngine.Random.Range(0,10) + UnityEngine.Random.Range(0,10) + UnityEngine.Random.Range(0,10) + UnityEngine.Random.Range(0,5) + UnityEngine.Random.Range(0,5) + UnityEngine.Random.Range(0,5);	
		r.portrait = portraits[UnityEngine.Random.Range(0,portraits.Count)];
		researchers.Add(r);

		return r;
	}

	void GenerateConnections ()
	{
		int connectionCount = 100;
		int techCount = 10;
		for (int i = 0; i < techCount; i++) {
			//for each tech, decide if it leads to something else
			if(UnityEngine.Random.Range(0,100) > 50)
			{
				//if it does, link it to something
				var newConnection = new Vector2(i, UnityEngine.Random.Range(0,techCount));
				if(newConnection.x != newConnection.y)
				{
					connections.Add( newConnection );
				}
			}
		}
		while (connections.Count < connectionCount) {
			var newConnection = new Vector2(UnityEngine.Random.Range(0,techCount), UnityEngine.Random.Range(0,techCount));
			if(newConnection.x != newConnection.y)
			{
				connections.Add( newConnection );
			}			
		}
	}

	public void StartResearch()
	{
		if (!isResearching) 
		{
			isResearching = true;
			researchPerSecond = researchers.Count;
			var oldTaskSize = researchTaskSize;
			researchTaskSize = UnityEngine.Random.Range(researchTaskSize*0.95f,researchTaskSize*1.1f);
			progress = 0;
		}
	}

	public void CompleteResearch()
	{
		progress = 0;
		List<string> letters = new List<string>{"A","B","C","D","E","F","G","H","I","J","K","L","M","N","O","P","Q","R","S","T","U","V","W","X","Y","Z",};
		ResearchTask task = new ResearchTask();
		task.name = "Project " + letters[UnityEngine.Random.Range(0,letters.Count)];
		task.timeToComplete = 1f;
		task.taskEffects = new List<string>{"Good Researching Experience"};
		foreach (var item in researchers) {
			item.researchKnowledge.Add(task,1);
		}
		tech++;
	}

	void WriteConnections()
	{
		string fileName = "Connections " + DateTime.Now.ToLongDateString()+".txt";
		StreamWriter streamWriter;
		if (File.Exists(fileName))
		{
			//streamWriter = File.AppendText(fileName);
			streamWriter = File.CreateText(fileName);
		}
		else{
			streamWriter = File.CreateText(fileName);
		}
		streamWriter.WriteLine ("File written at " +DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString());

		for (int i = 0; i < connections.Count; i++) {
			streamWriter.WriteLine (connections[i].x.ToString() + "->" + connections[i].y.ToString());
		}

		streamWriter.Close();
	}
}

public class ResearchNode
{
	public List<ResearchTask> tasks;
}

public class ResearchTask
{
	public ResearchTask()
	{
		;
	}
	public string name;
	public float timeToComplete;
	public List<string> taskEffects;
}

public class Researcher
{
	public string name;
	public Sprite portrait;
	public Dictionary<ResearchTask,float> researchKnowledge = new Dictionary<ResearchTask, float>();
	public ResearchTask currentTask = null;
	public float age = 30;
}
