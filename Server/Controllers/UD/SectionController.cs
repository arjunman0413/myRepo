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

namespace OCTOBER.Server.Controllers.UD
{
    [Route("api/[controller]")]
    [ApiController]
    public class SectionController : BaseController, GenericRestController<SectionDTO>
    {
        public SectionController(OCTOBEROracleContext context,
                                IHttpContextAccessor httpContextAccessor,
                                IMemoryCache memoryCache)
                : base(context, httpContextAccessor)
        {
        }

        [HttpDelete]
        [Route("Delete/{SectionId}")]
        public async Task<IActionResult> Delete(int SectionId)
        {
            try
            {
                await _context.Database.BeginTransactionAsync();
                var itm = await _context.Sections.Where(x => x.SectionId == SectionId).FirstOrDefaultAsync();
                if (itm != null)
                {
                    _context.Sections.Remove(itm);
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
                var result = await _context.Sections.Select(sp => new SectionDTO
                {
                    SectionId = sp.SectionId,
                    CourseNo = sp.CourseNo,
                    SectionNo = sp.SectionNo,
                    InstructorId = sp.InstructorId,
                    Capacity = sp.Capacity,
                    Location = sp.Location,
                    StartDateTime = sp.StartDateTime,
                    CreatedBy = sp.CreatedBy,
                    CreatedDate = sp.CreatedDate,
                    ModifiedBy = sp.ModifiedBy,
                    ModifiedDate = sp.ModifiedDate,
                    SchoolId = sp.SchoolId,
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
        [Route("Get/{SchoolID}/{SectionId}")]
        public async Task<IActionResult> Get(int SchoolID, int SectionId)
        {
            try
            {
                await _context.Database.BeginTransactionAsync();
                SectionDTO? result = await _context.Sections
                    .Where(x => x.SectionId == SectionId)
                    .Where(x => x.SchoolId == SchoolID)
                    .Select(sp => new SectionDTO
                    {
                        SectionId = sp.SectionId,
                        CourseNo = sp.CourseNo,
                        SectionNo = sp.SectionNo,
                        InstructorId = sp.InstructorId,
                        Capacity = sp.Capacity,
                        Location = sp.Location,
                        StartDateTime = sp.StartDateTime,
                        CreatedBy = sp.CreatedBy,
                        CreatedDate = sp.CreatedDate,
                        ModifiedBy = sp.ModifiedBy,
                        ModifiedDate = sp.ModifiedDate,
                        SchoolId = sp.SchoolId,
                    })
                .SingleAsync();
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
        //Implemented the version up, with 2 parameters
        public Task<IActionResult> Get(int KeyVal)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        [Route("Post")]

        public async Task<IActionResult> Post([FromBody] SectionDTO _SectionDTO)
        {
            try
            {
                await _context.Database.BeginTransactionAsync();

                var itm = await _context.Sections.Where(x => x.SectionId == _SectionDTO.SectionId)
                    .Where(x => x.SchoolId == _SectionDTO.SchoolId)
                    .FirstOrDefaultAsync();

                if (itm == null)
                {
                    Section s = new Section
                    {
                        //SchoolId = _SectionDTO.SchoolId,
                        SectionId = _SectionDTO.SectionId,
                        CourseNo = _SectionDTO.CourseNo,
                        SectionNo = _SectionDTO.SectionNo,
                        InstructorId = _SectionDTO.InstructorId,
                        Capacity = _SectionDTO.Capacity,
                        Location = _SectionDTO.Location,
                        StartDateTime = _SectionDTO.StartDateTime,
                    };
                    _context.Sections.Add(s);
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

        public async Task<IActionResult> Put([FromBody] SectionDTO _SectionDTO)
        {
            try
            {
                await _context.Database.BeginTransactionAsync();

                var itm = await _context.Sections.Where(x => x.SectionId == _SectionDTO.SectionId)
                    .Where(x => x.SchoolId == _SectionDTO.SchoolId)
                    .FirstOrDefaultAsync();

                itm.SchoolId = _SectionDTO.SchoolId;
                itm.CourseNo = _SectionDTO.CourseNo;
                itm.SectionNo = _SectionDTO.SectionNo;
                itm.InstructorId = _SectionDTO.InstructorId;
                itm.Capacity = _SectionDTO.Capacity;
                itm.Location = _SectionDTO.Location;
                itm.StartDateTime = _SectionDTO.StartDateTime;

                _context.Sections.Update(itm);
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

    }
}
