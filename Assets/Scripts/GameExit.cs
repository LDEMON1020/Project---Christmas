using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameExit : MonoBehaviour
{
    public void QuitGame()
    {
#if UNITY_EDITOR
        // 에디터에서 테스트할 때 플레이 모드 종료
        UnityEditor.EditorApplication.isPlaying = false;
#else
            // 실제 빌드(Windows/Mac/Linux/모바일)에서는 애플리케이션 종료
            Application.Quit();
#endif
    }
}
