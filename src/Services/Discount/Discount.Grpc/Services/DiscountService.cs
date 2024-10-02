using Discount.Grpc.Data;
using Discount.Grpc.Entities;
using Grpc.Core;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Discount.Grpc.Services
{
    public class DiscountService(DiscountDbContext dbContext, ILogger<DiscountService> logger) : DiscountProtoService.DiscountProtoServiceBase
    {
        public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
        {
            var coupon = await dbContext.Coupons.FirstOrDefaultAsync(x => x.ProductName == request.ProductName);
            coupon ??= new Coupon
            {
                ProductName = "No Discount",
                Amount = 0,
                Description = "No Discount Description"
            };
            return coupon.Adapt<CouponModel>();
        }

        public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
        {
            var coupon = request.Coupon.Adapt<Coupon>();
            if (coupon is null) throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid request."));

            await dbContext.Coupons.AddAsync(coupon);
            await dbContext.SaveChangesAsync();
            logger.LogInformation("Coupon with product name {ProductName} was created successfully", coupon.ProductName);
            return request.Coupon;
        }

        public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
        {
            var coupon = request.Coupon.Adapt<Coupon>();
            dbContext.Coupons.Update(coupon);
            await dbContext.SaveChangesAsync();
            logger.LogInformation("Coupon with product name {ProductName} was updated successfully", coupon.ProductName);
            return coupon.Adapt<CouponModel>();
        }

        public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
        {
            var coupon = await dbContext.Coupons.FirstOrDefaultAsync(x => x.ProductName == request.ProductName)
                ?? throw new RpcException(new Status(StatusCode.NotFound, $"Discount with name {request.ProductName} Not Found."));

            dbContext.Coupons.Remove(coupon);
            await dbContext.SaveChangesAsync();
            logger.LogInformation("Coupon with product name {ProductName} was deleted successfully", coupon.ProductName);

            return new DeleteDiscountResponse { IsSuccess = true };
        }
    }
}