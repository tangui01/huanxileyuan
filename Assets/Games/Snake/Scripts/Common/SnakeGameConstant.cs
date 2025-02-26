using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/****************************************************
    文件：Constant.cs
    作者：tg
    邮箱: 1075670319@qq.com
    日期：2024/11/19 11:50
    功能：存储常量和方法
*****************************************************/
public class SnakeGameConstant 
{
    //食物生成位置和移动速度和旋转速度限制
    public static float minimumFieldX = -73.5f;
    public static float minimumFieldY = -73.5f;
    public static float maximumFieldX = 73.5f;
    public static float maximumFieldY = 73.5f;
    public static float moveSpeedMax = 8f;
    public static float moveSpeedBoostMax = 2.0f;
    public static float rotationSpeedBoostPenalty = 0.6f;
    //与蛇相关
    public static int defaultSnakeCount = 15;                   //When the game begins, this many bot snakes will be created.
    public static int initialSnakeCount;
    public static int foodIntoBodypart = 4;                     //This much food is needed to be collected in order to grow a new bodypart unit
    public static int ghostfoodIntoBodypart = 2;                //This much ghost-food is needed to be collected in order to grow a new bodypart unit//Each new snake object should contain this many bodyparts when created
    public static int initialBodyParts= 5;
    public static int minimumSnakeBodyparts;                    //When we advance in game, we need to create snakes with more initial bodyparts. This variable holds the additional number
    public static float bodypartsFollowDelayNormal = 0.12f;     //Default = 0.09f
    public static float bodypartsFollowDelayBoost =  0.08f;      //Default = 0.08f
    public static int maxSnakeSizeForScale = 50;
    
    public static float maxDistanceToTriggerShake = 20f;
    public static int GetInitialBodyparts()
    {
        return initialBodyParts + minimumSnakeBodyparts;
    }
    public static int GetRandomInitialBodyparts()
    {
        //For a very rare chance, spawn huge snakes
        if (Random.value > 0.98f)
            return Random.Range(30, 80);

        return Random.Range(1, 12);
    }
    public static Vector3 GetRandomPositionInMap()
    {
        return new Vector3(Random.Range(minimumFieldX, maximumFieldX), Random.Range(minimumFieldY, maximumFieldY), 0);
    }
    //预制体路径
    public static string PlayBodyPre = "Prefabs/Snakes/PlayerBody-01";
    public static string BotBodyPre = "Prefabs/Snakes/BotBody-01";
    public static string BotHeadPre = "Prefabs/Snakes/BotHead-01";
    public static string FoodPre = "Prefabs/Foods/Food-01";
    public static string GameConfPath = "Data/Game Conf"; 
    public static string AddScoreUIPath = "Prefabs/Vfx/AddScoreUI";
    //预制体名字
    public static string PlayBodyPreName = "PlayBodyPre";
    public static string BotBodyPreName = "BotBody";
    public static string BotHeadPreName = "BotHead";
    public static string FoodPreName = "Food";
    public static string AddScoreUIName = "AddScoreUI";

}
