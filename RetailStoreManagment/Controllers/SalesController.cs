using BussinessLayer;
using DataAccessLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace RetailStoreMangementApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class SalesController : ControllerBase
    {
        [Authorize(Roles = "Admin,StaffOrCashier,Viewer")]
        [HttpGet("Listsales", Name = "GetAllSales")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public ActionResult<List<SalesDTO>> GetAll()
        {
            return Ok(Sales.GetAllListSeles());
        }

        [Authorize(Roles = "Admin,StaffOrCashier,Viewer")]
        [HttpGet("FindsalesByID/{id}", Name = "FindSaleById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public ActionResult<SalesDTO> GetById(int id)
        {
            var sale = Sales.Find(id);
            if (sale == null)
                return NotFound();

            return Ok(sale.SalesDTO);
        }

        [Authorize(Roles = "Admin,StaffOrCashier")]
        [HttpPost("AddSale", Name = "AddSale")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public ActionResult<SalesDTO> Add([FromBody] SalesDTO salesDTO)
        {
            var sale = new Sales(salesDTO);
            sale.Mode = Sales.enMode.AddNew;

            if (!sale.Save())
                return BadRequest("Failed to add sale.");

            return CreatedAtAction(nameof(GetById), new { id = sale.ID }, sale.SalesDTO);
        }

        [Authorize(Roles = "Admin,StaffOrCashier")]
        [HttpPut("UpdateSaleBy/{id}", Name = "UpdateSaleByid")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public ActionResult Update(int id, [FromBody] SalesDTO salesDTO)
        {
            if (id != salesDTO.ID)
                return BadRequest("ID mismatch.");

            var sale = new Sales(salesDTO);
            if (!sale.Save())
                return BadRequest("Failed to update sale.");

            return NoContent();
        }

        [HttpDelete("DeleteSaleBy/{id}", Name = "DeleteSaleByid")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult Delete(int id)
        {
            var deleted = Sales.DeleteSale(id);
            if (!deleted)
                return NotFound();

            return NoContent();
        }

        [HttpGet("sales/total", Name = "Get_Total_Sales")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<int> GetTotalSales()
        {
            return Ok(Sales.GetTotalSales());
        }
    }
}
