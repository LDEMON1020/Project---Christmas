using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainSceneManager : MonoBehaviour
{
    public GameObject targetObject; // 클릭 허용할 오브젝트만 넣기
    public string sceneName; // 로드할 씬 이름

    void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        if (Input.GetMouseButtonDown(0))
        {
            Vector2 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(worldPos, Vector2.zero);

            if (hit.collider != null)
            {
                // 클릭된 오브젝트가 targetObject인지 확인
                if (hit.collider != null && hit.collider.gameObject == gameObject)
                {
                    SceneManager.LoadScene(sceneName);
                }
            }
        }
    }
}
