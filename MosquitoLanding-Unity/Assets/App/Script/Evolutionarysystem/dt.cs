using UnityEngine;
using System.Collections;


public class dt : MonoBehaviour
{
    public delegate void MyDelegate(int num);
    public MyDelegate myDelegate;


    void Start()
    {
        myDelegate = PrintNum;
        myDelegate(50);

        myDelegate = DoubleNum;
        myDelegate(50);
    }

    void PrintNum(int num)
    {
        print("Print Num: " + num);
    }

    void DoubleNum(int num)
    {
        print("Double Num: " + num * 2);
    }
}