using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroUIManager : MonoBehaviour
{
    public Animator subAnimator;

    private void Awake()
    {
        //subAnimator.enabled = false;
        if (GlobalGameManager.Instance.gameWon)//use loop animation
        {
            subAnimator.SetBool("recover", true);
        }

        StartCoroutine(GlobalGameManager.Instance.DoTransition(TransitionDirection.OUT, () => {  }));

    }

    public void LoadGameScene()
    {
        StartCoroutine(GlobalGameManager.Instance.DoTransition(TransitionDirection.IN, () =>
        {
            SceneManager.LoadSceneAsync(GlobalGameManager.GAMEPLAY_SCENE_NAME);
        }));
    }

    public void PlayCrashSFX()
    {
        GlobalGameManager.Instance.soundManager.PlaySound(SFXType.SubmarineCrash);
    }

    public void PlayRecoverSFX()
    {
        GlobalGameManager.Instance.soundManager.PlaySound(SFXType.SubmarineRecover);
    }

}
