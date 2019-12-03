using CourtRooms.Extensions;
using CourtRoomsDataLayer.Entities;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Action = CourtRoomsDataLayer.Entities.Action;

namespace CourtRooms.Helpers
{
    public class Parser
    {
        HtmlDocument Document;
        public Parser()
        {
            Document = new HtmlDocument();
        }

        public Defendant GetDefendant(string html)
        {
            Document.LoadHtml(html);

            if (IsError())
                return null;

            var defendant = new Defendant { CaseNumber = GetCaseNumber() };

            FillCaseInfo(defendant);
            FillPartyInfo(defendant);

            defendant.Violations = GetViolations();
            defendant.Bonds = GetBonds();
            defendant.Costs = GetCosts();
            defendant.Actions = GetActions();
            defendant.Sentences = GetSentences();

            return defendant;
        }

        public bool IsIncorrectCaptcha(string html)
        {
            Document.LoadHtml(html);

            var nodes = Document.DocumentNode.SelectNodes(@"//p[@class=""error""]");
            if (nodes == null || nodes.Count == 0)
                return false;

            var captchaErrorNode = nodes.Where(x => x.InnerText.Contains("Code entered is incorrect"));
            return captchaErrorNode.Any();
        }

        public bool IsNoRecords(string html)
        {
            Document.LoadHtml(html);

            var nodes = Document.DocumentNode.SelectNodes(@"//p[@class=""error""]");
            if (nodes == null || nodes.Count == 0)
                return false;

            var noRecordsNode = nodes.Where(x => x.InnerText.Contains("No records found"));
            return noRecordsNode.Any();
        }

        private bool IsError()
        {
            var nodes = Document.DocumentNode.SelectNodes(@"//p[@class=""error""]");
            return nodes != null && nodes.Count > 0;
        }

        public string GetCaseNumber()
        {
            var nodes = Document.DocumentNode.SelectNodes("//h2");
            if (nodes == null || nodes.Count < 2)
                throw new Exception("Cannot find case number");

            var node = nodes[1].Descendants().Where(x => x.Name == "span").LastOrDefault();
            var parts = node.InnerText.Split(' ');
            return parts.Last()?.Clear();
        }

        private void FillCaseInfo(Defendant defendant)
        {
            var nodes = Document.DocumentNode.SelectNodes(@"//table[@class=""status""]");
            if (nodes == null || nodes.Count == 0)
                throw new Exception("Cannot find status info");

            var trs = nodes[0].Descendants().Where(x => x.Name == "tr").ToArray();

            var tds = trs[1].Descendants().Where(x => x.Name == "td").ToArray();
            defendant.CaseStatus = tds[0].InnerText?.Clear();
            defendant.CaseType = tds[1].InnerText?.Clear();
            defendant.ViolationDate = tds[2].InnerText.ToShortDate();
            defendant.FiledDate = tds[3].InnerText.ToLongDate();

            defendant.Courtroom = tds[4].InnerText?.Clear();

            defendant.PayAmount = trs[2].NthChild("td", 1).InnerText.ToDoubleMoney();
            defendant.Location = trs[3].NthChild("td", 1).InnerText?.Clear();

            tds = trs[4].Descendants().Where(x => x.Name == "td").ToArray();
            defendant.AbNumber = tds[1].InnerText.ToInt();
            defendant.GoNumber = tds[3].InnerText?.Clear();
        }

        private void FillPartyInfo(Defendant defendant)
        {
            var nodes = Document.DocumentNode.SelectNodes(@"//table[@class=""party""]");
            if (nodes == null || nodes.Count == 0)
                throw new Exception("Cannot find party info");

            var trs = nodes[0].Descendants().Where(x => x.Name == "tr").ToArray();

            var tds = trs[1].Descendants().Where(x => x.Name == "td").ToArray();
            defendant.LastName = tds[1].InnerText?.Clear();
            defendant.FirstName = tds[2].InnerText;
            defendant.Mi = tds[3].InnerText?.Clear();
            defendant.Suffix = tds[4].InnerText?.Clear();
            defendant.DateOfBirth = tds[5].InnerText.ToShortDate();
            defendant.PartyStatus = tds[6].InnerText?.Clear();

            tds = trs[3].Descendants().Where(x => x.Name == "td").ToArray();
            defendant.Race = tds[0].InnerText?.Clear();
            defendant.Hair = tds[1].InnerText?.Clear();
            defendant.Weight = tds[2].InnerText.ToInt();
            defendant.Height = tds[3].InnerText.ToInt();
            defendant.EyeColor = tds[4].InnerText?.Clear();
            defendant.EyeGlasses = tds[5].InnerText?.Clear();

            for (var i = 5; i < trs.Length; i++)
            {
                tds = trs[i].Descendants().Where(x => x.Name == "td").ToArray();

                if (string.IsNullOrEmpty(tds[1].InnerText) && string.IsNullOrEmpty(tds[2].InnerText))
                    continue;

                defendant.Attorneys.Add(new Attorney
                {
                    Number = tds[1].InnerText.ToInt(),
                    Name = tds[2].InnerText?.Clear()
                });
            }
        }

        private List<Violation> GetViolations()
        {
            var nodes = Document.DocumentNode.SelectNodes(@"//table[@class=""violations""]");
            if (nodes == null || nodes.Count == 0)
                return null;

            var result = new List<Violation>();

            var trs = nodes[0].Descendants().Where(x => x.Name == "tr" && !x.HasClass("title")).ToArray();
            foreach (var tr in trs)
            {
                var tds = tr.ChildNodes.Where(x => x.Name == "td").ToArray();
                result.Add(new Violation
                {
                    Code = tds[0].InnerText?.Clear(),
                    Description = tds[1].InnerText?.Clear(),
                    Points = tds[2].InnerText.ToInt(),
                    Disposition = tds[3].InnerText?.Clear(),
                    ClassCode = tds[4].InnerText?.Clear(),
                });
            }

            return result;
        }

        private List<Bond> GetBonds()
        {
            var nodes = Document.DocumentNode.SelectNodes(@"//table[@class=""bonds""]");
            if (nodes == null || nodes.Count == 0)
                return null;

            return nodes.Select(GetBond).ToList();
        }

        private Bond GetBond(HtmlNode bondNode)
        {
            var bond = new Bond();

            var trs = bondNode.Descendants().Where(x => x.Name == "tr" && !x.HasClass("title")).ToArray();

            var tds = trs[0].ChildNodes.Where(x => x.Name == "td").ToArray();
            bond.Type = tds[0].InnerText.TrimBeforeColon();
            bond.BondNumber = tds[1].InnerText.TrimBeforeColon().ToInt();

            tds = trs[1].ChildNodes.Where(x => x.Name == "td").ToArray();
            bond.SuretyName = tds[0].InnerText.TrimBeforeColon();
            bond.ArrestNumber = tds[1].InnerText.TrimBeforeColon();

            tds = trs[2].ChildNodes.Where(x => x.Name == "td").ToArray();
            bond.PowerNumber = tds[0].InnerText.TrimBeforeColon();
            bond.Insurance = tds[1].InnerText.TrimBeforeColon();

            bond.BondDetails = GetBondDetails(bondNode);

            return bond;
        }

        private List<BondDetail> GetBondDetails(HtmlNode bondNode)
        {
            var nodes = bondNode.Descendants().Where(x => x.Name == "table" && x.HasClass("bond_detail")).ToArray();
            if (nodes == null || nodes.Length == 0)
                return null;

            var details = new List<BondDetail>();

            var trs = nodes[0].Descendants().Where(x => x.Name == "tr" && !x.HasClass("title")).ToArray();
            foreach (var tr in trs)
            {
                var tds = tr.ChildNodes.Where(x => x.Name == "td").ToArray();
                details.Add(new BondDetail
                {
                    Date = tds[1].InnerText.ToLongDate().Value,
                    ActionCode = tds[2].InnerText?.Clear(),
                    Amount = tds[3].InnerText.ToDoubleMoney(),
                    SoeDate = tds[4].InnerText.ToShortDate(),
                    RelToParty = tds[5].InnerText?.Clear(),
                });
            }

            return details;
        }

        private List<Cost> GetCosts()
        {
            var nodes = Document.DocumentNode.SelectNodes(@"//table[@class=""fines""]");
            if (nodes == null || nodes.Count == 0)
                return null;

            var costs = new List<Cost>();

            var trs = nodes[0].Descendants()
                .Where(x => x.Name == "tr" && !x.HasClass("title") && !x.HasClass("fines_total"))
                .ToArray();

            foreach (var tr in trs)
            {
                var tds = tr.ChildNodes.Where(x => x.Name == "td").ToArray();
                costs.Add(new Cost
                {
                    Description = tds[0].InnerText?.Clear(),
                    Imposed = tds[1].InnerText.ToDoubleMoney(),
                    Suspended = tds[2].InnerText.ToDoubleMoney(),
                    CcwpCts = tds[3].InnerText.ToDoubleMoney(),
                    Paid = tds[4].InnerText.ToDoubleMoney(),
                    Due = tds[5].InnerText.ToDoubleMoney(),
                });
            }

            return costs;
        }
        private List<Action> GetActions()
        {
            var nodes = Document.DocumentNode.SelectNodes(@"//table[@class=""actions""]");
            if (nodes == null || nodes.Count == 0)
                return null;

            var actions = new List<Action>();

            var trs = nodes[0].Descendants()
                .Where(x => x.Name == "tr" && !x.HasClass("title"))
                .ToArray();

            foreach (var tr in trs)
            {
                var tds = tr.ChildNodes.Where(x => x.Name == "td").ToArray();
                actions.Add(new Action
                {
                    Date = tds[0].InnerText.ToLongDate().Value,
                    Description = tds[1].InnerText?.Clear(),
                    JudicialOfficer = tds[2].InnerText?.Clear(),
                    Courtroom = tds[3].InnerText?.Clear(),
                    Dispo = tds[4].InnerText?.Clear(),
                    Amount = tds[5].InnerText?.ToDoubleMoney(),
                });
            }

            return actions;
        }

        private List<Sentence> GetSentences()
        {
            var nodes = Document.DocumentNode.SelectNodes(@"//table[@class=""sentences""]");
            if (nodes == null || nodes.Count == 0)
                return null;

            var sentences = new List<Sentence>();

            var trs = nodes[0].Descendants()
               .Where(x => x.Name == "tr" && !x.HasClass("title"))
               .ToArray();

            foreach (var tr in trs)
            {
                var tds = tr.ChildNodes.Where(x => x.Name == "td").ToArray();
                var date = tds[0].InnerText.ToLongDate();

                if (date.HasValue)
                {
                    sentences.Add(new Sentence
                    {
                        Date = date.Value,
                        Description = tds[1].InnerText?.Clear(),
                        Value = tds[2].InnerText.ToInt(),
                        Units = tds[3].InnerText?.Clear(),
                        DueDate = tds[4].InnerText?.ToShortDate(),
                        Status = tds[5].InnerText?.Clear(),
                    });
                }
                else
                {
                    sentences[sentences.Count - 1].SentenceDetails.Add(new SentenceDetail
                    {
                        Number = tds[1].InnerText?.ToInt(),
                        Description = tds[2].InnerText?.Clear(),
                        Value = tds[3].InnerText.ToInt(),
                        Units = tds[4].InnerText?.Clear(),
                    });
                }
            }

            return sentences;
        }

        public List<string> GetCaseLinks(string html)
        {
            Document.LoadHtml(html);

            var nodes = Document.DocumentNode.SelectNodes(@"//td[@class=""case_no""]/a");
            if (nodes == null || nodes.Count == 0)
                return null;
            
            return nodes.Select(x => HttpUtility.HtmlDecode(x.Attributes["href"]?.Value)).Where(x => x != null).ToList();
        }
    }
}