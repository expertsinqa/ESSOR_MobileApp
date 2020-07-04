using Prism.Navigation;
using Susu.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Susu.ViewModels
{
    public class CreateGroupPageViewModel : ViewModelBase
    {
        #region Properties
        public string _GroupName;
        public string GroupName { get { return _GroupName; } set { SetProperty(ref _GroupName, value); } }

        public string _Amount;
        public string Amount { get { return _Amount; } set { SetProperty(ref _Amount, value); } }

        public Color _Groupnameplaceholder = Color.Gray;
        public Color Groupnameplaceholder { get { return _Groupnameplaceholder; } set { SetProperty(ref _Groupnameplaceholder, value); } }

        public Color _AmountPlaceholder = Color.Gray;
        public Color AmountPlaceholder { get { return _AmountPlaceholder; } set { SetProperty(ref _AmountPlaceholder, value); } }


        public List<string> _lstperiod;
        public List<string> ContributionPeriod { get { return _lstperiod; } set { SetProperty(ref _lstperiod, value); } }

        public DateTime? _SelectedDate;
        public DateTime? SelectedDate { get { return _SelectedDate; } set { SetProperty(ref _SelectedDate, value); } }

        public bool _IsContributionDayVisible = false;
        public bool IsContributionDayVisible { get { return _IsContributionDayVisible; } set { SetProperty(ref _IsContributionDayVisible, value); } }

        public bool _IsContributionDateVisible = false;
        public bool IsContributionDateVisible { get { return _IsContributionDateVisible; } set { SetProperty(ref _IsContributionDateVisible, value); } }

        public List<string> _DaysList;
        public List<string> DaysList { get { return _DaysList; } set { SetProperty(ref _DaysList, value); } }

        public ICommand BackClicked
        {
            get { return new Command(Back); }
        }
        public ICommand CreateGroupClicked
        {
            get { return new Command(CreateGroup); }
        }

        public ICommand CustomRolesClicked
        {
            get { return new Command(customRules); }
        }

        public string _selectedPeriod;
        public string selectedPeriod { get { return _selectedPeriod; } set { SetProperty(ref _selectedPeriod, value); } }

        public bool _IsCustomRulesVisible = false;
        public bool IsCustomRulesVisible { get { return _IsCustomRulesVisible; } set { SetProperty(ref _IsCustomRulesVisible, value); } }

        public ICommand CloseClicked { get { return new Command(Close); } }
        public ICommand CreateRulesClicked { get { return new Command(CreateRules); } }

        public string _selectedContributionDay;
        public string selectedContributionDay { get { return _selectedContributionDay;} set { SetProperty(ref _selectedContributionDay, value); } }

        public string _CustomRulesText;
        public string CustomRulesText { get { return  _CustomRulesText; } set { SetProperty(ref _CustomRulesText, value); }  }

        public bool _IsGroupStartDateVisible = false;
        public bool IsGroupStartDateVisible { get { return _IsGroupStartDateVisible; } set { SetProperty(ref _IsGroupStartDateVisible, value); } }

        public bool _IsPayoutDateVisible = false;
        public bool IsPayoutDateVisible { get { return _IsPayoutDateVisible; } set { SetProperty(ref _IsPayoutDateVisible, value); } }

        public bool _IsPayoutDayVisible = false;
        public bool IsPayoutDayVisible { get { return _IsPayoutDayVisible; } set { SetProperty(ref _IsPayoutDayVisible, value); } }

        public DateTime? _GroupStartDate;
        public DateTime? GroupStartDate { get { return _GroupStartDate; } set { SetProperty(ref _GroupStartDate, value); } }

        public DateTime? _payoutDate;
        public DateTime? payoutDate { get { return _payoutDate; } set { SetProperty(ref _payoutDate, value); } }

        public string _selectedPayoutDay;
        public string selectedPayoutDay { get { return _selectedPayoutDay; } set { SetProperty(ref _selectedPayoutDay, value); } }

        #endregion
        #region Constructor
        public CreateGroupPageViewModel(INavigationService navigationService):base(navigationService)
        {
            _lstperiod = new List<string>() {"Weekly", "Monthly", "Yearly" };
            DaysList = new List<string>() { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };
            
        }
        #endregion
        #region Functions
        public async void Back()
        {
           await NavigationService.NavigateAsync("LandingPage");
        }

        public  void customRules()
        {
            //CustomRulesText = "";
            IsCustomRulesVisible = true;
        }
        public  void Close()
        {
            IsCustomRulesVisible = false;
        }
        public async void CreateGroup()
        {
            if(string.IsNullOrEmpty(GroupName))
            {
                Groupnameplaceholder = Color.Red;
                return;
            }
            else if(string.IsNullOrEmpty(Amount))
            {
                AmountPlaceholder = Color.Red;
                return;
            }
            else if(string.IsNullOrEmpty(CustomRulesText))
            {
                if(await App.Current.MainPage.DisplayAlert("","Please add the custom rules to the group","OK","Cancel"))
                {
                    IsCustomRulesVisible = true;
                    return;
                }
                else
                {
                    IsCustomRulesVisible = false;
                    //CreateGroup();
                }
            }
            IsLoading = true;
            GroupDto groupDto = new GroupDto();
            groupDto.GroupName = GroupName;
            groupDto.ContributionAmount = Decimal.Parse(Amount);
            groupDto.ContributionPeriod = selectedPeriod;
            groupDto.ContributionDate = SelectedDate;
            groupDto.CustomRules = CustomRulesText;
            groupDto.GroupStartDate = GroupStartDate;
            groupDto.PayOutDate = payoutDate;
            groupDto.PayOutDay = selectedPayoutDay;
            groupDto.ContributionDay = selectedContributionDay;
            if (App.Current.Properties.ContainsKey("UserId") && !string.IsNullOrEmpty(App.Current.Properties["UserId"].ToString()))
                groupDto.CreatorId = long.Parse(App.Current.Properties["UserId"].ToString());
            else
                groupDto.CreatorId = App.UserId;
            GroupDto group = await ServiceBase.SaveGroupInfo(groupDto);
            IsLoading = false;
            if (group!=null && group.Id > 0)
            {
                App.Current.Properties["GroupId"] = group.Id;
                await App.Current.SavePropertiesAsync();
                App.GroupId = group.Id;
                App.IsGroupAdmin = true;
                NavigationParameters np = new NavigationParameters();
                np.Add("GroupCode", group.GroupNumber);
                np.Add("GroupName", group.GroupName);
                await NavigationService.NavigateAsync("InviteScreenPage",np);
            }
            else if(group!=null && group.Id < 0 )
            {
                await App.Current.MainPage.DisplayAlert("Alert", "Group with this name already exists", "OK");
            }
                      
        }

        public void CreateRules()
        {
            IsCustomRulesVisible = false;
        }
        #endregion
    }
}
