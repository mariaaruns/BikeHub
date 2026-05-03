using BikeHub.Repository.IRepository;
using BikeHub.Shared.Common;
using BikeHub.Shared.Dto.Request;
using BikeHub.Shared.Dto.Response;
using Carter;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Razor;
using Razorpay.Api;
using Razorpay.Api.Errors;
using System.Net;
using static BikeHub.Shared.Enum.Enums;


namespace BikeHub.Features
{
    public class OrderModule : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
      
            app.MapPost("api/orders", async ([FromBody] GetOrderDto dto, IOrderRepository orderRepository) =>
            {
                try
                {
                    var result = await orderRepository.GetOrdersAsync(dto);

                    if (result.Data.Any()) {
                        
                        foreach (var item in result.Data)
                        {
                            item.Image = Path.Combine(commonInfo.BaseUrl, commonInfo.CUSTOMER_IMG_PATH, item.Image);
                        }
                        return Results.Ok(ApiResponse<PagedResult<OrdersDto>>.Success(result));
                    }
                        
                    return Results.Ok(ApiResponse<PagedResult<OrdersDto>>.Fail("No orders found"));

                }
                catch (Exception ex)
                {
                    
                    return Results.InternalServerError(ApiResponse<PagedResult<OrdersDto>>.Fail("Internal Server error, "+ex.Message));
                }
                

            }).WithTags("Orders")
              .WithName("GetOrders")
              .WithDescription("To Get Orders data")
              .WithSummary("To Get Orders data")
              .RequireAuthorization("ORDER_VIEW")
              .Produces<ApiResponse<PagedResult<OrdersDto>>>((int)HttpStatusCode.OK)
              .Produces<ApiResponse<PagedResult<OrdersDto>>>((int)HttpStatusCode.InternalServerError);

            app.MapPost("/api/orders-new", async ([FromBody]AddOrderRequest req ,[FromServices]IOrderRepository orderRepository,
                IConfiguration _config) =>
            {
                try
                {
                    var IsValid = ModelValidator.Validate(req);
                    AddOrderResponse response= new AddOrderResponse();
                    if (!IsValid.IsValid)
                    {
                        return Results.BadRequest(ApiResponse<AddOrderResponse>.Fail("Invalid Request", IsValid.Errors));
                    }

                    var totalAmount= req.OrderItemRequests.Sum(x => {
                                var amount = x.UnitPrice * x.Quantity;
                                var discountAmount = amount * (x.Discount / 100);
                                var finalAmount = amount - discountAmount;
                                return finalAmount;
                            });

                    var razorpayKey = _config["Razorpay:KeyId"];
                    var razorpaySecret = _config["Razorpay:KeySecret"];
                    var client = new RazorpayClient(razorpayKey,razorpaySecret);
                    
                    Dictionary<string, object> options = new Dictionary<string, object>();
                    options.Add("amount", totalAmount * 100); 
                    options.Add("currency", "INR");
                    options.Add("receipt", Guid.NewGuid().ToString());
                    Razorpay.Api.Order razorpayOrder = client.Order.Create(options);
                    
                    
                    req.RazorpayOrderId = razorpayOrder["id"].ToString();



                    var result= await orderRepository.AddOrderAsync(req);

                    if (result!=0)
                    {
                        response.RazorpayOrderId = req.RazorpayOrderId;
                        response.OrderId = result;
                        response.RazorpaySecretKey=razorpayKey;
                        return Results.Ok(ApiResponse<AddOrderResponse>.Success(response,"Order Initiated"));
                    }

                    else
                        return Results.Ok(ApiResponse<AddOrderResponse>.Fail("Failed to add order"));
                }
                catch (Exception)
                {

                    return Results.InternalServerError(ApiResponse<AddOrderResponse>.Fail("Internal Server error!"));
                }
               

            })
               .WithTags("Orders")
               .RequireAuthorization("ORDER_ADD");

            app.MapPost("/api/orders/verify-payment", async ([FromBody] RazorpayVerificationDto data, IOrderRepository orderRepository, IConfiguration config) =>
            {
                try
                {
                    Dictionary<string, string> attributes = new Dictionary<string, string>
                        {
                            { "razorpay_payment_id", data.RazorpayPaymentId },
                            { "razorpay_order_id", data.RazorpayOrderId },
                            { "razorpay_signature", data.RazorpaySignature }
                        };

                    Utils.verifyPaymentSignature(attributes);//if payment no success it throws an error
                    var result = true;
                    var isPaymentSucessUpdate=  await orderRepository.ConfirmPaymentAndQueueEmailAsync(data.OrderId, data.RazorpayOrderId, data.RazorpayPaymentId, data.RazorpaySignature);

                    if (result)
                        return Results.Ok(ApiResponse<string>.Success("Payment verified and order updated"));

                    return Results.BadRequest(ApiResponse<string>.Fail("Payment valid, but failed to update local database"));
                }
                catch (SignatureVerificationError)
                {
                    await orderRepository.UpdateOrderPaymentStatusAsync(data.OrderId, data.RazorpayOrderId, PaymentStatus.Failed    );
                    return Results.BadRequest(ApiResponse<string>.Fail("Invalid payment signature. Security alert!"));
                }
                catch (Exception ex)
                {
                    await orderRepository.UpdateOrderPaymentStatusAsync(data.OrderId, data.RazorpayOrderId, PaymentStatus.Failed);
                    return Results.InternalServerError(ApiResponse<string>.Fail("Internal error during verification"));
                }
            })
               .WithTags("Orders")
               .RequireAuthorization();


            app.MapPut("api/orders/change-status", async ([FromBody]UpdateOrderStatusDto req, [FromServices] IOrderRepository orderRepository) =>
            {
                try
                {
                    var IsValid = ModelValidator.Validate(req);

                    if (!IsValid.IsValid)
                    {
                        return Results.BadRequest(ApiResponse<string>.Fail("Invalid Request", IsValid.Errors));
                    }

                    var result = await orderRepository.UpdateOrderStatusAsync(req);
                    if (!result)
                        return Results.Ok(ApiResponse<string>.Fail("Failed to update order status"));

                    return Results.Ok(ApiResponse<string>.Success("Order status updated successfully"));
                }
                catch (Exception)
                {

                    throw;
                }
                


            })
               .WithTags("Orders")
               .RequireAuthorization("ORDER_EDIT");


            app.MapGet("/api/orders/{id:int}/full-details", async (int id,IOrderRepository orderRepository) =>
            {
                try
                {
                    if (id is 0)
                    {
                        return Results.BadRequest(ApiResponse<IEnumerable<OrderDetailsDto>>.Fail("OrderId is required"));
                    }

                    var result = await orderRepository.GetOrderDetailsAsync(id);


                    if (result !=null)
                    {
                        
                            if (result.Image != null)
                            {
                                result.Image = Path.Combine(commonInfo.BaseUrl, commonInfo.CUSTOMER_IMG_PATH, result.Image); 
                            }

                        
                        return Results.Ok(ApiResponse<OrderDetailsDto>.Success(result));
                    }
                    return Results.Ok(ApiResponse<OrderDetailsDto>.Fail("No order details found"));

                }
                catch (Exception)
                {
                    return Results.InternalServerError(ApiResponse<OrderDetailsDto>.Fail("Internal Error!"));
                    throw;
                }
                

            })
               .WithTags("Orders")
               .RequireAuthorization("ORDER_VIEW");
        }
    }
}
