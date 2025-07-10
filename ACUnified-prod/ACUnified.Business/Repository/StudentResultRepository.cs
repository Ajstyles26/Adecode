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
using static ACUnified.Portal.MenuHelper.MainMenu;

namespace ACUnified.Business.Repository
{
    public class StudentResultRepository : IStudentResultRepository
    {
        private readonly DbContextOptions<ApplicationDbContext> dbOptions;
        private readonly IMapper _mapper;
        public StudentResultRepository(DbContextOptions<ApplicationDbContext> options, IMapper mapper)
        {
            _mapper = mapper;
            dbOptions = options;
        }

        public async Task<IEnumerable<StudentResultDto>> GetResultByMatric(string matric, long semesterId, long sessionId)
        {
            try
            {
                using (var db = new ApplicationDbContext(dbOptions))
                {
                    IEnumerable<StudentResultDto> studentDtos =
                       _mapper.Map<IEnumerable<StudentResult>, IEnumerable<StudentResultDto>>(db.StudentResult.
                       Include(x => x.Course).
                       Include(x => x.Session)
                       .Include(x => x.Semester).Where(x => x.SemesterId == semesterId
                       && x.SessionId == sessionId && x.MatricNo == matric && x.Grade != "R").Distinct());
                    return studentDtos;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public async Task<IEnumerable<StudentResultDto>> GetResultByStudentId(long studentId, long semesterId, long sessionId)
        {
            try
            {
                using (var db = new ApplicationDbContext(dbOptions))
                {
                    IEnumerable<StudentResultDto> studentDtos =
                        _mapper.Map<IEnumerable<StudentResult>, IEnumerable<StudentResultDto>>(db.StudentResult.
                        Include(x=>x.Course).
                        Include(x=>x.Session).
                        Include(x=>x.Semester).
                        Where(x => x.SemesterId == semesterId
                        && x.SessionId == sessionId && x.StudentId == studentId && x.Grade != "R").Distinct());
                    return studentDtos;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<IEnumerable<StudentResultListDto>> GetResultListingByMatric(string matric)
        {
            try
            {
                using (var db = new ApplicationDbContext(dbOptions))
                {
                    var studentDto = db.StudentResult.Include(x=>x.Course)
                .Where(sr => sr.MatricNo == matric)
                .GroupBy(sr => new { sr.StudentId, sr.SessionId, sr.SemesterId })
                .Select(g => new StudentResultListDto
                {
                    StudentId = g.Key.StudentId,
                    SessionId = g.Key.SessionId,
                    SemesterId = g.Key.SemesterId,
                    Name = $"Session {g.Key.SessionId}, Semester {g.Key.SemesterId}"
                }).AsEnumerable();



                    return studentDto;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<IEnumerable<StudentResultListDto>> GetResultListingByStudentId(long studentId)
        {
            try
            {
                using (var db = new ApplicationDbContext(dbOptions))
                {
                    var studentDto = db.StudentResult
                .Where(sr => sr.StudentId == studentId)
                .GroupBy(sr => new { sr.StudentId, sr.SessionId, sr.SemesterId })
                .Select(g => new StudentResultListDto
                {
                    StudentId = g.Key.StudentId,
                    SessionId = g.Key.SessionId,
                    SemesterId = g.Key.SemesterId,
                    Name = $"Session {g.Key.SessionId}, Semester {g.Key.SemesterId}"
                }).AsEnumerable();



                    return studentDto;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

    }
}
