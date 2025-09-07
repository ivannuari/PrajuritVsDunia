using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;


    private AudioManager _audio;
    private AssetManager _asset;

    private void Awake()
    {
        if (Instance == null) 
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); 
        }
        DontDestroyOnLoad(gameObject);

        _audio = GetComponentInChildren<AudioManager>();
        _asset = GetComponentInChildren<AssetManager>();
    }

    public AudioManager GetAudio() { return _audio; }
    public AssetManager GetAsset() { return _asset; }
}
