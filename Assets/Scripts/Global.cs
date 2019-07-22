using System;
using System.Collections.Generic;
using UnityEngine;

public static class Global {
    public const int SM_World_Width = 25;
    public const int SM_World_Height = 25;
    public const int MED_World_Width = 50;
    public const int MED_World_Height = 50;
    public const int LG_World_Width = 100;
    public const int LG_World_Height = 100;

    public const string World_Data_Extension = ".map";

    public static readonly Dictionary<int, int> Tile_Config_To_Index_Map = new Dictionary<int, int>() {
      {2, 1},
      {8, 2},
      {10, 3},
      {11, 4},
      {16, 5},
      {18, 6},
      {22, 7},
      {24, 8},
      {26, 9},
      {27, 10},
      {30, 11},
      {31, 12},
      {64, 13},
      {66, 14},
      {72, 15},
      {74, 16},
      {75, 17},
      {80, 18},
      {82, 19},
      {86, 20},
      {88, 21},
      {90, 22},
      {91, 23},
      {94, 24},
      {95, 25},
      {104, 26},
      {106, 27},
      {107, 28},
      {120, 29},
      {122, 30},
      {123, 31},
      {126, 32},
      {127, 33},
      {208, 34},
      {210, 35},
      {214, 36},
      {216, 37},
      {218, 38},
      {219, 39},
      {222, 40},
      {223, 41},
      {248, 42},
      {250, 43},
      {251, 44},
      {254, 45},
      {255, 46},
      {0, 47}
    };

    public static readonly Dictionary<int, int> Inner_Sand_Edge_Map = new Dictionary<int, int>() {
      {248, 28},
      {212, 24},
      {160, 22},
      {105,16},
      {240,24},
      {232,16},
      {148, 29},
      {41, 29},
      {192,23},
      {128,18},
      {32, 26 },
      {208, 24},
      {96, 23 },
      {40,29},
      {104, 16},
      {224, 23},
      {214, 27},
      {164, 31},
      {144,29},
      {20,29},
      {150,26},
      {4,16},
      {132,17},
      {22,26},
      {1, 24},
      {43,18},
      {11,18},
      {9,29},
      {3,23},
      {7,23},
      {6,23},
      {23,26},
      {33,25},
      {107,19},
      {15,18},
      {44,17},
      {52,25},
      {5,20},
      {31,30}
    };
}

