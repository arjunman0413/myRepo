using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OCTOBER.EF.Data;
using OCTOBER.EF.Models;
using OCTOBER.Shared;
using Telerik.DataSource;
using Telerik.DataSource.Extensions;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Linq.Dynamic.Core;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Microsoft.CodeAnalysis;
using AutoMapper;
using OCTOBER.Server.Controllers.Base;
using OCTOBER.Shared.DTO;
using static System.Collections.Specialized.BitVector32;

namespace OCTOBER.Server.Controllers.UD
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnrollmentController : BaseController, GenericRestController<EnrollmentDTO>
    {
        public EnrollmentController(OCTOBEROracleContext context,
                        IHttpContextAccessor httpContextAccessor,
                        IMemoryCache memoryCache)
        : base(context, httpContextAccessor)
        {
        }
        [HttpDelete]
        [Route("Delete/{StudentId}/{SectionId}/{SchoolId}")]
        public async Task<IActionResult> Delete(int StudentId, int SectionId, int SchoolId)
        {
            try
            {
                await _context.Database.BeginTransactionAsync();

                var itm = await _context.Enrollments.Where(x => x.StudentId == StudentId)
                    .Where(x => x.SectionId == SectionId)
                    .Where(x => x.SchoolId == SchoolId).FirstOrDefaultAsync();
                if (itm != null)
                {
                    _context.Enrollments.Remove(itm);
                }
                await _context.SaveChangesAsync();
                await _context.Database.CommitTransactionAsync();
                return Ok();
            }
            catch (Exception Dex)
            {
                await _context.Database.RollbackTransactionAsync();
                //List<OraError> DBErrors = ErrorHandling.TryDecodeDbUpdateException(Dex, _OraTranslateMsgs);
                return StatusCode(StatusCodes.Status417ExpectationFailed, "An Error has occurred");
            }
        }
        [HttpGet]
        [Route("Get")]
        public async Task<IActionResult> Get()
        {
            try
            {
                await _context.Database.BeginTransactionAsync();

                var result = await _context.Enrollments.Select(sp => new EnrollmentDTO
                {
                    StudentId = sp.StudentId,
                    SchoolId = sp.SchoolId,
                    SectionId = sp.SectionId,
                    EnrollDate = sp.EnrollDate,
                    FinalGrade = sp.FinalGrade,
                    CreatedBy = sp.CreatedBy,
                    CreatedDate = sp.CreatedDate,
                    ModifiedBy = sp.ModifiedBy,
                    ModifiedDate = sp.ModifiedDate,
                })
                .ToListAsync();
                await _context.Database.RollbackTransactionAsync();
                return Ok(result);
            }
            catch (Exception Dex)
            {
                await _context.Database.RollbackTransactionAsync();
                //List<OraError> DBErrors = ErrorHandling.TryDecodeDbUpdateException(Dex, _OraTranslateMsgs);
                return StatusCode(StatusCodes.Status417ExpectationFailed, "An Error has occurred");
            }
        }
        [HttpGet]
        [Route("Get/{StudentId}/{SectionId}/{SchoolId}")]
        public async Task<IActionResult> Get(int StudentId, int SectionId, int SchoolId)
        {
            try
            {
                await _context.Database.BeginTransactionAsync();
                EnrollmentDTO? result = await _context
                    .Enrollments
                    .Where(x => x.StudentId == StudentId)
                    .Where(x => x.SectionId == SectionId)
                    .Where(x => x.SchoolId == SchoolId)
                     .Select(sp => new EnrollmentDTO
                     {
                         StudentId = sp.StudentId,
                         SchoolId = sp.SchoolId,
                         SectionId = sp.SectionId,
                         EnrollDate = sp.EnrollDate,
                         FinalGrade = sp.FinalGrade,
                         CreatedBy = sp.CreatedBy,
                         CreatedDate = sp.CreatedDate,
                         ModifiedBy = sp.ModifiedBy,
                         ModifiedDate = sp.ModifiedDate,
                     })
                .SingleOrDefaultAsync();
                await _context.Database.RollbackTransactionAsync();
                return Ok(result);
            }
            catch (Exception Dex)
            {
                await _context.Database.RollbackTransactionAsync();
                //List<OraError> DBErrors = ErrorHandling.TryDecodeDbUpdateException(Dex, _OraTranslateMsgs);
                return StatusCode(StatusCodes.Status417ExpectationFailed, "An Error has occurred");
            }
        }
        [HttpPost]
        [Route("Post")]
        public async Task<IActionResult> Post([FromBody] EnrollmentDTO _EnrollmentDTO)
        {
            try
            {
                await _context.Database.BeginTransactionAsync();
                var itm = await _context.Enrollments.Where(x => x.StudentId == _EnrollmentDTO.StudentId)
                    //To check if that enrollment already exists
                    .Where(x => x.SectionId == _EnrollmentDTO.SectionId)
                    .Where(x => x.SchoolId == _EnrollmentDTO.SchoolId)
                    .FirstOrDefaultAsync();
                if (itm == null)
                {
                    Enrollment c = new Enrollment
                    {
                        StudentId = _EnrollmentDTO.StudentId,
                        SchoolId = _EnrollmentDTO.SchoolId,
                        SectionId = _EnrollmentDTO.SectionId,
                        EnrollDate = _EnrollmentDTO.EnrollDate,
                        FinalGrade = _EnrollmentDTO.FinalGrade
                    };
                    _context.Enrollments.Add(c);
                    await _context.SaveChangesAsync();
                    await _context.Database.CommitTransactionAsync();
                }
                return Ok();
            }
            catch (Exception Dex)
            {
                await _context.Database.RollbackTransactionAsync();
                //List<OraError> DBErrors = ErrorHandling.TryDecodeDbUpdateException(Dex, _OraTranslateMsgs);
                return StatusCode(StatusCodes.Status417ExpectationFailed, "An Error has occurred");
            }
        }
        [HttpPut]
        [Route("Put")]
        public async Task<IActionResult> Put([FromBody] EnrollmentDTO _EnrollmentDTO)
        {
            try
            {
                await _context.Database.BeginTransactionAsync();
                var itm = await _context.Enrollments.Where(x => x.StudentId == _EnrollmentDTO.StudentId)
                                        //To check if that enrollment already exists
                                        .Where(x => x.SectionId == _EnrollmentDTO.SectionId)
                                        .Where(x => x.SchoolId == _EnrollmentDTO.SchoolId)
                                        .FirstOrDefaultAsync();
                itm.StudentId = _EnrollmentDTO.StudentId;
                itm.SchoolId = _EnrollmentDTO.SchoolId;
                itm.SectionId = _EnrollmentDTO.SectionId;
                itm.EnrollDate = _EnrollmentDTO.EnrollDate;
                itm.FinalGrade = _EnrollmentDTO.FinalGrade;
                _context.Enrollments.Update(itm);
                await _context.SaveChangesAsync();
                await _context.Database.CommitTransactionAsync();
                return Ok();
            }
            catch (Exception Dex)
            {
                await _context.Database.RollbackTransactionAsync();
                //List<OraError> DBErrors = ErrorHandling.TryDecodeDbUpdateException(Dex, _OraTranslateMsgs);
                return StatusCode(StatusCodes.Status417ExpectationFailed, "An Error has occurred");
            }
        }
        //Implemented the one with arguments at the top
        public Task<IActionResult> Get(int KeyVal)
        {
            throw new NotImplementedException();
        }
        public Task<IActionResult> Delete(int StudentId)
        {
            throw new NotImplementedException();
        }
    }
}