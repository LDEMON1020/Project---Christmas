using UnityEngine;

[CreateAssetMenu(fileName = "New item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    public string itemName;
    public Sprite itemIcon;
    public int Price;
    public GameObject weaponPrefab;

    [Header("Item Type")]
    public bool isEquippable = false;
    public bool consumeOnAttack = false;

    public WeaponType weaponType;

    public enum WeaponType
    {
        Candy, //지팡이 사탕
        Wreath, //크리스마스 리스
        Ball, //크리스마스 볼
        Star, //크리스마스 별
        Box, //선물 상자
        Cake, //크리스마스 케이크
        Cookie, //진저 쿠키
        Glove, //넉백 장갑
        Cocoa //코코아
    }
}