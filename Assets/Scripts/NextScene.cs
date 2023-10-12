using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextScene : MonoBehaviour
{
    public string nextSceneName;
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("Change scene!");
            SceneManager.LoadSceneAsync(nextSceneName, LoadSceneMode.Single);
            GameObject.Find("Mario").transform.position = new Vector3(-4.1f, -1.5f, 0.0f);
        }
    }
}
