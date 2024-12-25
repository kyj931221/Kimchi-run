using UnityEngine;

public enum GameState
{
    Intro,

    Playing,

    Dead
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameState state = GameState.Intro; 

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
