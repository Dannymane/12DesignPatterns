using System.Xml;

namespace HelloWorld
{
    public interface IStrategy{
        void Save_File(string[] lines_, string file_name_);
    }
    public class Concrete_Strategy_TXT: IStrategy{

        public void Save_File(string[] lines_, string file_name_){
            using (StreamWriter writer = new StreamWriter(file_name_+".txt"))
            {
                int i = 1;
                foreach (string line in lines_)
                {
                    writer.WriteLine(i+". "+line);
                    i++;
                }
            }
        }
    }
    public class Concrete_Strategy_XML: IStrategy{
        public void Save_File(string[] lines_, string file_name_){
            // Create a writer to write XML to the console.
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.OmitXmlDeclaration = false;
            XmlWriter writer = XmlWriter.Create(file_name_+".xml", settings);

            // Write the books element.
            writer.WriteStartElement("books");

            // Write the book elements.
            foreach (string b in lines_){
                writer.WriteStartElement("book");
                writer.WriteString(b);
                writer.WriteEndElement();
            }
            
            // Write the close tag for the root element.
            writer.WriteEndElement();
            // Write the XML and close the writer.
            writer.Close();
        }
    }
    public class Concrete_Strategy_HTML: IStrategy{
        public void Save_File(string[] lines_, string file_name_){
            // Create a writer to write HTML to the console.
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.OmitXmlDeclaration = true;
            XmlWriter writer = XmlWriter.Create(file_name_+".html", settings);

            // Write the html tags
            writer.WriteStartElement("html");
            writer.WriteStartElement("body");
            writer.WriteStartElement("ul");
            // Write the book elements.
            foreach (string b in lines_){
                writer.WriteStartElement("li");
                writer.WriteString(b);
                writer.WriteEndElement();
            }
            
            // Write the close tags 
            writer.WriteEndElement();
            writer.WriteEndElement();
            writer.WriteEndElement();
            // Write the HTML and close the writer.
            writer.Close();
        }
    }
    public class Context{

        private IStrategy istrategy;
        public string[] lines;
        public string new_file_name;
        public Context(string file_name, string new_file_name_){
            lines = File.ReadAllLines(file_name);
            istrategy = new Concrete_Strategy_TXT();
            new_file_name = new_file_name_;
        }
        public void set_strategy(IStrategy istrategy_){
            istrategy = istrategy_;
        }
        public void execute(){
            istrategy.Save_File(lines, new_file_name);
        }

    }


    class Program
    {
        static void Main(string[] args)
        {            
            Context c1 = new Context("books.txt", "books_new");
            IStrategy csTXT = new Concrete_Strategy_TXT();
            c1.set_strategy(csTXT);
            c1.execute();
            
            IStrategy csXML = new Concrete_Strategy_XML();
            c1.set_strategy(csXML);
            c1.execute();

            IStrategy csHTML = new Concrete_Strategy_HTML();
            c1.set_strategy(csHTML);
            c1.execute();

        }
    }
}