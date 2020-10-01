using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneStreamingTrigger : MonoBehaviour
{
    [SerializeField] private string streamTargetScene;

    private IEnumerator StreamingTargetScene()
    {
        var targetScene = SceneManager.GetSceneByName(streamTargetScene);
        if(targetScene.isLoaded)
        {
            AsyncOperation op = SceneManager.LoadSceneAsync(streamTargetScene, LoadSceneMode.Additive);

            while(!op.isDone)
            {
                yield return null;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            var dir = Vector3.Angle(transform.forward, other.transform.position - transform.position);
            if(dir < 90f)
            {
                StartCoroutine(StreamingTargetScene());
            }
        }
    }
}
