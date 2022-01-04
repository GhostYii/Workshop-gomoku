//ORG: Ghostyii & MoonLight Game
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Sprite black;
    public Sprite white;
    public Sprite blackDown;
    public Sprite whiteDown;
    public GameObject chessman;
    public Board board;
    //[HideInInspector]
    public BoardState currentState = BoardState.Black;
    public GameObject endPanel;
    //[HideInInspector]
    public BoardState playerState = BoardState.White;
    public List<Sprite> lostImages = new List<Sprite>();
    public List<Sprite> victoryImages = new List<Sprite>();
    public List<Sprite> drawImages = new List<Sprite>();

    public Image image;
    public Text text;

    public bool playerFirst = false;

    public AI ai = new AI();
    public ChessModel bm = new ChessModel();
    private Vector2 aiPos = Vector2.zero;
    private Vector2 prevPos = Vector2.zero;

    private GameResult res = GameResult.Lost;    
    public bool isOver = false;

    static private GameManager instance = null;
    static public GameManager Instance
    {
        get { return instance == null ? new GameManager() : instance; }
        private set { instance = value; }
    }

    private void Awake()
    {
        ai = new AI();
        if (instance == null)
            instance = this;
    }
    private void OnEnable()
    {
        if (!playerFirst)
        {
            int x = Random.Range(7, 13);
            int y = Random.Range(7, 13);
            ai.BoardData[x, y] = (int)BoardState.Black;
            DoChess(x * Board.CROSSCOUNT + y, BoardState.Black);
            bm.BoardData[x, y] = BoardState.Black;
            playerState = BoardState.White;
        }
        else
        {
            playerState = BoardState.Black;
        }
    }

    private void Update()
    {
        if (isOver || CheckDraw())
        {
            endPanel.SetActive(true);
            return;
        }

        if (currentState != playerState)
        {
            aiPos = Vector2.zero;
            int x = 0, y = 0;
            //Debug.Log(board.playerPos + "\n" + ai.BoardData[(int)board.playerPos.x, (int)board.playerPos.y]);
            ai.ComputerDo((int)board.playerPos.x, (int)board.playerPos.y, out x, out y);
            aiPos.x = x;
            aiPos.y = y;
            DoChess((int)(aiPos.x * Board.CROSSCOUNT + aiPos.y), currentState);
        }

    }

    public void DoChess(int chessIndex, BoardState state)
    {
        Image image = board.girds[(int)prevPos.x * Board.CROSSCOUNT + (int)prevPos.y].GetComponent<Image>();
        if (state == BoardState.Over) return;

        image.sprite = state == BoardState.Black ? white : black;

        bm.BoardData[chessIndex / Board.CROSSCOUNT, chessIndex % Board.CROSSCOUNT] = state;

        image = board.girds[chessIndex].GetComponent<Image>();
        if (state == BoardState.Over) return;

        image.sprite = state == BoardState.Black ? blackDown : whiteDown;
        prevPos.x = chessIndex / Board.CROSSCOUNT;
        prevPos.y = chessIndex % Board.CROSSCOUNT;
        image.color = Color.white;
        //Debug.Log((int)state);

        isOver = CheckEnd(chessIndex, state);
        bool isDraw = CheckDraw();
        if (isOver || isDraw)
        {
            res = isDraw ? GameResult.Draw : ( state == playerState ? GameResult.Victory : GameResult.Lost);

            switch (res)
            {
                case GameResult.Victory:
                    SetRandomVictoryImage();
                    break;
                case GameResult.Lost:
                    SetRandomLostImages();
                    break;
                case GameResult.Draw:
                    SetRandomDrawImage();
                    break;
                default:
                    break;
            }
            endPanel.SetActive(true);
            enabled = false;
        }
        currentState = state == BoardState.Black ? BoardState.White : BoardState.Black;
    }

    public bool CheckEnd(int chessIndex, BoardState state)
    {
        //BoardModel bm = new BoardModel();
        return bm.CheckLink(chessIndex / Board.CROSSCOUNT, chessIndex % Board.CROSSCOUNT, state) >= 5;
    }

    public bool CheckDraw()
    {
        for (int h = 0; h < Board.CROSSCOUNT; h++)
            for (int v = 0; v < Board.CROSSCOUNT; v++)
                if (bm.BoardData[h, v] == BoardState.Over)
                    return false;
        return true;
    }

    public void Retry()
    {
        isOver = false;
        playerFirst = false;
        board.Reset();
        ai = new AI();
        bm = new ChessModel();
       
        endPanel.SetActive(false);
    }

    public void SetRandomLostImages()
    {
        image.sprite = lostImages[Random.Range(0, lostImages.Count)];
        text.text = "YOU LOST!";
    }

    public void SetRandomVictoryImage()
    {
        image.sprite = victoryImages[Random.Range(0, lostImages.Count)];
        text.text = "YOU WIN!";
    }

    public void SetRandomDrawImage()
    {
        image.sprite = drawImages[Random.Range(0, drawImages.Count)];
        text.text = "DRAW!";
    }

}

public enum BoardState
{
    Over = 0,
    Black,
    White,
}

public enum GameResult
{
    Victory,
    Lost,
    Draw
}
