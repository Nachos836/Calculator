using JetBrains.Annotations;

// ReSharper disable once CheckNamespace
namespace System.Runtime.CompilerServices
{
    [UsedImplicitly]
    public class RequiredMemberAttribute : Attribute { }

    [UsedImplicitly]
    public class CompilerFeatureRequiredAttribute : Attribute
    {
        public CompilerFeatureRequiredAttribute(string name) { }
    }

    [UsedImplicitly]
    [AttributeUsage(AttributeTargets.Constructor, AllowMultiple = false, Inherited = false)]
    public sealed class SetsRequiredMembersAttribute : Attribute
    {
    }
}
