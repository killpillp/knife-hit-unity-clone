using UnityEngine;

public class GameMaster : MonoBehaviour{
    public static GameMaster instance;
    private void Awake()
    {
        if (instance == null) instance = this;
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }
}
