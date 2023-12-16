namespace SAPLink.API.Controllers
{
    [Route("api/v1/System/[controller]")]
    [ApiController]
    public partial class RecurringController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public RecurringController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // POST: api/v1/System/Recurring/AddAsync
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("AddAsync")]
        public async Task<ActionResult<List<Recurrence>>> AddAsync([FromBody] RecurrenceDto dto)
        {
            var recurrence = new Recurrence
            {
                Interval = dto.Interval,
                Recurring = dto.Recurring,
                Document = dto.Document,
                DayOfWeek = dto.DayOfWeek
            };
            await _unitOfWork.Recurrences.AddAsync(recurrence);
            _unitOfWork.SaveChanges();

            return Ok(recurrence);
        }
        // GET: api/v1/System/Recurring/GetAllAsync
        [HttpGet("GetAllAsync")]
        public async Task<ActionResult<Recurrence>> GetAllAsync()
        {
            var recurrence = await _unitOfWork.Recurrences.GetAllAsync();

            return Ok(recurrence);
        }

        // GET: api/v1/System/Recurring/GetByIdAsync/5
        [HttpGet("GetByIdAsync/{id}")]
        public async Task<ActionResult<Recurrence>> GetByIdAsync(int? id)
        {
            if (id == null)
                return NotFound();

            var recurrence = await _unitOfWork.Recurrences.GetByIdAsync((int)id);

            return Ok(recurrence);
        }


        // PUT: api/v1/System/Recurring/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("UpdateAsync/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Recurrence recurrence)
        {
            var req = _unitOfWork.Recurrences.FindAsync(x => x.Id == id);

            if (req != null && req.Id == id)
            {
                _unitOfWork.Recurrences.Update(recurrence);
                _unitOfWork.SaveChanges();

                return Ok(req);
            }
            return BadRequest();

        }
        // Delete: api/v1/System/Recurring/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpDelete("DeleteAsync/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var recurrence = _unitOfWork.Recurrences.GetById(id.ToString());
            if (recurrence != null)
            {
                _unitOfWork.Recurrences.Delete(recurrence);
                _unitOfWork.SaveChanges();
                return Ok($"Recurrence {recurrence.Document} is deleted");
            }

            return BadRequest();
        }
    }
}