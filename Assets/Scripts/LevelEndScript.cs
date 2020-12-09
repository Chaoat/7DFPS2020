using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEndScript : MonoBehaviour
{
    public string nextScene;

	private Collider collider;
	private void Start()
	{
		collider = GetComponent<BoxCollider>();
	}

	private void Update()
	{
		
	}

	private void OnTriggerEnter(Collider other)
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1, LoadSceneMode.Single);
	}

	//private void OnTrigger(Collider other)
	//{
	//	print("aaa");
	//	SceneManager.LoadScene(nextScene, LoadSceneMode.Single);
	//}
}