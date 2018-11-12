using System;


using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;

namespace MyToastNotification
{
    internal class MyToastNotifications
    {
        private string _appID = string.Empty;
        public string AppID { get { return _appID; } }

        public MyToastNotifications(string appid) : base()
        {
            _appID = appid;
        }

        public bool ShowNotification(string caption = "", string message = "")
        {
            bool ret = false;

            // Create toast XML
            XmlDocument toastXml = new XmlDocument();
            toastXml.LoadXml(string.Format("<toast launch='app-defined-string'>" +
                                                "<visual>" +
                                                    "<binding template='ToastGeneric'>" +
                                                        "<image placement='appLogoOverride' hint-crop='circle' src='{0}' />" +
                                                        "<text >{1}</text>" +
                                                        "<text >{2}</text>" +
                                                        "<text placement='attribution'> Via MyToast </text>" +
                                                    "</binding>" +
                                                "</visual>" +
                                                "<actions>" +
                                                    "<action content='Yes' arguments='MyYes'/>" +
                                                    "<action content='No' arguments='MyNo' />" +
                                                "</actions>" +
                                            "</toast>",
                                string.Format(@"{0}\{1}", Environment.CurrentDirectory, "AppLogo.jpg"),
                                caption,
                                message));

            ToastNotification newToast = new ToastNotification(toastXml);

            // Add event
            newToast.Activated += OnActivated;
            newToast.Dismissed += OnDismissed;

            try
            {
                if (!string.IsNullOrEmpty(_appID))
                {
                    ToastNotificationManager.CreateToastNotifier(_appID).Show(newToast);

                    ret = true;
                }
            }
            catch (Exception e)
            {
                ret = false;
            }

            return ret;
        }

        private void OnDismissed(ToastNotification sender, ToastDismissedEventArgs args)
        {
            System.Windows.Application.Current.Dispatcher.Invoke(delegate ()
            {
                 System.Windows.MessageBox.Show("OnDismissed:: Reason = " + args.Reason);
            });
        }

        private void OnActivated(ToastNotification sender, object args)
        {
            System.Windows.Application.Current.Dispatcher.Invoke(delegate ()
            {
                if (args != null && (args is ToastActivatedEventArgs))
                {
                    var arg = args as ToastActivatedEventArgs;
                    switch (arg.Arguments)
                    {
                        case "MyYes":
                            System.Windows.MessageBox.Show("OnActivated:: You click \"Yes\"");
                            break;
                        case "MyNo":
                            System.Windows.MessageBox.Show("OnActivated:: You click \"No\"");
                            break;
                    }
                }
            });
        }
    }
}
