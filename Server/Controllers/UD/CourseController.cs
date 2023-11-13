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
    public class CourseController : BaseController, GenericRestController<CourseDTO>
    {
        public CourseController(OCTOBEROracleContext context,
            IHttpContextAccessor httpContextAccessor,
            IMemoryCache memoryCache)
        : base(context, httpContextAccessor)
        {
        }
        [HttpGet]
        [Route("Get/{SchoolID}/{CourseNo}")]
        //  Route for this is....  <URL>/api/Course/Get/10
        public async Task<IActionResult> Get(int SchoolID, int CourseNo)
        {
            try
            {
                await _context.Database.BeginTransactionAsync();

                CourseDTO? result = await _context
                    .Courses
                    .Where(x => x.CourseNo == CourseNo)
                    .Where(x=>x.SchoolId == SchoolID) 
                     .Select(sp => new CourseDTO
                     {
                         Cost = sp.Cost,
                         CourseNo = sp.CourseNo,
                         CreatedBy = sp.CreatedBy,
                         CreatedDate = sp.CreatedDate,
                         Description = sp.Description,
                         ModifiedBy = sp.ModifiedBy,
                         ModifiedDate = sp.ModifiedDate,
                         Prerequisite = sp.Prerequisite
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
        [HttpGet]
        [Route("Get")]
        public async Task<IActionResult> Get()
        {
            try
            {
                await _context.Database.BeginTransactionAsync();

                var result = await _context.Courses.Select(sp => new CourseDTO
                {
                    Cost = sp.Cost,
                    CourseNo = sp.CourseNo,
                    CreatedBy = sp.CreatedBy,
                    CreatedDate = sp.CreatedDate,
                    Description = sp.Description,
                    ModifiedBy = sp.ModifiedBy,
                    ModifiedDate = sp.ModifiedDate,
                    Prerequisite = sp.Prerequisite
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
        [HttpPost]
        [Route("Post")]
        public async Task<IActionResult> Post([FromBody]
                                                CourseDTO _CourseDTO)
        {
            try
            {
                await _context.Database.BeginTransactionAsync();
                var itm = await _context.Courses.Where(x => x.CourseNo == _CourseDTO.CourseNo).FirstOrDefaultAsync();
                if (itm == null)
                {
                    Course c = new Course
                    {
                        Cost = _CourseDTO.Cost,
                        CourseNo = _CourseDTO.CourseNo,
                        Description = _CourseDTO.Description,
                        Prerequisite = _CourseDTO.Prerequisite
                    };
                    _context.Courses.Add(c);
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
        public async Task<IActionResult> Put([FromBody]
                                                CourseDTO _CourseDTO)
        {
            Debugger.Launch();
            try
            {
                await _context.Database.BeginTransactionAsync();
                var itm = await _context.Courses.Where(x => x.CourseNo == _CourseDTO.CourseNo).FirstOrDefaultAsync();
                itm.Description = _CourseDTO.Description;
                itm.Cost = _CourseDTO.Cost;
                itm.Prerequisite = _CourseDTO.Prerequisite;
                _context.Courses.Update(itm);
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
        [HttpDelete]
        [Route("Delete/{CourseNo}")]
        public async Task<IActionResult> Delete(int CourseNo)
        {
            Debugger.Launch();
            try
            {
                await _context.Database.BeginTransactionAsync();

                var itm = await _context.Courses.Where(x => x.CourseNo == CourseNo).FirstOrDefaultAsync();

                if (itm != null)
                {
                    _context.Courses.Remove(itm);
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
        public Task<IActionResult> Get(int KeyVal)
        {
            throw new NotImplementedException();
        }
    }
}
