using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEndScript : MonoBehaviour
{
    public string nextScene;

	public GameObject trash;

	private void Start()
	{
	}

	private bool deletingSceneWaiting = false;
	private int sceneToBeDeleted;

	private void Update()
	{
		//print(SceneManager.sceneCount);
		//if (deletingSceneWaiting) {
		//	if (SceneManager.sceneCount > 1) {
		//		deletingSceneWaiting = false;
		//		SceneManager.UnloadScene(sceneToBeDeleted);
		//
		//		
		//	}
		//}
	}

	private void OnTriggerEnter(Collider other)
	{
		int nextSceneI = SceneManager.sceneCount;
		SceneManager.LoadScene(nextSceneI, LoadSceneMode.Additive);

		Scene nextScene = SceneManager.GetSceneByBuildIndex(nextSceneI);

		//SceneManager.MoveGameObjectToScene(player, nextScene);
		//SceneManager.MoveGameObjectToScene(interfaceObject, nextScene);

		playerController.currentPlayer.transform.position = playerController.currentPlayer.transform.position - transform.position;
		//playerController.currentPlayer.GetComponent<Rigidbody>().velocity = Vector3.zero;

		playerController.currentPlayer.oxygenLeft = playerController.currentPlayer.oxygenMax;

		Destroy(trash);

		//deletingSceneWaiting = true;
		//sceneToBeDeleted = oldScene.buildIndex;
	}

	//private void OnTrigger(Collider other)
	//{
	//	print("aaa");
	//	SceneManager.LoadScene(nextScene, LoadSceneMode.Single);
	//}
}