/*
using System.Reflection;
using System.Text.RegularExpressions;

namespace qc_info_viewer.Main;

public class QCModel
{
    public string MdlModelName = "DefMdlName";
    public List<BodyGroup> BodyGroups = [];
    public List<Model> Models = [];
    public List<JiggleBone> Jigglebones = [];
}

public class BodyGroup
{
    public string Name = "DefBodyGroupName";
    public List<string> StudioFiles = [];
}

public class Model
{
    public string ModelName = "DefModelName";
    public string StudioModelName = "DefSmdName";
    public List<FlexFile> FlexFiles = [];
    public List<FlexController> FlexControllers = [];

    public class FlexFile
    {
        public string FlexFileName = "DefFlexFileName";
        public List<Flex> Flexes = [];
    }
    public struct Flex
    {
        public string FlexName;
        public int Frame;
    }
    public struct FlexController
    {
    
    }
}

public class JiggleBone
{
    public string JiggleboneName = "DefJiggleboneName";
}

public class Parser(string filePath)
{
    public QCModel Parse()
    {
        var model = new QCModel();
        string line;

        while((line = ReadAndTrim()) != null)
        {
            var tokens = line.Split(' ');

            switch (tokens[0])
            {
                case "$modelname":
                    model.MdlModelName = line.Split('"')[1];
                    break;
                case "$bodygroup":
                    model.BodyGroups.Add(ParseBodyGroup(line.Split('"')[1]));
                    break;
                case "$model":
                    var str = line.Split('"');
                    model.Models.Add(ParseModel(str[1], str[3]));
                    break;
                case "$jigglebone":
                    model.Jigglebones.Add(ParseJigglebone(tokens[1].Split('"')[1]));
                    break;
            }
        }

        return model;
    }
    private BodyGroup ParseBodyGroup(string bodyGroupName)
    {
        var bodyGroup = new BodyGroup()
        {
            Name = bodyGroupName
        };

        string line;
        while ((line = ReadAndTrim()) != "}")
        {
            var tokens = line.Split(' ');

            switch (tokens[0])
            {
                case "studio":
                    bodyGroup.StudioFiles.Add(line.Split('"')[1]);
                    break;
                case "blank":
                    bodyGroup.StudioFiles.Add("Blank");
                    break;
            }
        }
        return bodyGroup;
    }
    private Model ParseModel(string modelName, string smdName)
    {
        var model = new Model()
        {
            ModelName = modelName,
            StudioModelName = smdName
        };

        string line;
        while ((line = ReadAndTrim()) != "}")
        {
            var tokens = line.Split(' ');

            switch (tokens[0])
            {
                case "flexfile":
                    model.FlexFiles.Add(ParseFlexfile(tokens[1].Split('"')[1]));
                    break;
                case "flexcontroller":
                    model.FlexControllers.Add(new()
                    {

                    });
                    break;
            }
        }

        return model;
    }
    private Model.FlexFile ParseFlexfile(string flexfileName)
    {
        var flexfile = new Model.FlexFile()
        {
            FlexFileName = flexfileName
        };

        string line;
        while ((line = ReadAndTrim()) != "}")
        {
            var tokens = line.Split(' ');

            switch (tokens[0])
            {
                case "defaultflex":
                    flexfile.Flexes.Add(new()
                    {
                        FlexName = "Default Flex",
                        Frame = int.Parse(tokens[2])
                    });
                    break;
                case "flex":
                    flexfile.Flexes.Add(new()
                    {
                        FlexName = line.Split('"')[1],
                        Frame = int.Parse(line.Split('"')[2].Split(' ')[2])
                    });
                    break;
            }
        }

        return flexfile;
    }
    private JiggleBone ParseJigglebone(string  jiggleboneName)
    {
        var jigglebone = new JiggleBone()
        {
            JiggleboneName = jiggleboneName
        };

        return jigglebone;
    }
    private string? ReadAndTrim()
    {
        var line = sr.ReadLine();

        if(line == null)
        {
            return null;
        }
        return line.Trim();
    }

    private readonly StreamReader sr = new(filePath);
    private static Regex dquotes = new("");
}
*/