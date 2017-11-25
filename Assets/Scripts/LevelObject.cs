using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

[System.Serializable]
public class LevelData
{
	public List<LevelObject> levelObjects = new List<LevelObject> ();
}

[System.Serializable]
public class LevelObject {
	public string type;
	public Vector2 position;

	public LevelObject(string objectType, Vector2 pos)
	{
		type = objectType;
		position = pos;
	}
}
