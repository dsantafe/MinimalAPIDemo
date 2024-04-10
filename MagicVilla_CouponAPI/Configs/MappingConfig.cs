using AutoMapper;
using MagicVilla_CouponAPI.DTOs;
using MagicVilla_CouponAPI.Models;

namespace MagicVilla_CouponAPI.Configs
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<Coupon, CouponDTO>().ReverseMap();
        }
    }
}