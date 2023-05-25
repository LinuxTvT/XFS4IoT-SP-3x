using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XFS4IoTFramework.Common;
using XFS4IoTFramework.Crypto;
using XFS4IoTFramework.Keyboard;
using XFS4IoTFramework.KeyManagement;
using XFS4IoTFramework.PinPad;
using XFS4IoTServer;

namespace XFS3xPinPad
{
    public partial class XFS3xPinPadDevice : IPinPadDevice, IKeyboardDevice, ICryptoDevice, IKeyManagementDevice, ICommonDevice, IPersistentData  
    {
        private readonly string _keyDetailTypeName;

        TValue? IPersistentData.Load<TValue>(string name) where TValue : class
        {

            Type typeParameterType = typeof(TValue);
            if (name == _keyDetailTypeName)
            {
                var rets = GetKeyDetail();
                return rets as TValue;
            }
            else
            {
                Console.WriteLine($"<== Load value, name = [{name}][{typeParameterType}]");
                throw new NotImplementedException();
            }
        }

        bool IPersistentData.Store<TValue>(string name, TValue obj)
        {
            throw new NotImplementedException();
        }
    }
}
