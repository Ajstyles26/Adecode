using ACUnified.Data.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACUnified.Business.Repository.IRepository
{
    public interface IRemitaNotificationRepository
    {
        Task<RemitaNotificationDto> CreateRemitaNotification(RemitaNotificationDto remitaNotificationDto);
        Task<IEnumerable<RemitaNotificationDto>> GetAllRemitaNotification();
        Task<RemitaNotificationDto> GetRemitaNotification(long Id);
        Task<RemitaNotificationDto> GetRemitaNotificationByOrderId(string OrderId);
        Task DeleteRemitaNotification(long Id);
        Task<RemitaNotificationDto> UpdateRemitaNotification(RemitaNotificationDto remitaNotificationDto);
    }
}
