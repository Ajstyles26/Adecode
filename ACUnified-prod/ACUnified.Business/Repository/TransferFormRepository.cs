using ACUnified.Data.Models;

using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ACUnified.Data.Models;
using ACUnified.Data.DTOs;
using ACUnified.Business.Repository.IRepository;


public class TransferFormRepository : ITransferFormRepository
{

    private readonly IMapper _mapper;
    private readonly DbContextOptions<ApplicationDbContext> dbOptions;
    public TransferFormRepository(DbContextOptions<ApplicationDbContext> options, IMapper mapper)
    {
        _mapper = mapper;
        dbOptions = options;
    }

    public async Task<TransferFormDto> CreateTransferForm(TransferFormDto transferformDto)
    {
        using (var db = new ApplicationDbContext(dbOptions))
        {
            TransferForm transferform = _mapper.Map<TransferFormDto, TransferForm>(transferformDto);
            var addedTransferForm = db.TransferForm.Add(transferform);
            await db.SaveChangesAsync();
            return _mapper.Map<TransferForm, TransferFormDto>(addedTransferForm.Entity);
        }
    }

    public async Task<IEnumerable<TransferFormDto>> GetAllTransferForm()
    {
        try
        {
            using (var db = new ApplicationDbContext(dbOptions))
            {
                IEnumerable<TransferFormDto> transferformDtos =
                    _mapper.Map<IEnumerable<TransferForm>, IEnumerable<TransferFormDto>>(db.TransferForm);
                return transferformDtos;
            }
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    public async Task<TransferFormDto> GetTransferForm(string userid)
    {
        try
        {
            using (var db = new ApplicationDbContext(dbOptions))
            {
                TransferFormDto transferformDto =
                    _mapper.Map<TransferForm, TransferFormDto>(await db.TransferForm.FirstOrDefaultAsync(x => x.UserId == userid));
                //if (transferformDto == null) { return null; }
                return transferformDto;
            }
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    public async Task DeleteTransferForm(long Id)
    {
        using (var db = new ApplicationDbContext(dbOptions))
        {
            var transferform = db.TransferForm.FirstOrDefault(x => x.Id == Id);
            if (transferform != null)
            {
                db.TransferForm.Remove(transferform);
                db.SaveChanges();
            }
        }
    }

    public async Task<TransferFormDto> UpdateTransferForm(TransferFormDto transferformDto)
    {
        try
        {
            using (var db = new ApplicationDbContext(dbOptions))
            {
                var transferform = await db.TransferForm.FirstOrDefaultAsync(x => x.UserId == transferformDto.UserId);
                if (transferform == null)
                {
                    return await CreateTransferForm(transferformDto);
                }
                else
                {
                    _mapper.Map(transferformDto, transferform);
                    db.TransferForm.Update(transferform);
                    await db.SaveChangesAsync();
                    return _mapper.Map<TransferFormDto>(transferform);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating biodata for user {transferformDto.UserId}: {ex.Message}");
            return null;
        }
    }
}

   


