using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;

namespace CoreAspShop.Extensions
{
    public static class IEnumerableExtensions
    {
        public static IEnumerable<SelectListItem> ToSelectListItems<T>(this IEnumerable<T> items, int selectedValue)
        {
            return items.Select(x => new SelectListItem
            {
                //Высвечивится в самом выподающем списке (то, что видит пользователь. Например наш Test Special Tag)
                Text = x.GetPropertyValue("Name"),
                //То, что скрыто от пользователя. Используется для передачи, хранения информации. Чаще всего id
                Value = x.GetPropertyValue("Id"),
                //Тот эллемент, который выбран по умолчанию.
                Selected = x.GetPropertyValue("Id").Equals(selectedValue.ToString())
            });
        }
    }
}
