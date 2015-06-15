using iTextSharp.text.pdf.parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SAC.Services.Import
{
    public class SACTextExtractionStrategy : ITextExtractionStrategy
    {
        private Vector lastStart;
        private Vector lastEnd;
        private List<string> _teams = new List<string>();
        private bool _initialData;

        //Store each line individually. A SortedDictionary will automatically shuffle things around based on the key
        private SortedDictionary<int, StringBuilder> results = new SortedDictionary<int, StringBuilder>();

        private int pages;

        //Constructor and some methods that aren't used
        public SACTextExtractionStrategy(int pages, List<string> teams, bool initialData)
        {
            this.pages = pages;
            _teams.AddRange(teams);
            _initialData = initialData;
        }
        public virtual void BeginTextBlock() { }
        public virtual void EndTextBlock() { }
        public virtual void RenderImage(ImageRenderInfo renderInfo) { }

        //Convert our lines into a giant block of text
        public virtual String GetResultantText() {
            StringBuilder buf = new StringBuilder();
            bool inRank = false;

            if (_initialData)
            {
                foreach (var s in results)
                {
                    List<string> arr = s.Value.ToString().Split(new string[] { "\t" }, StringSplitOptions.RemoveEmptyEntries).ToList();
                    if (arr.Count != 5)
                    {
                        if (arr.Count < 3)
                            continue;
                        int bibNumber = -1;
                        string firstName = "";
                        string lastName = "";
                        string team = "";

                        if (arr.Count == 3)
                        {
                            string[] splited = arr[1].Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                            if (splited.Length < 3)
                                continue;
                            arr.Insert(1, splited[0]);
                            arr.Insert(2, splited[1]);
                            arr[3] = arr[3].Replace(splited[0], "").Replace(splited[1], "").TrimStart();
                        }
 
                        if (!int.TryParse(arr[0], out bibNumber))
                        {
                            string[] splited = arr[0].Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                            if (splited.Length < 2)
                                continue;
                            int.TryParse(splited[0], out bibNumber);
                            firstName = splited[1];
                            arr.RemoveAt(0);
                            arr.Insert(0, bibNumber.ToString());
                            arr.Insert(1, firstName);
                        }
                        if (arr.Count < 5)
                        {
                            string[] splited = arr[1].Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                            if (splited.Length > 1)
                            {
                                firstName = splited[0];
                                arr.Insert(1, firstName);
                                arr[2] = arr[2].Replace(firstName, "").TrimStart();
                            }
                        }
                        if (arr.Count < 5)
                        {
                            string[] splited = arr[2].Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                            if (splited.Length > 1)
                            {
                                lastName = splited[0];
                                arr.Insert(2, lastName);
                                arr[3] = arr[3].Replace(lastName, "").TrimStart();
                            }
                        }
                    }
                    if (arr.Count != 5)
                        continue;
                    buf.AppendLine(s.Value.ToString());
                }  
            }
            else
            {
                foreach (var s in results)
                {
                    string newSplit = string.Empty;
                    if (inRank)
                    {
                        List<string> arr = s.Value.ToString().Split(new string[] { "\t" }, StringSplitOptions.RemoveEmptyEntries).ToList();
                        if (arr.Count != 5 && arr.Count > 2)
                        {
                            var result = Regex.Match(arr[arr.Count - 1], @"\d+$").Value;
                            if (!string.IsNullOrWhiteSpace(result) && result != arr[arr.Count - 1])
                            {
                                arr[arr.Count - 1] = arr[arr.Count - 1].Replace(result, "");
                                arr.Add(result);
                            }
                            if (arr.Count == 4)
                            {
                                foreach (string t in _teams)
                                {
                                    if (arr[2].Contains(t))
                                    {
                                        arr[2] = arr[2].Replace(t, "");
                                        string points = arr[3];
                                        arr.RemoveAt(3);
                                        arr.Add(t);
                                        arr.Add(points);
                                        break;
                                    }
                                }
                            }
                            foreach (var str in arr)
                                newSplit += str + "\t";
                        }
                        if (arr.Count == 5)
                        {
                            int test;
                            if (!int.TryParse(arr[1].Trim(), out test))
                            {
                                var result = Regex.Match(arr[1], @"\d+$").Value;
                                if (!string.IsNullOrWhiteSpace(result) && result != arr[1])
                                {
                                    arr[1] = arr[1].Replace(result, "").TrimEnd();
                                    string arr2 = arr[2];
                                    string arr3 = arr[3];
                                    string arr4 = arr[4];
                                    arr.RemoveAt(4);
                                    arr.RemoveAt(3);
                                    arr.RemoveAt(2);
                                    arr.Add(result);
                                    arr.Add(arr2 + arr3);
                                    arr.Add(arr4);
                                }
                                newSplit = "";
                                foreach (var str in arr)
                                    newSplit += str + "\t";
                            }

                            if (!_teams.Contains(arr[3]))
                                _teams.Add(arr[3]);
                        }
                    }

                    //Append to the buffer
                    if (!string.IsNullOrWhiteSpace(newSplit))
                        buf.AppendLine(newSplit);
                    else
                        buf.AppendLine(s.Value.ToString());
                    if (s.Value.ToString().StartsWith("Class."))
                        inRank = true;
                    if (string.IsNullOrWhiteSpace(s.Value.ToString()))
                        inRank = false;
                }
            }
            
            results.Clear();
            return buf.ToString();
        }
        public virtual void RenderText(TextRenderInfo renderInfo) {
            bool firstRender = results.Count == 0;

            LineSegment segment = renderInfo.GetBaseline();
            Vector start = segment.GetStartPoint();
            Vector end = segment.GetEndPoint();

            //Use the Y value of the bottom left corner of the text for the key

            //if (!firstRender && (int)start[1] > (int)lastStart[1])
            //{
            //    results.Clear();
            //}

            int currentLineKey = (int)start[1];

            if (!firstRender) {
                Vector x0 = start;
                Vector x1 = lastStart;
                Vector x2 = lastEnd;

                float dist = (x2.Subtract(x1)).Cross((x1.Subtract(x0))).LengthSquared / x2.Subtract(x1).LengthSquared;

                float sameLineThreshold = 1f;
                //If we've detected that we're still on the same
                if (dist <= sameLineThreshold) {
                    //Use the previous Y coordinate
                    currentLineKey = (int)lastStart[1];
                }
            }
            //Hack: PDFs start with zero at the bottom so our keys will be upside down. Using negative keys cheats this.
            currentLineKey = currentLineKey * -1;

            //If this line hasn't been used before add a new line to our collection
            if (!results.ContainsKey(currentLineKey)) {
                results.Add(currentLineKey, new StringBuilder());
            }

            //Insert a space between blocks of text if it appears there should be
            if (!firstRender &&                                       //First pass never needs a leading space
                results[currentLineKey].Length !=0 &&                 //Don't append a space to the begining of a line
                !results[currentLineKey].ToString().EndsWith(" ") &&  //Don't append if the current buffer ends in a space already
                renderInfo.GetText().Length > 0 &&                    //Don't append if the new next is empty
                !renderInfo.GetText().StartsWith(" ")) {              //Don't append if the new text starts with a space
                //Calculate the distance between the two blocks
                float spacing = lastEnd.Subtract(start).Length;
                //If it "looks" like it should be a space
                if (spacing > renderInfo.GetSingleSpaceWidth() / 2f) {
                    //Add a space
                    results[currentLineKey].Append("\t");
                }
            }

            //Add the text to the line in our collection
            results[currentLineKey].Append(renderInfo.GetText());

            lastStart = start;
            lastEnd = end;
        }
    }
}
