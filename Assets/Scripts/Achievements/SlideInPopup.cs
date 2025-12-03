using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SlideInPopup : MonoBehaviour
{
    public float slideDuration = 0.4f;      // 슬라이드 들어오는 시간
    public float stayDuration = 1.5f;       // 화면에 머무르는 시간
    public float slideOutDuration = 0.3f;   // 슬라이드 나가는 시간

    RectTransform rect;

    Vector2 startPos;   // 화면 밖 위치
    Vector2 endPos;     // 화면 안 위치

    void Awake()
    {
        rect = GetComponent<RectTransform>();

        // 화면 왼쪽 밖 위치 계산
        float width = rect.rect.width;
        startPos = new Vector2(-width - 50f, rect.anchoredPosition.y);

        // 화면 안쪽 원래 위치 저장
        endPos = new Vector2(30f, rect.anchoredPosition.y);

        rect.anchoredPosition = startPos;
    }

    void Start()
    {
        StartCoroutine(SlideRoutine());
    }

    IEnumerator SlideRoutine()
    {
        // 1) 슬라이드 인
        float t = 0;
        while (t < slideDuration)
        {
            t += Time.deltaTime;
            float p = Mathf.Clamp01(t / slideDuration);
            rect.anchoredPosition = Vector2.Lerp(startPos, endPos, p);
            yield return null;
        }

        // 2) 일정 시간 머무름
        yield return new WaitForSeconds(stayDuration);

        // 3) 슬라이드 아웃
        t = 0;
        while (t < slideOutDuration)
        {
            t += Time.deltaTime;
            float p = Mathf.Clamp01(t / slideOutDuration);
            rect.anchoredPosition = Vector2.Lerp(endPos, startPos, p);
            yield return null;
        }

        Destroy(gameObject);
    }
}
