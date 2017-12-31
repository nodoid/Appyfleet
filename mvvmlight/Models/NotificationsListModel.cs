using System.Collections.Generic;

namespace mvvmframework
{
    public class NotificationsListModel
    {
        public NotificationsListModel()
        {
            Status = new StatusModel();
        }

        public List<NotificationModel> Notifications { get; set; }

        public StatusModel Status { get; set; }
    }
}
