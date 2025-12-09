using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SoundManager : MonoBehaviour
{
    public AudioSource bgm;

    // BGM을 재생하고 싶은 씬 이름들
    public string[] playScenes;

    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;

        // 첫 씬도 검사
        CheckSceneForBGM(SceneManager.GetActiveScene());
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        CheckSceneForBGM(scene);
    }

    void CheckSceneForBGM(Scene scene)
    {
        bool shouldPlay = false;

        foreach (string s in playScenes)
        {
            if (scene.name == s)
            {
                shouldPlay = true;
                break;
            }
        }

        if (shouldPlay)
        {
            if (!bgm.isPlaying)
                bgm.Play();
        }
        else
        {
            if (bgm.isPlaying)
                bgm.Stop();
        }
    }
}
