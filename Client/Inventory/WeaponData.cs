using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "LCS/Weapon Data")]
public class WeaponData : ScriptableObject
{
    [SerializeField] private int damagePerHit;
    [SerializeField] private int ammoPerClip;
}