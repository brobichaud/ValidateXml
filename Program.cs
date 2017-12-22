using System;
using System.Xml;
using System.Xml.Schema;

namespace ValidateXml
{
	class Program
	{
		static void Main(string[] args)
		{
			if (args.Length < 2)
			{
				Console.WriteLine();
				Console.WriteLine("Argument error.");
				Console.WriteLine(" Usage: ValidateXml xmlFile xsdFile showall");
				Console.WriteLine("   where showall is either true or false, true shows all errors, false just the first");
				Console.WriteLine();
				return;
			}

			string xmlFile = args[0];
			string xsdFile = args[1];
			string targetNamespace = null;
			bool enumerateAllErrors = true;

			try
			{
				XmlReaderSettings settings = new XmlReaderSettings();
				settings.ValidationType = ValidationType.Schema;
				settings.Schemas.Add(targetNamespace, xsdFile);

				if (enumerateAllErrors)
					settings.ValidationEventHandler += new System.Xml.Schema.ValidationEventHandler(settings_ValidationEventHandler);

				using (XmlReader reader = XmlReader.Create(xmlFile, settings))
				{
					while (reader.Read());
				}

				Console.WriteLine();
				Console.WriteLine("**Validate complete");
			}
			catch (XmlSchemaValidationException e)
			{
				WriteError(e.LineNumber, e.LinePosition, e.Message);
			}
			catch (XmlSchemaException e)
			{
				WriteError(e.LineNumber, e.LinePosition, e.Message);
			}
			catch (XmlException e)
			{
				WriteError(e.LineNumber, e.LinePosition, e.Message);
			}
		}

		static void settings_ValidationEventHandler(object sender, System.Xml.Schema.ValidationEventArgs e)
		{
			WriteError(e.Exception.LineNumber, e.Exception.LinePosition, e.Exception.Message);
		}

		static private void WriteError(int line, int pos, string message)
		{
			Console.WriteLine(string.Format("Line: {0}, Position: {1}, Error: {2}", line, pos, message));
		}
	}
}
