using System;

using JetBrains.Annotations;

namespace CorApi.ComInterop.Eventing
{
    /// <summary>
    /// An internal algorithm error. Debugger errors are usually conveyed another way. Should end up in the Exception Submitter probably.
    /// </summary>
    public delegate void InternalErrorDelegate([NotNull] Exception ex);
}