using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemEffect : MonoBehaviour
{
    [SerializeField] protected int Score = 1;
    [SerializeField] protected float RecoverValue = 10f;

    public int score
    { get { return Score; } }
    public float recoverValue { get { return this.RecoverValue; } }

}
