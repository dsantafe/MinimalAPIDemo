using FluentValidation;
using MagicVilla_CouponAPI.DTOs;

namespace MagicVilla_CouponAPI.Validations
{
    public class CouponValidation : AbstractValidator<CouponDTO>
    {
        public CouponValidation()
        {
            RuleFor(model => model.Name).NotEmpty();
            RuleFor(model => model.Percent).InclusiveBetween(1, 100);
        }
    }
}
