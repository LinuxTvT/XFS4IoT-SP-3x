using XFS4IoTFramework.Printer;
using XFS4IoTServer;
namespace Printer.XFS3xPrinter
{
    public partial class XFS3xPrinter : IPersistentData
    {
        private static Type _typeFromValue = typeof(Dictionary<string, Form>);
        private static Type _typeMediaValue = typeof(Dictionary<string, Media>);
        public TValue? Load<TValue>(string name) where TValue : class
        {
            Console.WriteLine($"Load PTRDevice name[{name}], type[{typeof(TValue)}][{typeof(TValue)}]");

            if (typeof(TValue) == _typeFromValue)
            {
                Dictionary<string, Form> forms = new();
                List<string> listFormName = GetFormNameList();
                foreach (string formName in listFormName)
                {
                    var form = QueryForm(formName, this);
                    forms.Add(formName, form);
                    QueryField(ref form);
                }
                return forms as TValue;
            }
            else if (typeof(TValue) == _typeMediaValue)
            {
                Dictionary<string, Media> medias = new();
                List<string> listMediaName = GetMediaNameList();
                foreach (string mediaName in listMediaName)
                {
                    Media media = QueryMedia(mediaName, this);
                    medias.Add(mediaName, media);
                }
                return medias as TValue;
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public bool Store<TValue>(string name, TValue obj) where TValue : class
        {
            throw new NotImplementedException();
        }
    }
}