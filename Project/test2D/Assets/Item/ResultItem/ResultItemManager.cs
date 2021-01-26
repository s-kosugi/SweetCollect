using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultItemManager : MonoBehaviour
{
    [SerializeField] float popInterval = 0.5f;
    [SerializeField] float popMinX = -200.0f;
    [SerializeField] float popMaxX = 200.0f;
    [SerializeField] float popY = 200.0f;
    [SerializeField] GameObject itemCandy = default;
    [SerializeField] GameObject itemPudding = default;
    [SerializeField] GameObject itemCake = default;
    [SerializeField] GameObject itemTaiyaki = default;

    float popCount = 0f;

    enum ITEM_TYPE
    {
        CANDY,
        PUDDING,
        CAKE,
        TAIYAKI,

        MAX,
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        popCount += Time.deltaTime;
        if (popCount >= popInterval)
        {
            popCount = 0f;

            ITEM_TYPE type = (ITEM_TYPE)Random.Range(0,(float)ITEM_TYPE.MAX);
            GameObject obj = default;
            switch (type)
            {
                case ITEM_TYPE.CANDY: obj = Instantiate(itemCandy,this.transform); break;
                case ITEM_TYPE.PUDDING: obj = Instantiate(itemPudding, this.transform); break;
                case ITEM_TYPE.CAKE: obj = Instantiate(itemCake, this.transform); break;
                case ITEM_TYPE.TAIYAKI: obj = Instantiate(itemTaiyaki, this.transform); break;
            }
            if (obj != default)
            {
                obj.transform.position = new Vector3(Random.Range(popMinX,popMaxX),popY,this.transform.position.z);
            }
        }
    }
}
