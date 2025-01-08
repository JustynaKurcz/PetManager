using System.Reflection;

namespace PetManager.Core.Common.EnumHelper;

public class EnumHelper
{
    public static List<EnumItem> GetEnumValues<T>() where T : Enum
    {
        return Enum.GetValues(typeof(T))
            .Cast<T>()
            .Select(e => new EnumItem(Convert.ToInt16(e), GetEnumDisplayName(e)))
            .ToList();
    }

    private static string GetEnumDisplayName<T>(T value) where T : Enum
    {
        var fieldInfo = value.GetType().GetField(value.ToString());
        var displayAttribute = fieldInfo?.GetCustomAttribute<DisplayAttribute>();
        return displayAttribute != null ? displayAttribute.Name : value.ToString();
    }
}