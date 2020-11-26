using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalcDamage : MonoBehaviour
{
    [SerializeField] private float MAXHP = 1000;
    [SerializeField] private float HP = 1000;
    [SerializeField] private float ATK = 100;

    public float maxhp { get { return this.MAXHP; } private set { } }
    public float hp { get { return this.HP; } private set { } }
    public float atk {  get { return this.ATK; } private set { } }

    public enum DAMAGE_STATE
    {
        NORMAL,
        DAMAGE,
        FLASH,
        DEAD,
    }
    [SerializeField] private DAMAGE_STATE m_State;
    public DAMAGE_STATE state
    {
        get { return this.m_State; }
        set { this.m_State = value; }
    }

    [SerializeField] private float MaxFlashTime = 2.0f;
    private float m_FlashCount = 0;

    void Start()
    {
        HP = MAXHP;
    }


    void Update()
    {
        switch (m_State)
        {
            case DAMAGE_STATE.NORMAL: NormalState(); break;
            case DAMAGE_STATE.DAMAGE: DamageState(); break;
            case DAMAGE_STATE.FLASH: FlashState(); break;
            case DAMAGE_STATE.DEAD: DeadState(); break;
        }
    }
    private void NormalState()
    {

    }
    private void DamageState()
    {
        m_State = DAMAGE_STATE.FLASH;
    }
    private void FlashState()
    {
        // 無敵時間が経過したら通常状態に戻す
        m_FlashCount += Time.deltaTime;
        if (m_FlashCount >= MaxFlashTime)
        {
            m_State = DAMAGE_STATE.NORMAL;
            m_FlashCount = 0f;
        }
    }
    private void DeadState()
    {
    }

    /// <summary>
    /// ダメージ計算を持つオブジェクトとのダメージ計算（自分側）を行う
    /// </summary>
    /// <param name="obj">相手オブジェクトのコンポーネント</param>
    public void Damage(CalcDamage obj)
    {
        if (obj.state == DAMAGE_STATE.NORMAL && this.state == DAMAGE_STATE.NORMAL)
        {
            HP -= obj.atk;
            if ((int)HP <= 0)
            {
                HP = 0;
                m_State = DAMAGE_STATE.DEAD;
            }
            else 
            {
                m_State = DAMAGE_STATE.DAMAGE;
            }
        }
    }
    /// <summary>
    /// ダメージを受ける処理のみを行う(ダメージ状態の変更は無し)
    /// </summary>
    /// <param name="value">ダメージ値</param>
    public void DamageValue(float value)
    {
        HP -= value;
        if ((int)HP <= 0)
        {
            HP = 0;
            m_State = DAMAGE_STATE.DEAD;
        }
    }

    /// <summary>
    /// 回復処理
    /// </summary>
    /// <param name="value">回復値</param>
    public void Recovery(float value)
    {
        HP += value;
        if (HP > MAXHP) HP = MAXHP;
    }
}
