/*
using System.Text.RegularExpressions;

namespace Main;

public enum TokenType
{
    Directive,
    String,
    Number,
    EndBracket,
    Other
}
public enum DirectiveTokenType
{
    ModelName,
    Bodygroup,
    Model,
    StudioFile,
    BlankStudioFile
}
public class Token(TokenType _type, string value)
{
    public TokenType Type { get; } = _type;
    public string Values { get; } = value;
}

partial class Parser2
{
    private static readonly Dictionary<Regex, TokenType> patterns = new()
    {
        { new Regex(@"^\$(\w+)"), TokenType.Directive },
        { new Regex(@"""([^\""""]+)\"""), TokenType.String },
        { new Regex(@"\b(\d+)\b"), TokenType.Number },
        { new Regex(@"\}"), TokenType.EndBracket }
    };

    public static List<Token> Parse(string path)
    {
        var sr = new StreamReader(path);
        var tokens = new List<Token>();

        string line;
        var nline = 1;
        while ((line = sr.ReadLine()) != null)
        {
            if (line.Trim().StartsWith("//"))
            {
                nline++;
                continue;
            }
            List<string> currentLineTokens = new List<string>();

            foreach (var pattern in patterns)
            {
                var matches = pattern.Key.Matches(line);

                foreach (Match match in matches)
                {
                    currentLineTokens.Add(match.Groups[1].Value);
                }
            }

            if (currentLineTokens.Count == 0)
            {
                tokens.Add(new Token(TokenType.Other, line));
            }
            else
            {
                var tokenValue = string.Join(" ", currentLineTokens);
                tokens.Add(new Token(TokenType.Other, tokenValue));
            }

            nline++;
        }

        return tokens;
    }
}
*/