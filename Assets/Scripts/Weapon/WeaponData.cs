using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewWeapon", menuName = "Weapon/WeaponItem")]
public class WeaponData : ScriptableObject
{
    public string weaponName;
    public Sprite icon;
    public GameObject weaponPrefab;

    public int damage;
    public float attackRate;

    public WeaponType weaponType;
}

public enum WeaponType
{
    Candy,              //지팡이 사탕
    Wreath,             //크리스마스 리스
    Ball,               //크리스마스 볼
    Star,               //크리스마스 별
    Box,                //선물 상자
    Cake,               //크리스마스 케이크
    Cookie              //진저 쿠키
}