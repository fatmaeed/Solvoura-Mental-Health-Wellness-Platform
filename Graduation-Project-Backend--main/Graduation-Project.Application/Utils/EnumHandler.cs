namespace Graduation_Project.Application.Utils {
    public class EnumHandler<T> where T : Enum {
        public static List<string> GetEnumNames() => Enum.GetNames(typeof(T)).ToList();
        public static List<T> GetEnumValues() => Enum.GetValues(typeof(T)).Cast<T>().ToList();
        public static string? GetEnumName(T value) => Enum.GetName(typeof(T), value);
        public static T GetEnumValue(string name) => (T)Enum.Parse(typeof(T), name);
    }
}
