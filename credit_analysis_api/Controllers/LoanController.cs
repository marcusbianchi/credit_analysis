using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace credit_analysis_api.Controllers
{
    [ApiController]
    [Route("loan")]
    public class LoanController : ControllerBase
    {


        private readonly ILoanService _loanService;
        private readonly IDBService _DBService;
        public LoanController(ILoanService loanService, IDBService DBService)
        {
            _loanService = loanService;
            _DBService = DBService;
        }

        /// <summary>
        /// Create a new Loan Request based on a Loan JSON
        /// </summary>
        /// <param name="loan">Loan JSON</param>
        /// <returns>The created Loan Request</returns>
        /// <response code="201">Returns the newly created item</response>
        /// <response code="400">If the json is invalid</response>       
        /// <response code="500">If there is an internal error</response>   
        [HttpPost]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<ActionResult> Post(Loan loan)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    loan.id = Guid.NewGuid().ToString();
                    var uuid = await _loanService.AddLoanRequestToQueue(loan);
                    if (uuid != "")
                        return Created("/loan/" + uuid, new Loan { id = uuid });
                    return StatusCode(500);
                }
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.ToString());
            }
        }

        /// <summary>
        /// Get a specific Request status
        /// </summary>
        /// <param name="id">Request ID</param>
        /// <returns>The requested Loan Request</returns>
        /// <response code="200">Returns the requested item</response>
        /// <response code="404">If the item was not found</response>       
        /// <response code="500">If there is an internal error</response>
        [HttpGet("{id}")]
        [Produces("application/json")]
        public async Task<ActionResult> Get(string id)
        {
            try
            {

                var item = await _DBService.Read(id);
                if (item == null)
                    return NotFound();
                return Ok(item);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.ToString());
            }
        }

        /// <summary>
        /// Used for backend APIs Update request status
        /// </summary>
        /// <param name="id">Request ID</param>
        /// <param name="loanRequest">Request New Status</param>
        /// <returns>The Updated Loan Request</returns>
        /// <response code="200">Returns the newly updated item</response>
        /// <response code="400">If the json is invalid</response>       
        /// <response code="500">If there is an internal error</response> 
        [HttpPut("/loan/api/{id}")]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<ActionResult> Put(string id, [FromBody]LoanRequest loanRequest)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var item = await _DBService.Update(id, loanRequest);
                    if (item == null)
                        return NotFound();
                    return Ok(item);
                }
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.ToString());
            }
        }
    }
}
