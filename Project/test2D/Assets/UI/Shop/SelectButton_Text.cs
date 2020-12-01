using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectButton_Text : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshPro select_text;

    // Start is called before the first frame update
    void Start()
    {
        select_text = this.GetComponent<TMPro.TextMeshPro>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
