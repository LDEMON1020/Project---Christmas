using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainSceneManager : MonoBehaviour
{
    public GameObject targetObject; // 클릭 허용할 오브젝트만 넣기
    public string sceneName; // 로드할 씬 이름
    public AudioSource clickSound;

    void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        if (Input.GetMouseButtonDown(0))
        {
            Vector2 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(worldPos, Vector2.zero);

            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                // 코루틴 실행: 사운드 재생 → 완료 후 씬 이동
                StartCoroutine(PlaySoundThenLoad());
            }
        }
    }

    private System.Collections.IEnumerator PlaySoundThenLoad()
    {
        if (clickSound != null)
        {
            clickSound.Play();
            yield return new WaitForSeconds(clickSound.clip.length);
        }

        SceneManager.LoadScene(sceneName);
    }
}
