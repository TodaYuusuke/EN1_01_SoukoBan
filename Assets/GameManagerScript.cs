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
        // �E�ړ�
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            int playerIndex = GetPlayerIndex();

            // �ړ��������֐���
            MoveNumber(1, playerIndex, playerIndex + 1);
        }
		// ���ړ�
		if (Input.GetKeyDown(KeyCode.LeftArrow))
		{
			int playerIndex = GetPlayerIndex();

			// �ړ��������֐���
			MoveNumber(1, playerIndex, playerIndex - 1);
		}


		PrintArray();
	}

    /// <summary>
    /// �z��̕\��
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
    /// �v���C���[�̃C���f�b�N�X���擾���郁�\�b�h
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
    /// �ړ��\���ǂ�����Ԃ����\�b�h
    /// </summary>
    /// <param name="number"></param>
    /// <param name="moveFrom"></param>
    /// <param name="moveTo"></param>
    /// <returns></returns>
    bool MoveNumber(int number, int moveFrom, int moveTo)
    {
        if(moveTo < 0 || moveTo >= map.Length) { return false; }
        // �ړ���ɔ���������
        if (map[moveTo] == 2)
        {
            // �ǂ̕����ֈړ����邩���Z�o
            int velocity = moveTo - moveFrom;
            // �v���C���[�̈ړ��悩�炳��ɐ��2(��)���ړ�������
            // ���̈ړ������AMoveNumber���\�b�h����MoveNumber���\�b�h���ĂсA�������ċN���Ă���A�ړ��s��bool�ŋL�^
            bool success = MoveNumber(2, moveTo, moveTo + velocity);
            // ���������ړ����s������A�v���C���[�̈ړ������s
            if(!success) { return false; }
        }

        map[moveTo] = number;
        map[moveFrom] = 0;
        return true;
    }

}
