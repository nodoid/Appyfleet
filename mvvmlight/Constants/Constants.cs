using System.Collections.Generic;
using mvvmframework.Languages;

namespace mvvmframework
{
    public class Constants
    {
        public static string ServiceUrl { get => ""; }
        public static string INSIGHTS_API_KEY { get => ""; }
        public static string HOCKEY_APP_APP_ID { get => ""; }

        public static string EMAIL_HASHING_KEY { get => ""; }

        public static string DriverIDPushPrefex { get => "driver_id:"; }

        public static string LogFileEmail { get => "support"; }
        public static int DashBoardSlideTime { get => 500; }
        public static int MenuSlideInTime { get => 500; }
        public static int MenuSlideOutTime { get => 500; }
        public static double DashBoardControlsScoreGap { get => 37.5f; }
        public static float TextMidpointOffset { get => 0.4f; }

        public static float kMetresPerSecondToKnots = 1.94384449f;
        public static float kRequiredAccuracy = 70.0f;


        public const string kIP = "192.168.0.1";
        public const int kPort = 5342;

        public static List<string> EventNames 
        {
            get
            {
                return new List<string>
                {
                    string.Empty,
                    Langs.Const_Event_Type_1,Langs.Const_Event_Type_2,Langs.Const_Event_Type_3,Langs.Const_Event_Type_4,Langs.Const_Event_Type_5,
                    Langs.Const_Event_Type_6,Langs.Const_Event_Type_7,Langs.Const_Event_Type_8,Langs.Const_Event_Type_9,Langs.Const_Event_Type_10,
                    Langs.Const_Event_Type_11,Langs.Const_Event_Type_12,Langs.Const_Event_Type_13,Langs.Const_Event_Type_14,Langs.Const_Event_Type_15,
                    Langs.Const_Event_Type_16,Langs.Const_Event_Type_17,Langs.Const_Event_Type_18,Langs.Const_Event_Type_19,Langs.Const_Event_Type_20,
                    Langs.Const_Event_Type_21,Langs.Const_Event_Type_22,Langs.Const_Event_Type_23,Langs.Const_Event_Type_24,Langs.Const_Event_Type_25,
                    Langs.Const_Event_Type_26,Langs.Const_Event_Type_27,Langs.Const_Event_Type_28,
                };
            }
        }

        public static string BingMapId { get => ""; }
    }
}
