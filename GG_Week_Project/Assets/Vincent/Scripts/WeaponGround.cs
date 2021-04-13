using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponGround : MonoBehaviour
{
    [HideInInspector]
    public enum WEAPON { PUNCH, SWORD, ARC, PIG }
    [Header("WEAPONS")]
    public WEAPON thisWeapon;

}
