using Microsoft.AspNetCore.Mvc.Filters;

namespace TaskManager.Common.Validation.ValidationModel
{
    public class ValidateModelAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                context.Result = new ValidationModelResult(context.ModelState);
            }
        }
    }
}