using System.Runtime.InteropServices;
using XFS3xAPI;
using XFS3xAPI.PIN;
using XFS4IoTFramework.Keyboard;
using XFS4IoTFramework.KeyManagement;
using static XFS4IoTFramework.KeyManagement.KeyDetail;
using HRESULT = System.Int32;
using LPVOID = System.IntPtr;
using LPWFSRESULT = System.IntPtr;
using WORD = System.UInt16;

namespace XFS3xPinPad
{
    public class PINDevice : XfsService
    {
        private WORD _wResetOut;

        private HRESULT _hCompleteResult;

        public PINDevice(string logicalName) : base(logicalName) { }

        public bool Init()
        {
            Logger.Info($"Init XFS Device 3.x service");
            try
            {
                //WFS_EXECUTE_COMPLETE += HandleCompleteEvent;
                //WFS_EXECUTE_EVENT += HandleExecuteEvent;
                //WFS_SERVICE_EVENT += HandleServiceEvent;
                Connect();
                return true;
            }
            catch (Exception e)
            {
                Logger.Error($"Init XFS Device 3.x service ERROR: [{e.Message}]");
                return false;
            }
        }

        protected override void HandleServiceEvent(ref WFSRESULT xfsResult)
        {
            throw new NotImplementedException();
        }

        protected override void HandleExecuteEvent(ref WFSRESULT xfsResult)
        {
            throw new NotImplementedException();
        }

        protected override void HandleCompleteEvent(ref WFSRESULT xfsResult)
        {
            throw new NotImplementedException();
        }

        public Dictionary<string, KeyDetail> GetKeyDetail()
        {
            LPWFSRESULT lpResult = LPWFSRESULT.Zero;
            try
            {
                GetInfo(CMD.WFS_INF_PIN_KEY_DETAIL, LPVOID.Zero, ref lpResult);
                if (lpResult != LPWFSRESULT.Zero)
                {
                    WFSRESULT wfsResult = Marshal.PtrToStructure<WFSRESULT>(lpResult);
                    int ptrSize = Marshal.SizeOf(typeof(IntPtr));
                    int idx = 0;
                    var keyDetailList = new Dictionary<string, KeyDetail>();
                    if (wfsResult.lpBuffer != LPVOID.Zero)
                    {

                        for (; ; )
                        {
                            IntPtr intPtr = Marshal.ReadIntPtr(wfsResult.lpBuffer, idx * ptrSize);
                            if (intPtr == IntPtr.Zero)
                            {
                                break;
                            }
                            else
                            {
                                WFSPINKEYDETAIL wFSIDCCARDDATA = Marshal.PtrToStructure<WFSPINKEYDETAIL>(intPtr);
                                var key = new KeyDetail(
                                             // string keyName
                                             wFSIDCCARDDATA.lpsKeyName,
                                             //int KeySlot,
                                             0,
                                             //string KeyUsage,
                                             FwUse.ToKeyUsage(wFSIDCCARDDATA.fwUse),
                                             //string Algorithm,
                                             "T",
                                             //string ModeOfUse,
                                             "B",
                                             //int KeyLength,
                                             32,
                                             //KeyStatusEnum KeyStatus,
                                             KeyStatusEnum.Loaded,
                                             //bool Preloaded,
                                             false,
                                             //string RestrictedKeyUsage,
                                             "C0",
                                             //string KeyVersionNumber,
                                             "V1",
                                             //string Exportability,
                                             "N",
                                             //List<byte> OptionalKeyBlockHeader,
                                             new List<byte>());
                                //ReadData.Add(WDataSource.ToEnum(wFSIDCCARDDATA.wDataSource), new ReadCardResult.CardData(wFSIDCCARDDATA.DataStatus, wFSIDCCARDDATA.DataAsList()));
                                Console.WriteLine($"Key: [{wFSIDCCARDDATA.lpsKeyName}]");
                                keyDetailList.Add(wFSIDCCARDDATA.lpsKeyName, key);
                            }
                            idx++;
                        }
                    }
                    return keyDetailList;

                }
                else
                {
                    throw new NullResultException();
                }
            }
            catch { throw; }
            finally
            {
                XfsService.FreeWFSResult(lpResult);
            }
        }

        public readonly Dictionary<EntryModeEnum, List<FrameClass>> KeyLayouts = new()
        {
            {
                EntryModeEnum.Data,
                new List<FrameClass>()
                {
                     {
                        new FrameClass(0, 0, 0, 0, FrameClass.FloatEnum.NotSupported,
                        new List<FrameClass.FunctionKeyClass>()
                        {
                            { new FrameClass.FunctionKeyClass(282, 204, 80, 80, "one", null) },
                            { new FrameClass.FunctionKeyClass(282, 294, 80, 80, "four", null) },
                            { new FrameClass.FunctionKeyClass(282, 384, 80, 80, "seven", null) },
                            { new FrameClass.FunctionKeyClass(372, 204, 80, 80, "two", null) },
                            { new FrameClass.FunctionKeyClass(372, 294, 80, 80, "five", null) },
                            { new FrameClass.FunctionKeyClass(372, 384, 80, 80, "eight", null) },
                            { new FrameClass.FunctionKeyClass(372, 474, 80, 80, "zero", null) },
                            { new FrameClass.FunctionKeyClass(462, 204, 80, 80, "three", null) },
                            { new FrameClass.FunctionKeyClass(462, 294, 80, 80, "six", null) },
                            { new FrameClass.FunctionKeyClass(462, 384, 80, 80, "nine", null) },
                            { new FrameClass.FunctionKeyClass(572, 209, 160, 80, "enter", null) },
                            { new FrameClass.FunctionKeyClass(572, 299, 160, 80, "clear", null) },
                            { new FrameClass.FunctionKeyClass(572, 389, 160, 80, "cancel", null) },
                            { new FrameClass.FunctionKeyClass(572, 479, 160, 80,  "backspace", null) }
                        })
                    },
                    {
                        new FrameClass(0, 0, 0, 768, FrameClass.FloatEnum.NotSupported,
                        new List<FrameClass.FunctionKeyClass>()
                        {
                            { new FrameClass.FunctionKeyClass(0, 0, 0, 187, "fdk01", null) },
                            { new FrameClass.FunctionKeyClass(0, 187, 0, 187, "fdk02", null) },
                            { new FrameClass.FunctionKeyClass(0, 374, 0, 187, "fdk03", null) },
                            { new FrameClass.FunctionKeyClass(0, 561, 0, 187, "fdk04", null) }
                        })
                    },
                    {
                        new FrameClass(1024, 0, 0, 768, FrameClass.FloatEnum.NotSupported,
                        new List<FrameClass.FunctionKeyClass>()
                        {
                            { new FrameClass.FunctionKeyClass(0, 0, 0, 187, "fdk05", null) },
                            { new FrameClass.FunctionKeyClass(0, 187, 0, 187, "fdk06", null) },
                            { new FrameClass.FunctionKeyClass(0, 374, 0, 187, "fdk07", null) },
                            { new FrameClass.FunctionKeyClass(0, 561, 0, 187, "fdk08", null) },
                        })
                    }
                }
            },
            {
                EntryModeEnum.Pin,
                new List<FrameClass>()
                {
                    {
                        new FrameClass(0, 0, 0, 0, FrameClass.FloatEnum.NotSupported,
                        new List<FrameClass.FunctionKeyClass>()
                        {
                            { new FrameClass.FunctionKeyClass(287, 209, 80, 80, "one", null) },
                            { new FrameClass.FunctionKeyClass(287, 299, 80, 80, "four", null) },
                            { new FrameClass.FunctionKeyClass(287, 389, 80, 80, "seven", null) },
                            { new FrameClass.FunctionKeyClass(377, 209, 80, 80, "two", null) },
                            { new FrameClass.FunctionKeyClass(377, 299, 80, 80, "five", null) },
                            { new FrameClass.FunctionKeyClass(377, 389, 80, 80, "eight", null) },
                            { new FrameClass.FunctionKeyClass(377, 479, 80, 80, "zero", null) },
                            { new FrameClass.FunctionKeyClass(467, 209, 80, 80, "three", null) },
                            { new FrameClass.FunctionKeyClass(467, 299, 80, 80, "six", null) },
                            { new FrameClass.FunctionKeyClass(467, 389, 80, 80, "nine", null) },
                            { new FrameClass.FunctionKeyClass(577, 209, 160, 80, "enter", null) },
                            { new FrameClass.FunctionKeyClass(577, 299, 160, 80, "clear", null) },
                            { new FrameClass.FunctionKeyClass(577, 389, 160, 80, "cancel", null) },
                            { new FrameClass.FunctionKeyClass(577, 479, 160, 80, "backspace", null) }
                        })
                    }
                }
            },
            {
                EntryModeEnum.Secure,
                new List<FrameClass>()
                {
                    {
                        new FrameClass(0, 0, 0, 0, FrameClass.FloatEnum.NotSupported,
                        new List<FrameClass.FunctionKeyClass>()
                        {
                            { new FrameClass.FunctionKeyClass(287, 209, 80, 80, "one", "a") },
                            { new FrameClass.FunctionKeyClass(287, 299, 80, 80, "four", "d") },
                            { new FrameClass.FunctionKeyClass(287, 389, 80, 80, "seven", null) },
                            { new FrameClass.FunctionKeyClass(287, 479, 80, 80, "shift", null) },
                            { new FrameClass.FunctionKeyClass(377, 209, 80, 80, "two", "b") },
                            { new FrameClass.FunctionKeyClass(377, 299, 80, 80, "five", "e") },
                            { new FrameClass.FunctionKeyClass(377, 389, 80, 80, "eight", null) },
                            { new FrameClass.FunctionKeyClass(377, 479, 80, 80, "zero", null) },
                            { new FrameClass.FunctionKeyClass(467, 209, 80, 80, "three", "c") },
                            { new FrameClass.FunctionKeyClass(467, 299, 80, 80, "six", "f") },
                            { new FrameClass.FunctionKeyClass(467, 389, 80, 80, "nine", null) },
                            { new FrameClass.FunctionKeyClass(577, 209, 160, 80, "enter", null) },
                            { new FrameClass.FunctionKeyClass(577, 299, 160, 80, "clear", null) },
                            { new FrameClass.FunctionKeyClass(577, 389, 160, 80, "cancel", null) }
                        })
                    }
                }
            }
        };
    }
}
