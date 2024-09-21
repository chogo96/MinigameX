using UnityEngine;
using System.Collections;

public class SceneChanger : MonoBehaviour {

	public string SceneName;
	
	void OnMouseDown(){
	
		Application.LoadLevel(SceneName);
		
	}
}
