using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
	// プレイヤーのプレハブ
	public GameObject playerPrefab;
	// ボックスのプレハブ
	public GameObject boxPrefab;
	// ゴールのプレハブ
	public GameObject goalPrefab;

	// クリアテキスト
	public GameObject clearText;


	// レベルデザイン用の配列
	// 1 ... プレイヤー
	// 2 ... 箱
	// 3 ... ゴール
	int[,] map;
	// ゲーム管理用の配列
	GameObject[,] field;

	// Start is called before the first frame update
	void Start()
	{
		map = new int[,] {
			{ 3, 0, 2, 0, 0 },
			{ 0, 0, 0, 2, 0 },
			{ 0, 2, 1, 0, 0 },
			{ 0, 0, 0, 3, 0 },
			{ 0, 0, 0, 0, 3 }
		};
		field = new GameObject[
			map.GetLength(0), 
			map.GetLength(1)
		];

		// プレイヤーとボックスのプレハブを追加
		for (int y = 0; y < map.GetLength(0); y++)
		{
			for (int x = 0; x < map.GetLength(1); x++)
			{
				switch (map[y, x])
				{
					case 1:
						field[y, x] = Instantiate(
							playerPrefab,
							new Vector3(x - (int)(map.GetLength(1) / 2.0f), -y + (int)(map.GetLength(0) / 2.0f), 0),
							Quaternion.identity
						);
						break;
					case 2:
						field[y, x] = Instantiate(
							boxPrefab,
							new Vector3(x - (int)(map.GetLength(1) / 2.0f), -y + (int)(map.GetLength(0) / 2.0f), 0),
							Quaternion.identity
						);
						break;
					case 3:
						Instantiate(
							goalPrefab,
							new Vector3(x - (int)(map.GetLength(1) / 2.0f), -y + (int)(map.GetLength(0) / 2.0f), 0),
							Quaternion.identity
						);
						break;
					default:
						break;
				}
			}
		}
	}

	// Update is called once per frame
	void Update()
	{
		// 上移動
		if (Input.GetKeyDown(KeyCode.UpArrow))
		{
			Vector2Int playerIndex = GetPlayerIndex();

			//// 移動処理を関数化
			MoveNumber("Player", playerIndex, new Vector2Int(playerIndex.x, playerIndex.y - 1));
		}
		// 下移動
		if (Input.GetKeyDown(KeyCode.DownArrow))
		{
			Vector2Int playerIndex = GetPlayerIndex();

			//// 移動処理を関数化
			MoveNumber("Player", playerIndex, new Vector2Int(playerIndex.x, playerIndex.y + 1));
		}
		// 左移動
		if (Input.GetKeyDown(KeyCode.LeftArrow))
		{
			Vector2Int playerIndex = GetPlayerIndex();

			//// 移動処理を関数化
			MoveNumber("Player", playerIndex, new Vector2Int(playerIndex.x - 1, playerIndex.y));
		}
		// 右移動
		if (Input.GetKeyDown(KeyCode.RightArrow))
		{
			Vector2Int playerIndex = GetPlayerIndex();

			//// 移動処理を関数化
			MoveNumber("Player", playerIndex, new Vector2Int(playerIndex.x + 1, playerIndex.y));
		}



		// クリア判定
		if (isCleard())
		{
			clearText.SetActive(true);
		}
	}



	/// <summary>
	/// プレイヤーのインデックスを取得するメソッド
	/// </summary>
	/// <returns></returns>
	Vector2Int GetPlayerIndex()
	{
		for (int y = 0; y < map.GetLength(0); y++) {
			for (int x = 0; x < map.GetLength(1); x++) {
				if (field[y, x] != null && field[y, x].tag == "Player")
				{
					return new Vector2Int(x, y);
				}
			}
		}

		return new Vector2Int(-1, -1);
	}

	/// <summary>
	/// 
	/// </summary>
	/// <returns></returns>
	bool isCleard()
	{
		for (int y = 0; y < map.GetLength(0); y++) {
			for (int x = 0; x < map.GetLength(1); x++) {
				// 格納場所か否かを判断
				if (map[y, x] == 3) {
					if (field[y, x] == null || field[y, x].tag != "Box") {
						// 一つでも箱がなかったら条件未達成なのでfalseを返す
						return false;
					}
				}
			}
		}

		return true;
	}



	/// <summary>
	/// 移動可能かどうかを返すメソッド
	/// </summary>
	/// <param name="number"></param>
	/// <param name="moveFrom"></param>
	/// <param name="moveTo"></param>
	/// <returns></returns>
	bool MoveNumber(string tag, Vector2Int moveFrom, Vector2Int moveTo)
	{
		if (moveTo.x < 0 || moveTo.x >= map.GetLength(1)) { return false; }
		if (moveTo.y < 0 || moveTo.y >= map.GetLength(0)) { return false; }

		// 移動先に箱がいたら
		if (field[moveTo.y, moveTo.x] != null && field[moveTo.y, moveTo.x].tag == "Box")
		{
			// どの方向へ移動するかを算出
			Vector2Int velocity = moveTo - moveFrom;
			// プレイヤーの移動先からさらに先へ2(箱)を移動させる
			// 箱の移動処理、MoveNumberメソッド内でMoveNumberメソッドを呼び、処理が再起している、移動可不可をboolで記録
			bool success = MoveNumber("Box", moveTo, moveTo + velocity);
			// もし箱が移動失敗したら、プレイヤーの移動も失敗
			if (!success) { return false; }
		}

		field[moveFrom.y, moveFrom.x].transform.position = new Vector3(moveTo.x - (int)(map.GetLength(1) / 2.0f), -moveTo.y + (int)(map.GetLength(0) / 2.0f), 0);
		field[moveTo.y, moveTo.x] = field[moveFrom.y, moveFrom.x];

		field[moveFrom.y, moveFrom.x] = null;
		return true;
	}

}
