using ACUnified.Business.Repository.IRepository;
using ACUnified.Data.DTOs;
using ACUnified.Data.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACUnified.Business.Repository
{
    public class RemitaNotificationRepository : IRemitaNotificationRepository
    {
        private readonly IMapper _mapper;
        private readonly DbContextOptions<ApplicationDbContext> dbOptions;
        public RemitaNotificationRepository(IMapper mapper, DbContextOptions<ApplicationDbContext> options)
        {
            _mapper = mapper;

            dbOptions = options;
        }

        public async Task<RemitaNotificationDto> CreateRemitaNotification(RemitaNotificationDto remitaNotificationDto)
        {
            using (var db = new ApplicationDbContext(dbOptions))
            {
                RemitaNotification
                    remitaNotification = _mapper.Map<RemitaNotificationDto, RemitaNotification>(remitaNotificationDto);

                var addedRemitaNotification = db.RemitaNotification.Add(remitaNotification);
                await db.SaveChangesAsync();
                return _mapper.Map<RemitaNotification, RemitaNotificationDto>(addedRemitaNotification.Entity);
            }
        }

        public  Task DeleteRemitaNotification(long Id)
        {
            using (var db = new ApplicationDbContext(dbOptions))
            {
                var remitaNotification = db.RemitaNotification.FirstOrDefault(x => x.Id == Id);
                if (remitaNotification != null)
                {
                    db.RemitaNotification.Remove(remitaNotification);
                    db.SaveChanges();
                }
            }

            return  Task.CompletedTask;
        }

        public async Task<IEnumerable<RemitaNotificationDto>> GetAllRemitaNotification()
        {
            try
            {
                using (var db = new ApplicationDbContext(dbOptions))
                {
                    IEnumerable<RemitaNotificationDto> remitaNotificationDtos = _mapper.Map<IEnumerable<RemitaNotification>, IEnumerable<RemitaNotificationDto>>( db.RemitaNotification);
                  
                    return  remitaNotificationDtos;
                }

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<RemitaNotificationDto> GetRemitaNotification(long Id)
        {
            try
            {
                using (var db = new ApplicationDbContext(dbOptions))
                {
                    RemitaNotificationDto remitaNotificationDto =
                        _mapper.Map<RemitaNotification, RemitaNotificationDto>(await db.RemitaNotification.FirstOrDefaultAsync(x => x.Id == Id));
                    return remitaNotificationDto;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<RemitaNotificationDto> GetRemitaNotificationByOrderId(string OrderId)
        {
            try
            {
                using (var db = new ApplicationDbContext(dbOptions))
                {
                    RemitaNotificationDto remitaNotificationDto =
                        _mapper.Map<RemitaNotification, RemitaNotificationDto>(await db.RemitaNotification.FirstOrDefaultAsync(x => x.orderId == OrderId));
                    return remitaNotificationDto;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<RemitaNotificationDto> UpdateRemitaNotification(RemitaNotificationDto remitaNotificationDto)
        {
            try
            {
                using (var db = new ApplicationDbContext(dbOptions))
                {
                    RemitaNotification remitaNotification = db.RemitaNotification.FirstOrDefault(x => x.Id == remitaNotificationDto.Id);
                    if (remitaNotification == null)
                    {
                        return null;
                    }
                    else
                    {
                        RemitaNotification remitaNotificationUpdate = _mapper.Map<RemitaNotificationDto, RemitaNotification>(remitaNotificationDto, remitaNotification);
                        var currentupdate = db.RemitaNotification.Update(remitaNotificationUpdate);

                        await db.SaveChangesAsync();
                        return _mapper.Map<RemitaNotification, RemitaNotificationDto>(currentupdate.Entity);
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
