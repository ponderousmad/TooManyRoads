using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameLevel : MonoBehaviour {		
	public LevelData level = new LevelData();

	public string filePath;

	[ContextMenu ("Save")]
	void Save () {
		string data = JsonUtility.ToJson (level, true);
		System.IO.File.WriteAllText(filePath, data);
	}

	[ContextMenu ("Load")]
	void Load() {
		string data = System.IO.File.ReadAllText (filePath);
		level = JsonUtility.FromJson<LevelData>(data);
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


}
