using BussinessLayer;
using DataAccessLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace RetailStoreMangementApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        [Authorize(Roles = "Admin,StaffOrCashier,Viewer")]
        [HttpGet("ListProducts", Name = "GetAllProducts")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public ActionResult<List<ProductDTO>> GetAll()
        {
            return Ok(Products.GetAllProducts());
        }

        [Authorize(Roles = "Admin,StaffOrCashier,Viewer")]
        [HttpGet("FindproductsByID/{id}", Name = "FindProductById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public ActionResult<ProductDTO> GetById(int id)
        {
            var product = Products.Find(id);
            if (product == null)
                return NotFound();

            return Ok(product.PDTO);
        }

        [Authorize(Roles = "Admin,StaffOrCashier")]
        [HttpGet("Findproducts/by-name/{ProductName}", Name = "FindProductByName")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public ActionResult<ProductDTO> GetByName(string ProductName)
        {
            var product = Products.Find(ProductName);
            if (product == null)
                return NotFound();

            return Ok(product.PDTO);
        }

        [Authorize(Roles = "Admin,StaffOrCashier")]
        [HttpPost("Addproducts", Name = "AddProduct")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public ActionResult<ProductDTO> Add([FromBody] ProductDTO productDTO)
        {
            var product = new Products(productDTO);
            product.Mode = Products.enMode.AddNew;

            if (!product.Save())
                return BadRequest("Failed to add product.");

            return CreatedAtAction(nameof(GetById), new { id = product.ID }, product.PDTO);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("Updateproducts/{id}", Name = "UpdateProduct")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public ActionResult Update(int id, [FromBody] ProductDTO productDTO)
        {
            if (id != productDTO.ID)
                return BadRequest("ID mismatch.");

            var product = new Products(productDTO);
            if (!product.Save())
                return BadRequest("Failed to update product.");

            return NoContent();
        }

        [Authorize(Roles = "Admin,StaffOrCashier")]
        [HttpDelete("Deleteproducts/{id}", Name = "DeleteProduct")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public ActionResult Delete(int id)
        {
            var deleted = Products.DeleteProduct(id);
            if (!deleted)
                return NotFound();

            return NoContent();
        }

        [HttpGet("low-stock-count")]
        public ActionResult<int> GetLowStockCount()
        {
            return Ok(Products.getLowProduct());
        }

        [HttpGet("total-count")]
        public ActionResult<int> GetTotalCount()
        {
            return Ok(Products.GetTotalProdutct());
        }
    }
}
