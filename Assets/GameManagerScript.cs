using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{

    int[] map;

    // Start is called before the first frame update
    void Start()
    {
        map = new int[] { 0, 0, 0, 1, 0, 2, 0, 2, 0 };
		// Debug.Log("Hello World");
		PrintArray();
	}

    // Update is called once per frame
    void Update()
    {
        // 右移動
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            int playerIndex = GetPlayerIndex();

            // 移動処理を関数化
            MoveNumber(1, playerIndex, playerIndex + 1);
        }
		// 左移動
		if (Input.GetKeyDown(KeyCode.LeftArrow))
		{
			int playerIndex = GetPlayerIndex();

			// 移動処理を関数化
			MoveNumber(1, playerIndex, playerIndex - 1);
		}


		PrintArray();
	}

    /// <summary>
    /// 配列の表示
    /// </summary>
    void PrintArray()
	{
		string debugText = "";

		for (int i = 0; i < map.Length; i++)
		{
			debugText += map[i].ToString() + ",";
		}

		Debug.Log(debugText);
	}

    /// <summary>
    /// プレイヤーのインデックスを取得するメソッド
    /// </summary>
    /// <returns></returns>
    int GetPlayerIndex()
    {
        for(int i = 0; i < map.Length; i++)
        {
            if (map[i] == 1)
            {
                return i;
            }
        }

        return -1;
    }

    /// <summary>
    /// 移動可能かどうかを返すメソッド
    /// </summary>
    /// <param name="number"></param>
    /// <param name="moveFrom"></param>
    /// <param name="moveTo"></param>
    /// <returns></returns>
    bool MoveNumber(int number, int moveFrom, int moveTo)
    {
        if(moveTo < 0 || moveTo >= map.Length) { return false; }
        // 移動先に箱がいたら
        if (map[moveTo] == 2)
        {
            // どの方向へ移動するかを算出
            int velocity = moveTo - moveFrom;
            // プレイヤーの移動先からさらに先へ2(箱)を移動させる
            // 箱の移動処理、MoveNumberメソッド内でMoveNumberメソッドを呼び、処理が再起している、移動可不可をboolで記録
            bool success = MoveNumber(2, moveTo, moveTo + velocity);
            // もし箱が移動失敗したら、プレイヤーの移動も失敗
            if(!success) { return false; }
        }

        map[moveTo] = number;
        map[moveFrom] = 0;
        return true;
    }

}
