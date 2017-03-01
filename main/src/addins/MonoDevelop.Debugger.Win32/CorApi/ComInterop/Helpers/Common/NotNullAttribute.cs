using System;
using System.Diagnostics;
#pragma warning disable CheckNamespace

namespace JetBrains.Annotations
{
  /// <summary>
  /// Indicates that the value of the marked element could never be <c>null</c>.
  /// </summary>
  /// <example><code>
  /// [NotNull] object Foo() {
  ///   return null; // Warning: Possible 'null' assignment
  /// }
  /// </code></example>
  [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Event | AttributeTargets.Interface | AttributeTargets.Parameter | AttributeTargets.Delegate | AttributeTargets.GenericParameter)]
  [Conditional("JETBRAINS_ANNOTATIONS")]
  public sealed class NotNullAttribute : Attribute
  {
  }

  /// <summary>
  /// Indicates that the return value of method invocation must be used.
  /// </summary>
  [AttributeUsage(AttributeTargets.Method)]
  [Conditional("JETBRAINS_ANNOTATIONS")]
  public sealed class MustUseReturnValueAttribute : Attribute
  {
    [CanBeNull]
    public string Justification { get; private set; }

    public MustUseReturnValueAttribute()
    {
    }

    public MustUseReturnValueAttribute([NotNull] string justification)
    {
      this.Justification = justification;
    }
  }

  /// <summary>
  /// Indicates that the value of the marked element could be <c>null</c> sometimes,
  /// so the check for <c>null</c> is necessary before its usage.
  /// </summary>
  /// <example><code>
  /// [CanBeNull] object Test() =&gt; null;
  /// 
  /// void UseTest() {
  ///   var p = Test();
  ///   var s = p.ToString(); // Warning: Possible 'System.NullReferenceException'
  /// }
  /// </code></example>
  [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Event | AttributeTargets.Interface | AttributeTargets.Parameter | AttributeTargets.Delegate | AttributeTargets.GenericParameter)]
  [Conditional("JETBRAINS_ANNOTATIONS")]
  public sealed class CanBeNullAttribute : Attribute
  {
  }


}