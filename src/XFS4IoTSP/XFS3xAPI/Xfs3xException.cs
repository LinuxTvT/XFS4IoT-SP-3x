using HRESULT = System.Int32;

namespace XFS3xAPI
{
    public class Xfs3xException : Exception
    {
        public Xfs3xException(HRESULT result, string? msg = null) : base($"{nameof(Xfs3xException)}: [{result},{RESULT.ToString(result)}]-{msg}")
        {
            ResultError = result;
        }
        public readonly HRESULT ResultError;
    }

    public class InternalException : Xfs3xException
    {
        public InternalException(string? msg = null) : base(RESULT.WFS_ERR_INTERNAL_ERROR, msg) { }
    }

    public class NullResultException : InternalException
    {
        public NullResultException() : base("lpResult is NULL") { }
    }

    public class NullBufferException : InternalException
    {
        public NullBufferException() : base("lpBuffer is NULL") { }
    }
}
