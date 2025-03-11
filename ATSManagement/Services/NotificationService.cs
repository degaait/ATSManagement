using ATSManagement.Models;
using ATSManagement.IModels;

namespace ATSManagement.Services
{
    public class NotificationService : INotificationService
    {
        private readonly AtsdbContext atsdbContext;
        public NotificationService(AtsdbContext atsdbContext)
        {
            this.atsdbContext = atsdbContext;
        }

        public void saveNotification(Guid createBy, List<Guid> createdTo, string message)
        {
            List<TblNotification> created = new List<TblNotification>();
            TblNotification notification;
            foreach (var item in createdTo)
            {
                notification = new TblNotification();
                notification.CreatedBy = createBy;
                notification.UserId = item;
                notification.NotificationDate = DateTime.Now;
                notification.NotificationDetail = message;
                notification.FromExternal=false;
                notification.ExterUserId = null;
                created.Add(notification);
            }
            atsdbContext.TblNotifications.AddRange(created);
            atsdbContext.SaveChanges();
        }

    }
}
