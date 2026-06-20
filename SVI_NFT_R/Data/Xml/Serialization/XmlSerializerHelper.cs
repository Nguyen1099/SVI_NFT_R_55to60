using System.Xml;
using System.Xml.Serialization;

namespace ENC.Data.Xml.Serialization
{
    /// <summary>
	/// XML 시리얼라이져 핼퍼 클래스 입니다. 기본 설정에대한 객체를 생성합니다.
	/// </summary>
	public static class XmlSerializerHelper
    {
        /// <summary>
        /// XML 네임스페이스가 없는 네임스페이스 객체를 생성한다.
        /// </summary>
        public static XmlSerializerNamespaces EmptyNamespaces
        {
            get
            {
                return getDefaultNamespaces();
            }
        }
        /// <summary>
        /// 기본 Indent 설정 객체를 생성한다. (탭문자로 정렬)
        /// </summary>
        public static XmlWriterSettings IndentedSettings
        {
            get
            {
                return getIndentedSettings();
            }
        }
        /// <summary>
        /// XML 쓰기 속성 객체를 생성한다.
        /// </summary>
        public static XmlWriterSettings NoXmlDeclarationSettings
        {
            get
            {
                return getNoXmlDeclarationSettings();
            }
        }
        /// <summary>
        /// XML 읽기 속성 객체를 생성한다.
        /// </summary>
        public static XmlReaderSettings XmlFragmentSettings
        {
            get
            {
                return getReaderSettings();
            }
        }

        private static XmlSerializerNamespaces getDefaultNamespaces()
        {
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("", ""); // this removes the namespaces
            return ns;
        }
        private static XmlWriterSettings getIndentedSettings()
        {
            XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
            xmlWriterSettings.Indent = true;
            xmlWriterSettings.IndentChars = "\t";

            return xmlWriterSettings;
        }
        private static XmlReaderSettings getReaderSettings()
        {
            XmlReaderSettings settings = new XmlReaderSettings();
            //settings.CheckCharacters = true;
            //settings.CloseInput = true;
            settings.ConformanceLevel = ConformanceLevel.Fragment;
            //settings.IgnoreComments = true;
            //settings.IgnoreProcessingInstructions = true;
            //settings.IgnoreWhitespace = true;
            //settings.Schemas = new System.Xml.Schema.XmlSchemaSet();
            //settings.

            return settings;
        }
        private static XmlWriterSettings getNoXmlDeclarationSettings()
        {
            XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
            //xmlWriterSettings.CheckCharacters = true;
            //xmlWriterSettings.CloseOutput = true;
            //xmlWriterSettings.ConformanceLevel = ConformanceLevel.Auto;
            //xmlWriterSettings.Encoding = Encoding.UTF8;
            //xmlWriterSettings.Indent = true;
            //xmlWriterSettings.NewLineChars = "\n";
            //xmlWriterSettings.NewLineHandling = NewLineHandling.None;
            //xmlWriterSettings.NewLineOnAttributes = false;
            //xmlWriterSettings.OmitXmlDeclaration = false;
            //xmlWriterSettings.OutputMethod = XmlOutputMethod.AutoDetect;

            xmlWriterSettings.OmitXmlDeclaration = true;

            return xmlWriterSettings;
        }
    }
}
