using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Dicount.Grpc.Entities;
using Dicount.Grpc.Repositories;
using Discount.Grpc.Protos;
using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace Discount.Grpc.Services
{
    public class DiscountService : DiscountProtoService.DiscountProtoServiceBase
    {
        private readonly IDiscountRepository _repository;
        private readonly ILogger<DiscountService> _logger;
        private readonly IMapper _mapper;

        public DiscountService(IDiscountRepository repository, ILogger<DiscountService> logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        public override async Task<CouponModel> GetDiscountAsync(GetDiscountRequest request, ServerCallContext context)
        {
            Coupon coupon = await _repository.GetDiscountAsync(request.ProductName);
            if(coupon == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, $"Discount for ProductName={request.ProductName} not found."));
            }

            _logger.LogInformation("Discount is retrieved for ProductName: {productName}, Amount {amount}", coupon.ProductName, coupon.Amount);
            CouponModel couponModel = _mapper.Map<CouponModel>(coupon);
            return couponModel;
        }

        public override async Task<CouponModel> CreateDiscountAsync(CreateDiscountRequest request, ServerCallContext context)
        {
            Coupon coupon = _mapper.Map<Coupon>(request.Coupon);
            bool result = await _repository.CreateDiscountAsync(coupon);

            if(result == true)
                _logger.LogInformation("Discount was succefully created. ProductName {productName}", coupon.ProductName);
            else
                _logger.LogError("Discount was not created. ProductName {productName}", coupon.ProductName);

            CouponModel couponModel = _mapper.Map<CouponModel>(coupon);
            return couponModel;
        }

        public override async Task<DeleteDiscountResponse> DeleteDiscountAsync(DeleteDiscountRequest request, ServerCallContext context)
        {
          
            bool result = await _repository.DeleteDiscountAsync(request.ProductName);

            if (result == true)
                _logger.LogInformation("Discount was succefully deleted. ProductName {productName}", request.ProductName);
            else
                _logger.LogError("Discount was not update. ProductName {productName}", request.ProductName);


            return new DeleteDiscountResponse
            {
                Success = result
            };
        }

        public override async Task<CouponModel> UpdateDiscountAsync(UpdateDiscountRequest request, ServerCallContext context)
        {
            Coupon coupon = _mapper.Map<Coupon>(request.Coupon);
            bool result = await _repository.UpdateDiscountAsync(coupon);

            if (result == true)
                _logger.LogInformation("Discount was succefully updated. ProductName {productName}", coupon.ProductName);
            else
                _logger.LogError("Discount was not update. ProductName {productName}", coupon.ProductName);

            CouponModel couponModel = _mapper.Map<CouponModel>(coupon);
            return couponModel;
        }

    }
}
