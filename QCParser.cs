using System.IO;
using System.Net.Mail;
using System.Windows.Controls;

namespace qc_reader;

public struct BodyGroups
{
    public string name;
    public List<string> models;
}

public struct Attachments
{
    public string AttachmentName;
    public string BoneName;
    public string Pos;
    public string Rot;
}

public struct IncludedModel
{
    public string model;
}

public struct MaterialsPath
{
    public string path;
}

public struct QCInfo
{
    public string modelName;
    public List<BodyGroups> bodyGroups;
    public List<Attachments> attachments;
    public List<IncludedModel> includedModels;
    public List<MaterialsPath> materialsPaths;
}

public class QCParser
{
    public QCParser(string fileName)
    {
        if (File.Exists(fileName))
        {
            sr = new StreamReader(fileName);
        }
        else
        {
            throw new FileNotFoundException(fileName, $"Can't find file.{fileName}");
        }
    }

    private string ReadAndTrim()
    {
        return sr.ReadLine().Trim();
    }
    public QCInfo Parse()
    {
        var info = new QCInfo()
        {
            bodyGroups = [],
            attachments = [],
            includedModels = [],
            materialsPaths = [],
        };
        
        string line;
        while ((line = sr.ReadLine()) != null)
        {
            if (line.StartsWith("$modelname"))
            {
                info.modelName = line.Split('"')[1];
            }
            else if (line.StartsWith("$bodygroup"))
            {
                var bodyGroups = new BodyGroups()
                {
                    name = line.Split('"')[1],
                    models = []
                };
                while ((line = ReadAndTrim()) != "}")
                {
                    if (line.StartsWith("studio"))
                    {
                        bodyGroups.models.Add(line.Split('"')[1]);
                    }
                    else if (line == "blank")
                    {
                        bodyGroups.models.Add("Blank");
                    }
                }
                info.bodyGroups.Add(bodyGroups);
            }
            else if (line.StartsWith("$attachment"))
            {
                var attachments = new Attachments()
                {
                    AttachmentName = line.Split('"')[1],
                    BoneName = line.Split('"')[3],
                    Pos = line.Split(' ')[3] + " " + line.Split(' ')[4] + " " + line.Split(' ')[5],
                    Rot = line.Split(' ')[7] + " " + line.Split(' ')[8] + " " + line.Split(' ')[9]
                };

                info.attachments.Add(attachments);
            }
            else if (line.StartsWith("$includemodel"))
            {
                var included_model = new IncludedModel()
                {
                    model = line.Split('"')[1]
                }; 

                info.includedModels.Add(included_model);
            }
            else if (line.StartsWith("$cdmaterials"))
            {
                var material_path = new MaterialsPath();
                material_path.path = line.Split('"')[1];

                info.materialsPaths.Add(material_path);
            }
        }
        return info;
    }
    private readonly StreamReader sr;
}
