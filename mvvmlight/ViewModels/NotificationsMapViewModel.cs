using mvvmframework.Interfaces;
using mvvmframework.ViewModels.Common;
using System.Linq;
using mvvmframework.Models;
using System.Collections.Generic;

namespace mvvmframework.ViewModels
{
    public class NotificationsMapViewModel : CommonJourneys
    {
        public NotificationsMapViewModel(ILocation loc, IRepository repo, ISockets sock) : base(loc, repo, sock)
        {
        }

        List<EventModel> journeyEvents;
        public List<EventModel> JourneyEvents
        {
            get => journeyEvents;
            set { Set(() => JourneyEvents, ref journeyEvents, value, true); }
        }

        void GetJourneyEvents()
        {
            var notification = Notifications.FirstOrDefault(x => x.JourneyId == SelectedJourneyId);
            if (notification != null)
                JourneyEvents = notification.Events;
        }

        long selectedJourneyId;
        public long SelectedJourneyId
        {
            get => selectedJourneyId;
            set { Set(() => SelectedJourneyId, ref selectedJourneyId, value); }
        }

        int selectedEventId;
        public int SelectedEventId
        {
            get => selectedEventId;
            set { Set(() => SelectedEventId, ref selectedEventId, value); }
        }

        JourneyData selectedJourney;
        public JourneyData SelectedJourney
        {
            get => selectedJourney;
            set { Set(() => SelectedJourney, ref selectedJourney, value); }
        }

        EventModel selectedEvent;
        public EventModel SelectedEvent
        {
            get => selectedEvent;
            set { Set(() => SelectedEvent, ref selectedEvent, value); }
        }

        public string EventType => SelectedEvent.EventType;

        public void GetSelectedJourneyAndEvent()
        {
            SelectedJourney = GetSpecificJourney((int)SelectedJourneyId);
            SelectedEvent = Notifications.FirstOrDefault(w => w.JourneyId == SelectedJourneyId).Events[SelectedEventId];
            LocData = SelectedJourney.GPSData;
        }

        List<JourneyCoordinates> locData;
        public List<JourneyCoordinates> LocData
        {
            get => locData;
            set { Set(() => LocData, ref locData, value, true); }
        }
    }
}