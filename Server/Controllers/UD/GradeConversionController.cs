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
    public class GradeConversionController : BaseController, GenericRestController<GradeConversionDTO>
    {
        public GradeConversionController(OCTOBEROracleContext context,
                        IHttpContextAccessor httpContextAccessor,
                        IMemoryCache memoryCache)
        : base(context, httpContextAccessor)
        {
        }
        [HttpDelete]
        [Route("Delete/{SchoolId}/{LetterGrade}")]
        public async Task<IActionResult> Delete(int SchoolId, string LetterGrade)
        {
            try
            {
                await _context.Database.BeginTransactionAsync();

                var itm = await _context.GradeConversions.Where(x => x.SchoolId == SchoolId)
                    .Where(x => x.LetterGrade.Equals(LetterGrade)).FirstOrDefaultAsync();

                if (itm != null)
                {
                    _context.GradeConversions.Remove(itm);
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

                var result = await _context.GradeConversions.Select(sp => new GradeConversionDTO
                {
                    SchoolId = sp.SchoolId,
                    LetterGrade = sp.LetterGrade,
                    GradePoint = sp.GradePoint,
                    MaxGrade = sp.MaxGrade,
                    MinGrade = sp.MinGrade,
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
        [Route("Get/{SchoolId}/{LetterGrade}")]
        public async Task<IActionResult> Get(int SchoolId, string LetterGrade)
        {
            try
            {
                await _context.Database.BeginTransactionAsync();

                GradeConversionDTO? result = await _context
                    .GradeConversions
                    .Where(x => x.SchoolId == SchoolId)
                    .Where(x => x.LetterGrade.Equals(LetterGrade))
                     .Select(sp => new GradeConversionDTO
                     {
                         SchoolId = sp.SchoolId,
                         LetterGrade = sp.LetterGrade,
                         GradePoint = sp.GradePoint,
                         MaxGrade = sp.MaxGrade,
                         MinGrade = sp.MinGrade,
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

        public async Task<IActionResult> Post([FromBody] GradeConversionDTO _GradeConversionDTO)
        {
            try
            {
                await _context.Database.BeginTransactionAsync();

                var itm = await _context.GradeConversions.Where(x => x.SchoolId == _GradeConversionDTO.SchoolId)
                    //To check if that enrollment already exists
                    .Where(x => x.LetterGrade.Equals(_GradeConversionDTO.LetterGrade))
                    .FirstOrDefaultAsync();

                if (itm == null)
                {
                    GradeConversion g = new GradeConversion
                    {
                        SchoolId = _GradeConversionDTO.SchoolId,
                        LetterGrade = _GradeConversionDTO.LetterGrade,
                        GradePoint = _GradeConversionDTO.GradePoint,
                        MaxGrade = _GradeConversionDTO.MaxGrade,
                        MinGrade = _GradeConversionDTO.MinGrade,
                    };
                    _context.GradeConversions.Add(g);
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

        public async Task<IActionResult> Put([FromBody] GradeConversionDTO _GradeConversionDTO)
        {
            try
            {
                await _context.Database.BeginTransactionAsync();

                var itm = await _context.GradeConversions.Where(x => x.SchoolId == _GradeConversionDTO.SchoolId)
                                        //To check if that enrollment already exists
                                        .Where(x => x.LetterGrade.Equals(_GradeConversionDTO.LetterGrade))
                                        .FirstOrDefaultAsync();

                itm.SchoolId = _GradeConversionDTO.SchoolId;
                itm.LetterGrade = _GradeConversionDTO.LetterGrade;
                itm.GradePoint = _GradeConversionDTO.GradePoint;
                itm.MaxGrade = _GradeConversionDTO.MaxGrade;
                itm.MinGrade = _GradeConversionDTO.MinGrade;

                _context.GradeConversions.Update(itm);
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
        public Task<IActionResult> Delete(int KeyVal)
        {
            throw new NotImplementedException();
        }

        public Task<IActionResult> Get(int KeyVal)
        {
            throw new NotImplementedException();
        }
    }
}
