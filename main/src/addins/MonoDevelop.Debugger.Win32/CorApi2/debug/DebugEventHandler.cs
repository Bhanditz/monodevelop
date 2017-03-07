using System;

using CorApi.ComInterop.Eventing;

namespace CorApi2.debug
{
    public delegate void DebugEventHandler<in TArgs> (Object sender, TArgs args) where TArgs : CorEventArgs;
}