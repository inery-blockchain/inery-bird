using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Klasa će nam koristiti za tip podatka
[Serializable]
public class HighscoreElement
{
    public string playerName;
    public int points;

    public HighscoreElement(string playerName, int points)
    {
        this.playerName = playerName;
        this.points = points;
    }
}
