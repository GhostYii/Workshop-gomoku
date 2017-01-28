using UnityEngine;

public class ChessModel
{
    //private int h = 1, v = 1, lb = 1, rb = 1;
    private BoardState[,] board = new BoardState[Board.CROSSCOUNT, Board.CROSSCOUNT];
    //private int[] winChessesIndex = new int[5];

    public BoardState[,] BoardData
    {
        get { return board; }
        set { board = value; }
    }

    public ChessModel()
    {
        //winChessesIndex = new int[5];
        board = new BoardState[Board.CROSSCOUNT, Board.CROSSCOUNT];
    }

    // 检查垂直方向连接情况
    int CheckVerticalLink(int px, int py, BoardState state)
    {
        //winChessesIndex[0] = px * Board.CROSSCOUNT + py;
        // 算上自己
        int linkCount = 1;

        // 朝上
        for (int y = py + 1; y < Board.CROSSCOUNT; y++)
        {
            if (BoardData[px, y] == state)
            {
                //winChessesIndex[linkCount] = px * Board.CROSSCOUNT + py;
                linkCount++;
                
                if (linkCount >= 5)
                {
                    return linkCount;
                }
            }
            else
            {
                break;
            }
        }


        // 朝下
        for (int y = py - 1; y >= 0; y--)
        {
            if (BoardData[px, y] == state)
            {
                //winChessesIndex[linkCount] = px * Board.CROSSCOUNT + py;
                linkCount++;

                if (linkCount >= 5)
                {
                    return linkCount;
                }
            }
            else
            {
                break;
            }
        }

        return linkCount;
    }

    // 检查水平方向连接情况
    int CheckHorizentalLink(int px, int py, BoardState state)
    {
        //winChessesIndex[0] = px * Board.CROSSCOUNT + py;
        int linkCount = 1;

        // 朝右
        for (int x = px + 1; x < Board.CROSSCOUNT; x++)
        {
            if (BoardData[x, py] == state)
            {
                //winChessesIndex[linkCount] = px * Board.CROSSCOUNT + py;
                linkCount++;

                if (linkCount >= 5)
                {
                    return linkCount;
                }
            }
            else
            {
                break;
            }
        }


        // 朝左
        for (int x = px - 1; x >= 0; x--)
        {
            if (BoardData[x, py] == state)
            {
                //winChessesIndex[linkCount] = px * Board.CROSSCOUNT + py;
                linkCount++;

                if (linkCount >= 5)
                {
                    return linkCount;
                }
            }
            else
            {
                break;
            }
        }

        return linkCount;
    }

    // 检查斜边情况
    int CheckBiasLink(int px, int py, BoardState state)
    {
        //winChessesIndex[0] = px * Board.CROSSCOUNT + py;
        int ret = 0;
        int linkCount = 1;

        // 左下
        for (int x = px - 1, y = py - 1; x >= 0 && y >= 0; x--, y--)
        {
            if (BoardData[x, y] == state)
            {
                //winChessesIndex[linkCount] = px * Board.CROSSCOUNT + py;
                linkCount++;

                if (linkCount >= 5)
                {
                    return linkCount;
                }
            }
            else
            {
                break;
            }
        }

        // 右上
        for (int x = px + 1, y = py + 1; x < Board.CROSSCOUNT && y < Board.CROSSCOUNT; x++, y++)
        {
            if (BoardData[x, y] == state)
            {
                //winChessesIndex[linkCount] = px * Board.CROSSCOUNT + py;
                linkCount++;

                if (linkCount >= 5)
                {
                    return linkCount;
                }
            }
            else
            {
                break;
            }
        }

        //winChessesIndex[0] = px * Board.CROSSCOUNT + py;
        ret = linkCount;
        linkCount = 1;
        // 左上
        for (int x = px - 1, y = py + 1; x >= 0 && y < Board.CROSSCOUNT; x--, y++)
        {
            if (BoardData[x, y] == state)
            {
                //winChessesIndex[linkCount] = px * Board.CROSSCOUNT + py;
                linkCount++;

                if (linkCount >= 5)
                {
                    return linkCount;
                }
            }
            else
            {
                break;
            }
        }


        // 右下
        for (int x = px + 1, y = py - 1; x < Board.CROSSCOUNT && y >= 0; x++, y--)
        {
            if (BoardData[x, y] == state)
            {
                //winChessesIndex[linkCount] = px * Board.CROSSCOUNT + py;
                linkCount++;

                if (linkCount >= 5)
                {
                    return linkCount;
                }
            }
            else
            {
                break;
            }
        }

        return Mathf.Max(ret, linkCount);
    }

    // 检查给定点周边的最大连接情况
    public int CheckLink(int px, int py, BoardState state)
    {
        int linkCount = 0;

        linkCount = Mathf.Max(CheckHorizentalLink(px, py, state), linkCount);
        linkCount = Mathf.Max(CheckVerticalLink(px, py, state), linkCount);
        linkCount = Mathf.Max(CheckBiasLink(px, py, state), linkCount);

        return linkCount;
    }

    ////获取五子连珠的索引
    ////应在有五子连珠的情况下获取
    //public int[] GetWinChessesIndex()
    //{ return winChessesIndex; }
}
