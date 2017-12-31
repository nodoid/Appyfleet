using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using mvvmframework.Helpers;
using mvvmframework.Interfaces;
using mvvmframework.Models;
using System.Linq;

namespace mvvmframework.Services
{
    public class JourneyService : IJourneyService
    {
        ILogFileService logService { get; set; } = SimpleIoc.Default.GetInstance<ILogFileService>();
        IConnection connectService { get; set; } = SimpleIoc.Default.GetInstance<IConnection>();
        ISockets socketService { get; set; } = SimpleIoc.Default.GetInstance<ISockets>();
        static object lockInstance = new object();

        static byte[] sReceivedData = new byte[128];
        static byte[] sSuccess = new byte[128];
        static bool IsInitialised { get; set; }

        List<LocationServiceData> mCurrentJourney = new List<LocationServiceData>();
        List<float> mSpeedHistory = new List<float>();
        List<string> MessageQueue { get; set; }

        int mDB0, mDB2, mDB4, mDB6, mDB8, mMaxDB0, mMaxDB2, mMaxDB4, mMaxDB6, mMaxDB8;
        LocationServiceData mMaxSpeedLocation = null;
        Int64 mMaxSpeedTime = 0;
        float mMaxSpeed = 0.0f;
        float mAverageSpeed = 0.0f;
        float mDistanceTravelled = 0.0f;
        float mOldSpeed = 0.0f;
        float mOldDirection = 0.0f;
        LocationServiceData mLastKnownLocation = null;
        LocationServiceData mOldLocation = null;

        bool mInJourney { get; set; }
        
        string SimNumber { get; set; }
        int Odometer { get; set; }
        bool GpsRunning { get; set; }
        string PairId { get; set; }
        int RecordNum { get; set; }
        DateTime AppStarted { get; set; }
        int JourneyCounter { get; set; }
        string VersionNum { get; set; }

        public void InitialiseService(List<string>queue, string sim, int odo, bool gpson, string pair, int rec, DateTime app, int journey, string version)
        {
            if (!IsInitialised)
            {
                MessageQueue = queue != null ? queue : new List<string>();
                SimNumber = sim;
                Odometer = odo;
                GpsRunning = gpson;
                PairId = pair;
                RecordNum = rec;
                AppStarted = app;
                JourneyCounter = journey;
                VersionNum = version;
                IsInitialised = true;
            }
        }

        void CreateSuccessByte()
        {
            sSuccess[0] = 13;
            sSuccess[1] = 10;
            sSuccess[2] = 26;
            sSuccess[3] = 62;
        }

        // Should be called every minute
        public bool AddLocationUpdate()
        {
            var res = false;
            if (mInJourney)
            {
                lock (lockInstance)
                {
                    res = AddEvent(mLastKnownLocation, "MV");
                }
            }
            ResetDriverBehaviourScores();
            return res;
        }

        bool AddEvent(LocationServiceData location, string eventType)
        {
            var res = false;
            try
            {
                lock (lockInstance)
                {
                    var nfi = new NumberFormatInfo
                    {
                        NumberDecimalSeparator = "."
                    };

                    if (location == null)
                    {
                        logService.WriteLog("JourneyManager:AddEvent", "ERROR: AddEvent location was NULL");
                        throw new Exception();
                    }

                    logService.WriteLog("JourneyManager:AddEvent", $"Adding event at {location.TimeStamp.TimeOfDay}, type = {eventType}");

                    var gpsTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(location.TimeStamp.TimeOfDay.TotalMilliseconds); // GPS time is milliseconds since 1970
                    var seconds = GPSHelper.TimeForTimeStamp(gpsTime.Ticks);
                    var latStr = GPSHelper.ToDegreesMinutesSeconds(location.Latitude, GPSCoordinate.Latitude);
                    var lngStr = GPSHelper.ToDegreesMinutesSeconds(location.Longitude, GPSCoordinate.Longitude);

                    var rr = string.Empty;

                    var horiAcc = (float)location.Accuracy;
                    var HDOP = 10 * (int)(horiAcc / 5.0f);

                    if (eventType == "MV" && mMaxSpeedTime > 0 && mMaxSpeed > 5.0f)
                    {
                        logService.WriteLog("JourneyManager:AddEvent", "Max speed position update");
                        // Need to increment counter an extra time if we have a max speed (.Position Update) packet
                        //UserData.Instance.RecordNumber += 1;
                        res = true; // increase record number

                        var maxSpeedLatStr = GPSHelper.ToDegreesMinutesSeconds(mMaxSpeedLocation.Latitude, GPSCoordinate.Latitude);
                        var maxSpeedLngStr = GPSHelper.ToDegreesMinutesSeconds(mMaxSpeedLocation.Longitude, GPSCoordinate.Longitude);
                        rr = string.Format("{0},{1}:{2}:{3}:{4}:8:{5}", mMaxSpeedTime, maxSpeedLatStr, maxSpeedLngStr, mMaxSpeed.ToString("0.00", nfi),
                                           (int)Math.Max(0.0f, mMaxSpeedLocation.Heading), HDOP);
                    }

                    var speedInKnots = (float)Math.Max(location.Speed, 0.0f) * Constants.kMetresPerSecondToKnots;
                    var version = VersionNum;

                    //var userData = UserData.Instance;
                    var sik = speedInKnots.ToString("0.00", nfi);
                    var gps = GpsRunning ? "#1" : string.Empty;
                    var buf = $"TF,{SimNumber}/{seconds},4T54H1.v{version},{RecordNum},{latStr}:{lngStr}:{sik}:{(int)Math.Max(0.0f, location.Heading)}:8:{HDOP},{(int)mDistanceTravelled},{eventType},{(AppStarted - DateTime.Now).Hours}:5:0061:04D0,1:-3:32:57:" +
                        $"{JourneyCounter}:{JourneyCounter}:0:|{mDB0}:{mDB2}:{mDB4}:{mDB6}:{mDB8}::::::,1:420,1:1:20:0,{Odometer},{(int)Math.Max(mAverageSpeed, 0.0f)},{rr},#{PairId}{gps}/\r\n";

                    MessageQueue.Add(buf);

                    res = true;

                    mMaxDB0 = Math.Max(mDB0, mMaxDB0);
                    mMaxDB2 = Math.Max(mDB2, mMaxDB2);
                    mMaxDB4 = Math.Max(mDB4, mMaxDB4);
                    mMaxDB6 = Math.Max(mDB6, mMaxDB6);
                    mMaxDB8 = Math.Max(mDB8, mMaxDB8);

                    // AB what does this do? :|
                    //NSString *DBMax = [[NSString alloc] initWithFormat:@"Max DB %d %d %d %d %d", self.maxDB0, self.maxDB2, self.maxDB4, self.maxDB6, self.maxDB8];
                    //[LocationData getInstance].String7 = [[NSString alloc] initWithString:DBMax];

                    SendUDP();
                }
            }
            catch (Exception ex)
            {
                logService.WriteLog("JourneyManager:AddEvent(catch)", ex.StackTrace);
            }
            return res;
        }

         void ResetDriverBehaviourScores()
        {
            logService.WriteLog("JourneyManager:ResetDriverBehaviourScores", "Resetting driver behaviour scores");
            mDB0 = 0;
            mDB2 = 0;
            mDB4 = 0;
            mDB6 = 0;
            mDB8 = 0;
            mMaxSpeedLocation = null;
            mMaxSpeedTime = 0;
            mMaxSpeed = 0.0f;
        }

        // AB these will be going...soon
         double mTempOldLat;
         double mTempOldLng;
         long mTempOldTime;
        // Should be called every second

        void UpdateDriverBehaviour()
        {
            float newSpeed = 0;
            lock (lockInstance)
            {
                if (mLastKnownLocation == null)
                {
                    logService.WriteLog("JourneyManager:UpdateDriverBehaviour", "Can't UpdateDriverBehaviour, lastKnownLocation is null.");
                    Messenger.Default.Send(new NotificationMessage(this, "UnknownLocation"));
                    return;
                }

                Messenger.Default.Send(new NotificationMessage(this, "RecordingData"));
                logService.WriteLog("JourneyManager:UpdateDriverBehaviour", $"UpdateDriverBehaviour - distanceTravelled = {mDistanceTravelled}, Accuracy {mLastKnownLocation.Accuracy}");
                newSpeed = (float)mLastKnownLocation.Speed * Constants.kMetresPerSecondToKnots;
                float newDirection = (float)mLastKnownLocation.Heading;

                // Only do this when the trip has been going for long enough and we have an accurate reading
                if (mDistanceTravelled > 10.0f && mLastKnownLocation.Accuracy < Constants.kRequiredAccuracy)
                {
                    if (newSpeed > 5.0f)
                    {
                        float speedChange = newSpeed - mOldSpeed;
                        if (speedChange > 7.0f)
                        {
                            mDB0++;
                            logService.WriteLog("JourneyManager:UpdateDriverBehaviour", "Speed change > 7");
                        }
                        if (speedChange > 9.0f)
                        {
                            mDB2++;
                            logService.WriteLog("JourneyManager:UpdateDriverBehaviour", "Speed change > 9");
                        }
                        if (speedChange < -7.0f)
                        {
                            mDB4++;
                            logService.WriteLog("JourneyManager:UpdateDriverBehaviour", "Speed change < -7");
                        }
                        if (speedChange < -9.0f)
                        {
                            mDB6++;
                            logService.WriteLog("JourneyManager:UpdateDriverBehaviour", "Speed change < -9");
                        }

                        var angleDiff = Math.Abs(newDirection - mOldDirection);
                        if (angleDiff > 30.0f && newSpeed > 20.0f && angleDiff < 330.0f)
                        {
                            mDB8++;
                            logService.WriteLog("JourneyManager:UpdateDriverBehaviour", "Excessive cornering");
                        }

                        var distance = mCurrentJourney.LastOrDefault().DistanceBetween(new double[] { mTempOldLat, mTempOldLng, mLastKnownLocation.Latitude, mLastKnownLocation.Longitude });
                        int millis = (int)(mLastKnownLocation.TimeStamp.TimeOfDay.TotalMilliseconds - mTempOldTime);

                        float calculatedSpeed = ((millis / 1000) * distance) * Constants.kMetresPerSecondToKnots;

                        logService.WriteLog("JourneyManager:UpdateDriverBehaviour", $"GPS Speed = {(mLastKnownLocation.Speed * Constants.kMetresPerSecondToKnots)}, calculated speed = {calculatedSpeed}, diff = {(mLastKnownLocation.Speed - calculatedSpeed)}");
                        logService.WriteLog("JourneyManager:UpdateDriverBehaviour", $"Speed has changed by {speedChange}, direction has changed by {angleDiff}");
                        logService.WriteLog("JourneyManager:UpdateDriverBehaviour", $"New speed = {newSpeed}, new bearing = {newDirection}");
                    }

                    mOldSpeed = newSpeed;
                    mOldDirection = newDirection;
                    mTempOldLat = mLastKnownLocation.Latitude;
                    mTempOldLng = mLastKnownLocation.Longitude;
                    mTempOldTime = mLastKnownLocation.TimeStamp.Ticks;
                }
            }


            lock (mCurrentJourney)
            {
                newSpeed = Math.Max(0.0f, newSpeed);
                if (mCurrentJourney.Count > 0)
                {
                    {
                        var lastPoint = mCurrentJourney[mCurrentJourney.Count - 1];

                        var distance = mOldLocation.DistanceTo(mLastKnownLocation);

                        mDistanceTravelled += 0.868976f * (float)(100.0f * 3600.0f * distance / 1609.344f);
                        logService.WriteLog("JourneyManager:UpdateDriverBehaviour", "Distance travelled = " + mDistanceTravelled);

                        // Handle average speed processing
                        var averageSpeed = 0.0f;
                        mSpeedHistory.Add(newSpeed);

                        // Store 8 speeds
                        while (mSpeedHistory.Count > 8)
                            mSpeedHistory.RemoveAt(0);

                        for (int i = 0; i < mSpeedHistory.Count; i++)
                        {
                            averageSpeed += mSpeedHistory[i];
                        }
                        averageSpeed /= mSpeedHistory.Count;
                        logService.WriteLog("JourneyManager:UpdateDriverBehaviour", "Average speed = " + averageSpeed);

                        mAverageSpeed = averageSpeed;
                        if (mMaxSpeedLocation == null || (newSpeed > mMaxSpeed && newSpeed > 5.0f))
                        {
                            mMaxSpeedLocation = mLastKnownLocation;
                            mMaxSpeed = newSpeed;
                            var locDateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(mLastKnownLocation.TimeStamp.TimeOfDay.TotalMilliseconds); // GPS time is milliseconds since 1970
                            mMaxSpeedTime = GPSHelper.TimeForTimeStamp(locDateTime.Ticks);
                            logService.WriteLog("JourneyManager:UpdateDriverBehaviour", "Setting new max speed for this segment = " + mMaxSpeed + " at: " + mMaxSpeedTime);
                        }

                        float dist = (float)mLastKnownLocation.DistanceTo(lastPoint);
                        var timeDiff = mLastKnownLocation.TimeStamp - lastPoint.TimeStamp;
                        if (dist > 10.0 && timeDiff.Minutes > 5.0)
                        {
                            mCurrentJourney.Add(mLastKnownLocation);
                            logService.WriteLog("JourneyManager:UpdateDriverBehaviour", "Adding new location to journey, new length = " + mCurrentJourney.Count);
                        }

                        mOldLocation = mLastKnownLocation;
                    }
                }
            }
        }

        public List<string> EndJourney()
        {
            logService.WriteLog("JourneyManager:EndJourney", "Journey ended");

            Messenger.Default.Send(new NotificationMessage(this, (int)(mDistanceTravelled / 3600.0f), "OdometerUpdate"));

            lock (lockInstance)
            {
                if (mInJourney && AddEvent(mLastKnownLocation, "OFF"))
                {
                    // If the off event was added successfully then show success notification
                    Messenger.Default.Send(new NotificationMessage(this, "JourneyEndedOK"));
                }
                else if (mLastKnownLocation == null || !mInJourney)
                {
                    // If it failed and mLastKnownLocation is null. We know why it failed.
                   logService.WriteLog("JourneyManager:EndJourney", "Journey was detected but never received a usable location. Cannot be stored.");
                    Messenger.Default.Send(new NotificationMessage(this, "JourneyEndedNR"));
                }
                else
                {
                    // Tell the user something went wrong.
                    logService.WriteLog("JourneyManager:EndJourney", "Journey was not stored for an unknown reason.");
                    Messenger.Default.Send(new NotificationMessage(this, "JourneyEndedUK"));
                }

                mInJourney = false;
                mDistanceTravelled = 0.0f;
                mLastKnownLocation = null;
                mOldLocation = null;
            }

            return SendUDP();
        }


        List<string> SendUDP()
        {
            Task.Run(()=>
            {
                lock (lockInstance)
                {
                    if (MessageQueue.Count == 0)
                    {
                        logService.WriteLog("JourneyManager:SendUDP", "Send UDP: No messages to send");
                        return;
                    }

                    if (!connectService.IsConnected)
                    {
                        logService.WriteLog("JourneyManager:SendUDP", $"Send UDP: Not connected to Internet, earlying out. Messages left to send: {MessageQueue.Count}");
                        return;
                    }

                    var i = 0;
                    var queue = MessageQueue;
                    while (i < queue.Count)
                    {
                        var message = MessageQueue[i];

                        if (!message.Contains("\r\n"))
                        {
                            message = message.Replace("\n", "\r\n");
                        }

                        if (!socketService.SendMessage(message, sReceivedData, sSuccess, i))
                        {
                            queue.RemoveAt(i);
                        }
                        i++;
                    }
                     MessageQueue = queue;
                    if (MessageQueue.Count == 0)
                    {
                        logService.WriteLog("JourneyManager:SendUDP", "Send UDP: Sent all messages, none remaining to send.");
                    }
                    else
                    {
                       logService.WriteLog("JourneyManager:SendUDP", $"Send UDP: Exiting early, there are still {MessageQueue.Count} messages remaining to send");
                    }

                   
                }
            }).Start();

            return MessageQueue;
        }
    }
}
