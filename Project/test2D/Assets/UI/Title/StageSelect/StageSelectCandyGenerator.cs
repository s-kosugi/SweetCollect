using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSelectCandyGenerator : MonoBehaviour
{
    [SerializeField] int EasyGameLevel = 1;
    [SerializeField] int NormalGameLevel = 3;
    [SerializeField] int HardGameLevel = 5;
    [SerializeField] int VeryHardGameLevel = 10;
    [SerializeField] StageSelectParent parent = default;
    [SerializeField] GameObject LevelCandyObject = default;
    [SerializeField] int CandySize = 32;
    float rectWidth = 0f;
    string oldDifficutName = default;


    void Start()
    {
        rectWidth = GetComponent<RectTransform>().sizeDelta.x;
    }


    void Update()
    {
        if (oldDifficutName != parent.difficultName)
        {
            // 子の全削除
            foreach (Transform n in transform)
            {
                GameObject.Destroy(n.gameObject);
            }
            int CandyNum = 1;
            switch (parent.difficultName)
            {
                case DifficultName.EASY: CandyNum = EasyGameLevel; break;
                case DifficultName.NORMAL: CandyNum = NormalGameLevel; break;
                case DifficultName.HARD: CandyNum = HardGameLevel; break;
                case DifficultName.VERYHARD: CandyNum = VeryHardGameLevel; break;
            }

            float StartXPos = -rectWidth / 2.0f + CandySize / 2.0f;

            // レベルの数だけキャンディを生成する
            for (int i = 0; i < CandyNum; i++)
            {
                GameObject candy = Instantiate(LevelCandyObject, this.transform);

                // キャンディの数が多い場合は重ねて表示する
                if (CandySize * CandyNum > rectWidth)
                {
                    candy.transform.localPosition = new Vector3(StartXPos + i * ((rectWidth - CandySize / 2) / CandyNum), 0);
                }
                else
                {
                    candy.transform.localPosition = new Vector3(StartXPos + i * CandySize, 0);
                }
            }

            oldDifficutName = parent.difficultName;
        }
    }
}
