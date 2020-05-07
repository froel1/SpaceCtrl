using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SpaceCtrl.Front.Models.Common;
using SpaceCtrl.Front.Models.Record;
using SpaceCtrl.Front.Services;

namespace SpaceCtrl.Front.Controllers
{
    [Route("v1/record")]
    public class RecordController : Controller
    {
        private readonly RecordService _service;

        public RecordController(RecordService service)
        {
            _service = service;
        }

        [HttpGet("list")]
        public async Task<ActionResult<PagedList<RecordModel>>> GetRecordsAsync([FromQuery] PaginationWithFilter<RecordFilterModel> model) =>
            await _service.GetRecordsAsync(model);

        [HttpGet("attendances")]
        public async Task<ActionResult<List<PersonGroupModel>>> GetAttendancesAsync(RecordFilterModel filter) =>
            await _service.GetAttendancesAsync(filter);
    }
}