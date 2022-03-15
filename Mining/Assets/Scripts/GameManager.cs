using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] Sprite[] trueBreakTextures;
    public static Sprite[] blockBreakingTextures;

    private void Awake()
    {
        blockBreakingTextures = trueBreakTextures;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            SceneManager.LoadScene(0);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}
