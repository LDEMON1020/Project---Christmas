using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneChanger : MonoBehaviour
{
    public AudioSource audioSource;   // 버튼 클릭 사운드
    public AudioClip clickSound;      // 재생할 사운드 클립

    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneAfterSound(sceneName));
    }

    private System.Collections.IEnumerator LoadSceneAfterSound(string sceneName)
    {
        // 사운드 재생
        if (audioSource != null && clickSound != null)
        {
            audioSource.PlayOneShot(clickSound);

            // 사운드 길이만큼 대기
            yield return new WaitForSeconds(clickSound.length);
        }

        // 씬 로드
        SceneManager.LoadScene(sceneName);
        Time.timeScale = 1f;
    }
}
