using MagicVilla_CouponAPI.Models;

namespace MagicVilla_CouponAPI.Data
{
    public static class CouponStore
    {
        public static List<Coupon> Coupons =
        [
            new() { Id = 1, Name = "10OFF", Percent = 10, IsActive = true},
            new() { Id = 1, Name = "20OFF", Percent = 20, IsActive = false}
        ];
    }
}
