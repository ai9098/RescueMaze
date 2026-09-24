using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    // オーディオミキサー
    [SerializeField] private AudioMixer audioMixer;
 
    // スライダーUI
    [SerializeField] private Slider BGMSlider;
    [SerializeField] private Slider SESlider;

    // セーブ用キー名
    private const string BGM_KEY = "BGM_Volume_Key";
    private const string SE_KEY = "SE_Volume_Key";

    private void Start()
    {
        // 保存された音量をロード
        // 初期値は0.8f
        float savedBGM = PlayerPrefs.GetFloat(BGM_KEY, 0.8f);
        float savedSE = PlayerPrefs.GetFloat(SE_KEY, 0.8f);

        if (BGMSlider != null)
        {
            BGMSlider.value = savedBGM;

            // スライダーの値をボリュームとして参照
            SetBGMVolume(savedBGM);
            // スライダーの値が変わった時にSetSEVolumeを呼び出すリスナーを登録
            BGMSlider.onValueChanged.AddListener(SetBGMVolume);
        }

        if (SESlider != null)
        {
            SESlider.value = savedSE;

            // スライダーの値をボリュームとして参照
            SetSEVolume(savedSE);
            // スライダーの値が変わった時にSetSEVolumeを呼び出すリスナーを登録
            SESlider.onValueChanged.AddListener(SetSEVolume);
        }
    }

    // BGM音量
    public void SetBGMVolume(float volume)
    {
        // 0f以下に設定されたら
        if (volume <= 0f)
        {
            // BGM_Volumeの値を無音に
            audioMixer.SetFloat("BGM_Volume", -80f);
        }
        else
        {
            // デシベル変換 (スライダーの値：0.0001〜1.0 想定)
            audioMixer.SetFloat("BGM_Volume", Mathf.Log10(volume) * 20);
        }

        // 値をパソコンに保存
        PlayerPrefs.SetFloat(BGM_KEY, volume);
        PlayerPrefs.Save();
    }

    // SE音量
    public void SetSEVolume(float volume)
    {
        // 0f以下に設定されたら
        if (volume <= 0f)
        {
            // SE_Volumeの値を無音に
            audioMixer.SetFloat("SE_Volume", -80f);
        }
        else
        {
            // デシベル変換 (スライダーの値：0.0001〜1.0 想定)
            audioMixer.SetFloat("SE_Volume", Mathf.Log10(volume) * 20);
        }

        // 値をパソコンに保存
        PlayerPrefs.SetFloat(SE_KEY, volume);
        PlayerPrefs.Save();
    }
}
