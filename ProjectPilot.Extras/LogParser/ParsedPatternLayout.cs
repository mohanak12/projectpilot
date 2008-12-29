using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Reflection;
using System.Text.RegularExpressions;
using log4net;

namespace ProjectPilot.Extras.LogParser
{
    public class ParsedPatternLayout
    {
        public ParsedPatternLayout()
        {
            conversionPatternsMap.Add("a", typeof(AppDomainPatternLayoutElement).FullName);
            conversionPatternsMap.Add("appdomain", typeof(AppDomainPatternLayoutElement).FullName);
            conversionPatternsMap.Add("logger", typeof(LoggerPatternLayoutElement).FullName);
            conversionPatternsMap.Add("c", typeof(LoggerPatternLayoutElement).FullName);
            conversionPatternsMap.Add("C", typeof(TypePatternLayoutElement).FullName);
            conversionPatternsMap.Add("class", typeof(TypePatternLayoutElement).FullName);
            conversionPatternsMap.Add("type", typeof(TypePatternLayoutElement).FullName);
            conversionPatternsMap.Add("d", typeof(DatePatternLayoutElement).FullName);
            conversionPatternsMap.Add("exception", typeof(ExceptionPatternLayoutElement).FullName);
            conversionPatternsMap.Add("F", typeof(FilePatternLayoutElement).FullName);
            conversionPatternsMap.Add("file", typeof(FilePatternLayoutElement).FullName);
            conversionPatternsMap.Add("identity", typeof(IdentityPatternLayoutElement).FullName);
            conversionPatternsMap.Add("u", typeof(IdentityPatternLayoutElement).FullName);
            conversionPatternsMap.Add("l", typeof(LocationPatternLayoutElement).FullName);
            conversionPatternsMap.Add("location", typeof(LocationPatternLayoutElement).FullName);
            conversionPatternsMap.Add("L", typeof(LinePatternLayoutElement).FullName);
            conversionPatternsMap.Add("line", typeof(LinePatternLayoutElement).FullName);
            conversionPatternsMap.Add("message", typeof(MessagePatternLayoutElement).FullName);
            conversionPatternsMap.Add("mdc", typeof(MdcPatternLayoutElement).FullName);
            conversionPatternsMap.Add("X", typeof(MdcPatternLayoutElement).FullName);
            conversionPatternsMap.Add("method", typeof(MethodPatternLayoutElement).FullName);
            conversionPatternsMap.Add("n", typeof(NewLinePatternLayoutElement).FullName);
            conversionPatternsMap.Add("newline", typeof(NewLinePatternLayoutElement).FullName);
            conversionPatternsMap.Add("ndc", typeof(NdcPatternLayoutElement).FullName);
            conversionPatternsMap.Add("x", typeof(NdcPatternLayoutElement).FullName);
            conversionPatternsMap.Add("level", typeof(LevelPatternLayoutElement).FullName);
            conversionPatternsMap.Add("p", typeof(LevelPatternLayoutElement).FullName);
            conversionPatternsMap.Add("P", typeof(PropertyPatternLayoutElement).FullName);
            conversionPatternsMap.Add("properties", typeof(PropertyPatternLayoutElement).FullName);
            conversionPatternsMap.Add("property", typeof(PropertyPatternLayoutElement).FullName);
            conversionPatternsMap.Add("r", typeof(TimestampPatternLayoutElement).FullName);
            conversionPatternsMap.Add("timestamp", typeof(TimestampPatternLayoutElement).FullName);
            conversionPatternsMap.Add("t", typeof(ThreadPatternLayoutElement).FullName);
            conversionPatternsMap.Add("thread", typeof(ThreadPatternLayoutElement).FullName);
            conversionPatternsMap.Add("username", typeof(UsernamePatternLayoutElement).FullName);
            conversionPatternsMap.Add("w", typeof(UsernamePatternLayoutElement).FullName);
            conversionPatternsMap.Add("utcdate", typeof(UtcDatePatternLayoutElement).FullName);
        }

        public IList<ILog4NetPatternLayoutElement> Elements
        {
            get { return elements; }
        }

        [SuppressMessage("Microsoft.Performance", "CA1820:TestForEmptyStringsUsingStringLength")]
        public void ParsePattern(string line)
        {
            //Regex regex= new Regex(@"%-?[0-9]*\.?[0-9]*(?<pattern>[a-zA-Z]+)");
            Regex regex = new Regex(
                @"(?<literal>[^%]*)%((?<min>-?[0-9]*)(\.(?<max>[0-9]*)))?(?<patternname>[a-zA-Z]+)(?<literal2>[^%]*)",
                RegexOptions.Compiled | RegexOptions.ExplicitCapture);

            Match match = regex.Match(line);

            while (match.Success)
            {
                string literal = match.Groups["literal"].Value;

                if (false == String.IsNullOrEmpty(literal))
                    elements.Add(new LiteralPatternLayoutElement(literal));

                if (match.Groups["patternname"].Success)
                {
                    string patternName = match.Groups["patternname"].Value;
                    int? minLength = null;
                    int? maxLength = null;

                    if (match.Groups["min"].Success)
                        minLength = int.Parse(match.Groups["min"].Value, CultureInfo.InvariantCulture);

                    if (match.Groups["max"].Success)
                        maxLength = int.Parse(match.Groups["max"].Value, CultureInfo.InvariantCulture);

                    ConversionPatternBase conversionPattern = FindLayoutElement(patternName);
                    conversionPattern.MinLength = minLength;
                    conversionPattern.MaxLength = maxLength;
                    elements.Add(conversionPattern);

                    //log.DebugFormat("{0} {1} {2} {3}", patternName, minLength, maxLength, literal);
                }

                string literal2 = match.Groups["literal2"].Value;

                if (false == String.IsNullOrEmpty(literal2))
                    elements.Add(new LiteralPatternLayoutElement(literal2));

                match = match.NextMatch();
            }
          
            //foreach (string patternInLine in parseLine)
            //{
            //    string[] parseElement = patternInLine.Split('%');
            //    foreach (string patternInElement in parseElement)
            //    {
            //        if (patternInElement == string.Empty)
            //            continue;

            //        string pattern = patternInElement;
            //        pattern = "%" + pattern;

            //        //PatternElement
            //        if (regex.IsMatch(pattern))
            //        {
            //            Match match = regex.Match(pattern);
            //            tempLayout.Add(match.Value);
            //            pattern = pattern.Remove(0, match.Length);
            //        }
            //        else
            //        {
            //            pattern = pattern.Remove(0, 1);
            //        }

            //        //Literal
            //        if (pattern.Length > 0)
            //        {
            //            tempLayout.Add(pattern); 
            //        }
            //    }
            //}
        }

        private ConversionPatternBase FindLayoutElement (string patternName)
        {
            if (false == conversionPatternsMap.ContainsKey(patternName))
                throw new KeyNotFoundException(
                    String.Format(
                        CultureInfo.InvariantCulture,
                        "The pattern '{0}' is not supported.",
                        patternName));

            string className = conversionPatternsMap[patternName];

            ConversionPatternBase patternLayoutElement =
                (ConversionPatternBase)Assembly.GetExecutingAssembly().CreateInstance(className);

            patternLayoutElement.PatternName = patternName;

            return patternLayoutElement;
        }

        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        private static readonly ILog log = LogManager.GetLogger(typeof(ParsedPatternLayout));
        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        private List<ILog4NetPatternLayoutElement> elements = new List<ILog4NetPatternLayoutElement>();
        private Dictionary<string, string> conversionPatternsMap = new Dictionary<string, string>();
    }
}