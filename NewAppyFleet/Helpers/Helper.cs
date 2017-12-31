using System.Collections.Generic;
using mvvmframework;
using Xamarin.Forms;

namespace NewAppyFleet
{
    public class Helper
    {
        public static string BoldFont
        {
            get
            {
                var font = string.Empty;
                switch(Device.RuntimePlatform)
                {
                    case Device.Android:
                        font = "Roboto-Bold.ttf#Roboto-Bold";
                        break;
                    case Device.iOS:
                        font = "Roboto-Bold";
                        break;
                    case Device.Windows:
                        font = "/Assets/Fonts/Roboto-Bold.ttf#Roboto";
                        break;
                }
                return font.CorrectedFontSource();
            }
        }

        public static string RegFont
        {
            get
            {
                var font = string.Empty;

                switch(Device.RuntimePlatform)
                {
                    case Device.Android:
                        font=  "Roboto-Regular.ttf#Roboto-Regular";
                        break;
                    case Device.iOS:
                        font = "Roboto-Regular";
                        break;
                    case Device.Windows:
                        font = "/Assets/Fonts/Roboto-Regular.ttf#Roboto";
                        break;
                }
                return font.CorrectedFontSource();
            }
        }

        public static double KeyLineSize
        {
            get
            {
                
                switch (App.Self.Density)
                {
                    case DisplayDensity.LDPI:
                    case DisplayDensity.MDPI:
                        return 1.0;
                    case DisplayDensity.HDPI:
                        return 0.75;
                    case DisplayDensity.XHDPI:
                        return 0.5;
                    case DisplayDensity.XXHDPI:
                    case DisplayDensity.XXXHDPI:
                        return 0.25;
                }
                return 1.0;
            }
        }

        public static string KeyLineSizeText
        {
            get
            {
                switch (App.Self.Density)
                {
                    case DisplayDensity.LDPI:
                    case DisplayDensity.MDPI:
                        return "1.0";
                    case DisplayDensity.HDPI:
                        return "0.75";
                    case DisplayDensity.XHDPI:
                        return "0.5";
                    case DisplayDensity.XXHDPI:
                    case DisplayDensity.XXXHDPI:
                        return "0.25";
                }
                return "1.0";
            }
        }

        public static Point GetScreenPos(VisualElement veIn)
        {
            var x = veIn.X;
            var y = veIn.Y;

            var ve = veIn.Parent as VisualElement;

            while (ve != null)
            {
                x += ve.X;
                y += ve.Y;

                ve = ve.Parent != null ? ve.Parent as VisualElement : null;
            }

            return new Point(x, y);
        }

        public static Dictionary<int, SOSPageModel> GetSOSPageData
        {
            get
            {
                var ret = new Dictionary<int, SOSPageModel>();

                /*ret.Add(0, new SOSPageModel { Image = "sos_exclamation_icon.png", Title = LanguageControler.GetTextForItem(LanguageItems.SOSTitle1), Text = LanguageControler.GetTextForItem(LanguageItems.SOSText1) });
                ret.Add(1, new SOSPageModel { Image = "sos_phone_icon.png", Title = LanguageControler.GetTextForItem(LanguageItems.SOSTitle2), Text = LanguageControler.GetTextForItem(LanguageItems.SOSText2) });
                ret.Add(2, new SOSPageModel { Image = "sos_info_icon.png", Title = LanguageControler.GetTextForItem(LanguageItems.SOSTitle3), Text = LanguageControler.GetTextForItem(LanguageItems.SOSText3) });
                ret.Add(3, new SOSPageModel { Image = "sos_numberplate_icon.png", Title = LanguageControler.GetTextForItem(LanguageItems.SOSTitle4), Text = LanguageControler.GetTextForItem(LanguageItems.SOSText4) });
                ret.Add(4, new SOSPageModel { Image = "sos_contact_icon.png", Title = LanguageControler.GetTextForItem(LanguageItems.SOSTitle5), Text = LanguageControler.GetTextForItem(LanguageItems.SOSText5) });
                ret.Add(5, new SOSPageModel { Image = "sos_exchange_icon.png", Title = LanguageControler.GetTextForItem(LanguageItems.SOSTitle6), Text = LanguageControler.GetTextForItem(LanguageItems.SOSText6) });
                ret.Add(6, new SOSPageModel { Image = "sos_photo_icon.png", Title = LanguageControler.GetTextForItem(LanguageItems.SOSTitle7), Text = LanguageControler.GetTextForItem(LanguageItems.SOSText7) });*/

                return ret;
            }
        }

        public static string GetNotificationEventOverViewText(int type, int eventCount)
        {
            var ret = string.Empty;

            /*switch ((JourneyEventTypes)type)
            {
                case JourneyEventTypes.NoEvent:
                    ret = eventCount > 1 ? LanguageControler.GetTextForItem(LanguageItems.JourneyEvent_NoEvent_GroupTitle) :
                    LanguageControler.GetTextForItem(LanguageItems.JourneyEvent_NoEvent_GroupTitlePloral);
                    break;
                case JourneyEventTypes.Speeding:
                    ret = eventCount > 1 ? LanguageControler.GetTextForItem(LanguageItems.JourneyEvent_Speeding_GroupTitle) :
                    LanguageControler.GetTextForItem(LanguageItems.JourneyEvent_Speeding_GroupTitlePloral);
                    break;
                case JourneyEventTypes.ExcessiveDriving:
                    ret = eventCount > 1 ? LanguageControler.GetTextForItem(LanguageItems.JourneyEvent_ExcessiveDriving_GroupTitle) :
                    LanguageControler.GetTextForItem(LanguageItems.JourneyEvent_ExcessiveDriving_GroupTitlePloral);
                    break;
                case JourneyEventTypes.LowBattery:
                    ret = eventCount > 1 ? LanguageControler.GetTextForItem(LanguageItems.JourneyEvent_LowBattery_GroupTitle) :
                        LanguageControler.GetTextForItem(LanguageItems.JourneyEvent_LowBattery_GroupTitlePloral);
                    break;
                case JourneyEventTypes.PowerFailure:
                    ret = eventCount > 1 ? LanguageControler.GetTextForItem(LanguageItems.JourneyEvent_PowerFailure_GroupTitle) :
                        LanguageControler.GetTextForItem(LanguageItems.JourneyEvent_PowerFailure_GroupTitlePloral);
                    break;
                case JourneyEventTypes.Idling:
                    ret = eventCount > 1 ? LanguageControler.GetTextForItem(LanguageItems.JourneyEvent_Idling_GroupTitle) :
                        LanguageControler.GetTextForItem(LanguageItems.JourneyEvent_Idling_GroupTitlePloral);
                    break;
                case JourneyEventTypes.ExceedingMileage:
                    ret = eventCount > 1 ? LanguageControler.GetTextForItem(LanguageItems.JourneyEvent_ExceedingMileage_GroupTitle) :
                        LanguageControler.GetTextForItem(LanguageItems.JourneyEvent_ExceedingMileage_GroupTitlePloral);
                    break;
                case JourneyEventTypes.NewUpdates:
                    ret = eventCount > 1 ? LanguageControler.GetTextForItem(LanguageItems.JourneyEvent_NewUpdates_GroupTitle) :
                        LanguageControler.GetTextForItem(LanguageItems.JourneyEvent_NewUpdates_GroupTitlePloral);
                    break;
                case JourneyEventTypes.Tamper:
                    ret = eventCount > 1 ? LanguageControler.GetTextForItem(LanguageItems.JourneyEvent_Tamper_GroupTitle) :
                        LanguageControler.GetTextForItem(LanguageItems.JourneyEvent_Tamper_GroupTitlePloral);
                    break;
                case JourneyEventTypes.IgnitionOn:
                    ret = eventCount > 1 ? LanguageControler.GetTextForItem(LanguageItems.JourneyEvent_IgnitionOn_GroupTitle) :
                        LanguageControler.GetTextForItem(LanguageItems.JourneyEvent_IgnitionOn_GroupTitlePloral);
                    break;
                case JourneyEventTypes.PositionUpdate:
                    ret = eventCount > 1 ? LanguageControler.GetTextForItem(LanguageItems.JourneyEvent_PositionUpdate_GroupTitle) :
                        LanguageControler.GetTextForItem(LanguageItems.JourneyEvent_PositionUpdate_GroupTitlePloral);
                    break;
                case JourneyEventTypes.Motion:
                    ret = eventCount > 1 ? LanguageControler.GetTextForItem(LanguageItems.JourneyEvent_Motion_GroupTitle) :
                        LanguageControler.GetTextForItem(LanguageItems.JourneyEvent_Motion_GroupTitlePloral);
                    break;
                case JourneyEventTypes.IgnitionOff:
                    ret = eventCount > 1 ? LanguageControler.GetTextForItem(LanguageItems.JourneyEvent_IgnitionOff_GroupTitle) :
                        LanguageControler.GetTextForItem(LanguageItems.JourneyEvent_IgnitionOff_GroupTitlePloral);
                    break;
                case JourneyEventTypes.UpdatesFailed:
                    ret = eventCount > 1 ? LanguageControler.GetTextForItem(LanguageItems.JourneyEvent_UpdatesFailed_GroupTitle) :
                        LanguageControler.GetTextForItem(LanguageItems.JourneyEvent_UpdatesFailed_GroupTitlePloral);
                    break;
                case JourneyEventTypes.FailedToReport:
                    ret = eventCount > 1 ? LanguageControler.GetTextForItem(LanguageItems.JourneyEvent_FailedToReport_GroupTitle) :
                        LanguageControler.GetTextForItem(LanguageItems.JourneyEvent_FailedToReport_GroupTitlePloral);
                    break;
                case JourneyEventTypes.TowAway:
                    ret = eventCount > 1 ? LanguageControler.GetTextForItem(LanguageItems.JourneyEvent_TowAway_GroupTitle) :
                        LanguageControler.GetTextForItem(LanguageItems.JourneyEvent_TowAway_GroupTitlePloral);
                    break;
                case JourneyEventTypes.DoubleFailedToReport:
                    ret = eventCount > 1 ? LanguageControler.GetTextForItem(LanguageItems.JourneyEvent_DoubleFailedToReport_GroupTitle) :
                        LanguageControler.GetTextForItem(LanguageItems.JourneyEvent_DoubleFailedToReport_GroupTitlePloral);
                    break;
                case JourneyEventTypes.SecondaryTamper:
                    ret = eventCount > 1 ? LanguageControler.GetTextForItem(LanguageItems.JourneyEvent_SecondaryTamper_GroupTitle) :
                        LanguageControler.GetTextForItem(LanguageItems.JourneyEvent_SecondaryTamper_GroupTitlePloral);
                    break;
                case JourneyEventTypes.HardwareTamper:
                    ret = eventCount > 1 ? LanguageControler.GetTextForItem(LanguageItems.JourneyEvent_HardwareTamper_GroupTitle) :
                        LanguageControler.GetTextForItem(LanguageItems.JourneyEvent_HardwareTamper_GroupTitlePloral);
                    break;
                case JourneyEventTypes.Impact:
                    ret = eventCount > 1 ? LanguageControler.GetTextForItem(LanguageItems.JourneyEvent_Impact_GroupTitle) :
                        LanguageControler.GetTextForItem(LanguageItems.JourneyEvent_Impact_GroupTitlePloral);
                    break;
                case JourneyEventTypes.WakeUp:
                    ret = eventCount > 1 ? LanguageControler.GetTextForItem(LanguageItems.JourneyEvent_WakeUp_GroupTitle) :
                        LanguageControler.GetTextForItem(LanguageItems.JourneyEvent_WakeUp_GroupTitlePloral);
                    break;
                case JourneyEventTypes.XTPR:
                    ret = eventCount > 1 ? LanguageControler.GetTextForItem(LanguageItems.JourneyEvent_XTPR_GroupTitle) :
                        LanguageControler.GetTextForItem(LanguageItems.JourneyEvent_XTPR_GroupTitlePloral);
                    break;
                case JourneyEventTypes.STPR:
                    ret = eventCount > 1 ? LanguageControler.GetTextForItem(LanguageItems.JourneyEvent_STPR_GroupTitle) :
                        LanguageControler.GetTextForItem(LanguageItems.JourneyEvent_STPR_GroupTitlePloral);
                    break;
                case JourneyEventTypes.StaticG:
                    ret = eventCount > 1 ? LanguageControler.GetTextForItem(LanguageItems.JourneyEvent_StaticG_GroupTitle) :
                        LanguageControler.GetTextForItem(LanguageItems.JourneyEvent_StaticG_GroupTitlePloral);
                    break;
                case JourneyEventTypes.LowG:
                    ret = eventCount > 1 ? LanguageControler.GetTextForItem(LanguageItems.JourneyEvent_LowG_GroupTitle) :
                        LanguageControler.GetTextForItem(LanguageItems.JourneyEvent_LowG_GroupTitlePloral);
                    break;
                case JourneyEventTypes.HighG:
                    ret = eventCount > 1 ? LanguageControler.GetTextForItem(LanguageItems.JourneyEvent_HighG_GroupTitle) :
                        LanguageControler.GetTextForItem(LanguageItems.JourneyEvent_HighG_GroupTitlePloral);
                    break;
                case JourneyEventTypes.XYZ:
                    ret = eventCount > 1 ? LanguageControler.GetTextForItem(LanguageItems.JourneyEvent_XYZ_GroupTitle) :
                        LanguageControler.GetTextForItem(LanguageItems.JourneyEvent_XYZ_GroupTitlePloral);
                    break;
                case JourneyEventTypes.PositionUpdate2:
                    ret = eventCount > 1 ? LanguageControler.GetTextForItem(LanguageItems.JourneyEvent_PositionUpdate2_GroupTitle) :
                        LanguageControler.GetTextForItem(LanguageItems.JourneyEvent_PositionUpdate2_GroupTitlePloral);
                    break;
            }*/

            return ret;
        }

        public static string GetNotificationEventText(int type)
        {
            var ret = string.Empty;

            /*switch ((JourneyEventTypes)type)
            {
                case JourneyEventTypes.NoEvent:
                    ret = LanguageControler.GetTextForItem(LanguageItems.JourneyEvent_NoEvent_EventText);
                    break;
                case JourneyEventTypes.Speeding:
                    ret = LanguageControler.GetTextForItem(LanguageItems.JourneyEvent_Speeding_EventText);
                    break;
                case JourneyEventTypes.ExcessiveDriving:
                    ret = LanguageControler.GetTextForItem(LanguageItems.JourneyEvent_ExcessiveDriving_EventText);
                    break;
                case JourneyEventTypes.LowBattery:
                    ret = LanguageControler.GetTextForItem(LanguageItems.JourneyEvent_LowBattery_EventText);
                    break;
                case JourneyEventTypes.PowerFailure:
                    ret = LanguageControler.GetTextForItem(LanguageItems.JourneyEvent_PowerFailure_EventText);
                    break;
                case JourneyEventTypes.Idling:
                    ret = LanguageControler.GetTextForItem(LanguageItems.JourneyEvent_Idling_EventText);
                    break;
                case JourneyEventTypes.ExceedingMileage:
                    ret = LanguageControler.GetTextForItem(LanguageItems.JourneyEvent_ExceedingMileage_EventText);
                    break;
                case JourneyEventTypes.NewUpdates:
                    ret = LanguageControler.GetTextForItem(LanguageItems.JourneyEvent_NewUpdates_EventText);
                    break;
                case JourneyEventTypes.Tamper:
                    ret = LanguageControler.GetTextForItem(LanguageItems.JourneyEvent_Tamper_EventText);
                    break;
                case JourneyEventTypes.IgnitionOn:
                    ret = LanguageControler.GetTextForItem(LanguageItems.JourneyEvent_IgnitionOn_EventText);
                    break;
                case JourneyEventTypes.PositionUpdate:
                    ret = LanguageControler.GetTextForItem(LanguageItems.JourneyEvent_PositionUpdate_EventText);
                    break;
                case JourneyEventTypes.Motion:
                    ret = LanguageControler.GetTextForItem(LanguageItems.JourneyEvent_Motion_EventText);
                    break;
                case JourneyEventTypes.IgnitionOff:
                    ret = LanguageControler.GetTextForItem(LanguageItems.JourneyEvent_IgnitionOff_EventText);
                    break;
                case JourneyEventTypes.UpdatesFailed:
                    ret = LanguageControler.GetTextForItem(LanguageItems.JourneyEvent_UpdatesFailed_EventText);
                    break;
                case JourneyEventTypes.FailedToReport:
                    ret = LanguageControler.GetTextForItem(LanguageItems.JourneyEvent_FailedToReport_EventText);
                    break;
                case JourneyEventTypes.TowAway:
                    ret = LanguageControler.GetTextForItem(LanguageItems.JourneyEvent_TowAway_EventText);
                    break;
                case JourneyEventTypes.DoubleFailedToReport:
                    ret = LanguageControler.GetTextForItem(LanguageItems.JourneyEvent_DoubleFailedToReport_EventText);
                    break;
                case JourneyEventTypes.SecondaryTamper:
                    ret = LanguageControler.GetTextForItem(LanguageItems.JourneyEvent_SecondaryTamper_EventText);
                    break;
                case JourneyEventTypes.HardwareTamper:
                    ret = LanguageControler.GetTextForItem(LanguageItems.JourneyEvent_HardwareTamper_EventText);
                    break;
                case JourneyEventTypes.Impact:
                    ret = LanguageControler.GetTextForItem(LanguageItems.JourneyEvent_Impact_EventText);
                    break;
                case JourneyEventTypes.WakeUp:
                    ret = LanguageControler.GetTextForItem(LanguageItems.JourneyEvent_WakeUp_EventText);
                    break;
                case JourneyEventTypes.XTPR:
                    ret = LanguageControler.GetTextForItem(LanguageItems.JourneyEvent_XTPR_EventText);
                    break;
                case JourneyEventTypes.STPR:
                    ret = LanguageControler.GetTextForItem(LanguageItems.JourneyEvent_STPR_EventText);
                    break;
                case JourneyEventTypes.StaticG:
                    ret = LanguageControler.GetTextForItem(LanguageItems.JourneyEvent_StaticG_EventText);
                    break;
                case JourneyEventTypes.LowG:
                    ret = LanguageControler.GetTextForItem(LanguageItems.JourneyEvent_LowG_EventText);
                    break;
                case JourneyEventTypes.HighG:
                    ret = LanguageControler.GetTextForItem(LanguageItems.JourneyEvent_HighG_EventText);
                    break;
                case JourneyEventTypes.XYZ:
                    ret = LanguageControler.GetTextForItem(LanguageItems.JourneyEvent_XYZ_EventText);
                    break;
                case JourneyEventTypes.PositionUpdate2:
                    ret = LanguageControler.GetTextForItem(LanguageItems.JourneyEvent_PositionUpdate2_EventText);
                    break;
            }*/

            return ret;
        }
    }
}
