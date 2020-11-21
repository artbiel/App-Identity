using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.Api
{
    public static class Extentions
    {
        public static Dictionary<string, string> ToErrorsDictionary(this ModelStateDictionary modelState) =>
            modelState.ToDictionary(kvp => kvp.Key.Split('.').Last(), kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).FirstOrDefault());

        public static Dictionary<string, string> ToErrorsDictionary(this IEnumerable<IdentityError> errors) =>
            errors.ToDictionary(_ => "", e => e.Description);
    }
}
