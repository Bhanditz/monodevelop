using System.Runtime.InteropServices;

namespace CorApi2.SymStore
{
    [Guid("7DAC8207-D3AE-4c75-9B67-92801A497D44"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [ComVisible(true)]
    public interface IMetadataImportPrivateComVisible
    {
        // Just need a single placeholder method so that it doesn't complain about an empty interface.
        void Placeholder();
    }
}