using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Reflection;

namespace BookShop.Pages
{
    public class ReflectModel : PageModel
    {
        public string? Result { get; set; }
        public string? SelectedClass { get; set; }
        public List<string> AvailableClasses { get; set; } = new List<string>
        {
            "BookShop.Models.Book",
            "BookShop.Models.Author",
            "BookShop.Models.Category",
            "BookShop.Models.Order",
            "BookShop.Models.Publisher",
            "BookShop.Models.Customer"
        };

        public void OnGet()
        {
        }

        public void OnPost(string selectedClass)
        {
            SelectedClass = selectedClass;
            
            Assembly assembly = Assembly.Load("BookShop.Models");
            Type t = assembly.GetType(selectedClass);

            if (t == null)
            {
                Result = "Type not found";
                return;
            }

            string result = "";

            object instance = Activator.CreateInstance(t);
           
            result += "Properties in " + t.Name + " class\n";
            PropertyInfo[] properties = t.GetProperties();
            foreach (PropertyInfo property in properties)
            {
                result += property.PropertyType.Name + " " + property.Name + "\n";
            }
            result += "\n";

            result += "Methods in " + t.Name + " class\n";
            MethodInfo[] methods = t.GetMethods();
            foreach (MethodInfo method in methods)
            {
                result += method.ReturnType.Name + " " + method.Name + "\n";
            }
            result += "\n";

            MethodInfo toStringMethod = t.GetMethod("ToString");
            object methodResult = toStringMethod.Invoke(instance, null);
            result += "Invoked using MethodInfo.Invoke: " + methodResult + "\n\n";

            result += "Constructors in " + t.Name + " class\n";
            ConstructorInfo[] constructors = t.GetConstructors();
            foreach (ConstructorInfo constructor in constructors)
            {
                result += constructor.ToString() + "\n";
            }

            Result = result;
        }
    }
}
