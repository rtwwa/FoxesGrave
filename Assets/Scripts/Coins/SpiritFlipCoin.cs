
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiritFlipCoin : MonoBehaviour, ICoin
{
    public bool IsCooldown()
    {
        return false;
    }
    public void UseAbility()
    {
        // �� ��� �����, ������ ��� ���� ����������� �������� ������ PlayerMovement, � �� ���������, �� ��� �������
    }

    public void ShowAnimation(GameObject target)
    {
        throw new System.NotImplementedException();
    }
}
