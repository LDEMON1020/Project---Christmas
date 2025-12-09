using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    public Slider volumeSlider;

    void Start()
    {
        // 슬라이드 초기값을 현재 볼륨으로 설정
        volumeSlider.value = AudioListener.volume;

        // 슬라이드 값이 바뀌면 AudioManager 호출
        volumeSlider.onValueChanged.AddListener((value) =>
        {
            AudioManager.Instance.SetVolume(value);
        });
    }
}