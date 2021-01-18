using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

static class XPManager
{
    public static int CalculateXP(Enemy e)
    {
        //근처레벨대의 몹을 사냥할 경우 평균 경험치
        int baseXP = (Player.instance.MyLevel * 5) + 45;

        int grayLevel = CalculateGrayLevel();

        int totalXP = 0;

        //몹의 레벨이 더 높거나 같은경우
        if (e.MyLevel >= Player.instance.MyLevel)
        {
            totalXP = (int)(baseXP * (1 + 0.05 * (e.MyLevel - Player.instance.MyLevel)));
        }
        //몹의 레벨이 낮은경우 
        //ex) 플레이어가 50렙일경우 grayLevel값은 40 되며, 이경우 41이상의 몹을 잡아야 경험치를 획득
        else if (e.MyLevel > grayLevel)
        {
            //295 * (1 - (50 - 41) / 15 ) = 295
            totalXP = (baseXP) * (1 - (Player.instance.MyLevel - e.MyLevel) / ZeroDifference());
        }

        return totalXP;
    }
    

    //퀘스트
    public static int CalculateXP(Quest e)
    {
        if (Player.instance.MyLevel <= e.MyLevel + 5)
        {
            return e.MyXp;
        }
        //퀘스트레벨보다 플레이어가 6높을때
        if (Player.instance.MyLevel == e.MyLevel + 6)
        {
            return (int)(e.MyXp * 0.8 / 5) * 5;
        }
        if (Player.instance.MyLevel == e.MyLevel + 7)
        {
            return (int)(e.MyXp * 0.6 / 5) * 5;
        }
        if (Player.instance.MyLevel == e.MyLevel + 8)
        {
            return (int)(e.MyXp * 0.4 / 5) * 5;
        }
        if (Player.instance.MyLevel == e.MyLevel + 9)
        {
            return (int)(e.MyXp * 0.2 / 5) * 5;
        }
        if (Player.instance.MyLevel >= e.MyLevel + 10)
        {
            return (int)(e.MyXp * 0.1 / 5) * 5;
        }

        return 0;
    }
    

    //몹의 레벨이 플레이어보다 낮은 경우
    //반환값이 클수록 플레이어가 낮은몹을 잡을때 줄어드는 경험치가 적다
    private static int ZeroDifference()
    {
        if (Player.instance.MyLevel <= 7)
        {
            return 5;
        }
        if (Player.instance.MyLevel >= 8 && Player.instance.MyLevel <= 9)
        {
            return 6;
        }
        if (Player.instance.MyLevel >= 10 && Player.instance.MyLevel <= 11)
        {
            return 7;
        }
        if (Player.instance.MyLevel >= 12 && Player.instance.MyLevel <= 15)
        {
            return 8;
        }
        if (Player.instance.MyLevel >= 16 && Player.instance.MyLevel <= 19)
        {
            return 9;
        }
        if (Player.instance.MyLevel >= 20 && Player.instance.MyLevel <= 29)
        {
            return 11;
        }
        if (Player.instance.MyLevel >= 30 && Player.instance.MyLevel <= 39)
        {
            return 12;
        }
        if (Player.instance.MyLevel >= 40 && Player.instance.MyLevel <= 44)
        {
            return 13;
        }
        if (Player.instance.MyLevel >= 45 && Player.instance.MyLevel <= 49)
        {
            return 14;
        }
        if (Player.instance.MyLevel >= 50 && Player.instance.MyLevel <= 54)
        {
            return 15;
        }
        if (Player.instance.MyLevel >= 55 && Player.instance.MyLevel <= 59)
        {
            return 16;
        }

        return 17;

    }

    public static int CalculateGrayLevel()
    {
        if (Player.instance.MyLevel <= 5)
        {
            return 0;
        }
        //6레벨 ~ 49 레벨의 경우 -> 1레벨 이상 ~ 40랩 사냥
        else if (Player.instance.MyLevel >= 6 && Player.instance.MyLevel <= 49)
        {
            return Player.instance.MyLevel - (Player.instance.MyLevel / 10) - 5;
        }
        //50랩일경우 41랩 이상 사냥
        else if (Player.instance.MyLevel == 50)
        {
            return Player.instance.MyLevel - 10;
        }
        else if (Player.instance.MyLevel >= 51 && Player.instance.MyLevel <= 59)
        {
            return Player.instance.MyLevel - (Player.instance.MyLevel / 5) - 1;
        }

        return Player.instance.MyLevel - 9;
    }
}