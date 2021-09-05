using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefManager : MonoBehaviour
{
    static RefManager instance;
    void Awake() { instance = this; }

    public GameObject Player;

    public static void SetPlayer(GameObject player) { instance.Player = player; }
    public static GameObject GetPlayer { get { return instance.Player; } }
}
