using Core.Dtos;
using Core.Interfaces;
using Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VendorController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public VendorController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        // GET: api/v1/Vendors/GetByIdAsync/5
        [HttpGet("GetByIdAsync/{id}")]
        public async Task<ActionResult<Document>> GetByIdAsync(string? id)
        {
            if (id == null)
                return NotFound();

            var document = await _unitOfWork.Vendors.GetByIdAsync(id);

            return Ok(document);
        }

        // GET: api/v1/Vendors/GetAllAsync
        [HttpGet("GetAllAsync")]
        public async Task<ActionResult<IEnumerable<Vendor>>> GetAllAsync()
        {
            return Ok(await _unitOfWork.Vendors.GetAllAsync());
        }

        // PUT: api/v1/Vendors/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("UpdateAsync/{id}")]
        public async Task<IActionResult> PutDocuments(string id, [FromBody] Vendor vendor)
        {
            if (id != vendor.VendCode)
            {
                return BadRequest();
            }

            _unitOfWork.Vendors.Update(vendor);
            _unitOfWork.Complete();

            return Ok(vendor);
        }

        // POST: api/v1/Vendors
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("AddAsync")]
        public async Task<ActionResult<List<Document>>> PostDocuments([FromBody] Vendor dto)
        {
            var vendor = new Vendor
            {
                VendCode = dto.VendCode,
                VendName = dto.VendName,  

                OriginApplication = dto.OriginApplication,
                Regional = dto.Regional,
                SbsSid = dto.SbsSid,
                ApFlag = dto.ApFlag

            };
            await _unitOfWork.Vendors.AddAsync(vendor);
            _unitOfWork.Complete();

            return Ok(vendor);
        }
    }
}
