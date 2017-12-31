using System;
using System.Collections.Generic;
using System.Linq;
using mvvmframework.Enums;
using mvvmframework.Models;

namespace mvvmframework.ViewModels
{
    public class NotificationsViewModel : BaseViewModel
    {
        public NotificationsViewModel()
        {}

        public int GetUnreadNotifications => Notifications.Count(t=>!t.Read);
        public bool GetHasNotifications => GetUnreadNotifications != 0;

        bool allFilter;
        public bool AllFilter
        {
            get => allFilter;
            set { Set(() => AllFilter, ref allFilter, value, true); }
        }

        bool unreadFilter;
        public bool UnreadFilter
        {
            get => unreadFilter;
            set { Set(() => UnreadFilter, ref unreadFilter, value, true); }
        }

        bool readFilter;
        public bool ReadFilter
        {
            get => readFilter;
            set { Set(() => ReadFilter, ref readFilter, value, true); }
        }

        NotificationFiltering CurrentFilter { get; set; }

        List<NotificationModel> notes;
        public List<NotificationModel> Notes
        {
            get => notes;
            set 
            {
                switch (CurrentFilter)
                {
                    case NotificationFiltering.All:
                        Set(() => Notes, ref notes, value, true);
                        break;
                    case NotificationFiltering.Read:
                        Set(() => Notes, ref notes, value.Where(t=>t.Read).ToList(), true);
                        break;
                    case NotificationFiltering.Unread:
                        Set(() => Notes, ref notes, value.Where(t => !t.Read).ToList(), true);
                        break;
                }

                var nl = new List<SmallNotificationModel>();
                foreach(var v in notes)
                {
                    nl.Add(new SmallNotificationModel(v.EventCount)
                    {
                        JourneyDate = v.JourneyDate,
                        Id = v.Id,
                        JourneyId = v.JourneyId,
                        Read = v.Read
                    });
                }

                SmallNoteList = nl;
            }
        }

        List<SmallNotificationModel> smallNoteList;
        public List<SmallNotificationModel> SmallNoteList
        {
            get => smallNoteList;
            set { Set(() => SmallNoteList, ref smallNoteList, value, true); }
        }

        long selectedJourneyId;
        public long SelectedJourneyId
        {
            get => selectedJourneyId;
            set { Set(() => SelectedJourneyId, ref selectedJourneyId, value); }
        }

        public void SetFilter(string name)
        {
            switch(name)
            {
                case "All":
                    AllFilter = true;
                    UnreadFilter = ReadFilter = false;
                    CurrentFilter = NotificationFiltering.All;
                    break;
                case "Unread":
                    UnreadFilter = true;
                    AllFilter = ReadFilter = false;
                    CurrentFilter = NotificationFiltering.Unread;
                    break;
                case "Read":
                    ReadFilter = true;
                    UnreadFilter = AllFilter = false;
                    CurrentFilter = NotificationFiltering.Read;
                    break;
            }
            Notes = Notifications;
        }
    }
}
