//ORG: Ghostyii & MoonLight Game
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIEvents : MonoBehaviour
{
    public GameManager gameManager;

    public void LoadLevel(int index)
    {
        SceneManager.LoadScene(index);
    }

    public void LoadLevel(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }

    public void StartClick(bool playerFirst)
    {
        gameManager.playerFirst = playerFirst;
        gameManager.enabled = true;
    }

    public void RetryClick()
    {
        gameManager.isOver = false;
        gameManager.playerFirst = false;
        gameManager.board.Reset();
        gameManager.ai = new AI();
        gameManager.bm = new ChessModel();

        gameManager.endPanel.SetActive(false);
    }

}