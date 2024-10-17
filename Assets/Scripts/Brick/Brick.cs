using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BrickType
{
    Unbreakable = -1,
    Normal = 0,
}

[Serializable]
public struct BrickStat
{
    public int durability;
    public BrickType type;
} 

// 벽돌의 기능 : 공에 맞아 부서지기
[RequireComponent(typeof(BrickAnimation))]
public class Brick : MonoBehaviour
{    
    BrickManager manager;
    BrickAnimation animation;
    private string playerName;

    public BrickStat stat;
    [SerializeField] int durability = 1;
    public int Durability
    { 
        get { return durability; }
        protected set
        {
            if (durability == 0)
                return;

            durability = value;

            if (Durability <= 0)
                Break(playerName);
        } 
    }


    void Awake()
    {
        manager = transform.parent.GetComponent<BrickManager>();
        animation = GetComponent<BrickAnimation>();
    }

    void Start()
    {
        Durability = stat.durability;
    }

    /// <summary>
    /// 벽돌 체력 깎는 메서드
    /// </summary>
    public void Hit(string playerName)
    {
        this.playerName = playerName;

        // OnBrickHitted?.Invoke();
        manager.CallOnBrickHitted(this);
        animation.Hit();

        if (stat.type.Equals(BrickType.Unbreakable)) 
            return;

        Durability--;
    }

    public void Break(string playerName)
    {
        GetComponent<Collider2D>().enabled = false;
        // OnBrickBroken?.Invoke();
        manager.CallOnBrickBroken(playerName);

        // 1초 뒤 제거
        Destroy(gameObject, 1f);

        // TODO : 파괴 시 이펙트 추가
    }
}
