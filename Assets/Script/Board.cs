//ORG: Ghostyii & MoonLight Game
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class Board : MonoBehaviour
{
    public const int CROSSCOUNT = 19;
    public List<Button> girds = new List<Button>();
    public AudioClip chessDown;
    [HideInInspector]   
    public Vector2 playerPos = Vector2.zero;

    public void SingleBoardClick(Button b)
    {        
        playerPos.x = (int)(girds.IndexOf(b) / CROSSCOUNT);
        playerPos.y = (int)(girds.IndexOf(b) % CROSSCOUNT);

        if (GameManager.Instance.ai.HasChess((int)playerPos.x, (int)playerPos.y))
            return;

        //AudioSource.PlayClipAtPoint(chessDown, Vector3.zero);

        if (GameManager.Instance.currentState == BoardState.Over) return;
        //Debug.Log(girds.IndexOf(b) + " " + (int)GameManager.Instance.playerState);
        GameManager.Instance.DoChess(girds.IndexOf(b), GameManager.Instance.playerState);


    }

    public void Reset()
    {
        foreach (Button b in girds)
        {
            Image i = b.GetComponent<Image>();
            i.color = Color.clear;
            i.sprite = null;
        }
    }
}
