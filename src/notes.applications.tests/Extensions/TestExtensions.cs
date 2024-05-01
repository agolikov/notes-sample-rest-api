using System.Reflection;

namespace notes.applications.tests.Extensions
{
    public static class TestExtensions
    {
        public static T ShallowCopy<T>(this T @this)
        {
            MethodInfo method = @this.GetType().GetMethod("MemberwiseClone", BindingFlags.NonPublic | BindingFlags.Instance);
            return (T)method.Invoke(@this, null);
        }
    }
}
