using System.Collections.Generic;
using System.Linq;
using mvvmframework.Enums;
using mvvmframework.Models;
using mvvmframework.Interfaces;
using mvvmframework.ViewModels.Common;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Messaging;

namespace mvvmframework.ViewModels
{
    public class ExpensesViewModel: CommonJourneys
    {
        IConnection connectService;
        IUserSettings settingsService;
        IWebSevices webService;
        IExpenses expenseService;
        IRepository repoService;
        ICultureInfo cultureService;

        public ExpensesViewModel(IConnection con, IUserSettings user, IWebSevices web, IExpenses exp, 
                                 IRepository repo, ICultureInfo cult, ILocation loc, ISockets sock) : base (loc, repo, sock)
        {
            webService = web;
            settingsService = user;
            connectService = con;
            expenseService = exp;
            repoService = repo;
            cultureService = cult;

            ExportType = ExpenseTypes.SelectedPeriod;
            Journeys = repoService.GetList<DBJourneyModel>();
        }

        bool selectAll;
        public bool SelectAll
        {
            get => selectAll;
            set => Set(ref selectAll, value, true);
        }

        ExpenseTypes exportType;
        public ExpenseTypes ExportType
        {
            get { return exportType; }
            set { Set(() => ExportType, ref exportType, value, true); }
        }

        public void ToggleJourney(long id)
        {
            var _ = GetExpensesList.FirstOrDefault(t => t.JourneyId == id);
            _.Selected = !_.Selected;
            _.ImageName = !_.Selected ? "switch_button_off" : "switch_button_on";
        }

        List<long> expensesToExport;
        public List<long> ExpensesToExport
        {
            get => expensesToExport;
            set => Set(() => ExpensesToExport, ref expensesToExport, value);
        }

        public async Task GetNotifications()
        {
            if (connectService.IsConnected)
            {
                IsBusy = true;
                await webService.GetNotifications(settingsService.LoadSetting<string>("Username", SettingType.String),
                                                  settingsService.LoadSetting<string>("Password", SettingType.String)).ContinueWith((t) =>
                                                  {
                                                      if (t.IsCompleted)
                                                      {
                                                          IsBusy = false;
                                                          if (!t.IsFaulted && !t.IsCanceled)
                                                              Notifications = t.Result.Notifications;
                                                          else
                                                              Messenger.Default.Send(new NotificationMessage(t.Result.Status.Message));
                                                      }
                                                  });
            }
        }

        List<ExpenseModel> expenses;
        public List<ExpenseModel> Expenses
        {
            get => expenses;
            set { Set(() => Expenses, ref expenses, value, true); }
        }

        public List<ExpenseModel> GetExpensesList
        {
            get
            {
                var exps = new List<ExpenseModel>();
                foreach(var j in Journeys)
                {
                    exps.Add(new ExpenseModel
                    {
                        JourneyId = j.JourneyId,
                        StartDate = j.StartDate,
                        EndDate = j.EndDate,
                        OverallScore = j.OverallScore,
                        SmoothScore = j.SmoothScore,
                        SpeedScore = j.SpeedScore,
                        StartLocation = j.StartLocation,
                        EndLocation= j.EndLocation,
                        Private = j.Private,
                        Miles =j.Miles,
                        HasNotifications = Notifications.Count(t=>t.JourneyId == j.JourneyId) != 0
                    });
                }
                if (ShowPrivate)
                    exps = exps.Where(t => t.Private).ToList();
                return exps;
            }
        }

        public string StartDateText => $"{StartDate.Date.ToString("dd MMM yyyy")}";

        public string EndDateText => $"{EndDate.Date.ToString("dd MMM yyyy")}";

        public void SelectPrivate()
        {
            var _ = GetExpensesList;
        }

        public void PerformSearch()
        {
            var exp = GetExpensesList.Where(t => t.StartDate >= StartDate).Where(w => w.EndDate <= EndDate).Where(z=>z.Private == ShowPrivate).ToList();
            Expenses = exp;
        }

        public void AllJourneys()
        {
            foreach (var _ in GetExpensesList)
                _.Selected = SelectAll;
            
            if (SelectAll)
            {
                ExpensesToExport.Clear();
            }
            else
            {
                ExpensesToExport.AddRange(GetExpensesList.Select(t => t.JourneyId).ToList());
            }
        }

        public void ExportExpensesToFile()
        {
            if (ExpensesToExport.Count == 0)
                return;
            
            var journeysToExport = (from j in Journeys
                                    from e in ExpensesToExport
                                    where e == j.JourneyId
                                    select j).ToList();

            switch (ExportType)
            {
                case ExpenseTypes.PrivateJourney:
                    var privateJourneys = Journeys?.Where(t => t.Private).OrderBy(t => t.EndDate.Year).ThenBy(t => t.EndDate.Month).
                        ThenBy(t => t.EndDate.Day).ThenBy(t => t.EndDate.Hour).ThenBy(t => t.EndDate.Minute).ToList();
                    journeysToExport.Add(privateJourneys[0]);
                    break;
                case ExpenseTypes.BusinessJourney:
                    var businessJourneys = Journeys?.Where(t => !t.Private).OrderBy(t => t.EndDate.Year).ThenBy(t => t.EndDate.Month).
                        ThenBy(t => t.EndDate.Day).ThenBy(t => t.EndDate.Hour).ThenBy(t => t.EndDate.Minute).ToList();
                    journeysToExport.Add(businessJourneys[0]);
                    break;
                case ExpenseTypes.ToDate:
                    var dates = Journeys.Where(t => t.EndDate <= EndDate).OrderBy(t => t.EndDate.Year).ThenBy(t => t.EndDate.Month).
                        ThenBy(t => t.EndDate.Day).ThenBy(t => t.EndDate.Hour).ThenBy(t => t.EndDate.Minute).ToList();
                    journeysToExport.AddRange(dates);
                    break;
                case ExpenseTypes.SelectedPeriod:
                    var filtered = (from j in Journeys
                                    where j.StartDate >= StartDate
                                    where j.EndDate <= EndDate
                                    select j).OrderBy(t => t.EndDate.Year).
                        ThenBy(t => t.EndDate.Month).
                        ThenBy(t => t.EndDate.Day).
                        ThenBy(t => t.EndDate.Hour).
                        ThenBy(t => t.EndDate.Minute).ToList();
                    journeysToExport.AddRange(filtered);
                    break;
            }

            if (journeysToExport.Count > 0)
            {
                var exportList = new List<JourneyModel>();
                foreach (var db in journeysToExport)
                {
                    var _ = new JourneyModel(cultureService.currentCulture)
                    {
                        EndDate = db.EndDate,
                        EndLocation = db.EndLocation,
                        Id = db.Id,
                        PolicyId = db.PolicyId,
                        JourneyId = db.JourneyId,
                        JourneyNumber = db.JourneyNumber,
                        StartLocation = db.StartLocation,
                        Miles = db.Miles,
                        Nickname = db.Nickname,
                        OverallScore = db.OverallScore,
                        Private = db.Private,
                        StartDate = db.StartDate,
                        SpeedScore = db.SpeedScore,
                        SmoothScore = db.SmoothScore,
                        UsageScore = db.UsageScore,
                    };
                    exportList.Add(_);
                } 

                expenseService.ExportJourneysToEmail(exportList);
            }

        }
    }
}
