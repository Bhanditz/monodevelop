using System.Diagnostics.SymbolStore;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace CorApi2.SymStore
{
    /// <include file='doc\ISymScope.uex' path='docs/doc[@for="ISymbolScope"]/*' />
    [
        ComVisible(false)
    ]
    public interface ISymbolEncUpdate
    {
        /// <include file='doc\ISymENCUpdate.uex' path='docs/doc[@for="ISymbolEncUpdate.UpdateSymbolStore"]/*' />
   
        void UpdateSymbolStore(IStream stream, SymbolLineDelta[] symbolLineDeltas);
        /// <include file='doc\ISymENCUpdate.uex' path='docs/doc[@for="ISymbolEncUpdate.GetLocalVariableCount"]/*' />
    
        int GetLocalVariableCount(SymbolToken mdMethodToken);
        /// <include file='doc\ISymENCUpdate.uex' path='docs/doc[@for="ISymbolEncUpdate.GetLocalVariables"]/*' />
    
        ISymbolVariable[] GetLocalVariables(SymbolToken mdMethodToken);
    }
}