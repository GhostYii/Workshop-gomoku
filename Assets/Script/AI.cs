public class AI
{
    // 19*19共有19 * 15 *2 + 15 * 15 * 2种五子连珠的可能性
    const int MaxFiveChainCount = 1020;

    //玩家的可能性
    bool[,,] playerTable = new bool[Board.CROSSCOUNT, Board.CROSSCOUNT, MaxFiveChainCount];

    //电脑的可能性
    bool[,,] conputerTable = new bool[Board.CROSSCOUNT, Board.CROSSCOUNT, MaxFiveChainCount];

    //记录2位玩家所有可能的连珠数，-1则为永远无法5连珠
    int[,] win = new int[2, MaxFiveChainCount];

    //记录每格的分值
    int[,] cGrades = new int[Board.CROSSCOUNT, Board.CROSSCOUNT];
    int[,] pGrades = new int[Board.CROSSCOUNT, Board.CROSSCOUNT];

    //记录棋盘
    int[,] board = new int[Board.CROSSCOUNT, Board.CROSSCOUNT];

    int cGrade, pGrade;
    int iCount, m, n;
    int mat, nat, mde, nde;

    public int[,] BoardData
    {
        get { return board; }
        set { board = value; }
    }

    public AI()
    {
        for (int i = 0; i < Board.CROSSCOUNT; i++)
        {
            for (int j = 0; j < Board.CROSSCOUNT; j++)
            {
                pGrades[i, j] = 0;
                cGrades[i, j] = 0;
                board[i, j] = 0;
            }
        }
        //board[9, 9] = 1;

        //遍历所有的五连子可能情况的权值  
        //横  
        for (int i = 0; i < Board.CROSSCOUNT; i++)
        {
            for (int j = 0; j < Board.CROSSCOUNT - 4; j++)
            {
                for (int k = 0; k < 5; k++)
                {
                    playerTable[j + k, i, iCount] = true;
                    conputerTable[j + k, i, iCount] = true;
                }

                iCount++;
            }
        }

        //横  
        for (int i = 0; i < Board.CROSSCOUNT; i++)
        {
            for (int j = 0; j < Board.CROSSCOUNT - 4; j++)
            {
                for (int k = 0; k < 5; k++)
                {
                    playerTable[i, j + k, iCount] = true;
                    conputerTable[i, j + k, iCount] = true;
                }

                iCount++;
            }
        }

        // 右斜
        for (int i = 0; i < Board.CROSSCOUNT - 4; i++)
        {
            for (int j = 0; j < Board.CROSSCOUNT - 4; j++)
            {
                for (int k = 0; k < 5; k++)
                {
                    playerTable[j + k, i + k, iCount] = true;
                    conputerTable[j + k, i + k, iCount] = true;
                }

                iCount++;
            }
        }

        // 左斜
        for (int i = 0; i < Board.CROSSCOUNT - 4; i++)
        {
            for (int j = Board.CROSSCOUNT - 1; j >= 4; j--)
            {
                for (int k = 0; k < 5; k++)
                {
                    playerTable[j - k, i + k, iCount] = true;
                    conputerTable[j - k, i + k, iCount] = true;
                }

                iCount++;
            }
        }

        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < MaxFiveChainCount; j++)
            {
                win[i, j] = 0;
            }
        }

        iCount = 0;
    }

    void CalcScore()
    {
        cGrade = 0;
        pGrade = 0;
        board[m, n] = 1;//电脑下子位置     

        for (int i = 0; i < MaxFiveChainCount; i++)
        {
            if (conputerTable[m, n, i] && win[0, i] != -1)
            {
                win[0, i]++;//给白子的所有五连子可能的加载当前连子数  
            }

            if (playerTable[m, n, i])
            {
                playerTable[m, n, i] = false;
                win[1, i] = -1;
            }
        }
    }

    void CalcCore()
    {
        //遍历棋盘上的所有坐标  
        for (int i = 0; i < Board.CROSSCOUNT; i++)
        {
            for (int j = 0; j < Board.CROSSCOUNT; j++)
            {
                //该坐标的黑子奖励积分清零  
                pGrades[i, j] = 0;

                //在还没下棋子的地方遍历  
                if (board[i, j] == 0)
                {
                    //遍历该棋盘可落子点上的黑子所有权值的连子情况，并给该落子点加上相应奖励分  
                    for (int k = 0; k < MaxFiveChainCount; k++)
                    {
                        if (playerTable[i, j, k])
                        {
                            switch (win[1, k])
                            {
                                case 1://一连子  
                                    pGrades[i, j] += 5;
                                    break;
                                case 2://两连子  
                                    pGrades[i, j] += 50;
                                    break;
                                case 3://三连子  
                                    pGrades[i, j] += 180;
                                    break;
                                case 4://四连子  
                                    pGrades[i, j] += 400;
                                    break;
                            }
                        }
                    }

                    cGrades[i, j] = 0;//该坐标的白子的奖励积分清零  
                    if (board[i, j] == 0)//在还没下棋子的地方遍历  
                    {
                        //遍历该棋盘可落子点上的白子所有权值的连子情况，并给该落子点加上相应奖励分  
                        for (int k = 0; k < MaxFiveChainCount; k++)
                        {
                            if (conputerTable[i, j, k])
                            {
                                switch (win[0, k])
                                {
                                    case 1://一连子  
                                        cGrades[i, j] += 5;
                                        break;
                                    case 2: //两连子  
                                        cGrades[i, j] += 52;
                                        break;
                                    case 3://三连子  
                                        cGrades[i, j] += 130;
                                        break;
                                    case 4: //四连子  
                                        cGrades[i, j] += 10000;
                                        break;
                                }
                            }
                        }

                    }


                }
            }
        }

    }

    void SetPlayerPiece(int playerX, int playerY)
    {
        int m = playerX;
        int n = playerY;

        if (board[m, n] == 0)
        {
            board[m, n] = 2;

            for (int i = 0; i < MaxFiveChainCount; i++)
            {
                if (playerTable[m, n, i] && win[1, i] != -1)
                {
                    win[1, i]++;
                }
                if (conputerTable[m, n, i])
                {
                    conputerTable[m, n, i] = false;
                    win[0, i] = -1;
                }
            }
        }
    }

    // AI计算输出, 需要玩家走过的点
    public void ComputerDo(int playerX, int playerY, out int finalX, out int finalY)
    {
        SetPlayerPiece(playerX, playerY);

        CalcCore();

        for (int i = 0; i < Board.CROSSCOUNT; i++)
        {
            for (int j = 0; j < Board.CROSSCOUNT; j++)
            {
                //找出棋盘上可落子点的黑子白子的各自最大权值，找出各自的最佳落子点 
                if (board[i, j] == 0)
                {
                    if (cGrades[i, j] >= cGrade)
                    {
                        cGrade = cGrades[i, j];
                        mat = i;
                        nat = j;
                    }

                    if (pGrades[i, j] >= pGrade)
                    {
                        pGrade = pGrades[i, j];
                        mde = i;
                        nde = j;
                    }

                }
            }
        }

        //如果白子的最佳落子点的权值比黑子的最佳落子点权值大，则电脑的最佳落子点为白子的最佳落子点，否则相反  
        if (cGrade >= pGrade)
        {
            m = mat;
            n = nat;
        }
        else
        {
            m = mde;
            n = nde;
        }


        CalcScore();

        finalX = m;
        finalY = n;
    }

    //返回该位置是否可以下子
    public bool HasChess(int x, int y)
    {
        return board[x, y] != 0;
    }
}

