using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGameMonoBehaviour : MonoBehaviour
{
    protected enum DIFFICULTY
    {
        A = 0,
        B = 1,
        C = 2,
        D = 3,
        E = 4,
        F = 5,
        G = 6,
        H = 7,
        I = 8,
        J = 9
    }

    protected DIFFICULTY CheckDifficulty(int value)
    {
        DIFFICULTY diff;
        switch (value)
        {
            case 0:
                diff = DIFFICULTY.A;
                break;
            case 1:
                diff = DIFFICULTY.B;
                break;
            case 2:
                diff = DIFFICULTY.C;
                break;
            case 3:
                diff = DIFFICULTY.D;
                break;
            case 4:
                diff = DIFFICULTY.E;
                break;
            case 5:
                diff = DIFFICULTY.F;
                break;
            case 6:
                diff = DIFFICULTY.G;
                break;
            case 7:
                diff = DIFFICULTY.H;
                break;
            case 8:
                diff = DIFFICULTY.I;
                break;
            case 9:
                diff = DIFFICULTY.J;
                break;
            default:
                diff = DIFFICULTY.J;
                break;
        }
        return diff;
    }
}
