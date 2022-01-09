using DineNDash.DataAccess;
using DineNDash.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DineNDash.Controllers
{
    [Route("api/productOrder")]
    [ApiController]
    public class ProductOrderController : ControllerBase
    {
        ProductOrderRepository _productOrderRepository;
        OrderRepository _orderRepository;
        ProductRepository _productRepository;

        public ProductOrderController(ProductOrderRepository productOrderRepo, OrderRepository orderRepo, ProductRepository productRepo)
        {
            _productOrderRepository = productOrderRepo;
            _orderRepository = orderRepo;
            _productRepository = productRepo;
        }

        // Get all Product Orders
        [HttpGet]
        public IActionResult GetAllProductOrders()
        {
            return Ok(_productOrderRepository.GetAll());
        }

        // Get Single Produc Order by Id //
        [HttpGet("{id}")]
        public IActionResult GetProductOrderById(Guid id)
        {
            var prdouctOrder = _productOrderRepository.GetById(id);

            if (prdouctOrder == null)
            {
                return NotFound($"No Product Order with the Id of {id} was found.");
            }


            return Ok(prdouctOrder);
        }

        // Get Orders's ProductOrders by Order Id //
        [HttpGet("order/{orderId}")]
        public IActionResult GetProductOdersByOrderId(Guid orderId)
        {
            return Ok(_productOrderRepository.GetAssociatedProductOrders(orderId));
        }

        // Get User's Orders //
        [HttpGet("user/{userId}")]
        public IActionResult GetAllUserOrders(Guid userId)
        {
            return Ok(_orderRepository.GetUserOrders(userId));
        }

        // Update ProductOrder by Id //
        [HttpPut]

        public IActionResult UpdateProductOrder(Guid id, ProductOrder productOrder)
        {

            var productOrdertoUpdate = _productOrderRepository.GetById(id);

            if (productOrdertoUpdate == null)
            {
                return NotFound($"The Product Order associated with the Id of {id} could not be located ");
            }

            var updatedProductOrder = _productOrderRepository.UpdateProductOrder(id, productOrder);

            return Ok(updatedProductOrder);

        }
        [HttpPost]

        //Create(Add) Product Order //
        // A product or is the order of the indivual products that make up a full order //

        public IActionResult CreateProductOrder(CreateProductOrderCommand command)
        {
            // Re-uses previously written method to grab the product by the Id of the ProductOrder Command Model property and sets it as the 'productToOrder' variable //
            var productToOrder = _productRepository.GetById(command.ProductId);

            // Re-uses previously written method to grab the order by the Id of the ProductOrder Command Model property and sets it as the 'orderToCreate' variable //
            var orderToCreate = _orderRepository.GetById(command.OrderId);


            // Checks to see if this product is in the database. If not it will return the string below //
            if (productToOrder == null)
                return NotFound("There was no matching Product in the database.");

            // Checks to see if this order is already created in the database. If not it will return the string below //
            if (orderToCreate == null)
                return NotFound("There was no matching Order in the database");


            // Creating new variable that abstiantiates a new ProductOrder //
            // Set's the properties of this new ProductOrder //
            var newProductOrder = new ProductOrder
            {
                ProductId = productToOrder.Id,
                OrderId = orderToCreate.Id,
                ProductQuantity = command.ProductOrderQuantity,
                // Multiplies the price of the product by the quantity of that product being ordered //
                TotalCost = productToOrder.Price * command.ProductOrderQuantity
            };

            // Re-uses previously written method to add the new Product Order to the product order repo //
            _productOrderRepository.Add(newProductOrder);

            // Sets variable to grab the current quantity of the chosen product in the database //
            var currentProductQauntity = productToOrder.Quantity;

            // Grabs the quantity of the chosen product in the database and sets it to a variable that subrtracts it by the product quantity of the product order //
            // Thus, always updating the database product quantity when someone has items in their cart // 
            // This makes sure that no two customers can have items in their cart, but then when it comes to checkout the product actually be out of stock if another customer checkouts before them //
            var difference = productToOrder.Quantity - command.ProductOrderQuantity;

            // Creates a new instance of the updated product information and sets it to a variable with all the new values //
            var subtractProductOrder = new Product
            {
                ProductName = productToOrder.ProductName,
                Id = productToOrder.Id,
                Quantity = difference,
                ProductDescription = productToOrder.ProductDescription,
                Price = productToOrder.Price,
            };


            // Will return if item is not in stock //
            if (currentProductQauntity <= 0)
            {
                return NotFound($"{subtractProductOrder.ProductName} is not in stock. Please choose another product.");
            }

            // Will return if the user tries to order a larger quantity of a product than what is available in stock //
            if (subtractProductOrder.Quantity < 0)
            {
                return NotFound($"There are only {currentProductQauntity} {productToOrder.ProductName} in stock. You added {newProductOrder.ProductQuantity} to your cart. Please add {currentProductQauntity} or less to you order.");
            }

            // Re-uses put method written to update the product repo with the new values //
            // Subtracts the quantity chosen for the productOrder from the product quantity //
            _productRepository.Update(command.ProductId, subtractProductOrder);


            return Created($"/api/orders/{newProductOrder.Id}", newProductOrder);

        }


        // Delete Product Order //
        [HttpDelete("{id}")]
        public IActionResult DeleteProductOrder(Guid id)
        {
            _productOrderRepository.Remove(id);

            return Ok();
        }
    }
}
