using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingButton : MonoBehaviour
{
    public GameObject Setting; // 클릭 허용할 오브젝트만 넣기
    public GameObject Settingpanel;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(worldPos, Vector2.zero);

            if (hit.collider != null)
            {
                // 클릭된 오브젝트가 targetObject인지 확인
                if (hit.collider.gameObject == Setting)
                {
                        Settingpanel.SetActive(true);
                }
            }
        }
    }
}
