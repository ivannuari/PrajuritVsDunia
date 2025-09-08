using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private GameState currentState;

    public event Action<GameState> OnStateChanged;

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

    public void ChangeState(GameState newState)
    {
        if (newState == currentState) { return; }
        
        currentState = newState;
        OnStateChanged?.Invoke(currentState);
    }

    public AudioManager GetAudio() { return _audio; }
    public AssetManager GetAsset() { return _asset; }
}
