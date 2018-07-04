using SampleApp.Context;
using SampleApp.Converters;
using SampleApp.Model;
using SampleApp.Utils;
using SampleApp.View;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using Zenvia.Api;
using Zenvia.Api.Models.Enumerators;
using Zenvia.Api.Models.Requests;

namespace SampleApp.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private SendMessageWindow SendWindow { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<MessageModel> Messages { get; }

        private Visibility _progressVisible;
        public Visibility ProgressVisible { get { return _progressVisible; } set { _progressVisible = value; NotifyPropertyChanged(); } }

        private bool _gridEnabled;
        public bool GridEnabled { get { return _gridEnabled; } set { _gridEnabled = value; NotifyPropertyChanged(); } }

        public AppCommand NewCommand { get; }
        public AppCommand ReceiveCommand { get; }
        public AppCommand UpdateCommand { get; }

        public AppCommand SendCommand { get; }

        private string _sender;
        public string Sender { get { return _sender; } set { _sender = value; NotifyPropertyChanged(); } }

        private string _receiver;
        public string Receiver { get { return _receiver; } set { _receiver = value; NotifyPropertyChanged(); } }

        private string _message;
        public string Message { get { return _message; } set { _message = value; NotifyPropertyChanged(); } }


        private AppDBContext DBContext { get; }

        public MainViewModel()
        {
            NewCommand = new AppCommand(OnNewCommand);
            ReceiveCommand = new AppCommand(OnReceiveCommand);
            UpdateCommand = new AppCommand(OnUpdateCommand);
            SendCommand = new AppCommand(OnSendCommand, OnCanSendCommand);
            Messages = new ObservableCollection<MessageModel>();
            DBContext = new AppDBContext();
            UpdateCommand.Execute(null);
        }

        private ZenviaApi GetApi()
        {
            var api = new ZenviaApi(RuntimeMode.MOCK_SERVER, Properties.Resources.MOCK_CODE);
            api.Username = Properties.Resources.USERNAME;
            api.Password = Properties.Resources.PASSWORD;

            return api;
        }

        public async Task ReloadDatabase()
        {
            Messages.Clear();
            try
            {
                await DBContext.Messages.LoadAsync();
                foreach (var msg in DBContext.Messages.Local)
                {
                    Messages.Add(msg);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Falha ao recarregar base de dados: {ex.Message}");
            }
        }

        private void OnNewCommand(object obj)
        {
            SendWindow = new SendMessageWindow(this);
            SendWindow.Owner = Application.Current.MainWindow;
            SendWindow.ShowDialog();
        }

        private async void OnReceiveCommand(object obj)
        {
            SetBusy(true);

            var api = GetApi();
            var response = await api.ListUnreadMessages();

            if (response.HasMessages)
            {
                foreach (var msg in response.ReceivedMessages)
                {
                    if (DBContext.Messages.Where(x => x.MtId == msg.MtId).FirstOrDefault() == null)
                    {
                        DBContext.Messages.Add(new MessageModel(msg));
                    }
                }
                await DBContext.SaveChangesAsync();
                UpdateCommand.Execute(obj);
            }
        }

        private async void OnUpdateCommand(object obj)
        {
            SetBusy(true);
            await ReloadDatabase();
            SetBusy(false);
        }

        private async void OnSendCommand(object obj)
        {
            SendWindow.IsEnabled = false;
            var api = GetApi();

            var msg = new SingleMessageSms();
            msg.From = (string)PhoneConverter.ConvertBack(this.Sender);
            msg.To = (string)PhoneConverter.ConvertBack(this.Receiver);
            msg.Msg = this.Message;

            try
            {
                var response = await api.SendSms(msg);

                if (response.StatusCode.Equals("00"))
                {
                    MessageBox.Show($"Mensagem enviada com sucesso [{response.DetailCode}]!");
                    var msgs = new MessageModel(msg);
                    DBContext.Messages.Add(msgs);
                    DBContext.SaveChanges();
                    this.Messages.Add(msgs);
                }
                else
                {
                    MessageBox.Show($"Falha ao enviar: [{response.StatusCode}] {response.StatusDescription} \n[{response.DetailCode}] {response.DetailDescription}");
                }
                SendWindow.Close();

                this.Sender = string.Empty;
                this.Receiver = string.Empty;
                this.Message = string.Empty;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Falha ao enviar: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
            }
            finally
            {
                SendWindow.IsEnabled = true;
            }
        }

        private bool OnCanSendCommand(object obj)
        {
            return !string.IsNullOrEmpty(this.Sender?.Trim()) && !string.IsNullOrEmpty(this.Receiver?.Trim()) && !string.IsNullOrEmpty(this.Message?.Trim());
        }

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void SetBusy(bool value)
        {
            GridEnabled = !value;
            ProgressVisible = value ? Visibility.Visible : Visibility.Hidden;
            NewCommand.Enabled = !value;
            ReceiveCommand.Enabled = !value;
            UpdateCommand.Enabled = !value;
        }
    }
}
