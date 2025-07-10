using ACUnified.Data.Models;

using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ACUnified.Data.Models;
using ACUnified.Data.DTOs;


namespace ACUnified.Business.Repository
{
    public class ReferenceRepository : IReferenceRepository
    {
      
        private readonly IMapper _mapper;
        private readonly DbContextOptions<ApplicationDbContext> dbOptions;
        public ReferenceRepository(DbContextOptions<ApplicationDbContext> options, IMapper mapper)
        {
            _mapper = mapper;
            dbOptions = options;
        }
    
        public async Task<ReferenceDto> CreateReference(ReferenceDto referenceDto)
        {
            using (var db = new ApplicationDbContext(dbOptions))
            {
                Reference reference = _mapper.Map<ReferenceDto, Reference>(referenceDto);
                var addedReference = db.Reference.Add(reference);
                await db.SaveChangesAsync();
                return _mapper.Map<Reference, ReferenceDto>(addedReference.Entity);
            }
        }
    
        public async Task<IEnumerable<ReferenceDto>> GetAllReference()
        {
            try
            {
                using (var db = new ApplicationDbContext(dbOptions))
                {
                    IEnumerable<ReferenceDto> referenceDtos =
                        _mapper.Map<IEnumerable<Reference>, IEnumerable<ReferenceDto>>(db.Reference);
                    return referenceDtos;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<ReferenceDto> GetReference(string userid)
        {
            try
            {
                using (var db = new ApplicationDbContext(dbOptions))
                {
                    ReferenceDto referenceDto =
                        _mapper.Map<Reference, ReferenceDto>(await db.Reference.FirstOrDefaultAsync(x => x.UserId == userid));
                    //if (referenceDto == null) { return null; }
                    return referenceDto;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task DeleteReference(long Id)
        {
            using (var db = new ApplicationDbContext(dbOptions))
            {
                var reference = db.Reference.FirstOrDefault(x => x.Id == Id);
                if (reference != null)
                {
                    db.Reference.Remove(reference);
                    db.SaveChanges();
                }
            }
        }

        public async Task<ReferenceDto> UpdateReference(ReferenceDto referenceDto)
        {
            try
            {
                using (var db = new ApplicationDbContext(dbOptions))
                {
                    var reference =await db.Reference.FirstOrDefaultAsync(x => x.UserId == referenceDto.UserId);
                    if (reference == null)
                    {
                        return await CreateReference(referenceDto);
                    }
                    else
                    {
                         _mapper.Map(referenceDto, reference);
                        db.Reference.Update(reference);
                        await db.SaveChangesAsync();
                        return _mapper.Map<ReferenceDto>(reference);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating biodata for user {referenceDto.UserId}: {ex.Message}");
                return null;
            }
        }
    }

}
