using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Loading : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GlobalGameManager.Instance.DoTransition(TransitionDirection.IN, () =>
        {
            SceneManager.LoadSceneAsync(GlobalGameManager.INTRO_SCENE_NAME);
        }));
    }
}
