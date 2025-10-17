using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StaticData
{
    //Data => Property; Logic => Function;
    public static class S_GameManager
    {
        public static class StringConst
        {
            //Input
            public static string Horizontal { get => "Horizontal"; }
            public static string Vertical { get => "Vertical"; }
            public static string Run { get => "Fire3"; }
            public static string Jump { get => "Jump"; }
            public static string MouseX { get => "Mouse X"; }
            public static string MouseY { get => "Mouse Y"; }
            public static string Escape { get => "Cancel"; }
        }

        public static class InfoScene
        {
            public static int Disclaimer { get => 0; }
            public static int MainMenu { get => 1; }
            public static int GameWorld { get => 2; }
        }
    }
}