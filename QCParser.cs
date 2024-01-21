using System.IO;
using System.Net.Mail;

namespace qc_reader;

public struct BodyGroups
{
    public string name;
    public List<string> models;
}
public struct Model
{
    public struct Flex
    {
        public string name;
        public int frame;
    }
    public struct FlexFile
    {
        public string name;
        public List<Flex> flexes;
    }
    public struct mvar
    {
        public string name;
        public string value;
    }
    public string name;
    public string model;
    public FlexFile flexFile;
    //public List<FlexController> flexControllers;
    public List<mvar> vars;
}

public struct FlexController
{
    public string unk0;
    public string unk1;
    public int unk2;
    public int unk3;
    public string name;
}

public struct Bone {

}

public struct Attachments
{
    public string AttachmentName;
    public string BoneName;
}

public struct IncludedModel
{
    public string model;
}

public struct QCInfo
{
    public string modelName;
    public List<BodyGroups> bodyGroups;
    public List<Model> models;
    public List<Bone> bones;
    public List<Attachments> attachments;
    public List<IncludedModel> includedModels;
}
public class QCParser
{
    public QCParser(string fileName)
    {
        sr = new StreamReader(fileName);
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
            models = [],
            attachments = [],
            includedModels = [],
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
                    BoneName = line.Split('"')[3]
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
        }
        return info;
    }
    private readonly StreamReader sr;
}
