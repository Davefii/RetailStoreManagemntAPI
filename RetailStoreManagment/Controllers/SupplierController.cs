using BussinessLayer;
using BussinessLayer.DTOs;
using DataAccessLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using SupplierDTO = BussinessLayer.DTOs.SupplierDTO;

namespace RetailStoreMangementApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SupplierController : ControllerBase
    {
        [Authorize(Roles = "Admin,StaffOrCashier,Viewer")]
        [HttpGet("Listsuppliers", Name = "GetAllSuppliers")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public ActionResult<List<SupplierDTO>> GetAll()
        {
            return Ok(Supplier.GetSuppliers());
        }

        [Authorize(Roles = "Admin,StaffOrCashier")]
        [HttpGet("FindsuppliersByID/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public ActionResult<SupplierDTO> GetById(int id)
        {
            var supplier = Supplier.Find(id);
            if (supplier == null)
                return NotFound();

            return Ok(supplier.SDTO);
        }

        [Authorize(Roles = "Admin,StaffOrCashier")]
        [HttpGet("Findsuppliers/by-name/{SupplierName}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public ActionResult<SupplierDTO> GetByName(string SupplierName)
        {
            var supplier = Supplier.Find(name);
            if (supplier == null)
                return NotFound();

            return Ok(supplier.SDTO);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("Addsuppliers", Name = "AddSupplier")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public ActionResult<SupplierDTO> Add([FromBody] SupplierDTO supplierDTO)
        {
            if (supplierDTO == null)
                return BadRequest("Request body is required.");
            var dataDto = new DataAccessLayer.SuppliersData.SupplierDTO(
                supplierDTO.ID,
                supplierDTO.Supplier_Name,
                supplierDTO.Contact_Person,
                supplierDTO.Phone_Number,
                supplierDTO.Address,
                supplierDTO.Email,
                supplierDTO.Status
            );
            var supplier = new Supplier(dataDto, Supplier.enMode.AddNew);

            if (!supplier.Save())
                return BadRequest("Failed to add supplier.");

            return CreatedAtAction(nameof(GetById), new { id = supplier.ID }, supplier.SDTO);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("Updatesuppliers/{id}", Name = "UpdateSupplier")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public ActionResult Update(int id, [FromBody] SupplierDTO supplierDTO)
        {
            if (supplierDTO == null)
                return BadRequest("Request body is required.");

            if (id != supplierDTO.ID)
                return BadRequest("ID mismatch.");
            var dataDto = new DataAccessLayer.SuppliersData.SupplierDTO(
                supplierDTO.ID,
                supplierDTO.Supplier_Name,
                supplierDTO.Contact_Person,
                supplierDTO.Phone_Number,
                supplierDTO.Address,
                supplierDTO.Email,
                supplierDTO.Status
            );
            var supplier = new Supplier(dataDto, Supplier.enMode.Update);

            if (!supplier.Save())
                return BadRequest("Failed to update supplier.");

            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("Deletesuppliers/{id}", Name = "DeleteSupplier")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public ActionResult Delete(int id)
        {
            var deleted = Supplier.DeleteSupplier(id);
            if (!deleted)
                return NotFound();

            return NoContent();
        }

        [HttpGet("total-count-Suppliers")]
        public ActionResult<int> GetTotalCount()
        {
            return Ok(Supplier.GelTotalSuppliers());
        }
    }
}
